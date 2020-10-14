using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class UserList
    {
        public ScrollViewer scroll;
        public UserList(string[] vals, bool isPromoting)
        {
            Grid grille = new Grid();
            int i = 0;
            foreach(string val in vals)
            {
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(0, GridUnitType.Auto);
                grille.RowDefinitions.Add(ligne);
                UserContainer myUser = new UserContainer(val.Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries), isPromoting,i%2);
                Grid.SetRow(myUser.grille,i);
                grille.Children.Add(myUser.grille);
                i++;
            }
            scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = grille;
        }
    }
}
