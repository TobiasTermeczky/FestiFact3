namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedusername : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tickets", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "UserName", c => c.String());
        }
    }
}
