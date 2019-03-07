namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "CreateUser", c => c.String());
            AlterColumn("dbo.Products", "UpdateUser", c => c.String());
            AlterColumn("dbo.Variants", "CreateUser", c => c.String());
            AlterColumn("dbo.Variants", "UpdateUser", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Variants", "UpdateUser", c => c.Long(nullable: false));
            AlterColumn("dbo.Variants", "CreateUser", c => c.Long(nullable: false));
            AlterColumn("dbo.Products", "UpdateUser", c => c.Long(nullable: false));
            AlterColumn("dbo.Products", "CreateUser", c => c.Long(nullable: false));
        }
    }
}
