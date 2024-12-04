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

        public void LoadProducts()
        {
            Products.Clear();
        }

        public void SaveProducts()
        {
            XDocument doc = new XDocument(
                new XElement("ShoppingLists", 
                    Products.Select(product => new XElement("Counter", 
                        new XAttribute("Name", product.Name), 
                        new XAttribute("TypeOfMeasurement", product.TypeOfMeasurement), 
                        new XAttribute("Quantity", product.Quantity), 
                        new XAttribute("IsBought", product.IsBought)))));
        }
    }
}
