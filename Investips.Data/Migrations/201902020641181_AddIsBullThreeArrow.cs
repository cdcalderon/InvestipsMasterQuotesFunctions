namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsBullThreeArrow : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsBullThreeArrow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsBullThreeArrow");
        }
    }
}
