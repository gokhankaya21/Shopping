namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class products : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SupplierId = c.Short(nullable: false),
                        BrandId = c.Short(nullable: false),
                        CaregoryId = c.Short(nullable: false),
                        Description = c.String(),
                        Unit = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.Long(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdateUser = c.Long(nullable: false),
                        OnSale = c.Boolean(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.BrandId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropIndex("dbo.Products", new[] { "SupplierId" });
            DropTable("dbo.Products");
        }
    }
}
