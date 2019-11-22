using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stonk.Models
{
    public class Person
    {
        [Key]
        public int id { get; set; }
        public bool isAdmin { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
