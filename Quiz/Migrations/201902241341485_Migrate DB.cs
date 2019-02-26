namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UsersQuestionAnswers", newName: "QuestionAnswers");
            RenameColumn(table: "dbo.Checks", name: "UsersQuestionAnswerId", newName: "QuestionAnswerId");
            RenameIndex(table: "dbo.Checks", name: "IX_UsersQuestionAnswerId", newName: "IX_QuestionAnswerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Checks", name: "IX_QuestionAnswerId", newName: "IX_UsersQuestionAnswerId");
            RenameColumn(table: "dbo.Checks", name: "QuestionAnswerId", newName: "UsersQuestionAnswerId");
            RenameTable(name: "dbo.QuestionAnswers", newName: "UsersQuestionAnswers");
        }
    }
}
