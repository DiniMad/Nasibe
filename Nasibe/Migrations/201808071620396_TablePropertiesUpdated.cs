namespace Nasibe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablePropertiesUpdated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "ProductUnitCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductUnitCount", c => c.Int());
        }
    }
}
