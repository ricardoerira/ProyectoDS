namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cursoes", "depto", c => c.String());
            AddColumn("dbo.Cursoes", "duracion", c => c.String());
            AddColumn("dbo.Cursoes", "lugar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cursoes", "lugar");
            DropColumn("dbo.Cursoes", "duracion");
            DropColumn("dbo.Cursoes", "depto");
        }
    }
}
