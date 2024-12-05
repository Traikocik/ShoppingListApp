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
    internal class AllShoppingLists
    {
        public ObservableCollection<ShoppingList> ShoppingLists { get; set; } = new();
        private string FileName;

        public AllShoppingLists()
        {
            FileName = Path.Combine(FileSystem.AppDataDirectory, "shoppingLists.xml");
            LoadShoppingLists();
        }

        public void LoadShoppingLists()
        {            
            if (File.Exists(FileName))
            {
                ShoppingLists.Clear();

                var doc = XDocument.Load(FileName);

                foreach (var shoppingListElement in doc.Root.Elements("ShoppingList"))
                {
                    string name = shoppingListElement.Element("Name").Value;
                    ObservableCollection<Product> products = ShoppingList.GetProductsFromElement(shoppingListElement.Element("Products"));
                    // Ewentualnie zamiast tworzenia nazwy i produktów i potem z nich tworzenia obiektu oraz używania metody jako statycznej,
                    // można najpierw tworzyć obiekt tylko za pomocą nazwy i potem zmienić funkcję GetProductsFromElement na SetProductsFromElement i ona już tam w ShoppingList.cs będzie ustawiała
                    ShoppingList shoppingList = new ShoppingList(name, products);
                    ShoppingLists.Add(shoppingList);
                }
            }
        }

        public void SaveShoppingLists()
        {
            var rootElement = new XElement("ShoppingLists");

            foreach (var shoppingList in ShoppingLists)
            {
                XElement productsElement = new XElement(shoppingList.GetElementFromProducts());
                var shoppingListElement = new XElement("ShoppingList",
                    new XElement("Name", shoppingList.Name),
                    productsElement);
            }

            var doc = new XDocument(rootElement);
            doc.Save(FileName);
        }

        public void DeleteShoppingList()
        {



            SaveShoppingLists();
        }
    }
}