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

namespace Nasibe.Views
{
    /// <summary>
    /// Interaction logic for RemoveWindow.xaml
    /// </summary>
    public partial class RemoveWindow : Window
    {
        public bool Accept { get; set; } = false;
        public string WindowTitle { get; set; }
        public string Caption { get; set; }
        public bool OneBtn { get; set; } = false;
        public string Btn1 { get; set; } = "";
        public string Btn2 { get; set; }
        public bool InformationIcon { get; set; } = false;
        public RemoveWindow()
        {
            InitializeComponent();
        }
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            Accept = true;
            Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Accept = false;
            Close();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void WindowRemove_Loaded(object sender, RoutedEventArgs e)
        {
            LblTitle.Content = WindowTitle;
            LblCaption.Content = Caption;
            btnAccept.Content = Btn1;
            if (OneBtn)
            {
                btnCancel.Margin = new Thickness(0, 198, 0, 0);
                btnAccept.Visibility = Visibility.Hidden;
            }

            if (InformationIcon)
            {
                ImgIcon.Source = new BitmapImage(new Uri("..\\..\\Images\\InformationIcon.png", UriKind.Relative));
                ImgIcon.Width = ImgIcon.Height = 74;
                ImgIcon.Margin = new Thickness(10, 3, 0, 0);
            }
            btnCancel.Content = Btn2;
        }
    }
}
