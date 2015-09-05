namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipoes", "observaciones", c => c.String());
            AddColumn("dbo.Induccions", "nombre", c => c.String());
            DropColumn("dbo.Equipoes", "dependencia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Equipoes", "dependencia", c => c.String());
            DropColumn("dbo.Induccions", "nombre");
            DropColumn("dbo.Equipoes", "observaciones");
        }
    }
}
