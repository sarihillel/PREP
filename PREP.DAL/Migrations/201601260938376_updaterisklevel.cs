namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updaterisklevel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RiskLevel", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RiskLevel", "Name", c => c.String());
        }
    }
}
