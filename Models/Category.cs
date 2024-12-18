using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList4F1.Models
{
    public class Category : INotifyPropertyChanged
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        private bool _isCategoryNameVisibled;
        public bool IsCategoryNameVisible
        {
            get => _isCategoryNameVisibled;
            set
            {
                if (_isCategoryNameVisibled != value)
                {
                    _isCategoryNameVisibled = value;
                    OnPropertyChanged(nameof(IsCategoryNameVisible));
                }
            }
        }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Product> Products { get; set; } = new();

        public Category() 
        {
            IsCategoryNameVisible = true;
        }

        public Category(string name)
        {
            Name = name;
            IsCategoryNameVisible = true;
        }

        public Category(string id, string name)
        {
            Id = id;
            Name = name;
            IsCategoryNameVisible = true;
        }
    
        public void SetProductsFromElement(XElement productsElement)
        {
            ObservableCollection<Product> products = new();

            foreach (XElement productElement in productsElement.Elements("Product"))
            {
                string id = productElement.Attribute("Id").Value;
                string name = productElement.Attribute("Name").Value;
                string unitId = productElement.Attribute("UnitId").Value;
                bool isBought = bool.Parse(productElement.Attribute("IsBought").Value);
                bool isOptional = bool.Parse(productElement.Attribute("IsOptional").Value);
                double quantity = double.Parse(productElement.Attribute("Quantity").Value, CultureInfo.InvariantCulture);
                string shopId = productElement.Attribute("ShopId").Value;

                Product product = new Product(id, name, unitId, isBought, isOptional, quantity, shopId);
                products.Add(product);
            }

            Products = products;
        }

        public XElement GetElementFromProducts()
        {
            XElement productsElement = new XElement("Products");

            foreach (var product in Products)
            {
                XElement productElement = new XElement("Product",
                    new XAttribute("Id", product.Id),
                    new XAttribute("Name", product.Name),
                    new XAttribute("UnitId", product.UnitId),
                    new XAttribute("IsBought", product.IsBought),
                    new XAttribute("IsOptional", product.IsOptional),
                    new XAttribute("Quantity", product.Quantity),
                    new XAttribute("ShopId", product.ShopId ?? string.Empty));

                productsElement.Add(productElement);
            }

            return productsElement;
        }
    }
}
