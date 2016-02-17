namespace WeatherParsing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.oceanWeathers", newName: "WaveWindModels");
            CreateTable(
                "dbo.OceanModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UTC = c.DateTime(nullable: false),
                        lat = c.String(),
                        lon = c.String(),
                        DENSITY = c.Double(nullable: false),
                        SSS = c.Double(nullable: false),
                        SST = c.Double(nullable: false),
                        Current_UV = c.Double(nullable: false),
                        Current_VV = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.WaveWindModels", "DENSITY");
            DropColumn("dbo.WaveWindModels", "SSS");
            DropColumn("dbo.WaveWindModels", "SST");
            DropColumn("dbo.WaveWindModels", "Current_UV");
            DropColumn("dbo.WaveWindModels", "Current_VV");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WaveWindModels", "Current_VV", c => c.Double(nullable: false));
            AddColumn("dbo.WaveWindModels", "Current_UV", c => c.Double(nullable: false));
            AddColumn("dbo.WaveWindModels", "SST", c => c.Double(nullable: false));
            AddColumn("dbo.WaveWindModels", "SSS", c => c.Double(nullable: false));
            AddColumn("dbo.WaveWindModels", "DENSITY", c => c.Double(nullable: false));
            DropTable("dbo.OceanModels");
            RenameTable(name: "dbo.WaveWindModels", newName: "oceanWeathers");
        }
    }
}
