using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for Demo.xaml
    /// </summary>
    public partial class Demo : Window
    {
        int i;
        public Demo()
        {
            //déclare l'évènement quand on clique sur une touche dans la fenêtre
            i = 1;
            InitializeComponent();
            KeyDown += new KeyEventHandler(enterPressed);

        }
        /// <summary>
        /// Fait avancer la démo quand on appuie sur entrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterPressed(object sender, KeyEventArgs e)
        { 
            //on vérifie que la touche soit espace
            if(e.Key == Key.Space)
            {
                if(i == 1)
                {
                    clients.Text = "Nombre clients : " + CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.GetNumber();
                }else if(i == 2)
                {
                    cdr.Text = "Nombre Cdr : " + CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.GetNumber();
                }else if(i == 3)
                {
                    string[] infos = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.getValidCdrWithQuantity();
                    cdrContainer.Content = new Utils.ItemList(infos, false).grille;
                }else if(i == 4)
                {
                    recettes.Text = "Nombre Recettes : " + CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.GetNumer();
                }else if (i == 5)
                {
                    string[] infos = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.GetProductToReload();
                    prodContainer.Content = new Utils.ItemList(infos, false).grille;
                }else if (i == 6)
                {
                    TextBox txt = new TextBox()
                    {
                        Text = "Entrez un id produit",
                        TextAlignment = TextAlignment.Center
                    };
                    txt.KeyDown += new KeyEventHandler(enterPressedTxt);
                    Grid.SetRow(txt, 6);
                    grille.Children.Add(txt);
                }
                i++;
            }
        }

        //le nom est trompeur c'est l'event déclenché quand on apuis sur entrée pour valide rla recherche produit
        private void enterPressedTxt(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {

                int result;
                if(Int32.TryParse((sender as TextBox).Text, out result)){
                    string[] prods = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.getRecettes(result.ToString());
                    Grid items = new Utils.ItemList(prods, false).grille;
                    Grid.SetRow(items, 0);
                    RecetteContainer.Content = items;
                }
            }
        }
    }
}
