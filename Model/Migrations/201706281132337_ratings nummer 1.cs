namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingsnummer1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rate = c.Int(nullable: false),
                        UserId = c.String(),
                        Festival_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id)
                .Index(t => t.Festival_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Festival_Id", "dbo.Festivals");
            DropIndex("dbo.Ratings", new[] { "Festival_Id" });
            DropTable("dbo.Ratings");
        }
    }
}
