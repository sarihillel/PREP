namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParamerType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubArea", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Characteristic", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Product", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Stakeholder", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stakeholder", "Type", c => c.Int());
            AlterColumn("dbo.Product", "Type", c => c.Int());
            AlterColumn("dbo.Characteristic", "Type", c => c.Int());
            DropColumn("dbo.SubArea", "Type");
        }
    }
}
