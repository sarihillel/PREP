namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAreaScoreRelationship1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AreaScore", new[] { "AreaID" });
            DropIndex("dbo.AreaScore", new[] { "ReleaseID" });
            DropIndex("dbo.AreaScore", new[] { "CPID" });
            CreateIndex("dbo.AreaScore", new[] { "ReleaseID", "CPID", "AreaID" }, unique: true, name: "IX_Release_CP_Area");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AreaScore", "IX_Release_CP_Area");
            CreateIndex("dbo.AreaScore", "CPID");
            CreateIndex("dbo.AreaScore", "ReleaseID");
            CreateIndex("dbo.AreaScore", "AreaID");
        }
    }
}
