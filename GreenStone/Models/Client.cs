using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenStone.Models
{
   public class Client : PersonProfile
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Client()
        {
            Products = new List<Product>();
        }
         
    }
}
