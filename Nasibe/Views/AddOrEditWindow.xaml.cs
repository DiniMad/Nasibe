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
using System.Windows.Shapes;
using Nasibe.Controller;

namespace Nasibe.Views
{
    /// <summary>
    /// Interaction logic for AddOrEditWindow.xaml
    /// </summary>
    public partial class AddOrEditWindow : Window
    {
        public int Pid { get; set; }
        public AddOrEditWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEditContent();
        }

        public void LoadEditContent()
        {
            ComboBoxUnitQty.ItemsSource = CatalogTable.SelectFromCatalogTable();
            ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
            ComboBoxUnitQty.SelectedValuePath = "CatalogId";
            var product = ProductTable.SelectSingleProduct(Pid);
            TxtTitle.Text = product.ProductName;
            TxtPrice.Text = product.ProductUnitPrice.ToString();
            TxtQty.Text = product.ProductCount.ToString();
            TxtDescription.Text = string.Empty;
            TxtDescription.AppendText(product.ProductDescription);
            var ppl = product.ProductPopularSupport;
            if (ppl)
            {
                YesToPpl.IsChecked = true;
            }
            else
            {
                NoToPpl.IsChecked = true;
            }
            if (product.Catalog != null)
                ComboBoxUnitQty.SelectedValue = product.Catalog.CatalogId;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            var catalogItemsWindow = new UnitQtyItmesWindow { Changed = false };
            catalogItemsWindow.ShowDialog();
            if (catalogItemsWindow.Changed)
            {
                ComboBoxUnitQty.ItemsSource = CatalogTable.SelectFromCatalogTable();
                ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
                ComboBoxUnitQty.SelectedValuePath = "CatalogId";
                ComboBoxUnitQty.SelectedIndex = ComboBoxUnitQty.Items.Count - 1;
            }
            else
            {
                var product = ProductTable.SelectSingleProduct(Pid);
                if (product.Catalog == null) return;
                ComboBoxUnitQty.SelectedValue = product.Catalog.CatalogValue;
            }
        }
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "عنوان را پر کنید",
                    Caption = "عنوان نمیتواند خالی باشد.",
                    InformationIcon = true,
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtQty.Text))
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "مقدار را پر کنید",
                    Caption = "مقدار نمیتواند خالی باشد.",
                    InformationIcon = true,
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            if (string.IsNullOrWhiteSpace(ComboBoxUnitQty.Text))
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "واحد مقدار را انتخاب کنید",
                    Caption = "واحد مقدار نمیتواند خالی باشد.",
                    InformationIcon = true,
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtPrice.Text) || TxtPrice.IsEnabled == false)
                TxtPrice.Text = "0";
            TxtPrice.Text = TxtPrice.Text.Replace(" ", string.Empty);
            TxtQty.Text = TxtQty.Text.Replace(" ", string.Empty);
            if (ProductTable.UpdateProductTabel(new Product()
            {
                ProductId = Pid,
                ProductName = TxtTitle.Text,
                ProductUnitPrice = int.Parse(TxtPrice.Text.Trim()),
                ProductCount = float.Parse(TxtQty.Text),
                Catalog = CatalogTable.SelectFromCatalogTable().Single(c => c.CatalogId == int.Parse(ComboBoxUnitQty.SelectedValue.ToString())),
                ProductPopularSupport = YesToPpl.IsChecked == true,
                ProductDescription = TxtDescription.Text
            }))
                Close();

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
        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textRegex = new Regex("[^0-9a-zA-Zآ-ی-]+");
            e.Handled = textRegex.IsMatch(e.Text);
        }

        private void YesToPpl_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (YesToPpl.IsChecked == true)
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
    }
}
