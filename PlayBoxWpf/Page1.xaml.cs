using System;
using System.Windows;
using System.Windows.Controls;

namespace PlayBoxWpf
{
    /// <summary>
    /// Page1.xaml etkileşim mantığı
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            // ... See if a date is selected.
            if (calendar.SelectedDate.HasValue)
            {
                // ... Display SelectedDate in Title. 
                DateTime date = calendar.SelectedDate.Value;
                Title = date.ToShortDateString();
            }
        }
        private void HandleChecked(object sender)
        {
            var cb = sender as CheckBox;
            if (cb.Name == "cb1")
            {
                tblk1.Text = "2 state checkbox is checked";
            }
            else
            {
                tblk2.Text = "3 state checkbox is checked";
            }
        }
        private void HandleUnchecked(object sender)
        {
            var cb = sender as CheckBox;
            if (cb.Name == "cb1")
            {
                tblk1.Text = "2 state checkbox is unchecked";
            }
            else
            {
                tblk2.Text = "3 state checkbox is unchecked";
            }
        }

        private void Cb1_Checked(object sender, RoutedEventArgs e)
        {
            HandleChecked(sender);
        }

        private void Cb1_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUnchecked(sender);
        }

        private void Cb2_Indeterminate(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            tblk2.Text = "3 state checkbox is in indeterminate state";
        }

        private void Cb2_Checked(object sender, RoutedEventArgs e)
        {
            HandleChecked(sender);
        }

        private void Cb2_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleUnchecked(sender);
        }
    }
}
