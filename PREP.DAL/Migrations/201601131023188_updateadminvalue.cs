namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateadminvalue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionCharacteristic", "AdminValue", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionCPRevModeQ", "AdminValue", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionCPRevMode", "AdminValue", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionProduct", "AdminValue", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionFamilyProduct", "AdminValue", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionStakeholder", "AdminValue", c => c.Int(nullable: false));
           // AlterColumn("dbo.QuestionSubArea", "AdminValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.QuestionSubArea", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionStakeholder", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionFamilyProduct", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionProduct", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionCPRevMode", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionCPRevModeQ", "AdminValue", c => c.Boolean(nullable: false));
            AlterColumn("dbo.QuestionCharacteristic", "AdminValue", c => c.Boolean(nullable: false));
        }
    }
}
