namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActividadAcademicas", "IPS_ESE_IPS_ESEId", c => c.Int());
            AddForeignKey("dbo.ActividadAcademicas", "IPS_ESE_IPS_ESEId", "dbo.IPS_ESE", "IPS_ESEId");
            CreateIndex("dbo.ActividadAcademicas", "IPS_ESE_IPS_ESEId");
           
        }
        
        public override void Down()
        {
            DropIndex("dbo.ActividadAcademicas", new[] { "IPS_ESE_IPS_ESEId" });
            DropForeignKey("dbo.ActividadAcademicas", "IPS_ESE_IPS_ESEId", "dbo.IPS_ESE");
            DropColumn("dbo.ActividadAcademicas", "IPS_ESE_IPS_ESEId");
        }
    }
}
