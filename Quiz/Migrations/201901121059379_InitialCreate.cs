namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        AuthorId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StatusOfQuestionAnswerId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.User_Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.StatusOfQuestionAnswers", t => t.StatusOfQuestionAnswerId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.StatusOfQuestionAnswerId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        СheckPattern = c.String(),
                        AuthorId = c.String(),
                        CreateTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Surname = c.String(),
                        Name = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UniversityId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Universities", t => t.UniversityId)
                .Index(t => t.UniversityId);
            
            CreateTable(
                "dbo.Universities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Сheck",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        Mark = c.Int(nullable: false),
                        Notes = c.String(),
                        UserId = c.Int(nullable: false),
                        UsersQuestionAnswerId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.User_Id)
                .ForeignKey("dbo.UsersQuestionAnswers", t => t.UsersQuestionAnswerId, cascadeDelete: true)
                .Index(t => t.UsersQuestionAnswerId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.StatusOfQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersQuestionAnswers", "StatusOfQuestionAnswerId", "dbo.StatusOfQuestionAnswers");
            DropForeignKey("dbo.UsersQuestionAnswers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Сheck", "UsersQuestionAnswerId", "dbo.UsersQuestionAnswers");
            DropForeignKey("dbo.Сheck", "User_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.UsersQuestionAnswers", "User_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.Questions", "User_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfiles", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "UniversityId", "dbo.Universities");
            DropIndex("dbo.Сheck", new[] { "User_Id" });
            DropIndex("dbo.Сheck", new[] { "UsersQuestionAnswerId" });
            DropIndex("dbo.Groups", new[] { "UniversityId" });
            DropIndex("dbo.UserProfiles", new[] { "GroupId" });
            DropIndex("dbo.Questions", new[] { "User_Id" });
            DropIndex("dbo.UsersQuestionAnswers", new[] { "User_Id" });
            DropIndex("dbo.UsersQuestionAnswers", new[] { "StatusOfQuestionAnswerId" });
            DropIndex("dbo.UsersQuestionAnswers", new[] { "QuestionId" });
            DropTable("dbo.StatusOfQuestionAnswers");
            DropTable("dbo.Сheck");
            DropTable("dbo.Universities");
            DropTable("dbo.Groups");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Questions");
            DropTable("dbo.UsersQuestionAnswers");
        }
    }
}
