namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatenullableinchecklist : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "AreaID" });
            AlterColumn("dbo.ReleaseChecklistAnswer", "AreaID", c => c.Int(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "AreaID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReleaseChecklistAnswer", "AreaID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area", "AreaID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "AreaID" });
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "AreaID", c => c.Int());
            AlterColumn("dbo.ReleaseChecklistAnswer", "AreaID", c => c.Int());
            CreateIndex("dbo.ReleaseChecklistAnswer", "AreaID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area", "AreaID");
        }
    }
}
