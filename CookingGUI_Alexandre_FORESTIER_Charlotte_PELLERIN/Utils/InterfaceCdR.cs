using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN.Utils
{
    class InterfaceCdR
    {
        public Grid grille;
        string id;
        string idcdr;
        Client client;
        public InterfaceCdR(string id,bool cdr, Client client)
        {
            refreshGrille(id,cdr);
            this.client = client;
        }

        void refreshGrille(string id, bool cdr)
        {
            grille = new Grid();
            this.id = id;
            if (!cdr)
            {
                //si le client n'est pas cdr
                //on crée 2 ligne une pour le message lui disant qu'il est pas cdr et une deuxième pour mettre le bouton
                //de requête cdr si il n' a pas déjà demandé
                for (int i = 0; i < 2; i++)
                {
                    RowDefinition ligne = new RowDefinition();
                    ligne.Height = new GridLength(1, GridUnitType.Star);
                    grille.RowDefinitions.Add(ligne);
                }
                //on crée le texte
                TextBlock txt = new TextBlock();
                txt.Text = "Vous n'êtes pas un créateur de recette";
                txt.VerticalAlignment = VerticalAlignment.Bottom;
                txt.HorizontalAlignment = HorizontalAlignment.Center;
                txt.TextAlignment = TextAlignment.Center;
                txt.TextWrapping = TextWrapping.Wrap;

                //si il a demandé le cdr on lui dit que la demande est enregistrée
                if (CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.hasRequestedCdr(id))
                {
                    txt.Text = txt.Text + "\n Nous avons enregistré votre demande un administrateur va rapidement revenir vers vous :)";

                }
                else
                {
                    //si il n' a pas fait sa demande on lui affiche le bouton pour faire une demande
                    Button btn = new Button();
                    btn.Content = "Cliquez sur ce bouton si vous voulez devenir cdr";
                    btn.Click += new RoutedEventHandler(On_click);
                    btn.Height = 20;
                    btn.Width = 150;
                    btn.VerticalAlignment = VerticalAlignment.Top;
                    btn.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(btn, 1);
                    grille.Children.Add(btn);

                }
                Grid.SetRow(txt, 0);
                grille.Children.Add(txt);
            }
            else
            {

                //Ici c'est que le client est cdr on recup ses infos et on crée l'interface 
               idcdr = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.GetId(id).ToString();
                //Création des lignes
                for (int i = 0; i < 5; i++)
                {
                    RowDefinition ligne = new RowDefinition();
                    ligne.Height = new GridLength(1, GridUnitType.Auto);
                    if(i==2 || i == 4)
                    {
                        ligne.Height = new GridLength(1, GridUnitType.Star);
                    }
                    grille.RowDefinitions.Add(ligne);
                }

                Button btn = new Button();
                btn.Content = "Créer une recette";
                btn.Click += new RoutedEventHandler(CrerRec);
                Grid.SetRow(btn, 0);
                grille.Children.Add(btn);

                TextBlock txtRecVal= new TextBlock();
                txtRecVal.Text = "Vos recettes validées :";
                txtRecVal.HorizontalAlignment = HorizontalAlignment.Stretch;
                txtRecVal.TextAlignment = TextAlignment.Center;
                Grid.SetRow(txtRecVal, 1);
                grille.Children.Add(txtRecVal);
                
                
                //Création de la liste des reecttes validées du cdr
                string[] listeRecetteValidee = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.getValidRecipe(idcdr);
                RecipeList recetteValidees = new RecipeList(listeRecetteValidee, false);
                Grid.SetRow(recetteValidees.scroll, 2);
                grille.Children.Add(recetteValidees.scroll);


                //Création de la liste des recettes non validées du cdr
                TextBlock txtRecNoVal = new TextBlock();
                txtRecNoVal.Text = "Vos recettes non validées :";
                txtRecNoVal.HorizontalAlignment = HorizontalAlignment.Stretch;
                txtRecNoVal.TextAlignment = TextAlignment.Center;
                Grid.SetRow(txtRecNoVal, 3);
                grille.Children.Add(txtRecNoVal);
                
                string[] listeRecetteNoValidee = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.getUnvalidRecipe(idcdr);
                RecipeList recetteNoValidees = new RecipeList(listeRecetteNoValidee, false);
                Grid.SetRow(recetteNoValidees.scroll, 4);
                grille.Children.Add(recetteNoValidees.scroll);

            }
        }

        /// <summary>
        /// Event déclenché pour l'ouverture de la fenêtre de création des recettes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrerRec(object sender, RoutedEventArgs e)
        {
            CreateRecipe newRecipe = new CreateRecipe(idcdr,client,id);
            newRecipe.Show();
        }

        /// <summary>
        /// Event déclenché lorsque le client fait sademande de CdR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void On_click(object sender, RoutedEventArgs e)
        {
            CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.RequestCdr(id);
            string val = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.getCredentials(id);
            Client myC = new Client(val.Split('§'));
            myC.Show();
            client.Close();
        }
    }
}
