using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    public class Product : INotifyPropertyChanged
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string TypeOfMeasurement { get; set; }
        public bool IsBought { get; set; } = false;
        public bool IsOptional { get; set; } = false;

        private double quantity;
        public double Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public Product(string name, string typeOfMeasurement, bool isOptional, double quantity)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            TypeOfMeasurement = typeOfMeasurement;
            IsBought = false;
            IsOptional = isOptional;
            Quantity = quantity;
        }

        public Product(string id, string name, string typeOfMeasurement, bool isBought, bool isOptional, double quantity)
        {
            Id = id;
            Name = name;
            TypeOfMeasurement = typeOfMeasurement;
            IsBought = isBought;
            IsOptional = isOptional;
            Quantity = quantity;
        }
    }
}
