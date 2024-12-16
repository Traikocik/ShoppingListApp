using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList4F1.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public ObservableCollection<Product> Products { get; set; } = new();

        public Category() { }

        public Category(string name)
        {
            Name = name;
        }

        public Category(string id, string name)
        {
            Id = id;
            Name = name;
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
                bool isOptional = bool.Parse(productElement.Attribute("IsOptional").Value);
                int quantity = int.Parse(productElement.Attribute("Quantity").Value);

                Product product = new Product(id, name, typeOfMeasurement, isBought, isOptional, quantity);
                products.Add(product);
            }

            Products = products;
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
                    new XAttribute("IsOptional", product.IsOptional),
                    new XAttribute("Quantity", product.Quantity));

                productsElement.Add(productElement);
            }

            return productsElement;
        }
    }
}
