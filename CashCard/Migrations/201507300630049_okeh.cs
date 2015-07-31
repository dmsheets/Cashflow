namespace CashCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class okeh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PengeluaranRegular = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CashFlows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Note = c.String(),
                        State = c.Int(nullable: false),
                        LogNote = c.String(),
                        CutOffId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        SuperVisorId = c.String(maxLength: 128),
                        Total = c.Int(nullable: false),
                        RegularType = c.Int(),
                        CashType = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CutOffs", t => t.CutOffId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.SuperVisorId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CutOffId)
                .Index(t => t.UserId)
                .Index(t => t.SuperVisorId);
            
            CreateTable(
                "dbo.CutOffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        BranchId = c.Int(),
                        PreviousBallance = c.Int(nullable: false),
                        EndBallance = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        BranchId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CashInDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Note = c.String(),
                        CashFlowId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashFlows", t => t.CashFlowId)
                .Index(t => t.CashFlowId);
            
            CreateTable(
                "dbo.IrregularDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IrregularType = c.Int(nullable: false),
                        FlightNo = c.String(),
                        FlightDate = c.DateTime(nullable: false),
                        FromTo = c.String(),
                        Amount = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Note = c.String(),
                        SubTotal = c.Int(nullable: false),
                        CashFlowId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashFlows", t => t.CashFlowId)
                .Index(t => t.CashFlowId);
            
            CreateTable(
                "dbo.RegularDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegularQuizId = c.Int(),
                        Note1 = c.String(),
                        Note2 = c.String(),
                        Amount = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        SubTotal = c.Int(nullable: false),
                        CashFlowId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashFlows", t => t.CashFlowId)
                .ForeignKey("dbo.RegularQuizs", t => t.RegularQuizId)
                .Index(t => t.RegularQuizId)
                .Index(t => t.CashFlowId);
            
            CreateTable(
                "dbo.RegularQuizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quiz = c.String(nullable: false),
                        RegularGroupId = c.Int(),
                        RegularType = c.Int(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegularGroups", t => t.RegularGroupId)
                .Index(t => t.RegularGroupId);
            
            CreateTable(
                "dbo.RegularGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountNo = c.String(nullable: false),
                        AccountDesription = c.String(nullable: false),
                        GroupType = c.Int(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegularDetails", "RegularQuizId", "dbo.RegularQuizs");
            DropForeignKey("dbo.RegularQuizs", "RegularGroupId", "dbo.RegularGroups");
            DropForeignKey("dbo.RegularDetails", "CashFlowId", "dbo.CashFlows");
            DropForeignKey("dbo.IrregularDetails", "CashFlowId", "dbo.CashFlows");
            DropForeignKey("dbo.CashInDetails", "CashFlowId", "dbo.CashFlows");
            DropForeignKey("dbo.CashFlows", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CashFlows", "SuperVisorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.CashFlows", "CutOffId", "dbo.CutOffs");
            DropForeignKey("dbo.CutOffs", "BranchId", "dbo.Branches");
            DropIndex("dbo.RegularQuizs", new[] { "RegularGroupId" });
            DropIndex("dbo.RegularDetails", new[] { "CashFlowId" });
            DropIndex("dbo.RegularDetails", new[] { "RegularQuizId" });
            DropIndex("dbo.IrregularDetails", new[] { "CashFlowId" });
            DropIndex("dbo.CashInDetails", new[] { "CashFlowId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "BranchId" });
            DropIndex("dbo.CutOffs", new[] { "BranchId" });
            DropIndex("dbo.CashFlows", new[] { "SuperVisorId" });
            DropIndex("dbo.CashFlows", new[] { "UserId" });
            DropIndex("dbo.CashFlows", new[] { "CutOffId" });
            DropTable("dbo.RegularGroups");
            DropTable("dbo.RegularQuizs");
            DropTable("dbo.RegularDetails");
            DropTable("dbo.IrregularDetails");
            DropTable("dbo.CashInDetails");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CutOffs");
            DropTable("dbo.CashFlows");
            DropTable("dbo.Branches");
        }
    }
}
