namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Account", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Account", "Name", c => c.String(maxLength: 20));
        }
    }
}
