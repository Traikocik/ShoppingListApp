using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList4F1.Models
{
    internal class ShoppingList
    {
        public ObservableCollection<Product> Products { get; set; } = new();
        public string Name { get; set; }

        public ShoppingList() { }

        public ShoppingList(string name, ObservableCollection<Product> products) 
        {
            Name = name;
            Products = products;
        }

        public XElement GetElementFromProducts()
        {
            var productsElement = new XElement("Products");

            foreach (var product in Products)
            {
                var productElement = new XElement("Product",
                    new XAttribute("Name", product.Name),
                    new XAttribute("TypeOfMeasurement", product.TypeOfMeasurement),
                    new XAttribute("IsBought", product.IsBought),
                    new XAttribute("Quantity", product.Quantity));

                productsElement.Add(productElement);
            }

            return productsElement;
        }

        public static ObservableCollection<Product> GetProductsFromElement(XElement productsElement)
        {
            ObservableCollection<Product> products = new();

            foreach (XElement productElement in productsElement.Elements("Product"))
            {
                string name = productElement.Attribute("Name").Value;
                string typeOfMeasurement = productElement.Attribute("TypeOfMeasurement").Value;
                bool isBought = bool.Parse(productElement.Attribute("IsBought").Value);
                int quantity = int.Parse(productElement.Attribute("Quantity").Value);

                Product product = new Product(name, typeOfMeasurement, isBought, quantity);
                products.Add(product);
            }

            return products;
        }
    }
}
