namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePublicationSentByID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee");
            DropIndex("dbo.Account", new[] { "PrepFPEmpID" });
            AlterColumn("dbo.Account", "PrepFPEmpID", c => c.Int());
            AlterColumn("dbo.Publication", "SentByID", c => c.Int(nullable: false));
            CreateIndex("dbo.Account", "PrepFPEmpID");
            CreateIndex("dbo.Publication", "SentByID");
            AddForeignKey("dbo.Publication", "SentByID", "dbo.Employee", "EmployeeID", cascadeDelete: true);
            AddForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee", "EmployeeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee");
            DropForeignKey("dbo.Publication", "SentByID", "dbo.Employee");
            DropIndex("dbo.Publication", new[] { "SentByID" });
            DropIndex("dbo.Account", new[] { "PrepFPEmpID" });
            AlterColumn("dbo.Publication", "SentByID", c => c.String());
            AlterColumn("dbo.Account", "PrepFPEmpID", c => c.Int(nullable: false));
            CreateIndex("dbo.Account", "PrepFPEmpID");
            AddForeignKey("dbo.Account", "PrepFPEmpID", "dbo.Employee", "EmployeeID", cascadeDelete: true);
        }
    }
}
