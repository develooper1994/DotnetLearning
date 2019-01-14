using System.Windows;
using System.Windows.Controls;

namespace PlayBoxWpf
{
    /// <summary>
    /// Page2.xaml etkileşim mantığı
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, System.Windows.RoutedEventArgs e) => MessageBox.Show("Button 1 Pressed");

        private void Button2_Click(object sender, System.Windows.RoutedEventArgs e) => MessageBox.Show("Button 2 Pressed");

        private void Button3_Click(object sender, System.Windows.RoutedEventArgs e) => MessageBox.Show("Button 3 Pressed");
    }
}
