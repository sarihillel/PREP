namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrdertoSubArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubArea", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubArea", "Order");
        }
    }
}
