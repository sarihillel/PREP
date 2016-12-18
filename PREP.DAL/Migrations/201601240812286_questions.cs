namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questions : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee");
            //DropIndex("dbo.ReleaseChecklistAnswer", new[] { "QuestionOwnerID" });
            //AlterColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", c => c.Int(nullable: false));
            //CreateIndex("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
            //AddForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee", "EmployeeID", cascadeDelete: true);
            //DropColumn("dbo.ReleaseChecklistAnswer", "IsActive");
            AddColumn("dbo.Question", "Employee_EmployeeID", c => c.Int());
            DropForeignKey("dbo.Question", "Employee_EmployeeID", "dbo.Employee");
            DropIndex("dbo.Question", new[] { "Employee_EmployeeID" });
            DropColumn("dbo.Question", "Employee_EmployeeID");

            //DropForeignKey("dbo.Question", "Milestone_MilestoneID1", "dbo.Milestone");
            //DropForeignKey("dbo.Question", "Milestone_MilestoneID", "dbo.Milestone");
            //DropIndex("dbo.Question", new[] { "Milestone_MilestoneID1" });
            //DropIndex("dbo.Question", new[] { "Milestone_MilestoneID" });
            //DropColumn("dbo.Question", "Milestone_MilestoneID1");
            //DropColumn("dbo.Question", "Milestone_MilestoneID");
            //AddForeignKey("dbo.Question", "PreviousMilestoneID", "dbo.Milestone", "MilestoneID");
            //AddColumn("dbo.Question", "QuestionCode", c => c.String());
            //AddColumn("dbo.Question", "IsFocalPoint", c => c.Boolean());
        }

        public override void Down()
        {
            AddColumn("dbo.ReleaseChecklistAnswer", "IsActive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "QuestionOwnerID" });
            AlterColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", c => c.Int());
            CreateIndex("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee", "EmployeeID");
        }
    }
}
