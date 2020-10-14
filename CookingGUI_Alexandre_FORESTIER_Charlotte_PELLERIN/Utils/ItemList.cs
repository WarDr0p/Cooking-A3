using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class ItemList
    {
        public Grid grille;
        public  TextBox[] quantity;
        public ItemList(string[] tab,bool editable)
        {
            quantity = new TextBox[tab.Length];
            grille = new Grid();
            for (int i = 0; i < tab.Length; i++)
            {
                RowDefinition ligne = new RowDefinition();
                grille.RowDefinitions.Add(ligne);

                ItemContainer produit = new ItemContainer(tab[i].Split(new string[] { "§" },StringSplitOptions.RemoveEmptyEntries),editable,i%2);
                quantity[i] = produit.quantityBox;
                Grid.SetRow(produit.grille, i);
                grille.Children.Add(produit.grille);
            }
        }      
    }
}
