using System;
using System.Windows;
using System.Windows.Controls;

namespace PlayBoxWpf
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pg = new Page1();
            MainFrame.Content = pg;
            DateTime newTitle = pg.clndr.SelectedDate.Value;
            
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page2();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {

        }
    }
}
