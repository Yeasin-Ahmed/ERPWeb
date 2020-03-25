namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductionStockLineId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblProductionStockDetail", "LineId", c => c.Long());
            AddColumn("dbo.tblProductionStockInfo", "LineId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblProductionStockInfo", "LineId");
            DropColumn("dbo.tblProductionStockDetail", "LineId");
        }
    }
}
