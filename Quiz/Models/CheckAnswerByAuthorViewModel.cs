using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class CheckAnswerByAuthorViewModel
    {
        public QuestionAnswer QuestionAnswer { get; set; }
        public TableViewModel Table { get; set; }
        public int Mark { get; set; }
    }
}