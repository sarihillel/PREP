namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setDefaultValActualComplition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReleaseChecklistAnswer", "ActualComplation", c => c.Int(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "ActualComplation", c => c.Int(nullable: false));
            DropColumn("dbo.Question", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Question", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "ActualComplation", c => c.Int());
            AlterColumn("dbo.ReleaseChecklistAnswer", "ActualComplation", c => c.Int());
        }
    }
}
