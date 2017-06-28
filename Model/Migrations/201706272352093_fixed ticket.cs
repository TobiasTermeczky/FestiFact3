namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedticket : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tickets");
            AddColumn("dbo.Tickets", "TicketId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Tickets", "UserId", c => c.String());
            AddColumn("dbo.Tickets", "UserName", c => c.String());
            AddColumn("dbo.Tickets", "UserEmail", c => c.String());
            AddPrimaryKey("dbo.Tickets", "TicketId");
            DropColumn("dbo.Tickets", "Barcode");
            DropColumn("dbo.Tickets", "VisitorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "VisitorId", c => c.String());
            AddColumn("dbo.Tickets", "Barcode", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Tickets");
            DropColumn("dbo.Tickets", "UserEmail");
            DropColumn("dbo.Tickets", "UserName");
            DropColumn("dbo.Tickets", "UserId");
            DropColumn("dbo.Tickets", "TicketId");
            AddPrimaryKey("dbo.Tickets", "Barcode");
        }
    }
}
