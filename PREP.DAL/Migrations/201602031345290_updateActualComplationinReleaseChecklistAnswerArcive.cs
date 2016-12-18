namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateActualComplationinReleaseChecklistAnswerArcive : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReleaseChecklistAnswer", "ActualComplation", c => c.Int());
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "ActualComplation", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "ActualComplation", c => c.String());
            AlterColumn("dbo.ReleaseChecklistAnswer", "ActualComplation", c => c.String());
        }
    }
}
