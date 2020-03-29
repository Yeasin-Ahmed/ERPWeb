namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory_RepairItemStockInfoAndDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRepairStockDetails",
                c => new
                    {
                        RStockDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        RepairStockInfo_RStockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.RStockDetailId)
                .ForeignKey("dbo.tblRepairStockInfo", t => t.RepairStockInfo_RStockInfoId)
                .Index(t => t.RepairStockInfo_RStockInfoId);
            
            CreateTable(
                "dbo.tblRepairStockInfo",
                c => new
                    {
                        RStockInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RStockInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRepairStockDetails", "RepairStockInfo_RStockInfoId", "dbo.tblRepairStockInfo");
            DropIndex("dbo.tblRepairStockDetails", new[] { "RepairStockInfo_RStockInfoId" });
            DropTable("dbo.tblRepairStockInfo");
            DropTable("dbo.tblRepairStockDetails");
        }
    }
}
