namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSuperGapBearProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsSuperGapBear", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsSuperGapBear");
        }
    }
}
