namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubAreaScore1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubAreaScore",
                c => new
                    {
                        SubAreaScoreID = c.Int(nullable: false, identity: true),
                        AreaScoreID = c.Int(nullable: false),
                        SubAreaID = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.AreaScoreID, t.SubAreaID })
                .ForeignKey("dbo.AreaScore", t => t.AreaScoreID, cascadeDelete: true)
                .ForeignKey("dbo.SubArea", t => t.SubAreaID, cascadeDelete: true)
                .Index(t => t.AreaScoreID)
                .Index(t => t.SubAreaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubAreaScore", "SubAreaID", "dbo.SubArea");
            DropForeignKey("dbo.SubAreaScore", "AreaScoreID", "dbo.AreaScore");
            DropIndex("dbo.SubAreaScore", new[] { "SubAreaID" });
            DropIndex("dbo.SubAreaScore", new[] { "AreaScoreID" });
            DropTable("dbo.SubAreaScore");
        }
    }
}
