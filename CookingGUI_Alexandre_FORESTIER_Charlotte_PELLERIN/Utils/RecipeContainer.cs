using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class RecipeContainer
    {
        string id;
        public Grid Grille;
        public TextBlock quantite;


        /// <summary>
        /// Crée un contenneur de recette, celui-ci est utilisé pour les commandes passées
        /// </summary>
        /// <param name="tab">informations sur la recette</param>
        /// <param name="isCommanding">afficher au pas les boutons de commande (+,-,quantite)</param>
        /// <param name="quanti">Quantité commandée à afficher</param>
        public RecipeContainer(string[] tab, bool isCommanding,int quanti)
        {
            //On initialise le textblock pour afficher quantité de la recette
            Init(tab, isCommanding);
            quantite = new TextBlock();
            quantite.Text = quanti.ToString();
            quantite.HorizontalAlignment = HorizontalAlignment.Center;
            quantite.TextAlignment = TextAlignment.Center;
            Grid.SetRow(quantite, 2);
            Grid.SetColumn(quantite, 3);
            Grille.Children.Add(quantite);
            quantite.Tag = id;
        }

        /// <summary>
        /// Constructeur général utilisé pourl'affichage des recettes dans l'espace de commande
        /// </summary>
        /// <param name="tab">informations sur la recette</param>
        /// <param name="isCommanding">afficher oi pas les boutons de commande (+,-,quantite)</param>
        public RecipeContainer(string[] tab, bool isCommanding)
        {
            Init(tab, isCommanding);
        }

        /// <summary>
        /// Constructeur utilisé pour l'interface admin
        /// </summary>
        /// <param name="tab">informations sur la recette</param>
        /// <param name="isCommanding">toujours mettre à false</param>
        /// <param name="isAdminValidating">affiche le bouton de validation si true ou de suppression si false</param>
        public RecipeContainer(string[] tab, bool isCommanding, bool isAdminValidating)
        {
            //Initialisation
            Init(tab, isCommanding);

            //on a ajoute le bouton de commande 
            Button admin = new Button();
            admin.Tag = id;

            Grid.SetColumn(admin, 2);
            Grid.SetRow(admin, 3);
            Grid.SetColumnSpan(admin, 3);
            if (isAdminValidating)
            {
                admin.Content = "Valider";
            }
            else
            {
                admin.Content = "Supprimer";                
            }
            admin.Click += new RoutedEventHandler(AdminAction);
            Grille.Children.Add(admin);
            BitmapImage bitMapImage = new BitmapImage();

            //Affichage du prix de la recette et du logo coook
            bitMapImage.BeginInit();
            bitMapImage.UriSource = new Uri("Media/loco_cook.png", UriKind.Relative);
            bitMapImage.EndInit();
            Image Cook = new Image();
            Cook.Source = bitMapImage;
            Cook.Stretch = Stretch.Uniform;
            Cook.VerticalAlignment = VerticalAlignment.Center;
            Cook.HorizontalAlignment = HorizontalAlignment.Left;
            Cook.MaxHeight = 15;
            Cook.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Cook, 1);
            Grid.SetColumn(Cook, 4);
            Grille.Children.Add(Cook);
            TextBlock Prx = new TextBlock();
            TextBlock Prix = new TextBlock();
            Prx.Text = "Prix :";
            Prx.HorizontalAlignment = HorizontalAlignment.Right;
            Prx.VerticalAlignment = VerticalAlignment.Center;
            Prx.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Prx, 1);
            Grid.SetColumn(Prx, 2);
            Grille.Children.Add(Prx);
            Prix.Text = tab[4];
            Prix.HorizontalAlignment = HorizontalAlignment.Center;
            Prix.VerticalAlignment = VerticalAlignment.Center;
            Prix.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Prix, 1);
            Grid.SetColumn(Prix, 3);
            Grille.Children.Add(Prix);
        }

        /// <summary>
        /// Event déclenché lors du click d'un admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminAction(object sender, RoutedEventArgs e)
        {
            id = (sender as Button).Tag.ToString();
            string text = (sender as Button).Content.ToString();
            if (text == "Supprimer")
            {
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.DeleteRecette(id);
            }
            else
            {
                CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.ValiderRecette(id,"1");
            }
        }

        /// <summary>
        /// Initialisation de la strucutre générale du conteneur
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="isCommanding"></param>
        void Init(string[] tab, bool isCommanding)
        {
            //on déclare les champs
            this.id = tab[0];
            this.Grille = new Grid();
            TextBlock Name = new TextBlock();
            TextBlock Description = new TextBlock();
            TextBlock Numero = new TextBlock();
            TextBlock NbCommandes = new TextBlock();
            TextBlock Prx = new TextBlock();
            TextBlock Type = new TextBlock();
            TextBlock Prix = new TextBlock();
            Image Cook = new Image();
            Image ImageProduit = new Image();
            Button Plus = new Button();
            Button Moins = new Button();


            //On déclare les lignes et le colonnes
            for (int i = 0; i < 4; i++)
            {
                int taille = 2;
                if (i == 2) taille = 3;
                RowDefinition ligne = new RowDefinition();
                ligne.Height = new GridLength(taille, GridUnitType.Star);
                Grille.RowDefinitions.Add(ligne);
            }

            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition colonne = new ColumnDefinition();
                if (i == 0)
                {
                    colonne.Width = new GridLength(5, GridUnitType.Star);
                }
                else if (i == 1)
                {
                    colonne.Width = new GridLength(9, GridUnitType.Star);
                }
                else
                {
                    colonne.Width = new GridLength(10, GridUnitType.Auto);
                }


                Grille.ColumnDefinitions.Add(colonne);
            }

            Grille.Height = 80;
            Grille.Margin = new Thickness(10, 4, 10, 4);
            BitmapImage bitMapImage = new BitmapImage();

            //On affiche l'image avec un try catch si le liende l'image est mauvais on affiche un bol 
            bitMapImage.BeginInit();
            try
            {
                bitMapImage.UriSource = new Uri(tab[6]);
                bitMapImage.EndInit();
            }
            catch
            {
                bitMapImage.UriSource = new Uri("Media/Noimage.jpg", UriKind.Relative);
                bitMapImage.EndInit();
            }
            ImageProduit.Source = bitMapImage;
            ImageProduit.Stretch = Stretch.UniformToFill;
            ImageProduit.VerticalAlignment = VerticalAlignment.Center;
            ImageProduit.HorizontalAlignment = HorizontalAlignment.Center;
            ImageProduit.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(ImageProduit, 0);
            Grid.SetColumn(ImageProduit, 0);
            Grid.SetRowSpan(ImageProduit, 4);
            Grille.Children.Add(ImageProduit);

            //Nom de la recette
            Name.Text = tab[1];
            Name.VerticalAlignment = VerticalAlignment.Center;
            Name.HorizontalAlignment = HorizontalAlignment.Center;
            Name.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Name, 0);
            Grid.SetColumn(Name, 1);
            Grille.Children.Add(Name);

            //type de la recette
            Type.Text = tab[2];
            Type.VerticalAlignment = VerticalAlignment.Center;
            Type.HorizontalAlignment = HorizontalAlignment.Center;
            Type.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Type, 1);
            Grid.SetColumn(Type, 1);
            Grille.Children.Add(Type);

            //description de la recette
            Description.Text = tab[3];
            Description.VerticalAlignment = VerticalAlignment.Center;
            Description.HorizontalAlignment = HorizontalAlignment.Center;
            Description.TextWrapping = TextWrapping.Wrap;
            Description.TextAlignment = TextAlignment.Justify;
            Description.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Description, 2);
            Grid.SetColumn(Description, 1);
            Grille.Children.Add(Description);

            //numéro de la recettte
            Numero.Text = "Numéro Recette : " + tab[0];
            Numero.HorizontalAlignment = HorizontalAlignment.Center;
            Numero.VerticalAlignment = VerticalAlignment.Center;
            Numero.TextWrapping = TextWrapping.Wrap;
            Numero.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(Numero, 3);
            Grid.SetColumn(Numero, 1);
            Grille.Children.Add(Numero);

            //total de commande de la recettte
            NbCommandes.Text = "Total commandes : " + tab[5];
            NbCommandes.HorizontalAlignment = HorizontalAlignment.Center;
            NbCommandes.VerticalAlignment = VerticalAlignment.Center;
            NbCommandes.TextWrapping = TextWrapping.Wrap;
            NbCommandes.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetRow(NbCommandes, 0);
            Grid.SetColumn(NbCommandes, 2);
            Grid.SetColumnSpan(NbCommandes, 3);
            Grille.Children.Add(NbCommandes);

            //Affichage de l'interface de commande (+,-,quantité)
            if (isCommanding)
            {

                bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri("Media/loco_cook.png", UriKind.Relative);
                bitMapImage.EndInit();
                Cook.Source = bitMapImage;


                Cook.Stretch = Stretch.Uniform;
                Cook.VerticalAlignment = VerticalAlignment.Center;
                Cook.HorizontalAlignment = HorizontalAlignment.Left;
                Cook.MaxHeight = 15;
                Cook.Margin = new Thickness(2, 2, 2, 2);
                Grid.SetRow(Cook, 1);
                Grid.SetColumn(Cook, 4);
                Grille.Children.Add(Cook);

                Plus.Content = "+";
                Plus.HorizontalAlignment = HorizontalAlignment.Center;
                Plus.VerticalAlignment = VerticalAlignment.Center;
                Plus.HorizontalContentAlignment = HorizontalAlignment.Center;
                Plus.VerticalContentAlignment = VerticalAlignment.Center;
                Plus.Margin = new Thickness(2, 2, 2, 2);
                Plus.Click += new RoutedEventHandler(increase);
                Grid.SetRow(Plus, 2);
                Grid.SetColumn(Plus, 4);
                Plus.Width = 20;
                Plus.Height = 20;
                Grille.Children.Add(Plus);

                Moins.Content = "-";
                Moins.Margin = new Thickness(2, 2, 2, 2);
                Moins.HorizontalAlignment = HorizontalAlignment.Center;
                Moins.VerticalAlignment = VerticalAlignment.Center;
                Moins.HorizontalContentAlignment = HorizontalAlignment.Center;
                Moins.VerticalContentAlignment = VerticalAlignment.Center;
                Moins.Click += new RoutedEventHandler(decrease);
                Moins.Height = 20;
                Moins.Width = 20;
                Grid.SetRow(Moins, 2);
                Grid.SetColumn(Moins, 2);
                Grille.Children.Add(Moins);

                quantite = new TextBlock();
                quantite.Text = "0";
                quantite.HorizontalAlignment = HorizontalAlignment.Center;
                quantite.TextAlignment = TextAlignment.Center;
                Grid.SetRow(quantite, 2);
                Grid.SetColumn(quantite, 3);
                Grille.Children.Add(quantite);
                quantite.Tag = id;

                Prx.Text = "Prix :";
                Prx.HorizontalAlignment = HorizontalAlignment.Right;
                Prx.VerticalAlignment = VerticalAlignment.Center;
                Prx.Margin = new Thickness(2, 2, 2, 2);
                Grid.SetRow(Prx, 1);
                Grid.SetColumn(Prx, 2);
                Grille.Children.Add(Prx);

                Prix.Text = tab[4];
                Prix.HorizontalAlignment = HorizontalAlignment.Center;
                Prix.VerticalAlignment = VerticalAlignment.Center;
                Prix.Margin = new Thickness(2, 2, 2, 2);
                Grid.SetRow(Prix, 1);
                Grid.SetColumn(Prix, 3);
                Grille.Children.Add(Prix);
            }
        }

        /// <summary>
        /// Event déclenché lors du click sur le bouton +
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decrease(object sender, RoutedEventArgs e)
        {
            quantite.Text = (Convert.ToInt32(quantite.Text) - 1).ToString();
            if (Convert.ToInt32(quantite.Text) < 0) quantite.Text ="0";
        }

        /// <summary>
        /// Event déclenché lors du click sur le bouton -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increase(object sender, RoutedEventArgs e)
        {
            quantite.Text = (Convert.ToInt32(quantite.Text) + 1).ToString();
        }

        public string Id
        {
            get { return id; }
        }

        

    }
}
