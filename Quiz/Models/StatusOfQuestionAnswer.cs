using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class StatusOfQuestionAnswer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public StatusOfQuestionAnswer()
        {
            QuestionAnswers = new List<QuestionAnswer>();
        }
    }
}