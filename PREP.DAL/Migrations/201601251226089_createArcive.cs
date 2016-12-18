namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createArcive : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "QuestionOwnerID" });
            AlterColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", c => c.Int());
            CreateIndex("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee", "EmployeeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "QuestionOwnerID" });
            AlterColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee", "EmployeeID", cascadeDelete: true);
        }
    }
}
