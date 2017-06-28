namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removeddetailformperf : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Performances", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Performances", "Details", c => c.String());
        }
    }
}
