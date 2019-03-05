namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class suppliers1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Short(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Suppliers");
        }
    }
}
