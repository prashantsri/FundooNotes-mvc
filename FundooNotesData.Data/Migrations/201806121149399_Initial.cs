namespace FundooNotesData.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblLabelNotes", "LabelID", c => c.String());
            DropColumn("dbo.tblLabelNotes", "Label");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblLabelNotes", "Label", c => c.String());
            DropColumn("dbo.tblLabelNotes", "LabelID");
        }
    }
}
