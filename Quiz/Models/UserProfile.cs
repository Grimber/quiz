using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class UserProfile
    {
        public string Id { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }

        public int GroupId { get; set; }   
        public Group Group { get; set; }  


        public ICollection<Question> Questions { get; set; }
        public ICollection<Check> Сhecks { get; set; }
        public ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        public UserProfile()
        {
            Questions = new List<Question>();
            Сhecks = new List<Check>();
            QuestionAnswers = new List<QuestionAnswer>();
        }
    }
}