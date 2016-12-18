namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubAreaToReleaseChecklistAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReleaseChecklistAnswer", "SubAreaID", c => c.Int(nullable: false));
            AddColumn("dbo.ReleaseChecklistAnswerArcive", "SubAreaID", c => c.Int(nullable: false));
            CreateIndex("dbo.ReleaseChecklistAnswer", "SubAreaID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "SubAreaID", "dbo.SubArea", "SubAreaID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReleaseChecklistAnswer", "SubAreaID", "dbo.SubArea");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "SubAreaID" });
            DropColumn("dbo.ReleaseChecklistAnswerArcive", "SubAreaID");
            DropColumn("dbo.ReleaseChecklistAnswer", "SubAreaID");
        }
    }
}
