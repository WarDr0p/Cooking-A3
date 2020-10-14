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
    /// Interaction logic for InscClient.xaml
    /// </summary>
    public partial class InscClient : Window
    {
        public InscClient()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Event déclencher quand la personne valide son inscription
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nomF = "";
            string prenomF = "";
            string telF = "";
            string mdpF = "";

            if (nom.Text == "Nom")
            {
                ErrBox.Text = "Merci de rentrer un nom valide";
            }
            else if (prenom.Text == "Prenom")
            {
                ErrBox.Text = "Merci de rentrer un prenom valide";
            }
            else if(tel.Text == "Téléphone")
            {
                ErrBox.Text = "Merci de rentrer un téléphone valide";
            }
            else if(mdp.Text == "Mot de passe")
            {
                ErrBox.Text = "Merci de rentrer un mot de passe valide";
            }
            else
            {
                nomF = nom.Text;
                prenomF = prenom.Text;
                telF = tel.Text;
                mdpF = mdp.Text;
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.Create(nomF, prenomF, telF, mdpF.GetHashCode().ToString());
                ErrBox.Text = "Votre id est " + CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.GetID(nomF, prenomF, telF)+" gardez le tête";
            }
        }
    }
}
