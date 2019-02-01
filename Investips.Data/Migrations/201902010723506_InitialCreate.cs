namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Stock.Quote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(nullable: false),
                        High = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Low = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Close = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Open = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeStampDateTime = c.DateTime(nullable: false),
                        MovingAvg7 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MovingAvg10 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MovingAvg30 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Macd8179 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stochastics14505 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPriceCrossMovAvg30Up = c.Boolean(nullable: false),
                        IsStochCossing25Up = c.Boolean(nullable: false),
                        IsMacdCrossingHorizontalUp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Stock.Quote");
        }
    }
}
