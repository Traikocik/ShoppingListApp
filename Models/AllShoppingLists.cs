using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList4F1.Models
{
    public class AllShoppingLists
    {
        public static ObservableCollection<ShoppingList> ShoppingLists { get; set; } = new();
        public static ObservableCollection<Shop> Shops { get; set; } = new ObservableCollection<Shop>();
        private static string FileName;

        public AllShoppingLists()
        {
            FileName = Path.Combine(FileSystem.AppDataDirectory, "shoppingLists.xml");
            LoadShoppingLists();
        }

        public static void LoadShoppingLists()
        {            
            if (File.Exists(FileName))
            {
                ShoppingLists.Clear();
                Shops.Clear();

                var doc = XDocument.Load(FileName);

                var shopsElement = doc.Root.Element("Shops");
                if (shopsElement != null)
                {
                    foreach (var shopElement in shopsElement.Elements("Shop"))
                    {
                        string id = shopElement.Attribute("Id").Value;
                        string name = shopElement.Attribute("Name").Value;
                        Shops.Add(new Shop(id, name));
                    }
                }

                foreach (var shoppingListElement in doc.Root.Elements("ShoppingList"))
                {
                    string id = shoppingListElement.Attribute("Id").Value;
                    string name = shoppingListElement.Attribute("Name").Value;
                    ShoppingList shoppingList = new ShoppingList(id, name);
                    shoppingList.SetCategoriesFromElement(shoppingListElement.Element("Categories"));
                    ShoppingLists.Add(shoppingList);
                }
            }
        }

        public static void SaveShoppingLists()
        {
            var rootElement = new XElement("ShoppingLists");

            var shopsElement = new XElement("Shops");
            foreach (var shop in Shops)
            {
                var shopElement = new XElement("Shop",
                    new XAttribute("Id", shop.Id),
                    new XAttribute("Name", shop.Name));
                shopsElement.Add(shopElement);
            }
            rootElement.Add(shopsElement);

            foreach (var shoppingList in ShoppingLists)
            {
                XElement categoriesElement = new XElement(shoppingList.GetElementFromCategories());
                var shoppingListElement = new XElement("ShoppingList",
                    new XAttribute("Id", shoppingList.Id),
                    new XAttribute("Name", shoppingList.Name),
                    categoriesElement);
                rootElement.Add(shoppingListElement);
            }

            var doc = new XDocument(rootElement);
            doc.Save(FileName);
        }
    }
}