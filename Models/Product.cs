using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string TypeOfMeasurement { get; set; }
        public bool IsBought { get; set; } = false;
        public bool IsOptional { get; set; } = false;
        public double Quantity { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
            IsBought = false;
            IsOptional = false;
        }

        public Product(string name, string typeOfMeasurement, double quantity)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            TypeOfMeasurement = typeOfMeasurement;
            IsBought = false;
            IsOptional= false;
            Quantity = quantity;
        }

        public Product(string id, string name, string typeOfMeasurement, bool isBought, double quantity)
        {
            Id = id;
            Name = name;
            TypeOfMeasurement = typeOfMeasurement;
            IsBought = isBought;
            IsOptional = false;
            Quantity = quantity;
        }
    }
}
