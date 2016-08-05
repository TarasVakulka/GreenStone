using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenStone.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
        public int? StoneId { get; set; }
        public virtual Stone Stone { get; set; }
        public bool IsSold { get; set; }
        public bool IsMade { get; set; }

        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }
        
        public virtual ICollection<DpService> DpServices { get; set; }

        public Product()
        {
            DpServices = new List<DpService>();
           
        }
    }
}
