using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            StreamWriter writer = new StreamWriter("Commande.txt");
            writer.Write("test");
            writer.Close();
            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        /// <summary>
        /// Event déclenché quand la person valide sa connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton(object sender, RoutedEventArgs e)
        {
            string numero = id.Text;
            int idClient;
            bool inputValid = int.TryParse(numero, out idClient);
            if (inputValid)
            {
                if (Sldr.Value == 0)
                {//il s'agit dun client on le connecte
                    if (CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.Login(idClient, mdp.Password.GetHashCode().ToString()))
                    {
                        string val = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.getCredentials(idClient.ToString());
                        Client myC = new Client(val.Split('§'));
                        myC.Show();
                        this.Close();
                    }
                    else
                    {
                        ErrorBlock.Text = "Informations invalides";
                    }
                }
                else
                {
                    //il s'agit d'un admin
                    Trace.WriteLine(mdp.Password.GetHashCode().ToString());
                    if (CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.Login(idClient, mdp.Password.ToString().GetHashCode().ToString()))
                    {
                        Admin admin = new Admin(idClient.ToString());
                        admin.Show();
                        this.Close();
                    }
                    else
                    {
                        ErrorBlock.Text = "Informations invalides";
                    }
                }
            }
            else
            {
                ErrorBlock.Text = "Merci de rentrer un nombre";
            }
        }

        /// <summary>
        /// Event déclenché pour lancer la fenêtre d'inscription
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            InscClient inscription = new InscClient();
            inscription.Show();
        }

        /// <summary>
        /// Event déclenché pour lancer la demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchDemo(object sender, RoutedEventArgs e)
        {
            new Demo().Show();
        }
    }
}
