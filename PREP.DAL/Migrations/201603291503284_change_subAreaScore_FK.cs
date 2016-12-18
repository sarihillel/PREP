namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_subAreaScore_FK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubAreaScoreTemp", "PublicationID", "dbo.PublicationTemp");
            DropIndex("dbo.SubAreaScoreTemp", new[] { "PublicationID" });
            AddColumn("dbo.SubAreaScoreTemp", "AreaScoreTempID", c => c.Int(nullable: false));
            CreateIndex("dbo.SubAreaScoreTemp", "AreaScoreTempID");
            AddForeignKey("dbo.SubAreaScoreTemp", "AreaScoreTempID", "dbo.AreaScoreTemp", "AreaScoreTempID", cascadeDelete: true);
            DropColumn("dbo.SubAreaScoreTemp", "PublicationID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubAreaScoreTemp", "PublicationID", c => c.Int(nullable: false));
            DropForeignKey("dbo.SubAreaScoreTemp", "AreaScoreTempID", "dbo.AreaScoreTemp");
            DropIndex("dbo.SubAreaScoreTemp", new[] { "AreaScoreTempID" });
            DropColumn("dbo.SubAreaScoreTemp", "AreaScoreTempID");
            CreateIndex("dbo.SubAreaScoreTemp", "PublicationID");
            AddForeignKey("dbo.SubAreaScoreTemp", "PublicationID", "dbo.PublicationTemp", "PublicationID", cascadeDelete: true);
        }
    }
}
