namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDojiProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsDoji", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsDoji");
        }
    }
}
