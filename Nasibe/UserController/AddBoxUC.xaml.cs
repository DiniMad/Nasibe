using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Telerik.Windows.Controls.ColorEditor.ColorSchemas;

namespace Nasibe.UserController
{
    /// <summary>
    /// Interaction logic for AddBoxUC.xaml
    /// </summary>
    public partial class AddBoxUC : UserControl
    {
        public int LblId { get; set; }
        public AddBoxUC()
        {
            InitializeComponent();
        }
        private void TxtDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var digitRegex = new Regex("[^0-9.]+");
            e.Handled = digitRegex.IsMatch(e.Text);
        }
        private void TxtDigitPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var digitRegex = new Regex("[^0-9]+");
            e.Handled = digitRegex.IsMatch(e.Text);
        }
        private void TxtText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textRegex = new Regex("[^0-9a-zA-Zآ-ی-]+");
            e.Handled = textRegex.IsMatch(e.Text);
        }
        private void YesToPpl_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (RadioButtonByPpl.IsChecked == true)
            {
                TxtPrice.Text = "کمک مردمی";
                TxtPrice.IsEnabled = false;
            }
            else
            {
                TxtPrice.Text = string.Empty;
                TxtPrice.IsEnabled = true;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (BtnNewRecord.Visibility == Visibility.Hidden)
            {
                Background = Brushes.DimGray;
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = Brushes.Transparent;
        }
    }
}
