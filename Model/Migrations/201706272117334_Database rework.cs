namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Databaserework : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Festivals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Genre = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrganizerID = c.String(),
                        MaxTicket = c.Int(nullable: false),
                        ImageLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Performances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Artist = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Details = c.String(),
                        Stage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.Stage_Id)
                .Index(t => t.Stage_Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Festival_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .Index(t => t.Festival_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Barcode = c.String(nullable: false, maxLength: 128),
                        VisitorId = c.Int(nullable: false),
                        Festival_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Barcode)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .Index(t => t.Festival_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Performances", "Stage_Id", "dbo.Stages");
            DropForeignKey("dbo.Stages", "Festival_Id", "dbo.Festivals");
            DropIndex("dbo.Tickets", new[] { "Festival_Id" });
            DropIndex("dbo.Stages", new[] { "Festival_Id" });
            DropIndex("dbo.Performances", new[] { "Stage_Id" });
            DropTable("dbo.Tickets");
            DropTable("dbo.Stages");
            DropTable("dbo.Performances");
            DropTable("dbo.Festivals");
        }
    }
}
