namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Firstmig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Festivals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Barcode = c.String(),
                        Festival_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .Index(t => t.Festival_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Festival_Id", "dbo.Festivals");
            DropIndex("dbo.Tickets", new[] { "Festival_Id" });
            DropTable("dbo.Tickets");
            DropTable("dbo.Festivals");
            DropTable("dbo.Artists");
        }
    }
}
