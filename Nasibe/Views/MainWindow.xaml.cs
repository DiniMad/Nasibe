using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
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
using Nasibe.Controller;
using Telerik.Windows.Controls;
using Nasibe.Views;
using Telerik.Windows.Controls.Map;
using Telerik.Windows.Data;
using WindowState = System.Windows.WindowState;


namespace Nasibe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> _allProducts = ProductTable.SelectAllProducts();
        private int _radioBtnChecked = 0;
        private readonly double[] _screenSize = new double[4];
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void FillRadGridView(object sender, RoutedEventArgs e)
        {
            GridViewMain.ItemsSource = null;
            GridViewMain.ItemsSource = _allProducts;
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var windowRemove = new RemoveWindow
            {
                WindowTitle = "حذف",
                Caption = "از حذف این مورد اطمینان دارید؟",
                OneBtn = false,
                Btn1 = "بله",
                Btn2 = "خیر"
            };
            windowRemove.ShowDialog();
            if (!windowRemove.Accept) return;
            if (fe != null) ProductTable.DeleteFromProductTable(int.Parse(fe.Uid));
            _allProducts = ProductTable.SelectAllProducts();
            FillRadGridView(null, null);
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe != null)
            {
                var editWindow = new AddOrEditWindow { Pid = int.Parse(fe.Uid) };
                editWindow.ShowDialog();
            }
            _allProducts = ProductTable.SelectAllProducts();
            FillRadGridView(null, null);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddNewProducts();
            addWindow.ShowDialog();
            _allProducts = ProductTable.SelectAllProducts();
            FillRadGridView(null, null);
        }
        private void BtnCreateDocument_Click(object sender, RoutedEventArgs e)
        {
            var documentWindow = new DocumentWindow();
            documentWindow.ShowDialog();
            _allProducts = ProductTable.SelectAllProducts();
            FillRadGridView(null, null);
        }

        private void BtnPrintDocument_Click(object sender, RoutedEventArgs e)
        {
            if (RadioBtnAllProducts.IsChecked == false && RadioBtnPplProducts.IsChecked == false && RadioBtnBoughtProducts.IsChecked == false && GridViewMain.SelectedItems.Count == 0)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "هیچ گزینه ای انتخاب نشده",
                    Caption = "موارد مورد نظرتان از لیست اقلام\nیا یکی از گزینه های پرینت را انتخاب کنید.",
                    OneBtn = true,
                    InformationIcon = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            if (RadioBtnAllProducts.IsChecked == true)
                if (GridViewMain.SelectedItems.Count > 0)
                {
                    TwoPrintOptionSelected("همه اموال");
                    return;
                }
                else
                    PrintAllDocuments();
            else if (RadioBtnBoughtProducts.IsChecked == true)
                if (GridViewMain.SelectedItems.Count > 0)
                {
                    TwoPrintOptionSelected("خریده شده");
                    return;
                }
                else
                    PrintBoughtDocuments();
            else if (RadioBtnPplProducts.IsChecked == true)
                if (GridViewMain.SelectedItems.Count > 0)
                {
                    TwoPrintOptionSelected("مردمی");
                    return;
                }
                else
                    PrintByPplDocuments();
            else if (GridViewMain.SelectedItems.Count > 0)
                PrintSelectedItems();
        }

        public void TwoPrintOptionSelected(string printOption)
        {
            var windowRemove = new RemoveWindow
            {
                WindowTitle = "دو گزینه پرینت",
                Caption = $"شما گزینه {printOption} و همچنین موردی از جدول را انتخاب\nکرده اید. لطفا یکی را از حالت انتخاب خارج کنید.",
                OneBtn = true,
                InformationIcon = true,
                Btn2 = "باشه"
            };
            windowRemove.ShowDialog();
        }
        public void PrintSelectedItems()
        {
            int dataIndex = 0;
            String[][] data = new string[GridViewMain.SelectedItems.Count][];
            foreach (var gridItem in GridViewMain.SelectedItems)
            {
                var product = gridItem as Product;
                if (product.Catalog == null)
                    product.Catalog = new Catalog { CatalogValue = String.Empty };
                data[dataIndex] = new[] { $"{product.TotalPrice:#,0}", $"{product.ProductUnitPrice:#,0}", " " + product.Catalog.CatalogValue + product.ProductCount, product.ProductName, product.ProductId.ToString() };
                dataIndex++;
            }
            PrintDocuments.AssignmentDocument(data, 0, 5);
        }
        public void PrintAllDocuments()
        {
            var empty = true;
            int dataIndex = 0;
            String[][] data = new string[_allProducts.Count][];
            foreach (var product in _allProducts)
            {
                if (product.Catalog == null)
                    product.Catalog = new Catalog { CatalogValue = string.Empty };
                data[dataIndex] = new[] { $"{product.TotalPrice:#,0}", $"{product.ProductUnitPrice:#,0}", " " + product.Catalog.CatalogValue + product.ProductCount, product.ProductName, product.ProductId.ToString() };
                dataIndex++;
                empty = false;
            }

            if (empty)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "لیست خالی است",
                    Caption = "هیچ محصولی وجود ندارد.",
                    OneBtn = true,
                    InformationIcon = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            PrintDocuments.AssignmentDocument(data, 0, 2);
        }
        public void PrintByPplDocuments()
        {
            var empty = true;
            int dataIndex = 0;
            int gapItems = 0;
            String[][] data = new string[_allProducts.Count][];
            foreach (var product in _allProducts)
            {
                if (product.Catalog == null)
                    product.Catalog = new Catalog { CatalogValue = string.Empty };
                if (product.ProductPopularSupport)
                {
                    data[dataIndex] = new[] { " " + product.Catalog.CatalogValue + product.ProductCount, product.ProductName, product.ProductId.ToString() };
                    dataIndex++;
                    empty = false;
                }
                else
                    gapItems++;
            }

            if (empty)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "لیست خالی است",
                    Caption = "محصول کمک مردمی در لیست نیست.",
                    OneBtn = true,
                    InformationIcon = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            PrintDocuments.AssignmentDocument(data, gapItems, 3);
        }
        public void PrintBoughtDocuments()
        {
            var empty = true;
            int dataIndex = 0;
            int gapItems = 0;
            String[][] data = new string[_allProducts.Count][];
            foreach (var product in _allProducts)
            {

                if (!product.ProductPopularSupport)
                {
                    if (product.Catalog == null)
                        product.Catalog = new Catalog { CatalogValue = string.Empty };
                    data[dataIndex] = new[] { $"{product.TotalPrice:#,0}", $"{product.ProductUnitPrice:#,0}", " " + product.Catalog.CatalogValue + product.ProductCount, product.ProductName, product.ProductId.ToString() };
                    dataIndex++;
                    empty = false;
                }
                else
                    gapItems++;
            }
            if (empty)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "لیست خالی است",
                    Caption = "محصول کمک مردمی در لیست نیست.",
                    OneBtn = true,
                    InformationIcon = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            PrintDocuments.AssignmentDocument(data, gapItems, 4);
        }
        private void RadioBtnPplProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_radioBtnChecked == 3)
                RadioBtnPplProducts.IsChecked = false;
            else
                _radioBtnChecked = 3;
        }
        private void RadioBtnBoughtProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_radioBtnChecked == 2)
                RadioBtnBoughtProducts.IsChecked = false;
            else
                _radioBtnChecked = 2;
        }
        private void RadioBtnAllProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_radioBtnChecked == 1)
                RadioBtnAllProducts.IsChecked = false;
            else
                _radioBtnChecked = 1;
        }

        private void WindowMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LblTitle.FontSize = WindowMain.Width / 21.5;
        }

        private void BtnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            WindowMain.WindowState = WindowMain.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
