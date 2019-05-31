namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campaigns1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PromoCodes");
            AddColumn("dbo.PromoCodes", "Code", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.PromoCodes", "Code");
            DropColumn("dbo.PromoCodes", "Promo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoCodes", "Promo", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.PromoCodes");
            DropColumn("dbo.PromoCodes", "Code");
            AddPrimaryKey("dbo.PromoCodes", "Promo");
        }
    }
}
