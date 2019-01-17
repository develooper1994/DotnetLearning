using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExerciseWpf
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var desc = DescriptionTB.Text;
            var mesg = string.Format($"The description is: {desc}");
            if (string.IsNullOrEmpty(desc))
            {
                mesg = string.Format("Description is empty. Please fill the description.");
            }
            MessageBox.Show(mesg);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            WeldCB.IsChecked = AssemblyCB.IsChecked = PlasmaCB.IsChecked = LaserCB.IsChecked = PurchaseCB.IsChecked =
                LatheCB.IsChecked = DrillCB.IsChecked = FoldCB.IsChecked = RollCB.IsChecked = SawCB.IsChecked = false;

            LenghtTB.Text = string.Empty;
            NoteTxt.Text = string.Empty;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CB_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;

            StringBuilder builtString = new StringBuilder(); // Much more efficient. Get used to it. Not always efficient
            builtString.Append(cb.Content as string);

            LenghtTB.Text += builtString.ToString();
        }

        private void Finish_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteTxt is null)
                return;

            ComboBox cb = sender as ComboBox;
            ComboBoxItem cbItem = cb.SelectedItem as ComboBoxItem;
            string item = cbItem.Content as string;

            NoteTxt.Text = item;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Finish_SelectionChanged(FinishDropDown, null);
        }

        private void SupplierNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            MassTB.Text = SupplierNameTB.Text;
        }
    }
}
