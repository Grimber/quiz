using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class CheckAnswerViewModel
    {
        public int QuestionAnswerId { get; set; }

        [Required]
        [Display(Name = "Question Name")]
        public string QuestionName { get; set; }

        [Required]
        [Display(Name = "The text of the quiz")]
        public string QuestionText { get; set; }

        [Required]
        [Display(Name = "Template for verification")]
        public string QuestionСheckPattern { get; set; }

        [Required]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Required]
        [Display(Name = "Mark")]
        public int Mark { get; set; }

        [Required]
        [Display(Name = "Notes")]
        public int Notes { get; set; }
    }
}