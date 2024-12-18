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
        public string UnitId { get; set; }
        public string UnitName => GetUnitName();
        private bool isBought;
        public bool IsBought
        {
            get => isBought;
            set
            {
                if (isBought != value)
                {
                    isBought = value;
                    OnPropertyChanged(nameof(IsBought));
                }
            }
        }
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
        public string ShopId { get; set; }
        
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
            quantity = 1;
        }

        public Product(string name, string unitId, bool isOptional, double quantity)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            UnitId = unitId;
            IsBought = false;
            IsOptional = isOptional;
            Quantity = quantity;
        }

        public Product(string id, string name, string unitId, bool isBought, bool isOptional, double quantity, string shopId)
        {
            Id = id;
            Name = name;
            UnitId = unitId;
            IsBought = isBought;
            IsOptional = isOptional;
            Quantity = quantity;
            ShopId = shopId;
        }

        private string GetUnitName()
        {
            Unit unit = AllShoppingLists.Units.FirstOrDefault(u => u.Id == UnitId);
            return unit?.Name ?? string.Empty;
        }
    }
}
