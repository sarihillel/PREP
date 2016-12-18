namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAreaScoreRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaScore", "ReleaseCPID", "dbo.ReleaseCP");
            DropForeignKey("dbo.AreaScore", "Release_ReleaseID", "dbo.Release");
            DropIndex("dbo.AreaScore", "IX_ReleaseCPIDAndAreaID");
            DropIndex("dbo.AreaScore", new[] { "Release_ReleaseID" });
            DropIndex("dbo.ReleaseCP", "IX_ReleaseIDAndCPID");
            RenameColumn(table: "dbo.AreaScore", name: "Release_ReleaseID", newName: "ReleaseID");
            DropPrimaryKey("dbo.ReleaseCP");
            AddColumn("dbo.AreaScore", "CPID", c => c.Int(nullable: false));
            AlterColumn("dbo.AreaScore", "ReleaseID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ReleaseCP", new[] { "ReleaseID", "CPID" });
            CreateIndex("dbo.AreaScore", "AreaID");
            CreateIndex("dbo.AreaScore", "ReleaseID");
            CreateIndex("dbo.AreaScore", "CPID");
            CreateIndex("dbo.ReleaseCP", "ReleaseID");
            CreateIndex("dbo.ReleaseCP", "CPID");
            AddForeignKey("dbo.AreaScore", "CPID", "dbo.CP", "CPID", cascadeDelete: true);
            AddForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release", "ReleaseID", cascadeDelete: true);
            DropColumn("dbo.AreaScore", "ReleaseCPID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AreaScore", "ReleaseCPID", c => c.Int(nullable: false));
            DropForeignKey("dbo.AreaScore", "ReleaseID", "dbo.Release");
            DropForeignKey("dbo.AreaScore", "CPID", "dbo.CP");
            DropIndex("dbo.ReleaseCP", new[] { "CPID" });
            DropIndex("dbo.ReleaseCP", new[] { "ReleaseID" });
            DropIndex("dbo.AreaScore", new[] { "CPID" });
            DropIndex("dbo.AreaScore", new[] { "ReleaseID" });
            DropIndex("dbo.AreaScore", new[] { "AreaID" });
            DropPrimaryKey("dbo.ReleaseCP");
            AlterColumn("dbo.AreaScore", "ReleaseID", c => c.Int());
            DropColumn("dbo.AreaScore", "CPID");
            AddPrimaryKey("dbo.ReleaseCP", "ReleaseCPID");
            RenameColumn(table: "dbo.AreaScore", name: "ReleaseID", newName: "Release_ReleaseID");
            CreateIndex("dbo.ReleaseCP", new[] { "ReleaseID", "CPID" }, unique: true, name: "IX_ReleaseIDAndCPID");
            CreateIndex("dbo.AreaScore", "Release_ReleaseID");
            CreateIndex("dbo.AreaScore", new[] { "ReleaseCPID", "AreaID" }, unique: true, name: "IX_ReleaseCPIDAndAreaID");
            AddForeignKey("dbo.AreaScore", "Release_ReleaseID", "dbo.Release", "ReleaseID");
            AddForeignKey("dbo.AreaScore", "ReleaseCPID", "dbo.ReleaseCP", "ReleaseCPID", cascadeDelete: true);
        }
    }
}
