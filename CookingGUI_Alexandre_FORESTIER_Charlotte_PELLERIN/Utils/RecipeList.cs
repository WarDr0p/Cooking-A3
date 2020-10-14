using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class RecipeList
    {
        public ScrollViewer scroll;
        public RecipeContainer[] recettes;
        public TextBlock[] quantite;
        
        /// <summary>
        /// Constructeur pour l'interface administrateur
        /// </summary>
        /// <param name="tab">Infos des recettes</param>
        /// <param name="isCommanding">Toujours mettre à false</param>
        /// <param name="isAdminValidating">Est ce que l'admin vous affcihe rle bouton de validation(true) ou de suppression(false)</param>
        public RecipeList(string[] tab, bool isCommanding, bool isAdminValidating)
        {
            //Création de grille
            Grid grille = new Grid();
            recettes = new RecipeContainer[tab.Length];
            for (int i = 0; i < tab.Length; i++)
            {
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(0, GridUnitType.Auto);
                grille.RowDefinitions.Add(ligne);
            }
            //remplissage de la grille
            for (int i = 0; i < tab.Length; i++)
            {
                recettes[i] = new RecipeContainer(tab[i].Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries), isCommanding,isAdminValidating);
                Grid.SetRow(recettes[i].Grille, i);
                grille.Children.Add(recettes[i].Grille);
                Border bordure = new Border();
                bordure.BorderThickness = new Thickness(1, 1, 1, 1);
                bordure.BorderBrush = System.Windows.Media.Brushes.DarkGray;
                bordure.Margin = new Thickness(4, 2, 4, 2);
                Grid.SetRow(bordure, i);
                grille.Children.Add(bordure);
            }

            scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = grille;
        }

        /// <summary>
        /// Constructeur générique liste de recettes
        /// </summary>
        /// <param name="tab">Liste des recettes</param>
        /// <param name="isCommanding">Afficher l'interface de commande?</param>
        public RecipeList(string[] tab, bool isCommanding) {
            //création de la grille
            quantite = new TextBlock[tab.Length];
            Grid grille = new Grid();
            recettes = new RecipeContainer[tab.Length];
            for(int i = 0; i < tab.Length; i++)
            {
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(0, GridUnitType.Auto);
                grille.RowDefinitions.Add(ligne);
                
            }
            //remplissage des cases
            for(int i = 0; i < tab.Length; i++)
            {
                recettes[i] = new RecipeContainer(tab[i].Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries),isCommanding);
                Grid.SetRow(recettes[i].Grille,i);
                grille.Children.Add(recettes[i].Grille);
                Border bordure = new Border();
                bordure.BorderThickness = new Thickness(1, 1, 1, 1);
                bordure.BorderBrush = System.Windows.Media.Brushes.DarkGray;
                bordure.Margin = new Thickness(4, 2, 4, 2);
                Grid.SetRow(bordure, i);
                grille.Children.Add(bordure);
                quantite[i] = recettes[i].quantite;



            }
            scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = grille;

        }
        
    }
}
