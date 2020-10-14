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

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for Valid_Paiement.xaml
    /// </summary>
    public partial class Valid_Paiement : Window
    {
        public Valid_Paiement(int prix, int debit)
        {
            InitializeComponent();
            TextBlock paiement = new TextBlock();
            paiement.Text = "Nous avons bien enregistré votre commande, son montant est de " + prix + ", nous avons débité " + debit + " cook sur votre compte le reste à été payé par carte bancaire \n Si jamais certaines recettes que vous avez commandées sont indisponibles nous vous rembourserons le montant correspondant sous forme de cook";
            paiement.TextWrapping = TextWrapping.Wrap;
            grille.Children.Add(paiement);
        }
    }
}
