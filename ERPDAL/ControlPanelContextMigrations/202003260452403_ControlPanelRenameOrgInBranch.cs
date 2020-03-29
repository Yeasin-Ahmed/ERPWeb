namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanelRenameOrgInBranch : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.tblBranch", name: "OrgId", newName: "OrganizationId");
            RenameIndex(table: "dbo.tblBranch", name: "IX_OrgId", newName: "IX_OrganizationId");
            AddColumn("dbo.tblBranch", "Remarks", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblBranch", "Remarks");
            RenameIndex(table: "dbo.tblBranch", name: "IX_OrganizationId", newName: "IX_OrgId");
            RenameColumn(table: "dbo.tblBranch", name: "OrganizationId", newName: "OrgId");
        }
    }
}
