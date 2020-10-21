using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nasibe
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double? ProductCount { get; set; }
        public int? ProductUnitPrice { get; set; }
        public string ProductData { get; set; }
        public string ProductDescription { get; set; }
        public bool ProductPopularSupport { get; set; }
        public virtual Catalog Catalog { get; set; }
        public string ProductRoot => ProductPopularSupport ? "مردمی" : "خریداری شده";
        public double? TotalPrice => ProductUnitPrice * ProductCount;
        public string UnitPriceFormated
        {
            get
            {
                if (ProductUnitPrice > 0)
                    return $"{ProductUnitPrice:#,0}" + " ريال";
                else
                    return "اهدا شده";
            }
        }
        public string UnitPriceWithUnit
        {
            get
            {
                if (Catalog != null)
                    return ProductCount + " " + Catalog.CatalogValue;
                else
                    return ProductCount.ToString();
            }
        }
    }
}