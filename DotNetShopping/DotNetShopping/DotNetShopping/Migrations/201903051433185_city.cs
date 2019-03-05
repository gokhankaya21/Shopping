namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class city : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Short(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(),
                        StateId = c.Short(nullable: false),
                        CountryId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropTable("dbo.Cities");
        }
    }
}
