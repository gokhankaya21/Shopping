namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productimages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ImageId = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        VariantId = c.Long(nullable: false),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductImages");
        }
    }
}
