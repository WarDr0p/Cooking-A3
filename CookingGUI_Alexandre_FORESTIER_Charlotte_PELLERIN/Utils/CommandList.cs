using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class CommandList
    {
        public ScrollViewer scroll;
        public CommandList(string idClient)
        {
            scroll = new ScrollViewer();
            Grid grille = new Grid();
            //On récupère la liste des recettes du client
            List<string> liste = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.GetCommandes(idClient);
            if(liste != null && liste.Count() != 0)
            {
                
                for (int i = 0; i < liste.Count; i++)
                {
                    //On rajoute une ligne dans la grille ou l'on ajoute un conteneur de commande
                    RowDefinition ligne = new RowDefinition();
                    ligne.Height = new GridLength(0, GridUnitType.Auto);
                    grille.RowDefinitions.Add(ligne);
                    CommandeContainer commande = new CommandeContainer(liste[i]);
                    Grid.SetRow(commande.grille, i);
                    grille.Children.Add(commande.grille);                   
                }
            }
            scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = grille;
        }
    }
}
