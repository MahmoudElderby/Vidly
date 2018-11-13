namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingActiveRentals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rentals", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rentals", "Active");
        }
    }
}
