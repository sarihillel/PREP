namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQuestionArea : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.QuestionArea",
            //    c => new
            //        {
            //            QuestionAreaID = c.Int(nullable: false, identity: true),
            //            QuestionID = c.Int(nullable: false),
            //            AreaID = c.Int(nullable: false),
            //            AdminValue = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.QuestionID, t.AreaID })
            //    .ForeignKey("dbo.Area", t => t.AreaID, cascadeDelete: true)
            //    .ForeignKey("dbo.Question", t => t.QuestionID, cascadeDelete: true)
            //    .Index(t => t.QuestionID)
            //    .Index(t => t.AreaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionArea", "QuestionID", "dbo.Question");
            DropForeignKey("dbo.QuestionArea", "AreaID", "dbo.Area");
            DropIndex("dbo.QuestionArea", new[] { "AreaID" });
            DropIndex("dbo.QuestionArea", new[] { "QuestionID" });
            DropTable("dbo.QuestionArea");
        }
    }
}
