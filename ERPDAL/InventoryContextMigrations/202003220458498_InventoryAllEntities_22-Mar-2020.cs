namespace ERPDAL.InventoryContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryAllEntities_22Mar2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblItems",
                c => new
                    {
                        ItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ItemTypeId = c.Long(nullable: false),
                        UnitId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.tblItemTypes", t => t.ItemTypeId, cascadeDelete: true)
                .Index(t => t.ItemTypeId);
            
            CreateTable(
                "dbo.tblItemTypes",
                c => new
                    {
                        ItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WarehouseId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.tblWarehouses", t => t.WarehouseId, cascadeDelete: true)
                .Index(t => t.WarehouseId);
            
            CreateTable(
                "dbo.tblWarehouses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WarehouseName = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblUnits",
                c => new
                    {
                        UnitId = c.Long(nullable: false, identity: true),
                        UnitName = c.String(maxLength: 100),
                        UnitSymbol = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UnitId);
            
            CreateTable(
                "dbo.tblWarehouseStockDetails",
                c => new
                    {
                        StockDetailId = c.Long(nullable: false, identity: true),
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
                        WarehouseStockInfo_StockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.tblWarehouseStockInfo", t => t.WarehouseStockInfo_StockInfoId)
                .Index(t => t.WarehouseStockInfo_StockInfoId);
            
            CreateTable(
                "dbo.tblWarehouseStockInfo",
                c => new
                    {
                        StockInfoId = c.Long(nullable: false, identity: true),
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
                .PrimaryKey(t => t.StockInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblWarehouseStockDetails", "WarehouseStockInfo_StockInfoId", "dbo.tblWarehouseStockInfo");
            DropForeignKey("dbo.tblItems", "ItemTypeId", "dbo.tblItemTypes");
            DropForeignKey("dbo.tblItemTypes", "WarehouseId", "dbo.tblWarehouses");
            DropIndex("dbo.tblWarehouseStockDetails", new[] { "WarehouseStockInfo_StockInfoId" });
            DropIndex("dbo.tblItemTypes", new[] { "WarehouseId" });
            DropIndex("dbo.tblItems", new[] { "ItemTypeId" });
            DropTable("dbo.tblWarehouseStockInfo");
            DropTable("dbo.tblWarehouseStockDetails");
            DropTable("dbo.tblUnits");
            DropTable("dbo.tblWarehouses");
            DropTable("dbo.tblItemTypes");
            DropTable("dbo.tblItems");
        }
    }
}
