namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIsVisibleToIsDeleted : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Area", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Milestone", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ReleaseCP", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Characteristic", "IsDeleted", c => c.Boolean(nullable: false));
            //// AddColumn("dbo.CPReviewModeQ", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.CPReviewMode", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.FamilyProduct", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Product", "IsDeleted", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Stakeholder", "IsDeleted", c => c.Boolean(nullable: false));
            //DropColumn("dbo.Area", "isVisible");
            //DropColumn("dbo.Milestone", "IsVisible");
            //DropColumn("dbo.ReleaseCP", "IsVisible");
            //DropColumn("dbo.Characteristic", "IsVisible");
            ////DropColumn("dbo.CPReviewModeQ", "IsVisible");
            //DropColumn("dbo.CPReviewMode", "IsVisible");
            //DropColumn("dbo.FamilyProduct", "IsVisible");
            //DropColumn("dbo.Product", "IsVisible");
            //DropColumn("dbo.Stakeholder", "IsVisible");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stakeholder", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.FamilyProduct", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.CPReviewMode", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.CPReviewModeQ", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Characteristic", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReleaseCP", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Milestone", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Area", "isVisible", c => c.Boolean(nullable: false));
            DropColumn("dbo.Stakeholder", "IsDeleted");
            DropColumn("dbo.Product", "IsDeleted");
            DropColumn("dbo.FamilyProduct", "IsDeleted");
            DropColumn("dbo.CPReviewMode", "IsDeleted");
            DropColumn("dbo.CPReviewModeQ", "IsDeleted");
            DropColumn("dbo.Characteristic", "IsDeleted");
            DropColumn("dbo.ReleaseCP", "IsDeleted");
            DropColumn("dbo.Milestone", "IsDeleted");
            DropColumn("dbo.Area", "IsDeleted");
        }
    }
}
