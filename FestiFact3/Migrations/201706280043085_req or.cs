namespace FestiFact3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reqor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "OrganiserRequested", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OrganiserRequested");
        }
    }
}
