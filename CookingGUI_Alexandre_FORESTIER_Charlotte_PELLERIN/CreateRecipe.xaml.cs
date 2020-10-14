using System;
using System.Collections.Generic;
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
using CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for CreateRecipe.xaml
    /// </summary>
    public partial class CreateRecipe : Window
    {
        Client clt;
        TextBox[] quantiteProd;
        string idcdr;
        string idclient;
        public CreateRecipe (string idcdr,Client clt, string idclient)
        {
            this.idclient = idclient;
            this.clt = clt;
            this.idcdr = idcdr;
            InitializeComponent();

            //on crée la liste des ingrédients
            string[] listProd = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.GetAll();
            Utils.ItemList listePro = new Utils.ItemList(listProd, true);
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = listePro.grille;
            quantiteProd = listePro.quantity;
        }
        /// <summary>
        /// Event déclenché lorsque l'on clique sur la validation de la recette
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //on récupère tous les produits dont la quantié est au moins égale à 1
            //on crée une recette
            int idRecette = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.Create(nom.Text, type.Text, desc.Text, prix.Text, url.Text, idcdr);
            for (int i = 0; i < quantiteProd.Length; i++)
            {
                int quantity;
                if (Int32.TryParse(quantiteProd[i].Text, out quantity))
                {
                    if (quantity > 0)
                    {
                        //on ajoute le prod à la recette
                        //on actualise la limite de stock du produit
                        CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.AjouterProduit(idRecette, i + 1, quantity);
                        CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.ActualiserLimiteStock((i + 1).ToString());
                        
                    }
                }
            }
            //on ferme l'ancienne fenêtre et on ouvre une fenêtre avec les infos actualisées
            string val = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.getCredentials(idclient);
            clt.Close();
            Client myC = new Client(val.Split('§'));
            myC.Show();
            this.Close();
        }
    }
}
