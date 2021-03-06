namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanelAllEntities_25Mar2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblApplicationUsers",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        FullName = c.String(maxLength: 150),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(),
                        EmployeeId = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsRoleActive = c.Boolean(nullable: false),
                        RoleId = c.Long(),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        BranchId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.tblBranch", t => t.BranchId, cascadeDelete: true)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.tblBranch",
                c => new
                    {
                        BranchId = c.Long(nullable: false, identity: true),
                        BranchName = c.String(),
                        ShortName = c.String(),
                        MobileNo = c.String(),
                        Email = c.String(),
                        PhoneNo = c.String(),
                        Fax = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        OrgId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.BranchId)
                .ForeignKey("dbo.tblOrganizations", t => t.OrgId, cascadeDelete: true)
                .Index(t => t.OrgId);
            
            CreateTable(
                "dbo.tblOrganizations",
                c => new
                    {
                        OrgId = c.Long(nullable: false, identity: true),
                        OrganizationName = c.String(maxLength: 150),
                        ShortName = c.String(maxLength: 50),
                        Address = c.String(maxLength: 150),
                        Email = c.String(maxLength: 150),
                        PhoneNumber = c.String(maxLength: 50),
                        MobileNumber = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        Website = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        ContractDate = c.DateTime(),
                        OrgLogoPath = c.String(maxLength: 250),
                        ReportLogoPath = c.String(maxLength: 250),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrgId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblApplicationUsers", "BranchId", "dbo.tblBranch");
            DropForeignKey("dbo.tblBranch", "OrgId", "dbo.tblOrganizations");
            DropIndex("dbo.tblBranch", new[] { "OrgId" });
            DropIndex("dbo.tblApplicationUsers", new[] { "BranchId" });
            DropTable("dbo.tblOrganizations");
            DropTable("dbo.tblBranch");
            DropTable("dbo.tblApplicationUsers");
        }
    }
}
