namespace CashCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inisialize : DbMigration
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
                "dbo.CashCards",
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
                        CostCenter = c.Int(),
                        TypeOut = c.Int(),
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
                        CashInId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashCards", t => t.CashInId)
                .Index(t => t.CashInId);
            
            CreateTable(
                "dbo.CashOutDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuizId = c.Int(),
                        Note1 = c.String(),
                        Note2 = c.String(),
                        DateInfo = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        SubTotal = c.Int(nullable: false),
                        CashOutId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashCards", t => t.CashOutId)
                .ForeignKey("dbo.Quizs", t => t.QuizId)
                .Index(t => t.QuizId)
                .Index(t => t.CashOutId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Info = c.String(nullable: false),
                        QuizGroupId = c.Int(),
                        CostCenter = c.Int(nullable: false),
                        Note1Label = c.String(),
                        Note2Label = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizGroups", t => t.QuizGroupId)
                .Index(t => t.QuizGroupId);
            
            CreateTable(
                "dbo.QuizGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountNo = c.String(nullable: false),
                        AccountDesription = c.String(nullable: false),
                        GroupType = c.Int(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IrregularDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IrregularType = c.Int(nullable: false),
                        FlightNo = c.String(),
                        FromTo = c.String(),
                        FlightDate = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Note = c.String(),
                        SubTotal = c.Int(nullable: false),
                        CashFlowId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CashCards", t => t.CashFlowId)
                .Index(t => t.CashFlowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IrregularDetails", "CashFlowId", "dbo.CashCards");
            DropForeignKey("dbo.CashOutDetails", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Quizs", "QuizGroupId", "dbo.QuizGroups");
            DropForeignKey("dbo.CashOutDetails", "CashOutId", "dbo.CashCards");
            DropForeignKey("dbo.CashInDetails", "CashInId", "dbo.CashCards");
            DropForeignKey("dbo.CashCards", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CashCards", "SuperVisorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.CashCards", "CutOffId", "dbo.CutOffs");
            DropForeignKey("dbo.CutOffs", "BranchId", "dbo.Branches");
            DropIndex("dbo.IrregularDetails", new[] { "CashFlowId" });
            DropIndex("dbo.Quizs", new[] { "QuizGroupId" });
            DropIndex("dbo.CashOutDetails", new[] { "CashOutId" });
            DropIndex("dbo.CashOutDetails", new[] { "QuizId" });
            DropIndex("dbo.CashInDetails", new[] { "CashInId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "BranchId" });
            DropIndex("dbo.CutOffs", new[] { "BranchId" });
            DropIndex("dbo.CashCards", new[] { "SuperVisorId" });
            DropIndex("dbo.CashCards", new[] { "UserId" });
            DropIndex("dbo.CashCards", new[] { "CutOffId" });
            DropTable("dbo.IrregularDetails");
            DropTable("dbo.QuizGroups");
            DropTable("dbo.Quizs");
            DropTable("dbo.CashOutDetails");
            DropTable("dbo.CashInDetails");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CutOffs");
            DropTable("dbo.CashCards");
            DropTable("dbo.Branches");
        }
    }
}
