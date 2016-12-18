namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class www : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
