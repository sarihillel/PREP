namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productComments : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.QuestionProduct");
            AlterColumn("dbo.QuestionProduct", "Comments", c => c.String());
            AddPrimaryKey("dbo.QuestionProduct", new[] { "QuestionID", "ProductID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.QuestionProduct");
            AlterColumn("dbo.QuestionProduct", "Comments", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.QuestionProduct", new[] { "QuestionID", "Comments" });
        }
    }
}
