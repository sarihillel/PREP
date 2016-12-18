namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMDMCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employee", "MDMCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee", "MDMCode", c => c.Int());
        }
    }
}
