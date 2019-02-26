using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public string СheckPattern { get; set; }
        
        public DateTime CreateTime { get; set; }

        //author
        public string UserId { get; set; }   
        public UserProfile User { get; set; }

        //Whom the question is asked to + answer
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public Question()
        {
            QuestionAnswers = new List<QuestionAnswer>();
        }
    }
}