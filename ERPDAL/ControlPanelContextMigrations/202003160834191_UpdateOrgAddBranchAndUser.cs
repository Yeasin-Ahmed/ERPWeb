namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrgAddBranchAndUser : DbMigration
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
            
            AddColumn("dbo.tblOrganizations", "ShortName", c => c.String(maxLength: 50));
            AddColumn("dbo.tblOrganizations", "Email", c => c.String(maxLength: 150));
            AddColumn("dbo.tblOrganizations", "PhoneNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.tblOrganizations", "MobileNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.tblOrganizations", "Fax", c => c.String(maxLength: 50));
            AddColumn("dbo.tblOrganizations", "Website", c => c.String(maxLength: 100));
            AddColumn("dbo.tblOrganizations", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tblOrganizations", "ContractDate", c => c.DateTime());
            AddColumn("dbo.tblOrganizations", "OrgLogoPath", c => c.String(maxLength: 250));
            AddColumn("dbo.tblOrganizations", "ReportLogoPath", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblApplicationUsers", "BranchId", "dbo.tblBranch");
            DropForeignKey("dbo.tblBranch", "OrgId", "dbo.tblOrganizations");
            DropIndex("dbo.tblBranch", new[] { "OrgId" });
            DropIndex("dbo.tblApplicationUsers", new[] { "BranchId" });
            DropColumn("dbo.tblOrganizations", "ReportLogoPath");
            DropColumn("dbo.tblOrganizations", "OrgLogoPath");
            DropColumn("dbo.tblOrganizations", "ContractDate");
            DropColumn("dbo.tblOrganizations", "IsActive");
            DropColumn("dbo.tblOrganizations", "Website");
            DropColumn("dbo.tblOrganizations", "Fax");
            DropColumn("dbo.tblOrganizations", "MobileNumber");
            DropColumn("dbo.tblOrganizations", "PhoneNumber");
            DropColumn("dbo.tblOrganizations", "Email");
            DropColumn("dbo.tblOrganizations", "ShortName");
            DropTable("dbo.tblBranch");
            DropTable("dbo.tblApplicationUsers");
        }
    }
}
