using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunkDemo.Models
{
    public class BeerItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserRatingItem> UserRatings { get; set; } = new List<UserRatingItem>();
    }
}