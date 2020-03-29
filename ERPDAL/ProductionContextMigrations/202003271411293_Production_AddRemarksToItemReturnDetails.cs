namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Production_AddRemarksToItemReturnDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblItemReturnDetail", "Remarks", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblItemReturnDetail", "Remarks");
        }
    }
}
