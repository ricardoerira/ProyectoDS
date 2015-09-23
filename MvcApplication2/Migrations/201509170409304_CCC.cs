namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCC : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Docentes", "certificado_TPDTS", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Docentes", "certificado_TPDTS", c => c.String());
        }
    }
}
