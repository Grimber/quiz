using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public int? UniversityId { get; set; }  
        public University University { get; set; }  
        

        public ICollection<UserProfile> UserProfiles { get; set; }
        public Group()
        {
            UserProfiles = new List<UserProfile>();
        }
    }
}