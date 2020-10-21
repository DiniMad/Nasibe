namespace Nasibe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Catalogs",
                c => new
                    {
                        CatalogId = c.Int(nullable: false, identity: true),
                        CatalogValue = c.String(),
                    })
                .PrimaryKey(t => t.CatalogId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductCount = c.Double(),
                        ProductUnitCount = c.Int(),
                        ProductUnitPrice = c.Int(),
                        ProductData = c.String(),
                        ProductDescription = c.String(),
                        ProductPopularSupport = c.Boolean(nullable: false),
                        Catalog_CatalogId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Catalogs", t => t.Catalog_CatalogId)
                .Index(t => t.Catalog_CatalogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs");
            DropIndex("dbo.Products", new[] { "Catalog_CatalogId" });
            DropTable("dbo.Products");
            DropTable("dbo.Catalogs");
        }
    }
}
