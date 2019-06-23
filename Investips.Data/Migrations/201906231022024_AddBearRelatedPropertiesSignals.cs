namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBearRelatedPropertiesSignals : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsPriceCrossMovAvg30Down", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsPriceCrossMovAvg7Down", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsStoch145Cossing75Down", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsStoch101Cossing80Down", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsMacdCrossingHorizontalDown", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsMovingAvg30PointingDown", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsStoch14505PointingDown", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsBearEight45Degreed", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsBearThreeArrow", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsBullStoch307", c => c.Boolean(nullable: false));
            AddColumn("Stock.Quote", "IsBearStoch307", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsBearStoch307");
            DropColumn("Stock.Quote", "IsBullStoch307");
            DropColumn("Stock.Quote", "IsBearThreeArrow");
            DropColumn("Stock.Quote", "IsBearEight45Degreed");
            DropColumn("Stock.Quote", "IsStoch14505PointingDown");
            DropColumn("Stock.Quote", "IsMovingAvg30PointingDown");
            DropColumn("Stock.Quote", "IsMacdCrossingHorizontalDown");
            DropColumn("Stock.Quote", "IsStoch101Cossing80Down");
            DropColumn("Stock.Quote", "IsStoch145Cossing75Down");
            DropColumn("Stock.Quote", "IsPriceCrossMovAvg7Down");
            DropColumn("Stock.Quote", "IsPriceCrossMovAvg30Down");
        }
    }
}
