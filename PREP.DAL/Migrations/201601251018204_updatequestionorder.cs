namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatequestionorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.CP", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.Question", "QuestionOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Question", "QuestionOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.CP", "Order", c => c.Int());
            DropColumn("dbo.Question", "Order");
        }
    }
}
