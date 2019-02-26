using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models
{
    public class CreateQuestionViewModel
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "The text of the quiz")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Template for verification")]
        public string СheckPattern { get; set; }
       
        [Required]
        [Display(Name = "University")]
        public ICollection<int> Universities { get; set; }

        [Required]
        [Display(Name = "Group")]
        public ICollection<int> Groups { get; set; }
    }
}