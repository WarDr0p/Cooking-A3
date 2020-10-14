using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class ItemContainer
    {
        public TextBox quantityBox;
        public Grid grille;

        /// <summary>
        /// Créer un conteneur pour le infos d'une personne
        /// </summary>
        /// <param name="donnees">Données à afficher, la quantité doit être dans la quetrième case du tableau</param>
        /// <param name="edit">La textbox des données peut-elle être modfiée ?</param>
        /// <param name="value">Valeur utilisée pour la couleur d'arrière plan (1 gris) 0(transparent)</param>
        public ItemContainer(string[] donnees, bool edit,int value)
        {
            grille = new Grid();

            for (int i = 0; i < donnees.Length; i++)
            {
                //On créer les colonnes
                ColumnDefinition colonne = new ColumnDefinition();
                colonne.Width = new GridLength(1, GridUnitType.Star);
                grille.ColumnDefinitions.Add(colonne);
                int numCol = i;
                //TextBox contenant la quantité
                if (i == 4)
                {
                    quantityBox = new TextBox();
                    quantityBox.Text = donnees[i];
                    if (!edit) quantityBox.IsEnabled = false;
                    Grid.SetColumn(quantityBox, donnees.Length-1);
                    grille.Children.Add(quantityBox);
                }
                else
                {
                    //il s'agit d'un textblock
                    if(i > 4)
                    {
                        numCol = i - 1;
                    }
                    TextBlock txt = new TextBlock();
                    txt.Text = donnees[i];
                    Grid.SetColumn(txt, numCol);
                    grille.Children.Add(txt);
                    if (value % 2 == 1) txt.Background = Brushes.LightGray;
                }
            }
        }
    }
}
