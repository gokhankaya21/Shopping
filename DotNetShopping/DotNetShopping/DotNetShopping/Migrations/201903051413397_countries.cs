namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Short(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Short(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(nullable: false),
                        CountryId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.States", new[] { "CountryId" });
            DropTable("dbo.States");
            DropTable("dbo.Countries");
        }
    }
}
