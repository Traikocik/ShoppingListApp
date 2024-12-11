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
        //public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
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

        public void SetCategoriesFromElement(XElement categoriesElement)
        {
            ObservableCollection<Category> categories = new();

            foreach (XElement categoryElement in categoriesElement.Elements("Category"))
            {
                string id = categoryElement.Attribute("Id").Value;
                string name = categoryElement.Attribute("Name").Value;
                Category category = new Category(id, name);
                category.SetProductsFromElement(categoryElement.Element("Products"));
                categories.Add(category);
            }

            Categories = categories;
        }

        public XElement GetElementFromCategories()
        {
            var categoriesElement = new XElement("Categories");

            foreach (var category in Categories)
            {
                XElement productsElement = new XElement(category.GetElementFromCategories());
                var categoryElement = new XElement("Category",
                    new XAttribute("Id", category.Id),
                    new XAttribute("Name", category.Name),
                    productsElement);

                categoriesElement.Add(categoryElement);
            }

            return categoriesElement;
        }
    }
}
