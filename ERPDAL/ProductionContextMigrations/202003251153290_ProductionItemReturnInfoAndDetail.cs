namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductionItemReturnInfoAndDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblItemReturnDetail",
                c => new
                    {
                        IRDetailId = c.Long(nullable: false, identity: true),
                        IRCode = c.String(maxLength: 50),
                        ItemTypeId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        ItemQty = c.Int(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        IRInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.IRDetailId)
                .ForeignKey("dbo.tblItemReturnInfo", t => t.IRInfoId, cascadeDelete: true)
                .Index(t => t.IRInfoId);
            
            CreateTable(
                "dbo.tblItemReturnInfo",
                c => new
                    {
                        IRInfoId = c.Long(nullable: false, identity: true),
                        IRCode = c.String(maxLength: 50),
                        ReturnType = c.String(maxLength: 100),
                        FaultyCase = c.String(maxLength: 100),
                        LineId = c.Long(),
                        WarehouseId = c.Long(),
                        Remarks = c.String(maxLength: 100),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IRInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblItemReturnDetail", "IRInfoId", "dbo.tblItemReturnInfo");
            DropIndex("dbo.tblItemReturnDetail", new[] { "IRInfoId" });
            DropTable("dbo.tblItemReturnInfo");
            DropTable("dbo.tblItemReturnDetail");
        }
    }
}
