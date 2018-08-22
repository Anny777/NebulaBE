namespace ProjectOrderFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CookingDishes", "Comment", c => c.String());
            DropColumn("dbo.Customs", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customs", "Comment", c => c.String());
            DropColumn("dbo.CookingDishes", "Comment");
        }
    }
}
