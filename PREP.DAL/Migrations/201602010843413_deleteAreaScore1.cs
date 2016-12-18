namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteAreaScore1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaScore", "AreaID", "dbo.Area");
            DropForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release");
            DropIndex("dbo.AreaScore", new[] { "ReleaseID" });
            DropIndex("dbo.AreaScore", new[] { "AreaID" });
            DropTable("dbo.AreaScore");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AreaScore",
                c => new
                    {
                        AreaScoreID = c.Int(nullable: false, identity: true),
                        ReleaseID = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReleaseID, t.AreaID });
            
            CreateIndex("dbo.AreaScore", "AreaID");
            CreateIndex("dbo.AreaScore", "ReleaseID");
            AddForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release", "ReleaseID", cascadeDelete: true);
            AddForeignKey("dbo.AreaScore", "AreaID", "dbo.Area", "AreaID", cascadeDelete: true);
        }
    }
}
