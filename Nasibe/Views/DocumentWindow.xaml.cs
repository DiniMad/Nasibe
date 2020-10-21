using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using Nasibe.Controller;
using Spire.Doc;
using Spire.Doc.Documents;
using Telerik.Windows.Diagrams.Core;
using Nasibe.UserController;
using Telerik.Windows.Data;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Nasibe.Views
{
    /// <summary>
    /// Interaction logic for DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        private readonly List<Product> _allProducts = ProductTable.SelectAllProducts();
        private int _recordIndex = 1;
        static DateTime systemDateTime = DateTime.Now;
        static PersianCalendar persianDateTimem = new PersianCalendar();
        string persianDate = $"{persianDateTimem.GetYear(systemDateTime)}/{persianDateTimem.GetMonth(systemDateTime)}" +
                             $"/{persianDateTimem.GetDayOfMonth(systemDateTime)}";
        public DocumentWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddNewRecorc();
            AddNewRecorc();

        }
        private void BtnNewRecord_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecorc();
        }
        public void AddNewRecorc()
        {
            if (PanelProductRecord.Children.Count > 0)
            {
                var prevSearchBoxItem = PanelProductRecord.Children[PanelProductRecord.Children.Count - 1] as SearchBoxUC;
                prevSearchBoxItem.BtnNewRecord.Visibility = Visibility.Hidden;
                prevSearchBoxItem.SearchBox.Visibility = Visibility.Visible;
                prevSearchBoxItem.TxtUserQty.Visibility = Visibility.Visible;
                prevSearchBoxItem.LblQtyWithUnit.Visibility = Visibility.Visible;
                prevSearchBoxItem.LblPrice.Visibility = Visibility.Visible;
                prevSearchBoxItem.LblPpl.Visibility = Visibility.Visible;
                prevSearchBoxItem.LblStatus.Visibility = Visibility.Visible;
                prevSearchBoxItem.BtnDeleteRecord.Visibility = Visibility.Visible;
            }
            var newSearchBox = new SearchBoxUC();
            newSearchBox.SearchBox.ItemsSource = _allProducts;
            newSearchBox.SearchBox.Uid = _recordIndex.ToString();
            newSearchBox.TxtUserQty.Uid = _recordIndex.ToString();
            newSearchBox.LblQtyWithUnit.Uid = _recordIndex.ToString();
            newSearchBox.LblPrice.Uid = _recordIndex.ToString();
            newSearchBox.LblPpl.Uid = _recordIndex.ToString();
            newSearchBox.BtnDeleteRecord.Uid = _recordIndex.ToString();
            newSearchBox.SearchBox.LostFocus += AutoCompleText_OnLostFocus;
            newSearchBox.TxtUserQty.LostFocus += TxtUserQty_LostFocus;
            newSearchBox.BtnNewRecord.Click += BtnNewRecord_Click;
            newSearchBox.TxtUserQty.KeyDown += TxtUserQty_KeyDown;
            newSearchBox.BtnDeleteRecord.Click += BtnDeleteRecord_Click;
            PanelProductRecord.Children.Add(newSearchBox);
            _recordIndex++;
        }
        private void BtnDeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            foreach (var childItem in PanelProductRecord.Children)
            {
                var searchBoxItem = childItem as SearchBoxUC;
                if (searchBoxItem == null || searchBoxItem.BtnDeleteRecord.Uid != fe.Uid) continue;
                PanelProductRecord.Children.RemoveAt(PanelProductRecord.Children.IndexOf(searchBoxItem));
                return;

            }
        }
        private void TxtUserQty_KeyDown(object sender, KeyEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (e.Key == Key.Tab && int.Parse(fe.Uid) == _recordIndex - 2)
                BtnNewRecord_Click(null, null);
        }
        private void TxtUserQty_LostFocus(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            foreach (var child in PanelProductRecord.Children)
            {
                var srchBoxItem = child as SearchBoxUC;
                if (srchBoxItem != null && (!string.IsNullOrWhiteSpace(srchBoxItem.TxtUserQty.Text) && srchBoxItem.TxtUserQty.Uid == fe.Uid))
                {
                    if (srchBoxItem.SearchBox.SelectedItem != null)
                    {
                        srchBoxItem.ComboBoxUnitQty.Visibility = Visibility.Hidden;
                        srchBoxItem.BtnAddCatalogItem.Visibility = Visibility.Hidden;
                        var produc = srchBoxItem.SearchBox.SelectedItem as Product;
                        var count = produc.ProductCount - double.Parse(srchBoxItem.TxtUserQty.Text);
                        if (count >= 0)
                        {
                            srchBoxItem.LblStatus.Content = "موجود";
                            srchBoxItem.LblStatus.Foreground = Brushes.Green;
                        }
                        else
                        {
                            srchBoxItem.LblStatus.Content = "نیاز به خرید";
                            srchBoxItem.LblStatus.Foreground = Brushes.Red;
                        }
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(srchBoxItem.SearchBox.SearchText)) continue;
                    srchBoxItem.ComboBoxUnitQty.ItemsSource = null;
                    srchBoxItem.ComboBoxUnitQty.ItemsSource = CatalogTable.SelectFromCatalogTable();
                    srchBoxItem.ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
                    srchBoxItem.ComboBoxUnitQty.SelectedValuePath = "CatalogId";
                    srchBoxItem.ComboBoxUnitQty.Visibility = Visibility.Visible;
                    srchBoxItem.BtnAddCatalogItem.Visibility = Visibility.Visible;
                    srchBoxItem.BtnAddCatalogItem.Click += BtnAddCatalogItem_Click;
                    srchBoxItem.LblStatus.Content = "نیاز به خرید";
                    srchBoxItem.LblStatus.Foreground = Brushes.Red;
                    return;
                }
            }
        }

        public void ConfirmInventory()
        {
            foreach (var panelItem in PanelProductRecord.Children)
            {
                var searchBox = panelItem as SearchBoxUC;
                if (searchBox.LblStatus.Content == "نیاز به خرید")
                {
                    if (searchBox.SearchBox.SelectedItem != null)
                        searchBox.LblQtyWithUnit.Content = searchBox.TxtUserQty.Text.Replace(" ", String.Empty) + " " +
                                                           searchBox.LblCatalogValue.Content;
                    else
                        searchBox.LblQtyWithUnit.Content = searchBox.TxtUserQty.Text.Replace(" ", String.Empty) + " " +
                                                           searchBox.ComboBoxUnitQty.Text;
                    searchBox.LblStatus.Content = "موجود";
                    searchBox.LblStatus.Foreground = Brushes.Green;
                    searchBox.BtnAddCatalogItem.Visibility = Visibility.Hidden;
                    searchBox.ComboBoxUnitQty.Visibility = Visibility.Hidden;
                    searchBox.LblQtyWithUnit.Visibility = Visibility.Visible;
                }
            }
        }
        private void BtnAddCatalogItem_Click(object sender, RoutedEventArgs e)
        {
            var catalogItemsWindow = new UnitQtyItmesWindow();
            catalogItemsWindow.ShowDialog();
            var catalogItems = CatalogTable.SelectFromCatalogTable();
            foreach (var panleItem in PanelProductRecord.Children)
            {
                var searchBoxItem = panleItem as SearchBoxUC;
                searchBoxItem.ComboBoxUnitQty.ItemsSource = null;
                searchBoxItem.ComboBoxUnitQty.ItemsSource = catalogItems;
                searchBoxItem.ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
                searchBoxItem.ComboBoxUnitQty.SelectedValuePath = "CatalogId";
            }

        }
        private void AutoCompleText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var allProducts = ProductTable.SelectAllProducts();
            foreach (var panelItem in PanelProductRecord.Children)
            {
                var searchBoxItem = panelItem as SearchBoxUC;
                var match = allProducts.Find(ViewMain => ViewMain.ProductName == searchBoxItem.SearchBox.SearchText);
                if (match != null)
                    searchBoxItem.SearchBox.SelectedItem = match;
            }
            foreach (var child in PanelProductRecord.Children)
            {
                var srchBoxItem = child as SearchBoxUC;
                if (srchBoxItem.SearchBox.Uid != fe.Uid) continue;
                if (srchBoxItem.SearchBox.SelectedItem != null)
                {
                    var produc = srchBoxItem.SearchBox.SelectedItem as Product;
                    if (produc.Catalog == null)
                        produc.Catalog = new Catalog { CatalogValue = String.Empty };
                    srchBoxItem.LblQtyWithUnit.Content = produc.ProductCount + " " + produc.Catalog.CatalogValue;
                    srchBoxItem.LblQty = produc.ProductCount.ToString();
                    srchBoxItem.LblTotalPrice.Content = produc.ProductUnitPrice;
                    srchBoxItem.LblPrice.Content = $"{produc.ProductUnitPrice:#,0}" + " ريال";
                    srchBoxItem.LblPpl.Content = produc.ProductPopularSupport ? "مردمی" : "خریداری شده";
                    srchBoxItem.LblId = produc.ProductId;
                    srchBoxItem.LblStatus.Content = string.Empty;

                    srchBoxItem.LblCatalogValue.Content = produc.Catalog.CatalogValue;
                    srchBoxItem.ComboBoxUnitQty.Visibility = Visibility.Hidden;
                    srchBoxItem.BtnAddCatalogItem.Visibility = Visibility.Hidden;
                    srchBoxItem.TxtUserQty.Clear();
                }
                else
                {
                    srchBoxItem.TxtUserQty.Clear();
                    srchBoxItem.LblQtyWithUnit.Content = string.Empty;
                    srchBoxItem.LblPrice.Content = string.Empty;
                    srchBoxItem.LblPpl.Content = string.Empty;
                    srchBoxItem.LblStatus.Content = string.Empty;
                }
                return;
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnInventoryDocument_Click(object sender, RoutedEventArgs e)
        {
            bool emptyList = true;
            if (CheckForSameItem())
                return;
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var searchItem = panelChild as SearchBoxUC;
                if (!string.IsNullOrWhiteSpace(searchItem.LblStatus.Content.ToString()))
                {
                    if (searchItem.LblStatus.Content.ToString() == "نیاز به خرید")
                    {
                        var windowRemove = new RemoveWindow
                        {
                            WindowTitle = "موجودی انبار را تکمیل کنید",
                            Caption = $"موجودی \"{searchItem.SearchBox.SearchText}\" کافی نیست.",
                            InformationIcon = true,
                            OneBtn = true,
                            Btn2 = "باشه"
                        };
                        windowRemove.ShowDialog();
                        return;
                    }
                }
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var product = panelChild as SearchBoxUC;
                if (!string.IsNullOrWhiteSpace(product.LblStatus.Content.ToString()))
                {
                    emptyList = false;
                    break;
                }
            }
            if (emptyList)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "لیست خالی است",
                    Caption = "لیست خالی است.\nبرای افزودن به لیست روی جدید کلیک کنید.",
                    InformationIcon = true,
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            var windowWarning = new RemoveWindow
            {
                WindowTitle = "ایجاد حواله",
                Caption = "با ایجاد حواله مقادیر از موجودی کم میشوند.\nادامه میدهید؟",
                Btn1 = "ادامه",
                Btn2 = "انصراف"
            };
            windowWarning.ShowDialog();
            if (windowWarning.Accept)
            {

                int dataIndex = 0;
                int gapItem = 0;
                String[][] data = new string[PanelProductRecord.Children.Count][];
                foreach (var panelChild in PanelProductRecord.Children)
                {
                    var searchBoxItem = panelChild as SearchBoxUC;
                    var produc = new Product();
                    if (!string.IsNullOrWhiteSpace(searchBoxItem.LblStatus.Content.ToString()))
                    {
                        if (searchBoxItem.SearchBox.SelectedItem == null)
                        {
                            produc = ProductTable.SelectSingleProduct(searchBoxItem.SearchBox.SearchText);
                            searchBoxItem.LblId = produc.ProductId;
                        }
                        else
                            produc = ProductTable.SelectSingleProduct((searchBoxItem.SearchBox.SelectedItem as Product).ProductId);
                        if (string.IsNullOrWhiteSpace(searchBoxItem.LblTotalPrice.Content.ToString()))
                            searchBoxItem.LblTotalPrice.Content = "0";
                        searchBoxItem.LblQty = searchBoxItem.TxtUserQty.Text.Replace(" ", String.Empty);
                        var totalPrice = (Double.Parse(searchBoxItem.LblQty.ToString()) *
                                          int.Parse(searchBoxItem.LblTotalPrice.Content.ToString()));
                        data[dataIndex] = new[]
                                                {
                            $"{totalPrice:#,0}", $"{searchBoxItem.LblTotalPrice.Content:#,0}",
                            " " + produc.Catalog.CatalogValue + searchBoxItem.TxtUserQty.Text,
                            searchBoxItem.SearchBox.SearchText, searchBoxItem.LblId.ToString()
                        };
                        dataIndex++;
                        var newQty = (produc.ProductCount -
                                      double.Parse(searchBoxItem.TxtUserQty.Text.Replace(" ", String.Empty)));
                        if (newQty == 0)
                        {
                            ProductTable.DeleteFromProductTable(produc.ProductId);
                        }
                        else
                        {
                            ProductTable.UpdateProductTabel(new Product()
                            {
                                
                                ProductId = produc.ProductId,
                                ProductName = produc.ProductName,
                                ProductUnitPrice = produc.ProductUnitPrice,
                                ProductCount = newQty,
                                Catalog = CatalogTable.SelectFromCatalogTable().SingleOrDefault(c => c.CatalogId == produc.Catalog.CatalogId),
                                ProductPopularSupport = produc.ProductPopularSupport,
                                ProductDescription = produc.ProductDescription
                            });
                        }
                    }
                    else
                        gapItem++;
                }

                PrintDocuments.AssignmentDocument(data, gapItem, 1);
                PanelProductRecord.Children.Clear();
                Window_Loaded(null, null);
            }
        }
        private void BtnInventoryRequest_Click(object sender, RoutedEventArgs e)
        {
            var needRequestDocument = false;
            if (CheckForSameItem())
                return;
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var searchItem = panelChild as SearchBoxUC;
                if (searchItem.LblStatus.Content.ToString() == "نیاز به خرید")
                {
                    needRequestDocument = true;
                    break;
                }
            }
            if (!needRequestDocument)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "اقلام لیست در انبار موجود است",
                    Caption = "کسری موجودی برای درخواست خرید نیست.",
                    InformationIcon = true,
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var searchItem = panelChild as SearchBoxUC;
                if (searchItem.BtnNewRecord.Visibility == Visibility.Visible)
                    break;
                if (string.IsNullOrWhiteSpace(searchItem.SearchBox.SearchText))
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "عنوان نمیتواند خالی باشد",
                        Caption = "مقادیر عنوان را وارد کنید.",
                        InformationIcon = true,
                        OneBtn = true,
                        Btn2 = "باشه"
                    };
                    windowRemove.ShowDialog();
                    return;
                }
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var searchItem = panelChild as SearchBoxUC;
                if (searchItem.BtnNewRecord.Visibility == Visibility.Visible)
                    break;
                if (string.IsNullOrWhiteSpace(searchItem.TxtUserQty.Text))
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "مقدار نمیتواند خالی باشد",
                        Caption = "مقدار را وارد کنید.",
                        InformationIcon = true,
                        OneBtn = true,
                        Btn2 = "باشه"
                    };
                    windowRemove.ShowDialog();
                    return;
                }
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var searchItem = panelChild as SearchBoxUC;
                if (searchItem.BtnNewRecord.Visibility == Visibility.Visible)
                    break;
                if (!string.IsNullOrWhiteSpace(searchItem.LblQtyWithUnit.Content.ToString()))
                    continue;
                if (searchItem.ComboBoxUnitQty.SelectedValue == null)
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "مقدار نمیتواند خالی باشد",
                        Caption = "مقدار را وارد کنید.",
                        InformationIcon = true,
                        OneBtn = true,
                        Btn2 = "باشه"
                    };
                    windowRemove.ShowDialog();
                    return;
                }
            }
            var windowWarning = new RemoveWindow
            {
                WindowTitle = "ایجاد درخواست خرید",
                Caption = "با ایجاد درخواست خرید مقادیر به موجودی افزوده میشوند.\nادامه میدهید؟",
                Btn1 = "ادامه",
                Btn2 = "انصراف"
            };
            windowWarning.ShowDialog();
            if (windowWarning.Accept)
            {
                int dataIndex = 0;
                int gapItem = 0;
                String[][] data = new string[PanelProductRecord.Children.Count][];
                foreach (var panelChild in PanelProductRecord.Children)
                {
                    var searchBoxItem = panelChild as SearchBoxUC;


                    if (searchBoxItem.LblStatus.Content.ToString() == "نیاز به خرید")
                    {
                        var qty = searchBoxItem.TxtUserQty.Text.Replace("  ", String.Empty);
                        var produc = searchBoxItem.SearchBox.SelectedItem as Product;
                        if (searchBoxItem.SearchBox.SelectedItem != null)
                        {
                            qty = " " + produc.Catalog.CatalogValue + ((produc.ProductCount - double.Parse(searchBoxItem.TxtUserQty.Text.Replace(" ", String.Empty))) * -1);

                        }
                        else
                        {
                            qty = " " + searchBoxItem.ComboBoxUnitQty.Text + qty;
                        }
                        data[dataIndex] = new[] { "", searchBoxItem.SearchBox.SearchText, qty, (dataIndex + 1).ToString() };
                        dataIndex++;
                        if (searchBoxItem.SearchBox.SelectedItem != null)
                        {
                            ProductTable.UpdateProductTabel(new Product()
                            {
                                ProductId = produc.ProductId,
                                ProductName = produc.ProductName,
                                ProductUnitPrice = produc.ProductUnitPrice,
                                ProductCount = double.Parse(searchBoxItem.TxtUserQty.Text.Replace(" ", String.Empty)),
                                ProductPopularSupport = produc.ProductPopularSupport,
                                ProductDescription = produc.ProductDescription
                            });
                        }
                        else
                        {
                            ProductTable.InsertIntoProductTable(new Product()
                            {
                                ProductName = searchBoxItem.SearchBox.SearchText,
                                ProductCount = double.Parse(searchBoxItem.TxtUserQty.Text.Replace(" ", string.Empty)),
                                Catalog = CatalogTable.SelectFromCatalogTable().Single(c => c.CatalogId == int.Parse(searchBoxItem.ComboBoxUnitQty.SelectedValue.ToString())),
                                ProductUnitPrice = 0,
                                ProductData = persianDate,
                                ProductPopularSupport = false,
                                ProductDescription = ""
                            });
                        }
                    }
                    else
                        gapItem++;
                }
                PrintDocuments.RequestDocument(data, gapItem);
                ConfirmInventory();
            }
        }
        private bool CheckForSameItem()
        {
            var panelItems = PanelProductRecord.Children.Cast<SearchBoxUC>();
            var selectedProductNames = panelItems.Select(n => n.SearchBox.SearchText).ToArray();
            foreach (var productName in selectedProductNames)
            {
                if (selectedProductNames.Where(n => n == productName).ToList().Count <= 1) continue;
                {
                    var twoOperetionOnOnProduct = new RemoveWindow
                    {
                        WindowTitle = "تغیر مجدد",
                        Caption = $"شما کالای {productName} را دوبار انتخاب کرده اید.\nلطفا یکی را حذف کنید.",
                        OneBtn = true,
                        Btn2 = "باشه"
                    };
                    twoOperetionOnOnProduct.ShowDialog();
                    return true;
                }
            }
            return false;
        }
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
