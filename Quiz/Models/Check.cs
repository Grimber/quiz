using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Check
    {
        public int Id { get; set; }
        
        public DateTime CreateTime { get; set; }
        public int Mark { get; set; }
        public string Notes { get; set; }

        //author
        public string UserId { get; set; }   
        public UserProfile User { get; set; }  
        
        public int QuestionAnswerId { get; set; }   
        public QuestionAnswer QuestionAnswer { get; set; } 
    }
}