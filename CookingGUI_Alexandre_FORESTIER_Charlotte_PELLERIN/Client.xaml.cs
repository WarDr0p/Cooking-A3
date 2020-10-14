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
using CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        string id_client;
        TextBlock[] quantite;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        public Client(string [] infos)
        {
            InitializeComponent();
            nom.Text += infos[1];
            id.Text += infos[0];
            nbCom.Text += infos[4];
            prenom.Text += infos[2];
            solde.Text = infos[3];
            
            id_client = infos[0];

            InitCom();
            
            //Génération de l'interface cdr
            InterfaceCdR interfaceCdr = new InterfaceCdR(id_client, CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.IsCdr(id_client),this);
            Grid.SetRow(interfaceCdr.grille, 1);
            cdr.Children.Add(interfaceCdr.grille);

            CommandList coms = new CommandList(id_client);
            passedCommand.Children.Add(coms.scroll);
        }

        /// <summary>
        /// Initialisation du conteneur de commandes
        /// </summary>
        void InitCom()
        {
            string[] tab = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.GetAll();
            RecipeList recette = new RecipeList(tab, true);
            Grid.SetRow(recette.scroll, 1);
            commandes.Children.Add(recette.scroll);
            quantite = recette.quantite;


            //RecipeList recetteP = new RecipeList(tab, false);
        }

        /// <summary>
        /// Event triggered lors de la validation d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValiderCom(object sender, RoutedEventArgs e)
        {
            List<int> quanti = new List<int>();
            List<string> id = new List<string>();
            int prix = 0;
            foreach (TextBlock texte in quantite)
            {
                //On récupère tous les recettes comandées au moins une fois
                int nombre = Convert.ToInt32(texte.Text);
                if (nombre > 0)
                {
                    quanti.Add(nombre);
                    id.Add(texte.Tag.ToString());
                    prix = prix +CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Commande.GetPrice(texte.Tag.ToString(), nombre);                    
                }
            }
            int solde = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.GetSolde(id_client);
            int debit = 0;
            //gestion du solde
            if(solde >= prix)
            {
                //si le solde client est supérieur ou égal au prix on enlève le montant correspondnat du prix
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.Debiter(id_client, prix);
                debit = prix;
            }
            else if(solde != 0)
            {
                //sinon on lui enlève tout ses crédit et on débite le reste sur sa CB
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.Debiter(id_client, solde);
                debit = solde;
            }
            //fenêtre disant que le paiement s'est bien passé
            Valid_Paiement pymt = new Valid_Paiement(prix, debit);
            pymt.Show();
            //On traite la commande
            CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Commande.Process(id_client,id, quanti);
            //raffiche une interface actualisée
            Client myC = new Client(CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.getCredentials(id_client).Split('§'));
            myC.Show();
            this.Close();
        }


    }
}
