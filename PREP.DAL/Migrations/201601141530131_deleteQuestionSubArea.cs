namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteQuestionSubArea : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.QuestionSubArea", "QuestionID", "dbo.Question");
            //DropForeignKey("dbo.QuestionSubArea", "SubAreaID", "dbo.SubArea");
            //DropIndex("dbo.QuestionSubArea", new[] { "QuestionID" });
            //DropIndex("dbo.QuestionSubArea", new[] { "SubAreaID" });
            //DropTable("dbo.QuestionSubArea");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionSubArea",
                c => new
                    {
                        QuestionSubAreaID = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        SubAreaID = c.Int(nullable: false),
                        AdminValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionID, t.SubAreaID });
            
            CreateIndex("dbo.QuestionSubArea", "SubAreaID");
            CreateIndex("dbo.QuestionSubArea", "QuestionID");
            AddForeignKey("dbo.QuestionSubArea", "SubAreaID", "dbo.SubArea", "SubAreaID", cascadeDelete: true);
            AddForeignKey("dbo.QuestionSubArea", "QuestionID", "dbo.Question", "QuestionID", cascadeDelete: true);
        }
    }
}
