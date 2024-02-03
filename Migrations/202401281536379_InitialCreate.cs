namespace LibraryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Genre = c.String(maxLength: 50),
                        Author = c.String(maxLength: 50),
                        Description = c.String(maxLength: 100),
                        Copies = c.Int(nullable: false),
                        BorrowedCopies = c.Int(nullable: false),
                        Availability = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BookID);
            
            CreateTable(
                "dbo.Borrows",
                c => new
                    {
                        BorrowID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        BorrowDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BorrowID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Email = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Username = c.String(maxLength: 20),
                        Password = c.String(maxLength: 50),
                        UserType = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Borrows", "UserID", "dbo.Users");
            DropForeignKey("dbo.Borrows", "BookID", "dbo.Books");
            DropIndex("dbo.Borrows", new[] { "BookID" });
            DropIndex("dbo.Borrows", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.Borrows");
            DropTable("dbo.Books");
        }
    }
}
