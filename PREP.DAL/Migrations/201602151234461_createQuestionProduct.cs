namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createQuestionProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionProduct",
                c => new
                    {
                        QuestionProductID = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        AdminValue = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => new { t.QuestionID, t.ProductID })
                .ForeignKey("dbo.Product", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Question", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionProduct", "QuestionID", "dbo.Question");
            DropForeignKey("dbo.QuestionProduct", "ProductID", "dbo.Product");
            DropIndex("dbo.QuestionProduct", new[] { "ProductID" });
            DropIndex("dbo.QuestionProduct", new[] { "QuestionID" });
            DropTable("dbo.QuestionProduct");
        }
    }
}
