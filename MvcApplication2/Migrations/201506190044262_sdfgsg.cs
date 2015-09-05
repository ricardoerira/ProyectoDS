namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfgsg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepartamentoSaluds", "codigo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DepartamentoSaluds", "codigo");
        }
    }
}
