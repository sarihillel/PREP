namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addParamerType2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Area", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.SubArea", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubArea", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Area", "Type", c => c.Int());
        }
    }
}
