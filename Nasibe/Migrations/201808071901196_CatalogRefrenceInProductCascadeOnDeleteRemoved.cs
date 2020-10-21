namespace Nasibe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatalogRefrenceInProductCascadeOnDeleteRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs");
            AddForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs", "CatalogId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs");
            AddForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs", "CatalogId", cascadeDelete: true);
        }
    }
}
