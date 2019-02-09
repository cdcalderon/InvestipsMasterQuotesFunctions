namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraSignalsProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "Stochastics101", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("Stock.Quote", "IsPriceCrossMovAvg7Up", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsStoch145Cossing25Up", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsStoch101Cossing20Up", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsMovingAvg30PointingUp", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsNewLow", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsBullEight45Degreed", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "FourtyFiveDegreeLevel", c => c.Int(nullable: false));
            DropColumn("Stock.Quote", "IsStochCossing25Up");
        }
        
        public override void Down()
        {
            AddColumn("Stock.Quote", "IsStochCossing25Up", c => c.Boolean(nullable: false));
            DropColumn("Stock.Quote", "FourtyFiveDegreeLevel");
            DropColumn("Stock.Quote", "IsBullEight45Degreed");
            DropColumn("Stock.Quote", "IsNewLow");
            DropColumn("Stock.Quote", "IsMovingAvg30PointingUp");
            DropColumn("Stock.Quote", "IsStoch101Cossing20Up");
            DropColumn("Stock.Quote", "IsStoch145Cossing25Up");
            DropColumn("Stock.Quote", "IsPriceCrossMovAvg7Up");
            DropColumn("Stock.Quote", "Stochastics101");
        }
    }
}
