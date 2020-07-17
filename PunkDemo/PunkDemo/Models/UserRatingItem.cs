using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunkDemo.Models
{
    public class UserRatingItem
    {
        public string Username { get; set; }

        public decimal Rating { get; set; }

        public string Comments { get; set; } = string.Empty;
    }
}