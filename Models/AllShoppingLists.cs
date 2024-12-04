using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList4F1.Models
{
    internal class AllShoppingLists
    {
        public ObservableCollection<ShoppingList> ShoppingLists { get; set; } = new();

        public void LoadShoppingLists()
        {
            ShoppingLists.Clear();

            //XDocument doc = new XDocument();
        }
    }
}
