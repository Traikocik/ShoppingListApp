using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList4F1.Models
{
    public class ShoppingList
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public ObservableCollection<Product> Products { get; set; } = new();
        public string Name { get; set; }

        public ShoppingList() 
        {
            Id = Guid.NewGuid().ToString();
        }

        public ShoppingList(string name) 
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public ShoppingList(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public XElement GetElementFromProducts()
        {
            var productsElement = new XElement("Products");

            foreach (var product in Products)
            {
                var productElement = new XElement("Product",
                    new XAttribute("Id", product.Id),
                    new XAttribute("Name", product.Name),
                    new XAttribute("TypeOfMeasurement", product.TypeOfMeasurement),
                    new XAttribute("IsBought", product.IsBought),
                    new XAttribute("Quantity", product.Quantity));

                productsElement.Add(productElement);
            }

            return productsElement;
        }

        public void SetProductsFromElement(XElement productsElement)
        {
            ObservableCollection<Product> products = new();

            foreach (XElement productElement in productsElement.Elements("Product"))
            {
                string id = productElement.Attribute("Id").Value;
                string name = productElement.Attribute("Name").Value;
                string typeOfMeasurement = productElement.Attribute("TypeOfMeasurement").Value;
                bool isBought = bool.Parse(productElement.Attribute("IsBought").Value);
                int quantity = int.Parse(productElement.Attribute("Quantity").Value);

                Product product = new Product(id, name, typeOfMeasurement, isBought, quantity);
                products.Add(product);
            }

            Products = products;
        }
    }
}
