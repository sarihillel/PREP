namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreleaseactivitylog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity");
            DropPrimaryKey("dbo.Activity");
            AddColumn("dbo.ActivityLog", "ReleaseID", c => c.Int());
            AlterColumn("dbo.Activity", "ActivityID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Activity", "ActivityID");
            CreateIndex("dbo.ActivityLog", "ReleaseID");
            AddForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release", "ReleaseID");
            AddForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity", "ActivityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity");
            DropForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release");
            DropIndex("dbo.ActivityLog", new[] { "ReleaseID" });
            DropPrimaryKey("dbo.Activity");
            AlterColumn("dbo.Activity", "ActivityID", c => c.Int(nullable: false));
            DropColumn("dbo.ActivityLog", "ReleaseID");
            AddPrimaryKey("dbo.Activity", "ActivityID");
            AddForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity", "ActivityID", cascadeDelete: true);
        }
    }
}
