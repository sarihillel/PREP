namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionemloyee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "Employee_EmployeeID", c => c.Int());
            DropForeignKey("dbo.Question", "Employee_EmployeeID", "dbo.Employee");
            DropIndex("dbo.Question", new[] { "Employee_EmployeeID" });
            DropColumn("dbo.Question", "Employee_EmployeeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Question", "Employee_EmployeeID", c => c.Int());
            CreateIndex("dbo.Question", "Employee_EmployeeID");
            AddForeignKey("dbo.Question", "Employee_EmployeeID", "dbo.Employee", "EmployeeID");
        }
    }
}
