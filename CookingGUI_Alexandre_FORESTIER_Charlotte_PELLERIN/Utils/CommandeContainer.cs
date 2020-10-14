using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class CommandeContainer
    {
        public Grid grille;
        public CommandeContainer(string infos)
        {
            //On récupère les infos sur la commande
            string[] info = infos.Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries);
            //On récupère les recettes
            List<string> liste = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Commande.getRecettes(info[0]);  
            //On met les infos de la commande dans une entête
            ItemContainer item = new ItemContainer(info, false, 1);
            grille = new Grid();


            for(int i = 0; i < liste.Count+1; i++)
            {
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(1,GridUnitType.Auto);
                grille.RowDefinitions.Add(ligne);
                if (i == 0)
                {
                    //C'est la première ligne il faut rajouter l'entête généréé précédemment
                    grille.Children.Add(item.grille);
                }
                else
                {
                    //Sinon il s'agis de recettes on les affiches
                    string[] vals = liste[i - 1].Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries);
                    RecipeContainer recette = new RecipeContainer(CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.GetRecipe(vals[0]).Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries), false,Convert.ToInt32(vals[1]));
                    Grid.SetRow(recette.Grille,i);
                    grille.Children.Add(recette.Grille);
                }
                
            }
        }
    }
}
