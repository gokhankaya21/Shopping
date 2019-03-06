namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class variant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Variants",
                c => new
                    {
                        VariantId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProductId = c.Long(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Short(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.Long(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdateUser = c.Long(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VariantId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Variants", "ProductId", "dbo.Products");
            DropIndex("dbo.Variants", new[] { "ProductId" });
            DropTable("dbo.Variants");
        }
    }
}
