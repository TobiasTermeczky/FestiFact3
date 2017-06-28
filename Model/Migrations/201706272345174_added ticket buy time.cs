namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedticketbuytime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "BuyDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "BuyDateTime");
        }
    }
}
