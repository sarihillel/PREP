namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLastAutomaticUpdateDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReleaseChecklistAnswer", "LastAutomaticUpdateDate", c => c.DateTime());
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "LastAutomaticUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "LastAutomaticUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswer", "LastAutomaticUpdateDate", c => c.DateTime(nullable: false));
        }
    }
}
