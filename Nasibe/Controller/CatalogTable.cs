using System;
using System.Collections.Generic;
using System.Linq;
using Nasibe.Model;
using Nasibe.Views;

namespace Nasibe.Controller
{
    class CatalogTable
    {
        private static readonly NasibeContext Context = new NasibeContext();
        public static List<Catalog> SelectFromCatalogTable()
        {

            try
            {
                return Context.Catalogs.Where(c => true).ToList();
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "بارگزاری از واحد مقادیر",
                    Caption = "خطایی هنگام بارگزاری واحد های مقدار رخ داده است.\nاین پنجره را دوباره اجرا کنید.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return null;
            }

        }
        public static bool DeleteFromCatalogTabel(int pid)
        {
            try
            {
                var productShouldUpdate = Context.Products.Where(p => p.Catalog.CatalogId == pid).ToList();
                foreach (var product in productShouldUpdate)
                {
                    product.Catalog = null;
                }
                Context.Catalogs.Remove(Context.Catalogs.Single(c => c.CatalogId == pid));
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "حذف از واحد مقادیر",
                    Caption = "خطایی در هنگام حذف پیش آمده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }

        }
        public static bool InsertIntoCatalogTable(string value)
        {
            try
            {
                Context.Catalogs.Add(new Catalog { CatalogValue = value });
                Context.SaveChanges();
                return true;
            }
            catch
            {
                var windowRemove = new RemoveWindow
                {
                    WindowTitle = "حذف از واحد مقادیر",
                    Caption = "خطایی هنگام حذف رخ داده است.",
                    OneBtn = true,
                    Btn2 = "باشه"
                };
                windowRemove.ShowDialog();
                return false;
            }

        }
    }
}
