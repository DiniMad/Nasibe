using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Nasibe.Controller;
using Nasibe.UserController;

namespace Nasibe.Views
{
    /// <summary>
    /// Interaction logic for UnitQtyItmesWindow.xaml
    /// </summary>
    public partial class UnitQtyItmesWindow : Window
    {
        private List<Catalog> _catalogItems = CatalogTable.SelectFromCatalogTable();
        private readonly CatalogItemToAdd _btnAddToCatalog = new CatalogItemToAdd();
        public bool Changed { get; set; }
        public UnitQtyItmesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContent();
        }

        public void LoadContent()
        {
            _catalogItems = CatalogTable.SelectFromCatalogTable();
            PanelCatalogItem.Children.Clear();
            _btnAddToCatalog.TxtMain.Text = "";
            foreach (var item in _catalogItems)
            {
                var btnExistingItems = new CatalogItemUC
                {
                    LabelMain = { Content = item.CatalogValue },
                    ButtonMain = { Uid = item.CatalogId.ToString() },
                    Margin = new Thickness(0, 0, 0, 10),
                };
                btnExistingItems.ButtonMain.Click += ButtonRemove_Click;
                PanelCatalogItem.Children.Add(btnExistingItems);
            }
            _btnAddToCatalog.ButtonAdd.Click += ButtonAdd_Click;
            PanelCatalogItem.Children.Add(_btnAddToCatalog);
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_btnAddToCatalog.TxtMain.Text)) return;
            if (CatalogTable.InsertIntoCatalogTable(_btnAddToCatalog.TxtMain.Text))
            {
                LoadContent();
                Changed = true;
            }
            else
                Close();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe != null && CatalogTable.DeleteFromCatalogTabel(int.Parse(fe.Uid)))
            {
                LoadContent();
                Changed = true;
            }
            else
                Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonAdd_Click(null, null);

        }
    }
}
