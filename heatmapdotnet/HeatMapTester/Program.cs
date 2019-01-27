using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;

namespace HeatMapTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Image im = new Bitmap("TestImage/Jenna.jpg");

            float[] px = new float[]{
                                  41,
                                  72,
                                  73,
                                  73,
                                  73,
                                  73,
                                  73
                              };
            float[] py = new float[]{
                                  41,
                                  72,
                                  73,
                                  73,
                                  73,
                                  73,
                                  73
                              };


            var canvas = HeatMap.NET.HeatMap.GenerateHeatMap(im, px, py);

            #region Show The Heated Image
            var task = new Thread(new ThreadStart(Showimage));
            task.Start();
            task.Join();

            #endregion

            canvas.Save("Jenna.Heated.Jpg", ImageFormat.Jpeg);
            System.Console.ReadKey(true);
        }
        private static void Showimage()
        {
            var f = new Form
            {
                FormBorderStyle = FormBorderStyle.None
            };
            f.Controls.Add(new PictureBox() { ImageLocation = @"Jenna.Heated.Jpg", Dock = DockStyle.Fill });
            f.Show();
            Thread.Sleep(2000);
        }
    }
}
