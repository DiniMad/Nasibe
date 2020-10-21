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
using System.Windows.Shapes;
using Nasibe.Controller;
using Nasibe.UserController;

namespace Nasibe.Views
{
    /// <summary>
    /// Interaction logic for AddNewProducts.xaml
    /// </summary>
    public partial class AddNewProducts : Window
    {
        private List<Catalog> _catalogItems = CatalogTable.SelectFromCatalogTable();
        private readonly List<Product> _allProducts = ProductTable.SelectAllProducts();
        private int _recordIndex = 1;
        public AddNewProducts()
        {
            InitializeComponent();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddNewRecorc();
            AddNewRecorc();
        }
        public void AddNewRecorc()
        {
            if (PanelProductRecord.Children.Count > 0)
            {
                var prevAddBoxItem = PanelProductRecord.Children[PanelProductRecord.Children.Count - 1] as AddBoxUC;
                prevAddBoxItem.BtnNewRecord.Visibility = Visibility.Hidden;
                prevAddBoxItem.TxtTitle.Visibility = Visibility.Visible;
                prevAddBoxItem.TxtUserQty.Visibility = Visibility.Visible;
                prevAddBoxItem.ComboBoxUnitQty.Visibility = Visibility.Visible;
                prevAddBoxItem.BtnAddCatalogItem.Visibility = Visibility.Visible;
                prevAddBoxItem.TxtPrice.Visibility = Visibility.Visible;
                prevAddBoxItem.RadioButtonBought.Visibility = Visibility.Visible;
                prevAddBoxItem.RadioButtonByPpl.Visibility = Visibility.Visible;
                prevAddBoxItem.TxtDescription.Visibility = Visibility.Visible;
                prevAddBoxItem.BtnDeleteRecord.Visibility = Visibility.Visible;
            }
            var newAddBox = new AddBoxUC();
            newAddBox.TxtTitle.ItemsSource = _allProducts;
            newAddBox.TxtDescription.Uid = _recordIndex.ToString();
            newAddBox.TxtTitle.Uid = _recordIndex.ToString();
            newAddBox.TxtUserQty.Uid = _recordIndex.ToString();
            //   newAddBox.ComboBoxUnitQty.Uid = _recordIndex.ToString();
            //   newAddBox.BtnAddCatalogItem.Uid = _recordIndex.ToString();
            //   newAddBox.TxtPrice.Uid = _recordIndex.ToString();
            //   newAddBox.RadioButtonByPpl.Uid = _recordIndex.ToString();
            //   newAddBox.RadioButtonBought.Uid = _recordIndex.ToString();
            newAddBox.BtnDeleteRecord.Uid = _recordIndex.ToString();
            newAddBox.ComboBoxUnitQty.ItemsSource = _catalogItems;
            newAddBox.ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
            newAddBox.ComboBoxUnitQty.SelectedValuePath = "CatalogId";
            newAddBox.TxtTitle.LostFocus += AutoCompleText_OnLostFocus;
            newAddBox.BtnNewRecord.Click += BtnNewRecord_Click;
            newAddBox.TxtDescription.KeyDown += TxtDescription_KeyDown;
            newAddBox.TxtUserQty.KeyDown += TxtUserQty_KeyDown;
            newAddBox.BtnDeleteRecord.Click += BtnDeleteRecord_Click;
            newAddBox.BtnAddCatalogItem.Click += BtnAddCatalogItem_Click;
            PanelProductRecord.Children.Add(newAddBox);
            _recordIndex++;
        }

        private void TxtUserQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {

                var fe = sender as FrameworkElement;
                foreach (var panelItem in PanelProductRecord.Children)
                {
                    var addBoxItem = panelItem as AddBoxUC;
                    if (int.Parse(fe.Uid) == _recordIndex - 2 && int.Parse(fe.Uid) == int.Parse(addBoxItem.TxtUserQty.Uid) && addBoxItem.TxtTitle.SelectedItem != null)
                    {
                        BtnNewRecord_Click(null, null);
                        return;
                    }
                }
            }
        }

        // hotkey tab bara qty dar halate entekhab shode
        private void AutoCompleText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            foreach (var panelItem in PanelProductRecord.Children)
            {
                var addBoxItem = panelItem as AddBoxUC;
                if (addBoxItem.TxtTitle.Uid == fe.Uid)
                {
                    if (addBoxItem.TxtTitle.SelectedItem != null)
                    {
                        var produc = addBoxItem.TxtTitle.SelectedItem as Product;
                        addBoxItem.ComboBoxUnitQty.Visibility = Visibility.Hidden;
                        addBoxItem.BtnAddCatalogItem.Visibility = Visibility.Hidden;
                        addBoxItem.TxtPrice.Visibility = Visibility.Hidden;
                        addBoxItem.RadioButtonBought.Visibility = Visibility.Hidden;
                        addBoxItem.RadioButtonByPpl.Visibility = Visibility.Hidden;
                        addBoxItem.TxtDescription.Visibility = Visibility.Hidden;
                        addBoxItem.LblId = produc.ProductId;
                        if (produc.Catalog == null)
                            produc.Catalog = new Catalog { CatalogValue = string.Empty };
                        addBoxItem.LblQtyWithUnit.Content = produc.ProductCount + " " + produc.Catalog.CatalogValue;
                        addBoxItem.LblQty.Content = produc.ProductCount.ToString();
                        addBoxItem.LblPrice.Content = $"{produc.ProductUnitPrice:#,0}" + " ريال";
                        addBoxItem.LblByPpl.Content = produc.ProductRoot;
                        addBoxItem.LblDescription.Content = produc.ProductDescription;
                        addBoxItem.LblQtyWithUnit.Visibility = Visibility.Visible;
                        addBoxItem.LblQty.Visibility = Visibility.Visible;
                        addBoxItem.LblPrice.Visibility = Visibility.Visible;
                        addBoxItem.LblByPpl.Visibility = Visibility.Visible;
                        addBoxItem.LblDescription.Visibility = Visibility.Visible;
                        continue;
                    }

                    var match = ProductTable.SelectAllProducts().Find(p => p.ProductName == addBoxItem.TxtTitle.SearchText);
                    if (match != null)
                    {
                        addBoxItem.TxtTitle.SelectedItem = match;
                        addBoxItem.ComboBoxUnitQty.Visibility = Visibility.Hidden;
                        addBoxItem.BtnAddCatalogItem.Visibility = Visibility.Hidden;
                        addBoxItem.TxtPrice.Visibility = Visibility.Hidden;
                        addBoxItem.RadioButtonBought.Visibility = Visibility.Hidden;
                        addBoxItem.RadioButtonByPpl.Visibility = Visibility.Hidden;
                        addBoxItem.TxtDescription.Visibility = Visibility.Hidden;
                        addBoxItem.LblId = match.ProductId;
                        addBoxItem.LblQtyWithUnit.Content = match.ProductCount + " " + match.Catalog.CatalogValue;
                        addBoxItem.LblQty.Content = match.ProductCount.ToString();
                        addBoxItem.LblPrice.Content = $"{match.ProductUnitPrice:#,0}" + " ريال";
                        addBoxItem.LblByPpl.Content = match.ProductRoot;
                        addBoxItem.LblDescription.Content = match.ProductDescription; addBoxItem.LblQtyWithUnit.Visibility = Visibility.Visible;
                        addBoxItem.LblQty.Visibility = Visibility.Visible;
                        addBoxItem.LblPrice.Visibility = Visibility.Visible;
                        addBoxItem.LblByPpl.Visibility = Visibility.Visible;
                        addBoxItem.LblDescription.Visibility = Visibility.Visible;
                    }
                    else if (!string.IsNullOrWhiteSpace(addBoxItem.TxtTitle.SearchText))
                    {
                        addBoxItem.LblQtyWithUnit.Visibility = Visibility.Hidden;
                        addBoxItem.LblQty.Visibility = Visibility.Hidden;
                        addBoxItem.LblPrice.Visibility = Visibility.Hidden;
                        addBoxItem.LblByPpl.Visibility = Visibility.Hidden;
                        addBoxItem.LblDescription.Visibility = Visibility.Hidden;
                        addBoxItem.ComboBoxUnitQty.Visibility = Visibility.Visible;
                        addBoxItem.BtnAddCatalogItem.Visibility = Visibility.Visible;
                        addBoxItem.TxtPrice.Visibility = Visibility.Visible;
                        addBoxItem.RadioButtonBought.Visibility = Visibility.Visible;
                        addBoxItem.RadioButtonByPpl.Visibility = Visibility.Visible;
                        addBoxItem.TxtDescription.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        private void BtnDeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (PanelProductRecord.Children.Count == 2)
                return;
            foreach (var childItem in PanelProductRecord.Children)
            {
                var AddBoxItem = childItem as AddBoxUC;
                if (AddBoxItem == null || AddBoxItem.BtnDeleteRecord.Uid != fe.Uid) continue;
                PanelProductRecord.Children.RemoveAt(PanelProductRecord.Children.IndexOf(AddBoxItem));
                return;
            }
        }
        private void BtnAddCatalogItem_Click(object sender, RoutedEventArgs e)
        {
            var catalogItemsWindow = new UnitQtyItmesWindow();
            catalogItemsWindow.ShowDialog();
            _catalogItems = CatalogTable.SelectFromCatalogTable();
            foreach (var panleItem in PanelProductRecord.Children)
            {
                var prevSelectedItem = -1;
                var AddBoxItem = panleItem as AddBoxUC;
                if (AddBoxItem.ComboBoxUnitQty.SelectedValue != null)
                {
                    prevSelectedItem = int.Parse(AddBoxItem.ComboBoxUnitQty.SelectedValue.ToString());
                }
                AddBoxItem.ComboBoxUnitQty.ItemsSource = null;
                AddBoxItem.ComboBoxUnitQty.ItemsSource = _catalogItems;
                AddBoxItem.ComboBoxUnitQty.DisplayMemberPath = "CatalogValue";
                AddBoxItem.ComboBoxUnitQty.SelectedValuePath = "CatalogId";
                foreach (var item in _catalogItems)
                {
                    if (item.CatalogId != prevSelectedItem) continue;
                    AddBoxItem.ComboBoxUnitQty.SelectedValue = prevSelectedItem;
                    break;
                }
            }

        }

        private void TxtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (e.Key == Key.Tab && int.Parse(fe.Uid) == _recordIndex - 2)
                BtnNewRecord_Click(null, null);
        }
        private void BtnNewRecord_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecorc();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var done = true;
            if (CheckForSameItem())
                return;
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.TxtTitle.SearchText) && addBoxItem.BtnNewRecord.Visibility == Visibility.Hidden)
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
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.TxtUserQty.Text) && addBoxItem.BtnNewRecord.Visibility == Visibility.Hidden)
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
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.ComboBoxUnitQty.Text) && addBoxItem.ComboBoxUnitQty.Visibility == Visibility.Visible)
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "واحد مقدار نمیتواند خالی باشد",
                        Caption = "واحد مقدار را انتخاب کنید.",
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
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.TxtPrice.Text) && addBoxItem.TxtPrice.IsEnabled == true && addBoxItem.TxtPrice.Visibility == Visibility.Visible)
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "قیمت نمیتواند خالی باشد",
                        Caption = "مقادیر قیمت را وارد کنید.",
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
                var addBoxItem = panelChild as AddBoxUC;
                if (addBoxItem.TxtTitle.SelectedItem != null)
                {
                    var selectedWarningWindow = new RemoveWindow()
                    {
                        WindowTitle = "کالا موجود است",
                        Caption = "نام کالا موجود است.\nمقدار مورد نظر به کالای موجود افزوده میشود.",
                        Btn1 = "ادامه",
                        Btn2 = "انصراف"
                    };
                    selectedWarningWindow.ShowDialog();
                    if (selectedWarningWindow.Accept)
                        break;
                    return;
                }
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                if (addBoxItem.BtnNewRecord.Visibility == Visibility.Visible)
                    break;
                var byPpl = false;
                if (addBoxItem.RadioButtonByPpl.IsChecked == true)
                {
                    addBoxItem.TxtPrice.Text = "0";
                    byPpl = true;
                }

                if (addBoxItem.TxtTitle.SelectedItem == null)
                {
                    var cata = _catalogItems.Single(c =>
                        c.CatalogId == Convert.ToInt32(addBoxItem.ComboBoxUnitQty.SelectedValue.ToString()));
                    if (!ProductTable.InsertIntoProductTable(new Product()
                    {
                        ProductName = addBoxItem.TxtTitle.SearchText,
                        ProductCount = double.Parse(addBoxItem.TxtUserQty.Text),
                        ProductUnitPrice = int.Parse(addBoxItem.TxtPrice.Text),
                        ProductPopularSupport = byPpl,
                        ProductDescription = addBoxItem.TxtDescription.Text,
                        Catalog = cata
                    }))
                        done = false;
                }
                else
                {
                    var newCount = double.Parse(addBoxItem.TxtUserQty.Text.Replace(" ", string.Empty)) + double.Parse(addBoxItem.LblQty.Content.ToString());
                    if (!ProductTable.UpdateQty(int.Parse(addBoxItem.LblId.ToString()), newCount))
                        done = false;
                }

            }
            if (!done) return;
            var windowWarning = new RemoveWindow()
            {
                WindowTitle = "موفقیت آمیز",
                Caption = "عملیات با موفقیت انجام شد.",
                InformationIcon = true,
                OneBtn = true,
                Btn2 = "باشه"
            };
            windowWarning.ShowDialog();
            ResetNameTextBoxFocus();
        }

        private void ResetNameTextBoxFocus()
        {
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                addBoxItem.TxtTitle.Focus();
            }
        }

        private void BtnPrintReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForSameItem())
                return;
            var byPpl = false;
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                if (addBoxItem.RadioButtonByPpl.IsChecked != true &&
                    addBoxItem.LblByPpl.Content.ToString() != "مردمی") continue;
                byPpl = true;
                break;
            }
            if (!byPpl)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "هیچ کالای مردمی نیست",
                    Caption = "کالای اهدایی از طرف مردم در لیست نیست.",
                    OneBtn = true,
                    InformationIcon = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return;
            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.TxtTitle.SearchText) && addBoxItem.BtnNewRecord.Visibility == Visibility.Hidden)
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "عنوان نمیتواند خالی باشد",
                        Caption = ".مقادیر عنوان را وارد کنید",
                        OneBtn = true,
                        InformationIcon = true,
                        Btn2 = "باشه"
                    };
                    windowRemove.ShowDialog();
                    return;
                }

            }
            foreach (var panelChild in PanelProductRecord.Children)
            {
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.TxtUserQty.Text) && addBoxItem.BtnNewRecord.Visibility == Visibility.Hidden)
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
                var addBoxItem = panelChild as AddBoxUC;
                if (string.IsNullOrWhiteSpace(addBoxItem.ComboBoxUnitQty.Text) & addBoxItem.BtnNewRecord.Visibility == Visibility.Hidden && string.IsNullOrWhiteSpace(addBoxItem.LblByPpl.Content.ToString()))
                {
                    var windowRemove = new RemoveWindow
                    {
                        WindowTitle = "واحد مقدار نمیتواند خالی باشد",
                        Caption = "واحد مقدار را انتخاب کنید.",
                        InformationIcon = true,
                        OneBtn = true,
                        Btn2 = "باشه"
                    };
                    windowRemove.ShowDialog();
                    return;
                }

            }
            int dataIndex = 0;
            int gapItem = 0;
            String[][] data = new string[PanelProductRecord.Children.Count][];
            foreach (var panelItem in PanelProductRecord.Children)
            {
                var addBoxItem = panelItem as AddBoxUC;
                if (addBoxItem.BtnNewRecord.Visibility == Visibility.Visible)
                {
                    gapItem++;
                    break;
                }
                if (addBoxItem.RadioButtonByPpl.IsChecked == true || addBoxItem.LblByPpl.Content.ToString() == "مردمی")
                {
                    // if (string.IsNullOrWhiteSpace(searchBoxItem.LblTotalPrice.Content.ToString()))
                    //     searchBoxItem.LblTotalPrice.Content = "1";
                    // searchBoxItem.LblQty.Content = searchBoxItem.TxtUserQty.Text.Replace(" ", String.Empty);
                    // var totalPrice = (Double.Parse(searchBoxItem.LblQty.Content.ToString()) *
                    //                   int.Parse(searchBoxItem.LblTotalPrice.Content.ToString()));
                    if (string.IsNullOrWhiteSpace(addBoxItem.ComboBoxUnitQty.Text))
                        addBoxItem.ComboBoxUnitQty.Text =
                            (addBoxItem.TxtTitle.SelectedItem as Product)?.Catalog.CatalogValue;
                    data[dataIndex] = new[] { "", "0", "0", addBoxItem.TxtUserQty.Text.Replace(" ", string.Empty), addBoxItem.ComboBoxUnitQty.Text, addBoxItem.TxtTitle.SearchText, (dataIndex + 1).ToString() };
                    dataIndex++;
                }
                else
                    gapItem++;
            }
            PrintDocuments.ReceiptDocument(data, gapItem);
        }
        private bool CheckForSameItem()
        {
            var panelItems = PanelProductRecord.Children.Cast<AddBoxUC>();
            var selectedProductNames = panelItems.Select(n => n.TxtTitle.SearchText).ToArray();
            foreach (var productName in selectedProductNames)
            {
                if (selectedProductNames.Where(n => n == productName).ToList().Count <= 1) continue;
                {
                    var twoOperetionOnOnProduct = new RemoveWindow
                    {
                        WindowTitle = "مورد مشابه",
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
    }
}
