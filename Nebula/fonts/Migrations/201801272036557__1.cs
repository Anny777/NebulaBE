namespace ProjectOrderFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "UrlImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "UrlImage");
        }
    }
}
