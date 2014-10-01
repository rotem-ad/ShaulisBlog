namespace ShaulisBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Fan_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fan",
                c => new
                    {
                        FanID = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(),
                        Gender = c.String(),
                        BDate = c.DateTime(nullable: false),
                        Seniority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FanID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fan");
        }
    }
}
