using System.Collections.Generic;

namespace Nasibe
{
    public class Catalog
    {
        public int CatalogId { get; set; }
        public string CatalogValue { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
