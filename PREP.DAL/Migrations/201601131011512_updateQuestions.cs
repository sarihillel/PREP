namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateQuestions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Question", "Employee_EmployeeID", "dbo.Employee");
            DropIndex("dbo.Question", new[] { "Employee_EmployeeID" });
            DropColumn("dbo.Question", "Employee_EmployeeID");

            DropForeignKey("dbo.Question", "Milestone_MilestoneID1", "dbo.Milestone");
            DropForeignKey("dbo.Question", "Milestone_MilestoneID", "dbo.Milestone");
            DropIndex("dbo.Question", new[] { "Milestone_MilestoneID1" });
            DropIndex("dbo.Question", new[] { "Milestone_MilestoneID" });
            DropColumn("dbo.Question", "Milestone_MilestoneID1");
            DropColumn("dbo.Question", "Milestone_MilestoneID");
            AddForeignKey("dbo.Question", "PreviousMilestoneID", "dbo.Milestone", "MilestoneID");
            AddColumn("dbo.Question", "QuestionCode", c => c.String());
            AddColumn("dbo.Question", "IsFocalPoint", c => c.Boolean());

        }

        public override void Down()
        {
            DropForeignKey("dbo.Question", "Employee_EmployeeID", "dbo.Employee");
            DropIndex("dbo.Question", new[] { "Employee_EmployeeID" });
            AlterColumn("dbo.Question", "IsFocalPoint", c => c.Boolean());
            AlterColumn("dbo.Question", "QuestionCode", c => c.String());
            DropColumn("dbo.Question", "Employee_EmployeeID");
        }
    }
}
