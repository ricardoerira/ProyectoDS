namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdfad : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Docentes", "num_libreta_militar", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Docentes", "num_libreta_militar", c => c.Int(nullable: false));
        }
    }
}
