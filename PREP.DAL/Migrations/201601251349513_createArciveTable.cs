namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createArciveTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReleaseChecklistAnswerArcive",
                c => new
                    {
                        ReleaseChecklistAnswerArciveID = c.Int(nullable: false, identity: true),
                        ReleaseID = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        QuestionText = c.String(),
                        QuestionInfo = c.String(),
                        QuestionOwnerID = c.Int(),
                        HandlingStartDate = c.DateTime(),
                        ActualComplation = c.String(),
                        RiskLevelID = c.Int(nullable: false),
                        ExtrenalFocalPoint = c.String(),
                        Responsibility = c.String(),
                        Comments = c.String(),
                        LastAutomaticUpdateDate = c.DateTime(nullable: false),
                        QuestionOrder = c.Int(),
                        IsEdited = c.Boolean(nullable: false),
                        Log = c.Int(),
                        AsPlannedCounter = c.Int(),
                    })
                .PrimaryKey(t => t.ReleaseChecklistAnswerArciveID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReleaseChecklistAnswerArcive");
        }
    }
}
