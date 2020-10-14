using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class UserContainer
    {
        public Grid grille;
        /// <summary>
        /// Génère une ligne contennat les infos sur un utilisateur
        /// </summary>
        /// <param name="infos">infos sur l'utilisateur</param>
        /// <param name="isPromoting">bouton de promotion (true) ou bouton revoquation(false)</param>
        /// <param name="couleurId">1 arrière plan gris 0 arrière plan transparent</param>
        public UserContainer(string[]infos, bool isPromoting,int couleurId)
        {
            grille = new Grid();
            for(int i =0;  i <= infos.Length; i++)
            {
                
                ColumnDefinition colonne = new ColumnDefinition();
                colonne.Width = new GridLength(1, GridUnitType.Star);
                if (i < infos.Length)
                {
                    grille.ColumnDefinitions.Add(colonne);
                    TextBlock txt = new TextBlock();
                    txt.Text = infos[i];
                    Grid.SetColumn(txt, i);
                    grille.Children.Add(txt);
                    if (couleurId % 2 == 0)
                    {
                        txt.Background = Brushes.LightGray;
                    }
                }
                else
                {
                    Button bouton = new Button();
                    if (isPromoting)
                    {
                        bouton.Content = "Promouvoir";
                    }
                    else
                    {
                        bouton.Content = "Rétrograder";
                    }
                    bouton.Tag = infos[0];
                    bouton.Click += new RoutedEventHandler(Admin);
                    Grid.SetColumn(bouton, i);
                    grille.Children.Add(bouton);
                }
            }
        }

        /// <summary>
        /// Event déclenché lors du clique bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Tag.ToString();
            if ((sender as Button).Content.ToString() == "Promouvoir")
            {
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.PromoteCdr(id);
            }
            else
            {
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.RevokeCdr(id);
            }
        }
    }
}
