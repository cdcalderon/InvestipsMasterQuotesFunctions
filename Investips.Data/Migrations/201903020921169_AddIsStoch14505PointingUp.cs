namespace Investips.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsStoch14505PointingUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("Stock.Quote", "IsStoch14505PointingUp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Stock.Quote", "IsStoch14505PointingUp");
        }
    }
}
