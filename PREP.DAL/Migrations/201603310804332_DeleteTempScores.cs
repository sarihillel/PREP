namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTempScores : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaScoreTemp", "PublicationID", "dbo.PublicationTemp");
            DropForeignKey("dbo.SubAreaScoreTemp", "AreaScoreTempID", "dbo.AreaScoreTemp");
            DropIndex("dbo.AreaScoreTemp", new[] { "PublicationID" });
            DropIndex("dbo.SubAreaScoreTemp", new[] { "AreaScoreTempID" });
            DropTable("dbo.AreaScoreTemp");
            DropTable("dbo.PublicationTemp");
            DropTable("dbo.SubAreaScoreTemp");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubAreaScoreTemp",
                c => new
                    {
                        SubAreaScoreTempID = c.Int(nullable: false, identity: true),
                        AreaScoreTempID = c.Int(nullable: false),
                        SubAreaID = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SubAreaScoreTempID);
            
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
                "dbo.AreaScoreTemp",
                c => new
                    {
                        AreaScoreTempID = c.Int(nullable: false, identity: true),
                        PublicationID = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AreaScoreTempID);
            
            CreateIndex("dbo.SubAreaScoreTemp", "AreaScoreTempID");
            CreateIndex("dbo.AreaScoreTemp", "PublicationID");
            AddForeignKey("dbo.SubAreaScoreTemp", "AreaScoreTempID", "dbo.AreaScoreTemp", "AreaScoreTempID", cascadeDelete: true);
            AddForeignKey("dbo.AreaScoreTemp", "PublicationID", "dbo.PublicationTemp", "PublicationID", cascadeDelete: true);
        }
    }
}
