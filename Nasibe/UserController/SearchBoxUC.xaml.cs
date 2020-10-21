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

namespace Nasibe.UserController
{
    /// <summary>
    /// Interaction logic for SearchBoxUC.xaml
    /// </summary>
    public partial class SearchBoxUC : UserControl
    {
        public int LblId { get; set; }
        public string LblQty { get; set; }
        public SearchBoxUC()
        {
            InitializeComponent();
        }

        private void TxtUserQty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var digitRegex = new Regex("[^0-9.]+");
            e.Handled = digitRegex.IsMatch(e.Text);
        }
    }
}
