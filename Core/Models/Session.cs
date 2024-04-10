using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public ICollection<User> Users { get; set; }
    }


}
