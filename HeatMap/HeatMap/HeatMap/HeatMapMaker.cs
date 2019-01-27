using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace HeatMap
{
    public class HeatMapMaker
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Radius { get; set; }
        public float Opacity { get; set; }
        public ColorRamp ColorRamp { get; set; }
        public List<HeatPoint> HeatPoints { get; set; }
        public Bitmap GrayMap { get; private set; }

        public Task<Bitmap> MakeHeatMap()
        {
            return Task.Run(() =>
            {
                var result = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                GrayMap = MakeGrayMap().Result;
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var grayVal = GrayMap.GetPixel(x, y);
                        var index = grayVal.A;
                        var color = ColorUtil.GetColorInRamp(index, ColorRamp);
                        result.SetPixel(x, y, color);
                    }
                }
                return ColorUtil.AdjustOpacity(result, Opacity);
            });
        }

        private Task<Bitmap> MakeGrayMap()
        {
            return Task.Run(() =>
            {
                var result = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(result);

                var grayRamp = ColorUtil.GetGrayRamp();
                foreach (var point in HeatPoints)
                {
                    var r = Radius;
                    var Px = (int)point.X - r;
                    var Py = (int)point.Y - r;
                    var width = r * 2;
                    var height = r * 2;

                    var path = new GraphicsPath();
                    var rect = new Rectangle(x: Px, y: Py, width: width, height: height);
                    path.AddEllipse(rect);
                    graphics.FillEllipse(new PathGradientBrush(path)
                    {
                        InterpolationColors = grayRamp
                    }, rect: rect);
                }
                graphics.Dispose();
                return result;
            });
        }
    }
}