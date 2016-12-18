namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteSubAreaScore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" }, "dbo.AreaScore");
            DropForeignKey("dbo.SubAreaScores", "SubAreaID", "dbo.SubArea");
            DropIndex("dbo.SubAreaScores", new[] { "SubAreaID" });
            DropIndex("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" });
            DropTable("dbo.SubAreaScores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubAreaScores",
                c => new
                    {
                        SubAreaScoreID = c.Int(nullable: false, identity: true),
                        SubAreaID = c.Int(nullable: false),
                        AreaScoreID = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.SubAreaID);
            
            CreateIndex("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" });
            CreateIndex("dbo.SubAreaScores", "SubAreaID");
            AddForeignKey("dbo.SubAreaScores", "SubAreaID", "dbo.SubArea", "SubAreaID");
            AddForeignKey("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" }, "dbo.AreaScore", new[] { "ReleaseID", "AreaID" }, cascadeDelete: true);
        }
    }
}
