using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }

        public string Answer { get; set; }
        
        public DateTime CreateTime { get; set; }

        public int QuestionId { get; set; }   
        public Question Question { get; set; }  

        public string UserId { get; set; }  
        public UserProfile User { get; set; }  

        public int StatusOfQuestionAnswerId { get; set; }   
        public StatusOfQuestionAnswer StatusOfQuestionAnswer { get; set; }  

        public ICollection<Check> Сhecks { get; set; }
        public QuestionAnswer()
        {
            Сhecks = new List<Check>();
        }
    }
}