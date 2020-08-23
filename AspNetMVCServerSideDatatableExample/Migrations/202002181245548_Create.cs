namespace AspNetMVCServerSideDatatableExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RowGuid = c.Guid(nullable: false),
                        first_name = c.String(maxLength: 50),
                        last_name = c.String(maxLength: 50),
                        email = c.String(maxLength: 50),
                        gender = c.String(maxLength: 50),
                        ip_address = c.String(maxLength: 20),
                        is_active = c.Boolean(nullable: false),
                        date_time = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
