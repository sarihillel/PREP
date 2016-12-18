namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MilestoneIDnotNullinQuestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Question", "MilestoneID", "dbo.Milestone");
            DropIndex("dbo.Question", new[] { "MilestoneID" });
            AlterColumn("dbo.Question", "MilestoneID", c => c.Int(nullable: false));
            CreateIndex("dbo.Question", "MilestoneID");
            AddForeignKey("dbo.Question", "MilestoneID", "dbo.Milestone", "MilestoneID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "MilestoneID", "dbo.Milestone");
            DropIndex("dbo.Question", new[] { "MilestoneID" });
            AlterColumn("dbo.Question", "MilestoneID", c => c.Int());
            CreateIndex("dbo.Question", "MilestoneID");
            AddForeignKey("dbo.Question", "MilestoneID", "dbo.Milestone", "MilestoneID");
        }
    }
}
