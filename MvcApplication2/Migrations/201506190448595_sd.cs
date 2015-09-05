namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HojaVidas", "num_telefono", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HojaVidas", "num_telefono", c => c.Long(nullable: false));
        }
    }
}
