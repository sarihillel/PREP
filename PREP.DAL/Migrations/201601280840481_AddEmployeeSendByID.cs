namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeSendByID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReleaseCP", "SendByID", c => c.Int());
            AddColumn("dbo.ReleaseCP", "SendByName", c => c.String());
            CreateIndex("dbo.ReleaseCP", "SendByID");
            AddForeignKey("dbo.ReleaseCP", "SendByID", "dbo.Employee", "EmployeeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReleaseCP", "SendByID", "dbo.Employee");
            DropIndex("dbo.ReleaseCP", new[] { "SendByID" });
            DropColumn("dbo.ReleaseCP", "SendByName");
            DropColumn("dbo.ReleaseCP", "SendByID");
        }
    }
}
