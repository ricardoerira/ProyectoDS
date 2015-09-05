namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Programas", "codigo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Programas", "codigo");
        }
    }
}
