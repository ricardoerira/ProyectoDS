namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ad : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cursoes", "fechaCreacion", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cursoes", "fechaInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cursoes", "fechaFin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cursoes", "fechaFin", c => c.String());
            AlterColumn("dbo.Cursoes", "fechaInicio", c => c.String());
            AlterColumn("dbo.Cursoes", "fechaCreacion", c => c.String());
        }
    }
}
