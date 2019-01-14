using Microsoft.Win32;
using System.Windows;

namespace FirstWpfApp
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "İlk WPF uygulaması";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Title = e.GetPosition(this).ToString();
        }

        //private void Send_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show($"Hello {UsersName.Text.ToString()}");
        //}

        //private void Button1_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("The App is Closing");
        //    this.Close();
        //}

        //private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openDlg = new OpenFileDialog();
        //    openDlg.ShowDialog();
        //}

        //private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog openDlg = new SaveFileDialog();
        //    openDlg.ShowDialog();
        //}
    }
}
