namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateQuestionStakeholder2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuestionStakeholder", "QuestionStakeholderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionStakeholder", "QuestionStakeholderID", c => c.Int(nullable: false));
        }
    }
}
