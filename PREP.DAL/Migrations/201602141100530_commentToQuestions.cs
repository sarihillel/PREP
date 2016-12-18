namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentToQuestions : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.QuestionProduct");
            //AddColumn("dbo.QuestionArea", "Comments", c => c.String());
            //AddColumn("dbo.QuestionCharacteristic", "Comments", c => c.String());
            //AddColumn("dbo.QuestionCPRevModeQ", "Comments", c => c.String());
            //AddColumn("dbo.QuestionCPRevMode", "Comments", c => c.String());
            //AddColumn("dbo.QuestionFamilyProduct", "Comments", c => c.String());
            //AddColumn("dbo.QuestionProduct", "Comments", c => c.String(nullable: false, maxLength: 128));
            //AddColumn("dbo.QuestionStakeholder", "Comments", c => c.String());
            //AddPrimaryKey("dbo.QuestionProduct", new[] { "QuestionID", "Comments" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.QuestionProduct");
            DropColumn("dbo.QuestionStakeholder", "Comments");
            DropColumn("dbo.QuestionProduct", "Comments");
            DropColumn("dbo.QuestionFamilyProduct", "Comments");
            DropColumn("dbo.QuestionCPRevMode", "Comments");
            DropColumn("dbo.QuestionCPRevModeQ", "Comments");
            DropColumn("dbo.QuestionCharacteristic", "Comments");
            DropColumn("dbo.QuestionArea", "Comments");
            AddPrimaryKey("dbo.QuestionProduct", new[] { "QuestionID", "ProductID" });
        }
    }
}
