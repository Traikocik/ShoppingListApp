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
        public static ObservableCollection<Unit> Units { get; set; } = new ObservableCollection<Unit>();
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
                Units.Clear();

                var doc = XDocument.Load(FileName);
                var rootElement = doc.Root;

                if (rootElement != null)
                {
                    var unitsElement = rootElement.Element("Units");
                    if (unitsElement != null)
                    {
                        foreach (var unitElement in unitsElement.Elements("Unit"))
                        {
                            string id = unitElement.Attribute("Id").Value;
                            string name = unitElement.Attribute("Name").Value;
                            Units.Add(new Unit(id, name));
                        }
                    }

                    if (Units.Count == 0)
                    {
                        Units.Add(new Unit("pcs."));
                        Units.Add(new Unit("l."));
                        Units.Add(new Unit("kg."));
                    }

                    var shopsElement = rootElement.Element("Shops");
                    if (shopsElement != null)
                    {
                        foreach (var shopElement in shopsElement.Elements("Shop"))
                        {
                            string id = shopElement.Attribute("Id").Value;
                            string name = shopElement.Attribute("Name").Value;
                            Shops.Add(new Shop(id, name));
                        }
                    }

                    var shoppingListsElement = rootElement.Element("ShoppingLists");
                    if (shoppingListsElement != null)
                    {
                        foreach (var shoppingListElement in shoppingListsElement.Elements("ShoppingList"))
                        {
                            string id = shoppingListElement.Attribute("Id").Value;
                            string name = shoppingListElement.Attribute("Name").Value;
                            ShoppingList shoppingList = new ShoppingList(id, name);
                            shoppingList.SetCategoriesFromElement(shoppingListElement.Element("Categories"));
                            ShoppingLists.Add(shoppingList);
                        }
                    }
                }
            }
        }

        public static void SaveShoppingLists()
        {
            var rootElement = new XElement("ShoppingListsAppData");

            var unitsElement = new XElement("Units");
            foreach (var unit in Units)
            {
                var unitElement = new XElement("Unit",
                    new XAttribute("Id", unit.Id),
                    new XAttribute("Name", unit.Name));
                unitsElement.Add(unitElement);
            }
            rootElement.Add(unitsElement);

            var shopsElement = new XElement("Shops");
            foreach (var shop in Shops)
            {
                var shopElement = new XElement("Shop",
                    new XAttribute("Id", shop.Id),
                    new XAttribute("Name", shop.Name));
                shopsElement.Add(shopElement);
            }
            rootElement.Add(shopsElement);

            var shoppingListsElement = new XElement("ShoppingLists");
            foreach (var shoppingList in ShoppingLists)
            {
                XElement categoriesElement = new XElement(shoppingList.GetElementFromCategories());
                var shoppingListElement = new XElement("ShoppingList",
                    new XAttribute("Id", shoppingList.Id),
                    new XAttribute("Name", shoppingList.Name),
                    categoriesElement);
                shoppingListsElement.Add(shoppingListElement);
            }
            rootElement.Add(shoppingListsElement);

            var doc = new XDocument(rootElement);
            doc.Save(FileName);
        }
    }
}