namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParameterRelathionID : DbMigration
    {
        public override void Up()
        {
           // AddColumn("dbo.Table", "ParameterRelashionID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Table", "ParameterRelashionID");
        }
    }
}
