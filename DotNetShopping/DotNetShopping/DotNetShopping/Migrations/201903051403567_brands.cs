namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brands : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandId = c.Short(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BrandId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Brands");
        }
    }
}
