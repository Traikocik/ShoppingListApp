using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    public class Unit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public Unit() { }

        public Unit(string name)
        {
            Name = name;
        }

        public Unit(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
