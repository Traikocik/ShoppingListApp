using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    public class Product
    {
        public string Name;
        public string TypeOfMeasurement;
        public bool IsBought = false;
        public int Quantity;

        public Product(string name, string typeOfMeasurement, bool isBought, int quantity)
        {
            Name = name;
            TypeOfMeasurement = typeOfMeasurement;
            IsBought = isBought;
            Quantity = quantity;
        }
    }
}
