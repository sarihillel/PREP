namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatechecklistanswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiskLevel", "Name", c => c.String());
            DropColumn("dbo.ReleaseChecklistAnswer", "QuestionOrder");
            DropColumn("dbo.RiskLevel", "RiskLevelName");
            DropColumn("dbo.ReleaseChecklistAnswerArcive", "QuestionOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReleaseChecklistAnswerArcive", "QuestionOrder", c => c.Int());
            AddColumn("dbo.RiskLevel", "RiskLevelName", c => c.String());
            AddColumn("dbo.ReleaseChecklistAnswer", "QuestionOrder", c => c.Int());
            DropColumn("dbo.RiskLevel", "Name");
        }
    }
}
