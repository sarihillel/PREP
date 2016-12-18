namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSubAreaScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubAreaScores", "AreaScoreID", c => c.Int(nullable: false));
            AddColumn("dbo.SubAreaScores", "AreaID", c => c.Int(nullable: false));
            CreateIndex("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" });
            AddForeignKey("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" }, "dbo.AreaScore", new[] { "ReleaseID", "AreaID" }, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" }, "dbo.AreaScore");
            DropIndex("dbo.SubAreaScores", new[] { "AreaScoreID", "AreaID" });
            DropColumn("dbo.SubAreaScores", "AreaID");
            DropColumn("dbo.SubAreaScores", "AreaScoreID");
        }
    }
}
