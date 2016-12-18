namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateordercolumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Milestone", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.Stakeholder", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stakeholder", "Order", c => c.Int());
            AlterColumn("dbo.Milestone", "Order", c => c.Int());
        }
    }
}
