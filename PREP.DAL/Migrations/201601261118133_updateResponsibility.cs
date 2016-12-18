namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateResponsibility : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReleaseAreaOwner", "Resposibility", c => c.Int(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswer", "Responsibility", c => c.Int(nullable: false));
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "Responsibility", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReleaseChecklistAnswerArcive", "Responsibility", c => c.String());
            AlterColumn("dbo.ReleaseChecklistAnswer", "Responsibility", c => c.String());
            AlterColumn("dbo.ReleaseAreaOwner", "Resposibility", c => c.String());
        }
    }
}
