namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmployeeToAccount : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Account", "PrepFPEmpID");
            AddForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee", "EmployeeID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee");
            DropIndex("dbo.Account", new[] { "PrepFPEmpID" });
        }
    }
}
