namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hjyffd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RotacionEstudiantes", "horario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RotacionEstudiantes", "horario");
        }
    }
}
