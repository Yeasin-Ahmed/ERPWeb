namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_ItemReturn_AddUnitIdAndStateStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemReturnDetail", "UnitId", c => c.Long());
            AddColumn("dbo.tblItemReturnDetail", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.tblItemReturnInfo", "StateStatus", c => c.String(maxLength: 50));
            DropColumn("dbo.tblItemReturnDetail", "ItemQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblItemReturnDetail", "ItemQty", c => c.Int(nullable: false));
            DropColumn("dbo.tblItemReturnInfo", "StateStatus");
            DropColumn("dbo.tblItemReturnDetail", "Quantity");
            DropColumn("dbo.tblItemReturnDetail", "UnitId");
        }
    }
}
