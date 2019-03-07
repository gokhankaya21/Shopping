namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class variantid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Short(nullable: false));
            DropColumn("dbo.Products", "CaregoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CaregoryId", c => c.Short(nullable: false));
            DropColumn("dbo.Products", "CategoryId");
        }
    }
}
