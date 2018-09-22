using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppNeo4jApi.Models
{
    public class CustomRelationship
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }
}