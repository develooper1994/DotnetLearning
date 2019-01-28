using System;
using System.Collections.Generic;
using System.Windows.Forms;

// dotnet utilities
using System.Runtime.CompilerServices;

// tested functions
using HeatMap;
using TestHeatMap.Properties;


namespace TestHeatMap
{
    public partial class Form1 : Form
    {
        public List<HeatPoint> Points { get; set; }
        public ColorRamp ColorRamp { get; set; }
        public int Radius { get; set; }
        public float Opacity1 { get; set; }
        public int Count { get; set; }
        public int Height1 { get; set; }
        public int Width1 { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private List<HeatPoint> RandomPoints(int width, int height, int count)
        {
            List<HeatPoint> result = new List<HeatPoint>();

            var r = new Random();
            for (int i = 0; i < count; i++)
            {
                var x = r.Next(width);
                var y = r.Next(height);
                //var w = (float)r.NextDouble()/2;

                HeatPoint point = new HeatPoint
                {
                    X = x,
                    Y = y,
                    W = 1
                };
                result.Add(point);
            }
            return result;
        }

        private async void Make4Maps(int width, int height, int radius, List<HeatPoint> points, 
                                float opacity, ColorRamp cr) {
            HeatMapMaker hmMaker = new HeatMapMaker
            {
                Width = width,
                Height = height,
                Radius = radius,
                ColorRamp = cr,
                HeatPoints = points,
                Opacity = opacity
            };
            pictureBox2.BackgroundImage = await hmMaker.MakeHeatMap(); // ***renklendirilmiş asıl heatmap***
            pictureBox1.BackgroundImage = hmMaker.GrayMap;
            
            DisplayInfo();
        }

        private void DisplayInfo()
        {
            lblCount.Text = String.Format("{0}:{1}", "Point Count", Count);
            lblRadius.Text = String.Format("{0}:{1}", "Point Radius", Radius);
            lblOpacity.Text = String.Format("{0}:{1}", "Map Opacity", Opacity1);
        }

        //program çalışırken scrollarla oynadıktan sonrabir süre sonra hafıza kullanımı
        //1 Gb ulaştı ve GC iş bitince silmedi. Algoritmayı kontrol et.
        //[MethodImpl(MethodImplOptions.AggressiveInlining)] sebep olabilir.
        private void TbHPCount_Scroll(object sender, EventArgs e)
        {
            Count = tbHPCount.Value;
            Points = RandomPoints(Width1, Height1, Count);
            Make4Maps(Width1, Height1, Radius, Points, Opacity1, ColorRamp);
            GC.Collect();
        }

        private void TbHPRadius_Scroll(object sender, EventArgs e)
        {
            Radius = tbHPRadius.Value;
            Make4Maps(Width1, Height1, Radius, Points, Opacity1, ColorRamp);
            GC.Collect();
        }

        private void TbOpacity_Scroll(object sender, EventArgs e)
        {
            Opacity1 = tbOpacity.Value/10f;
            Make4Maps(Width1, Height1, Radius, Points, Opacity1, ColorRamp);
            GC.Collect();
        }

        private void CmbColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbColorRamp.SelectedIndex)
            {
                case 0:
                    pictureBox3.BackgroundImage = Resources.i_thermal;
                    break;
                case 1:
                    pictureBox3.BackgroundImage = Resources.i_rainbow;
                    break;
                case 2:
                    pictureBox3.BackgroundImage = Resources.i_redwhiteblue;
                    break;
            }
            ColorRampUpdate();
            Make4Maps(Width1, Height1, Radius, Points, Opacity1, ColorRamp);
            GC.Collect();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ColorRampUpdate() // updates ColorRamp property
            => ColorRamp = (ColorRamp)Enum.Parse(typeof(ColorRamp), value: cmbColorRamp.SelectedItem.ToString());

        private void Form1_Load(object sender, EventArgs e)
        {
            Width1 = pictureBox1.Width;
            Height1 = pictureBox1.Height;
            Opacity1 = tbOpacity.Value / 10f;
            Count = tbHPCount.Value;
            Radius = tbHPRadius.Value;
            Points = RandomPoints(Width1, Height1, Count);
            cmbColorRamp.SelectedIndex = 1;
            ColorRampUpdate(); // updates ColorRamp property
            Make4Maps(Width1, Height1, Radius, Points, Opacity1, ColorRamp);
        }
    }
}