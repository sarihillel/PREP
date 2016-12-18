namespace PREP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chracteristic_order_change : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Characteristic", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Characteristic", "Order", c => c.Int());
        }
    }
}
