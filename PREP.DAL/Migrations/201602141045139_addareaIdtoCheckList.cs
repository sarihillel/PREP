namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addareaIdtoCheckList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReleaseChecklistAnswer", "AreaID", c => c.Int());
            AddColumn("dbo.ReleaseChecklistAnswerArcive", "AreaID", c => c.Int());
            CreateIndex("dbo.ReleaseChecklistAnswer", "AreaID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area", "AreaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "AreaID", "dbo.Area");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "AreaID" });
            DropColumn("dbo.ReleaseChecklistAnswerArcive", "AreaID");
            DropColumn("dbo.ReleaseChecklistAnswer", "AreaID");
        }
    }
}
