namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteproductquestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionProduct", "ProductID", "dbo.Product");
            DropForeignKey("dbo.QuestionProduct", "QuestionID", "dbo.Question");
            DropIndex("dbo.QuestionProduct", new[] { "QuestionID" });
            DropIndex("dbo.QuestionProduct", new[] { "ProductID" });
            DropTable("dbo.QuestionProduct");
        }
        
        public override void Down()
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
                .PrimaryKey(t => new { t.QuestionID, t.ProductID });
            
            CreateIndex("dbo.QuestionProduct", "ProductID");
            CreateIndex("dbo.QuestionProduct", "QuestionID");
            AddForeignKey("dbo.QuestionProduct", "QuestionID", "dbo.Question", "QuestionID", cascadeDelete: true);
            AddForeignKey("dbo.QuestionProduct", "ProductID", "dbo.Product", "ProductID", cascadeDelete: true);
        }
    }
}
