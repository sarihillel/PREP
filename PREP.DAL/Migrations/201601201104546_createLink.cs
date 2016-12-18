namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createLink : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionSubArea", "QuestionID", "dbo.Question");
            DropForeignKey("dbo.QuestionSubArea", "SubAreaID", "dbo.SubArea");
            DropForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity");
            DropIndex("dbo.QuestionSubArea", new[] { "QuestionID" });
            DropIndex("dbo.QuestionSubArea", new[] { "SubAreaID" });
            DropPrimaryKey("dbo.Activity");
            //CreateTable(
            //    "dbo.QuestionArea",
            //    c => new
            //        {
            //            QuestionAreaID = c.Int(nullable: false, identity: true),
            //            QuestionID = c.Int(nullable: false),
            //            AreaID = c.Int(nullable: false),
            //            AdminValue = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.QuestionID, t.AreaID })
            //    .ForeignKey("dbo.Area", t => t.AreaID, cascadeDelete: true)
            //    .ForeignKey("dbo.Question", t => t.QuestionID, cascadeDelete: true)
            //    .Index(t => t.QuestionID)
            //    .Index(t => t.AreaID);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinksID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LinksID);
            
            //AddColumn("dbo.ActivityLog", "ReleaseID", c => c.Int());
            AddColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Activity", "ActivityID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Activity", "ActivityID");
           // CreateIndex("dbo.ActivityLog", "ReleaseID");
            CreateIndex("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
           // AddForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release", "ReleaseID");
            AddForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee", "EmployeeID", cascadeDelete: true);
            //AddForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity", "ActivityID", cascadeDelete: true);
           // DropColumn("dbo.Question", "PRRFPName");
            //DropColumn("dbo.Question", "Status");
           // DropTable("dbo.QuestionSubArea");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionSubArea",
                c => new
                    {
                        QuestionSubAreaID = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        SubAreaID = c.Int(nullable: false),
                        AdminValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionID, t.SubAreaID });
            
            AddColumn("dbo.Question", "Status", c => c.String());
            AddColumn("dbo.Question", "PRRFPName", c => c.String());
            DropForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity");
            DropForeignKey("dbo.ReleaseChecklistAnswer", "QuestionOwnerID", "dbo.Employee");
            DropForeignKey("dbo.ActivityLog", "ReleaseID", "dbo.Release");
            DropForeignKey("dbo.QuestionArea", "QuestionID", "dbo.Question");
            DropForeignKey("dbo.QuestionArea", "AreaID", "dbo.Area");
            DropIndex("dbo.ReleaseChecklistAnswer", new[] { "QuestionOwnerID" });
            DropIndex("dbo.ActivityLog", new[] { "ReleaseID" });
            DropIndex("dbo.QuestionArea", new[] { "AreaID" });
            DropIndex("dbo.QuestionArea", new[] { "QuestionID" });
            DropPrimaryKey("dbo.Activity");
            AlterColumn("dbo.Activity", "ActivityID", c => c.Int(nullable: false));
            DropColumn("dbo.ReleaseChecklistAnswer", "QuestionOwnerID");
            DropColumn("dbo.ActivityLog", "ReleaseID");
            DropTable("dbo.Links");
            DropTable("dbo.QuestionArea");
            AddPrimaryKey("dbo.Activity", "ActivityID");
            CreateIndex("dbo.QuestionSubArea", "SubAreaID");
            CreateIndex("dbo.QuestionSubArea", "QuestionID");
            AddForeignKey("dbo.ActivityLog", "ActivityID", "dbo.Activity", "ActivityID", cascadeDelete: true);
            AddForeignKey("dbo.QuestionSubArea", "SubAreaID", "dbo.SubArea", "SubAreaID", cascadeDelete: true);
            AddForeignKey("dbo.QuestionSubArea", "QuestionID", "dbo.Question", "QuestionID", cascadeDelete: true);
        }
    }
}
