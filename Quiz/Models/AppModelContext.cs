using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity;

namespace Quiz.Models
{
    public class AppModelContext : DbContext
    {
        public DbSet<University> Universities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<StatusOfQuestionAnswer> StatusOfQuestionAnswers { get; set; }

        public AppModelContext()
            : base("DefaultConnection")
        {
        }
    }
}
