namespace CashCard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IrregularDetails", "CashFlowId", "dbo.CashCards");
            DropIndex("dbo.IrregularDetails", new[] { "CashFlowId" });
            AlterColumn("dbo.CashOutDetails", "DateInfo", c => c.DateTime());
            DropTable("dbo.IrregularDetails");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.CashOutDetails", "DateInfo", c => c.DateTime(nullable: false));
            CreateIndex("dbo.IrregularDetails", "CashFlowId");
            AddForeignKey("dbo.IrregularDetails", "CashFlowId", "dbo.CashCards", "Id");
        }
    }
}
