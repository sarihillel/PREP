namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsDeletedToCpQuestionSubArea : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Question", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.CP", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.SubArea", "IsDeleted", c => c.Boolean(nullable: false));
            //DropColumn("dbo.CPReviewModeQ", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CPReviewModeQ", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.SubArea", "IsDeleted");
            DropColumn("dbo.CP", "IsDeleted");
            DropColumn("dbo.Question", "IsDeleted");
        }
    }
}
