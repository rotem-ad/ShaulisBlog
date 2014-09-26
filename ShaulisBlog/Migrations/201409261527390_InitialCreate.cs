namespace ShaulisBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        AuthorWebSite = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        Title = c.String(),
                        Writer = c.String(),
                        WriterWebSite = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Post", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comment", new[] { "PostID" });
            DropForeignKey("dbo.Comment", "PostID", "dbo.Post");
            DropTable("dbo.Comment");
            DropTable("dbo.Post");
        }
    }
}
