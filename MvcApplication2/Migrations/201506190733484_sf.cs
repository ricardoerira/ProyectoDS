namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sf : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Estudiantes", "codigo", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Estudiantes", "codigo", c => c.Int(nullable: false));
        }
    }
}
