﻿using Microsoft.Maui.Storage;
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

                var doc = XDocument.Load(FileName);

                foreach (var shoppingListElement in doc.Root.Elements("ShoppingList"))
                {
                    string id = shoppingListElement.Attribute("Id").Value;
                    string name = shoppingListElement.Element("Name").Value;
                    ShoppingList shoppingList = new ShoppingList(id, name);
                    shoppingList.SetProductsFromElement(shoppingListElement.Element("Products"));
                    ShoppingLists.Add(shoppingList);
                }
            }
        }

        public static void SaveShoppingLists()
        {
            var rootElement = new XElement("ShoppingLists");

            foreach (var shoppingList in ShoppingLists)
            {
                XElement productsElement = new XElement(shoppingList.GetElementFromProducts());
                var shoppingListElement = new XElement("ShoppingList",
                    new XAttribute("Id", shoppingList.Id),
                    new XElement("Name", shoppingList.Name),
                    productsElement);
                rootElement.Add(shoppingListElement);
            }

            var doc = new XDocument(rootElement);
            doc.Save(FileName);
        }
    }
}