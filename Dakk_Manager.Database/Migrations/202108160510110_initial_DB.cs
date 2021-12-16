namespace Dakk_Manager.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_DB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dakk_Data",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        UploadTime = c.String(),
                        Status = c.String(nullable: false),
                        ForwardTo = c.String(),
                        Comments = c.String(),
                        DateOnLetter = c.String(),
                        DateReceived = c.String(),
                        Department = c.String(nullable: false),
                        Subject = c.String(nullable: false),
                        Givennumber = c.String(nullable: false),
                        Pages = c.Int(nullable: false),
                        Addressee = c.String(nullable: false),
                        Sectionoforigin = c.String(nullable: false),
                        Receivedby = c.String(nullable: false),
                        Pdfdirectory = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Dakk_Data");
        }
    }
}