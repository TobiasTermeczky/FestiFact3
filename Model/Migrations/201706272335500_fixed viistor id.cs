namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedviistorid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "VisitorId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "VisitorId", c => c.Int(nullable: false));
        }
    }
}
