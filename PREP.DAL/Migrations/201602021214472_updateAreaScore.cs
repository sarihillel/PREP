namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAreaScore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release");
            DropIndex("dbo.AreaScore", "IX_ReleaseIDAndAreaID");
            DropIndex("dbo.ReleaseCP", new[] { "ReleaseID" });
            DropIndex("dbo.ReleaseCP", new[] { "CPID" });
            RenameColumn(table: "dbo.AreaScore", name: "ReleaseID", newName: "Release_ReleaseID");
            DropPrimaryKey("dbo.ReleaseCP");
            AddColumn("dbo.AreaScore", "ReleaseCPID", c => c.Int(nullable: false));
            AlterColumn("dbo.AreaScore", "Release_ReleaseID", c => c.Int());
            AlterColumn("dbo.AreaScore", "Score", c => c.Double(nullable: false));
            AlterColumn("dbo.SubAreaScore", "Score", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.ReleaseCP", "ReleaseCPID");
            CreateIndex("dbo.AreaScore", new[] { "ReleaseCPID", "AreaID" }, unique: true, name: "IX_ReleaseCPIDAndAreaID");
            CreateIndex("dbo.AreaScore", "Release_ReleaseID");
            CreateIndex("dbo.ReleaseCP", new[] { "ReleaseID", "CPID" }, unique: true, name: "IX_ReleaseIDAndCPID");
            AddForeignKey("dbo.AreaScore", "ReleaseCPID", "dbo.ReleaseCP", "ReleaseCPID", cascadeDelete: true);
            AddForeignKey("dbo.AreaScore", "Release_ReleaseID", "dbo.Release", "ReleaseID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AreaScore", "Release_ReleaseID", "dbo.Release");
            DropForeignKey("dbo.AreaScore", "ReleaseCPID", "dbo.ReleaseCP");
            DropIndex("dbo.ReleaseCP", "IX_ReleaseIDAndCPID");
            DropIndex("dbo.AreaScore", new[] { "Release_ReleaseID" });
            DropIndex("dbo.AreaScore", "IX_ReleaseCPIDAndAreaID");
            DropPrimaryKey("dbo.ReleaseCP");
            AlterColumn("dbo.SubAreaScore", "Score", c => c.Single(nullable: false));
            AlterColumn("dbo.AreaScore", "Score", c => c.Single(nullable: false));
            AlterColumn("dbo.AreaScore", "Release_ReleaseID", c => c.Int(nullable: false));
            DropColumn("dbo.AreaScore", "ReleaseCPID");
            AddPrimaryKey("dbo.ReleaseCP", new[] { "ReleaseID", "CPID" });
            RenameColumn(table: "dbo.AreaScore", name: "Release_ReleaseID", newName: "ReleaseID");
            CreateIndex("dbo.ReleaseCP", "CPID");
            CreateIndex("dbo.ReleaseCP", "ReleaseID");
            CreateIndex("dbo.AreaScore", new[] { "ReleaseID", "AreaID" }, unique: true, name: "IX_ReleaseIDAndAreaID");
            AddForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release", "ReleaseID", cascadeDelete: true);
        }
    }
}
