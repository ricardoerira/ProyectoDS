namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lilianita : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cursoes", "dirigidoOtro", c => c.String());
            AlterColumn("dbo.Cursoes", "dirigido", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cursoes", "dirigido", c => c.Int(nullable: false));
            DropColumn("dbo.Cursoes", "dirigidoOtro");
        }
    }
}
