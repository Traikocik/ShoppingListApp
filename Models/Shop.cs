using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    public class Shop
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public Shop() { }

        public Shop(string name)
        {
            Name = name;
        }

        public Shop(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
