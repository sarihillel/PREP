namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateQuestionStakeholder3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionStakeholder", "QuestionStakeholderID", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionStakeholder", "QuestionStakeholderID");
        }
    }
}
