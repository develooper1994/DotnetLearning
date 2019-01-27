using System;
using System.Collections.Generic;
using frm= System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.HeatMap
{
    public class HeatMap
    {
        private static int Width;
        private static int Height;
        private static Bitmap bmp;

        public HeatMap(int width, int height)
        {
            #region Bitmap Not Created before class creator
            Width = width;
            Height = height;
            bmp = new Bitmap(Width, Height);
            #endregion
        }
        public HeatMap(Bitmap b)
        {
            #region Bitmap Creation Before class creator
            bmp = b;
            #endregion
        }
        public Bitmap MapGenerate(frm.PictureBox picbox, List<HeatPoint> heatPoints)
        {
            // Create new memory bitmap the same size as the picture box
            Bitmap bMap = new Bitmap(picbox.Width, picbox.Height);
            // Initialize random number generator
            Random rRand = new Random();
            // Loop variables
            int iX, iY;
            byte iIntense;
            var iteration = 500;
            // Lets loop 500 times and create a random point each iteration
            for (int i = 0; i < iteration; i++)
            {
                // Pick random locations and intensity
                iX = rRand.Next(0, picbox.Width);
                iY = rRand.Next(0, picbox.Height);
                iIntense = Convert.ToByte(rRand.Next(0, 10));
                // Add heat point to heat points list
                heatPoints.Add(new HeatPoint(iX, iY, iIntense));
            }

            // var hp = new HeatMap();
            // Call CreateIntensityMask, give it the memory bitmap, and store the result back in the memory bitmap
            bMap = HeatMap.CreateIntensityMask(bMap, heatPoints);
            // Colorize the memory bitmap and assign it as the picture boxes image
            return HeatMap.Colorize(bMap, 255);
        }
        private static Bitmap CreateIntensityMask(Bitmap bSurface, List<HeatPoint> aHeatPoints)
        {
            // Create new graphics surface from memory bitmap
            Graphics DrawSurface = Graphics.FromImage(bSurface);
            // Set background color to white so that pixels can be correctly colorized
            DrawSurface.Clear(Color.White);
            // Traverse heat point data and draw masks for each heat point
            foreach (HeatPoint DataPoint in aHeatPoints)
            {
                // Render current heat point on draw surface
                DrawHeatPoint(DrawSurface, DataPoint, 15);
            }
            return bSurface;
        }
        private static void DrawHeatPoint(Graphics Canvas, HeatPoint HeatPoint, int Radius)
        {
            // Create points generic list of points to hold circumference points
            List<Point> CircumferencePointsList = new List<Point>();
            // Create an empty point to predefine the point struct used in the circumference loop
            Point CircumferencePoint;
            // Create an empty array that will be populated with points from the generic list
            Point[] CircumferencePointsArray;
            // Calculate ratio to scale byte intensity range from 0-255 to 0-1
            float fRatio = 1F / Byte.MaxValue;
            // Precalulate half of byte max value
            byte bHalf = Byte.MaxValue / 2;
            // Flip intensity on it's center value from low-high to high-low
            int iIntensity = (byte)(HeatPoint.Intensity - ((HeatPoint.Intensity - bHalf) * 2));
            // Store scaled and flipped intensity value for use with gradient center location
            float fIntensity = iIntensity * fRatio;
            // Loop through all angles of a circle
            // Define loop variable as a double to prevent casting in each iteration
            // Iterate through loop on 10 degree deltas, this can change to improve performance
            for (double i = 0; i <= 360; i += 10)
            {
                // Replace last iteration point with new empty point struct
                CircumferencePoint = new Point
                {
                    // Plot new point on the circumference of a circle of the defined radius
                    // Using the point coordinates, radius, and angle
                    // Calculate the position of this iterations point on the circle
                    X = Convert.ToInt32(HeatPoint.X + Radius * Math.Cos(ConvertDegreesToRadians(i))),
                    Y = Convert.ToInt32(HeatPoint.Y + Radius * Math.Sin(ConvertDegreesToRadians(i)))
                };
                // Add newly plotted circumference point to generic point list
                CircumferencePointsList.Add(CircumferencePoint);
            }
            // Populate empty points system array from generic points array list
            // Do this to satisfy the datatype of the PathGradientBrush and FillPolygon methods
            CircumferencePointsArray = CircumferencePointsList.ToArray();
            // Create new PathGradientBrush to create a radial gradient using the circumference points
            PathGradientBrush GradientShaper = new PathGradientBrush(CircumferencePointsArray);
            // Create new color blend to tell the PathGradientBrush what colors to use and where to put them
            ColorBlend GradientSpecifications = new ColorBlend(3)
            {
                // Define positions of gradient colors, use intesity to adjust the middle color to
                // show more mask or less mask
                Positions = new float[3] { 0, fIntensity, 1 },
                // Define gradient colors and their alpha values, adjust alpha of gradient colors to match intensity
                Colors = new Color[3]
            {
            Color.FromArgb(0, Color.White),
            Color.FromArgb(HeatPoint.Intensity, Color.Black),
            Color.FromArgb(HeatPoint.Intensity, Color.Black)
            }
            };
            // Pass off color blend to PathGradientBrush to instruct it how to generate the gradient
            GradientShaper.InterpolationColors = GradientSpecifications;
            // Draw polygon (circle) using our point array and gradient brush
            Canvas.FillPolygon(GradientShaper, CircumferencePointsArray);
        }
        private static double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }
        private static Bitmap Colorize(Bitmap Mask, byte Alpha)
        {
            // Create new bitmap to act as a work surface for the colorization process
            Bitmap Output = new Bitmap(Mask.Width, Mask.Height, PixelFormat.Format32bppArgb);
            // Create a graphics object from our memory bitmap so we can draw on it and clear it's drawing surface
            Graphics Surface = Graphics.FromImage(Output);
            Surface.Clear(Color.Transparent);
            // Build an array of color mappings to remap our greyscale mask to full color
            // Accept an alpha byte to specify the transparancy of the output image
            ColorMap[] Colors = CreatePaletteIndex(Alpha);
            // Create new image attributes class to handle the color remappings
            // Inject our color map array to instruct the image attributes class how to do the colorization
            ImageAttributes Remapper = new ImageAttributes();
            Remapper.SetRemapTable(Colors);
            // Draw our mask onto our memory bitmap work surface using the new color mapping scheme
            Surface.DrawImage(Mask, new Rectangle(0, 0, Mask.Width, Mask.Height), 0, 0, Mask.Width, Mask.Height, GraphicsUnit.Pixel, Remapper);
            // Send back newly colorized memory bitmap
            return Output;
        }

        private static dynamic Palette
        {
            get => bmp;
            set
            {
                string GetUrl(Uri uri) => uri?.IsAbsoluteUri == true ? uri?.AbsoluteUri : uri?.ToString();
                if (value is Bitmap)
                {
                    bmp = value as Bitmap;
                }
                else if(value is string)
                {
                    bmp= new Bitmap(Image.FromFile(filename: $"{Convert.ToString(value)}"));
                }
                else if(value is Uri)
                {
                    bmp = new Bitmap(Image.FromFile(filename: GetUrl(value)));
                }
            }
        }
        private static Bitmap RandomPalette
        {
            get
            {
                var rand = new Random();
                int a, r, g, b;
                for (int j = 0; j < Height; j++)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        a = rand.Next(0, 256);
                        r = rand.Next(0, 256);
                        g = rand.Next(0, 256);
                        b = rand.Next(0, 256);
                        bmp.SetPixel(x: i, y: j, color: Color.FromArgb(a, r, g, b));
                    }
                }
                return bmp;
            }
            set
            {
                if (value is Bitmap) // bitmap şeklinde verildiyse
                {
                    bmp = value;
                }
                else
                {
                    throw new NotImplementedException(); // implement it.
                }
            }
        }

        private static ColorMap[] CreatePaletteIndex(byte Alpha)
        {
            ColorMap[] OutputMap = new ColorMap[256];
            // Change this path to wherever you saved the palette image.
            // Bitmap Palette = (Bitmap)Bitmap.FromFile(@"C:\Users\Dylan\Documents\Visual Studio 2005\Projects\HeatMapTest\palette.bmp");
            Bitmap CreatePalette = RandomPalette;

            // Loop through each pixel and create a new color mapping
            for (int X = 0; X <= 255; X++)
            {
                OutputMap[X] = new ColorMap
                {
                    OldColor = Color.FromArgb(X, X, X),
                    NewColor = Color.FromArgb(Alpha, CreatePalette.GetPixel(X, 0))
                };
            }
            return OutputMap;
        }
    }

    public struct HeatPoint
    {
        public int X;
        public int Y;
        public byte Intensity;
        public HeatPoint(int iX, int iY, byte bIntensity)
        {
            X = iX;
            Y = iY;
            Intensity = bIntensity;
        }
    }
    
}
