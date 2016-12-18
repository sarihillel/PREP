namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateQuestionStakeholder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuestionStakeholder", "QuestionStakeholderID");
            AddColumn("dbo.QuestionStakeholder", "QuestionStakeholderID", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
        }
    }
}
