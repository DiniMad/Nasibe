using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nasibe.Controller;
using Nasibe.Model;
using Nasibe.Views;
using Int32 = System.Int32;

namespace Nasibe.Controller
{
    class ProductTable
    {
        private static readonly NasibeContext Context = new NasibeContext();
        public static List<Product> SelectAllProducts()
        {

            try
            {
                return Context.Products.Where(p => true).ToList();
            }
            catch (Exception e)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بارگزاری همه اموال",
                    Caption = "خطایی هنگام بارگزاری همه اموال رخ داده است.\nبرنامه را دوباره اجرا کنید.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return null;
            }
        }
        public static Product SelectSingleProduct(string productName)
        {

            try
            {
                return Context.Products.Single(p => p.ProductName == productName);
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بارگزاری از اموال",
                    Caption = "خطایی هنگام بارگزاری یکی از اموال رخ داده است.\nبرنامه را دوباره اجرا کنید.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return null;
            }

        }
        public static Product SelectSingleProduct(int productId)
        {
            try
            {
                return Context.Products.Single(p => p.ProductId == productId);
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بارگزاری از اموال",
                    Caption = "خطایی هنگام بارگزاری یکی از اموال رخ داده است.\nبرنامه را دوباره اجرا کنید.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return null;
            }
        }
        public static bool DeleteFromProductTable(int pid)
        {

            try
            {
                var g = Context.Products.Single(p => p.ProductId == pid);
                Context.Products.Remove(g);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "حذف از اموال",
                    Caption = "خطایی در هنگام حذف پیش آمده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }
        }
        public static bool UpdateProductTabel(Product product)
        {
            var date = DateTime.Now;
            var persianDate = new PersianCalendar();
            var productDate = $"{persianDate.GetYear(date)}/{persianDate.GetMonth(date)}" +
                       $"/{persianDate.GetDayOfMonth(date)}";
            var toppl = product.ProductPopularSupport ? 1 : 0;
            try
            {
                var productToUpdate = Context.Products.Single(p => p.ProductId == product.ProductId);
                productToUpdate.ProductName = product.ProductName;
                productToUpdate.ProductCount = product.ProductCount;
                productToUpdate.ProductUnitPrice = product.ProductUnitPrice;
                productToUpdate.ProductDescription = product.ProductDescription;
                productToUpdate.ProductPopularSupport = toppl == 1;
                productToUpdate.ProductData = productDate;
                if (product.Catalog != null)
                    productToUpdate.Catalog = Context.Catalogs.Single(c => c.CatalogId == product.Catalog.CatalogId);
                Context.SaveChanges();
                return true;
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بروزرسانی اموال",
                    Caption = "خطایی هنگام ویرایش  رخ داده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }
        }
        public static bool UpdateQty(int pid, double count)
        {
            var date = DateTime.Now;
            var persianDate = new PersianCalendar();
            var productDate = $"{persianDate.GetYear(date)}/{persianDate.GetMonth(date)}" +
                              $"/{persianDate.GetDayOfMonth(date)}";
            try
            {
                var product = Context.Products.Single(p => p.ProductId == pid);
                product.ProductCount = count;
                product.ProductData = productDate;
                if (string.IsNullOrWhiteSpace(product.Catalog.CatalogValue))
                    product.Catalog = null;
                Context.SaveChanges();
                return true;
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بروزرسانی اموال",
                    Caption = "خطایی هنگام ویرایش  رخ داده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }
        }
        public static bool InsertIntoProductTable(Product produc)
        {
            var date = DateTime.Now;
            var persianDate = new PersianCalendar();
            var productDate = $"{persianDate.GetYear(date)}/{persianDate.GetMonth(date)}" +
                              $"/{persianDate.GetDayOfMonth(date)}";
            try
            {
                Context.Products.Add(new Product()
                {
                    ProductName = produc.ProductName,
                    ProductCount = (int?)produc.ProductCount,
                    ProductUnitPrice = produc.ProductUnitPrice,
                    ProductPopularSupport = produc.ProductPopularSupport,
                    ProductDescription = produc.ProductDescription,
                    ProductData = productDate,
                    Catalog = Context.Catalogs.Single(c => c.CatalogId == produc.Catalog.CatalogId)

                });
                Context.SaveChanges();
                return true;
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "ثبت در اموال",
                    Caption = "خطایی هنگام ثبت رخ داده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }
        }
    }
}
