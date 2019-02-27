namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuoteProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Stock.Quote", "MovingAvg7", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "MovingAvg10", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "MovingAvg30", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "Macd8179", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "Stochastics14505", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "Stochastics101", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("Stock.Quote", "Stochastics101", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "Stochastics14505", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "Macd8179", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "MovingAvg30", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "MovingAvg10", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("Stock.Quote", "MovingAvg7", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
