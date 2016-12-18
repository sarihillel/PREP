namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oops : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActivityLog", "HistoryID", "dbo.History");
            DropForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release");
            DropForeignKey("dbo.ActivityLog", "TableID", "dbo.Table");
            DropIndex("dbo.ActivityLog", new[] { "HistoryID" });
            DropIndex("dbo.ActivityLog", new[] { "TableID" });
            DropIndex("dbo.ActivityLog", new[] { "ReleaseID" });
            AddColumn("dbo.History", "ItemID", c => c.Int(nullable: false));
            AddColumn("dbo.History", "TableID", c => c.Int(nullable: false));
            AddColumn("dbo.History", "ActivityLogID", c => c.Int(nullable: false));
            AddColumn("dbo.History", "ReleaseID", c => c.Int());
            CreateIndex("dbo.History", "TableID");
            CreateIndex("dbo.History", "ActivityLogID");
            CreateIndex("dbo.History", "ReleaseID");
            AddForeignKey("dbo.History", "ActivityLogID", "dbo.ActivityLog", "ActivityLogID", cascadeDelete: true);
            AddForeignKey("dbo.History", "ReleaseID", "dbo.Release", "ReleaseID");
            AddForeignKey("dbo.History", "TableID", "dbo.Table", "TableID", cascadeDelete: true);
            DropColumn("dbo.ActivityLog", "HistoryID");
            DropColumn("dbo.ActivityLog", "TableID");
            DropColumn("dbo.ActivityLog", "ItemID");
            DropColumn("dbo.ActivityLog", "ReleaseID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityLog", "ReleaseID", c => c.Int());
            AddColumn("dbo.ActivityLog", "ItemID", c => c.Int(nullable: false));
            AddColumn("dbo.ActivityLog", "TableID", c => c.Int(nullable: false));
            AddColumn("dbo.ActivityLog", "HistoryID", c => c.Int());
            DropForeignKey("dbo.History", "TableID", "dbo.Table");
            DropForeignKey("dbo.History", "ReleaseID", "dbo.Release");
            DropForeignKey("dbo.History", "ActivityLogID", "dbo.ActivityLog");
            DropIndex("dbo.History", new[] { "ReleaseID" });
            DropIndex("dbo.History", new[] { "ActivityLogID" });
            DropIndex("dbo.History", new[] { "TableID" });
            DropColumn("dbo.History", "ReleaseID");
            DropColumn("dbo.History", "ActivityLogID");
            DropColumn("dbo.History", "TableID");
            DropColumn("dbo.History", "ItemID");
            CreateIndex("dbo.ActivityLog", "ReleaseID");
            CreateIndex("dbo.ActivityLog", "TableID");
            CreateIndex("dbo.ActivityLog", "HistoryID");
            AddForeignKey("dbo.ActivityLog", "TableID", "dbo.Table", "TableID", cascadeDelete: true);
            AddForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release", "ReleaseID");
            AddForeignKey("dbo.ActivityLog", "HistoryID", "dbo.History", "HistoryID");
        }
    }
}
