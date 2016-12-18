namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSubAreaScore : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AreaScrce", newName: "AreaScore");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AreaScore", newName: "AreaScrce");
        }
    }
}
