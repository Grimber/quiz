using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Models
{
    public class AddGroupViewModel
    {
        public SelectList ListOfUniversities { get; set; }

        [Required]
        [Display(Name = "Universities")]
        public int UniversityId { get; set; }
        
        [Required]
        [Display(Name = "Group")]
        public string GroupName { get; set; }
    }
}