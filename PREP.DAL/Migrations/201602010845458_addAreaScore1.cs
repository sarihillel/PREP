namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAreaScore1 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.AreaScoreID)
                .ForeignKey("dbo.Area", t => t.AreaID, cascadeDelete: true)
                .ForeignKey("dbo.Release", t => t.ReleaseID, cascadeDelete: true)
                .Index(t => new { t.ReleaseID, t.AreaID }, unique: true, name: "IX_ReleaseIDAndAreaID");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release");
            DropForeignKey("dbo.AreaScore", "AreaID", "dbo.Area");
            DropIndex("dbo.AreaScore", "IX_ReleaseIDAndAreaID");
            DropTable("dbo.AreaScore");
        }
    }
}
