namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScoresTempTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaScoreTemps",
                c => new
                    {
                        AreaScoreTempID = c.Int(nullable: false, identity: true),
                        PublicationID = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AreaScoreTempID)
                .ForeignKey("dbo.PublicationTemps", t => t.PublicationID, cascadeDelete: true)
                .Index(t => t.PublicationID);
            
            CreateTable(
                "dbo.PublicationTemp",
                c => new
                    {
                        PublicationID = c.Int(nullable: false, identity: true),
                        ReleaseID = c.Int(nullable: false),
                        CPID = c.Int(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationID);
            
            CreateTable(
                "dbo.SubAreaScoreTemp",
                c => new
                    {
                        SubAreaScoreTempID = c.Int(nullable: false, identity: true),
                        PublicationID = c.Int(nullable: false),
                        SubAreaID = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SubAreaScoreTempID)
                .ForeignKey("dbo.PublicationTemps", t => t.PublicationID, cascadeDelete: true)
                .Index(t => t.PublicationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubAreaScoreTemps", "PublicationID", "dbo.PublicationTemps");
            DropForeignKey("dbo.AreaScoreTemps", "PublicationID", "dbo.PublicationTemps");
            DropIndex("dbo.SubAreaScoreTemps", new[] { "PublicationID" });
            DropIndex("dbo.AreaScoreTemps", new[] { "PublicationID" });
            DropTable("dbo.SubAreaScoreTemps");
            DropTable("dbo.PublicationTemps");
            DropTable("dbo.AreaScoreTemps");
        }
    }
}
