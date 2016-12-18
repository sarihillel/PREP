namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JoinPublicationAndReleaseCPTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReleaseCP", "PublicationCount", c => c.Int(nullable: false));
            AddColumn("dbo.ReleaseCP", "PublicationMail", c => c.String());
            AddColumn("dbo.ReleaseCP", "PublicationMailDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReleaseCP", "PublicationMailDate");
            DropColumn("dbo.ReleaseCP", "PublicationMail");
            DropColumn("dbo.ReleaseCP", "PublicationCount");
        }
    }
}
