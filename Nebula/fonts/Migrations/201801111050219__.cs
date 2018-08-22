namespace ProjectOrderFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dishes", "_id", c => c.String());
            AddColumn("dbo.Dishes", "extId", c => c.String());
            AddColumn("dbo.Dishes", "shopID", c => c.String());
            AddColumn("dbo.Dishes", "barcode", c => c.String());
            AddColumn("dbo.Dishes", "nameFull", c => c.String());
            AddColumn("dbo.Dishes", "VAT", c => c.String());
            AddColumn("dbo.Dishes", "sellingPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Dishes", "useGroupMarginRule", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "isAlcohol", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "alcoholCode", c => c.String());
            AddColumn("dbo.Dishes", "ownMarginRule", c => c.String());
            AddColumn("dbo.Dishes", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Dishes", "__v", c => c.Int(nullable: false));
            AddColumn("dbo.Dishes", "isService", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "uuid", c => c.String());
            AddColumn("dbo.Dishes", "isDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dishes", "code", c => c.String());
            AddColumn("dbo.Dishes", "isBeer", c => c.Boolean(nullable: false));
            DropColumn("dbo.Dishes", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dishes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Dishes", "isBeer");
            DropColumn("dbo.Dishes", "code");
            DropColumn("dbo.Dishes", "isDelete");
            DropColumn("dbo.Dishes", "uuid");
            DropColumn("dbo.Dishes", "isService");
            DropColumn("dbo.Dishes", "__v");
            DropColumn("dbo.Dishes", "modified");
            DropColumn("dbo.Dishes", "ownMarginRule");
            DropColumn("dbo.Dishes", "alcoholCode");
            DropColumn("dbo.Dishes", "isAlcohol");
            DropColumn("dbo.Dishes", "useGroupMarginRule");
            DropColumn("dbo.Dishes", "sellingPrice");
            DropColumn("dbo.Dishes", "VAT");
            DropColumn("dbo.Dishes", "nameFull");
            DropColumn("dbo.Dishes", "barcode");
            DropColumn("dbo.Dishes", "shopID");
            DropColumn("dbo.Dishes", "extId");
            DropColumn("dbo.Dishes", "_id");
        }
    }
}
