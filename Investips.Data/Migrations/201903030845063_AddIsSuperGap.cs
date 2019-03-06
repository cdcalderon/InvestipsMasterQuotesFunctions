namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSuperGap : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsSuperGap", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsSuperGap");
        }
    }
}
