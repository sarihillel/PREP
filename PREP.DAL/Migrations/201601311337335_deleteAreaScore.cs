namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteAreaScore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AreaScore", "AreaID", "dbo.Area");
            DropForeignKey("dbo.AreaScore", "PublicationID", "dbo.Publication");
            DropIndex("dbo.AreaScore", new[] { "PublicationID" });
            DropIndex("dbo.AreaScore", new[] { "AreaID" });
            DropTable("dbo.AreaScore");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AreaScore",
                c => new
                    {
                        AreaScrceID = c.Int(nullable: false, identity: true),
                        PublicationID = c.Int(nullable: false),
                        AreaID = c.Int(nullable: false),
                        Score = c.Int(),
                    })
                .PrimaryKey(t => new { t.PublicationID, t.AreaID });
            
            CreateIndex("dbo.AreaScore", "AreaID");
            CreateIndex("dbo.AreaScore", "PublicationID");
            AddForeignKey("dbo.AreaScore", "PublicationID", "dbo.Publication", "PublicationID", cascadeDelete: true);
            AddForeignKey("dbo.AreaScore", "AreaID", "dbo.Area", "AreaID", cascadeDelete: true);
        }
    }
}
