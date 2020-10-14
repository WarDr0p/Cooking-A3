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
using Microsoft.Win32;

namespace CookingGUI_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {

        public Admin(string id)
        {
            InitializeComponent();
            nom.Text += CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.GetName(id);
            idText.Text += id;
            InitRecVal();
            InitRecNonVal();
            InitCdrVal();
            InitCdrNonVal();
            InitProd();
            InitProdManquant();
            InitXmlExporter();
            InitDashBoard();
        }

        /// <summary>
        /// Initialise la liste des recettes non validées
        /// </summary>
        void InitRecNonVal()
        {
            string[] NovalRec = Recette.getUnvalidRecipe();
            Utils.RecipeList NoValider = new Utils.RecipeList(NovalRec, false, true);
            Grid.SetRow(NoValider.scroll, 3+1);
            GesRecGrid.Children.Add(NoValider.scroll);
        }

        /// <summary>
        /// Initialise la liste des Recettes validées
        /// </summary>
        void InitRecVal()
        {
            InitTxt(4);
            InitTxt(5);
            string[] valRec = Recette.getValidRecipe();
            Utils.RecipeList aValider = new Utils.RecipeList(valRec, false, false);
            Grid.SetRow(aValider.scroll, 1+1);
            GesRecGrid.Children.Add(aValider.scroll);
            GesRecGrid.Children.Add(RefreshButton("rec"));
        }

        /// <summary>
        /// Initialise la liste des cdr val
        /// </summary>
        void InitCdrNonVal()
        {
            string[] listNoVal = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.getPendingCdr();
            Utils.UserList listCdrNoVal = new Utils.UserList(listNoVal, true);
            Grid.SetRow(listCdrNoVal.scroll, 3+1);
            cdrGrid.Children.Add(listCdrNoVal.scroll);

        }

        /// <summary>
        /// Initialise le tableau des cdr validés
        /// </summary>
        void InitCdrVal()
        {
            cdrGrid.Children.Add(RefreshButton("cdr"));
            InitTxt(3);
            InitTxt(4);
            string[] listVal = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.CdR.getValidCdr();
            Utils.UserList listCdrVal = new Utils.UserList(listVal, false);
            Grid.SetRow(listCdrVal.scroll, 1+1);
            cdrGrid.Children.Add(listCdrVal.scroll);
        }

        /// <summary>
        /// Initialise le tableau des produits 
        /// </summary>
        void InitProd()
        {
            string[] listVal = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.GetAllAdmin();
            Utils.ItemList liste = new Utils.ItemList(listVal, false);
            ScrollViewer scroll = new ScrollViewer() {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = liste.grille
            };
            Grid.SetRow(scroll, 1+1);
            GesProdGrid.Children.Add(scroll);
            InitTxt(0);
            InitTxt(1);
            GesProdGrid.Children.Add(RefreshButton("prod"));
        }

        /// <summary>
        /// Initialise le tableau des produits manquants
        /// </summary>
        void InitProdManquant()
        {
            string[] listVal = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.GetProductToReload();
            Utils.ItemList liste = new Utils.ItemList(listVal, false);
            ScrollViewer scroll = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = liste.grille
            };
            Grid.SetRow(scroll, 3+1);
            GesProdGrid.Children.Add(scroll);
        }

        /// <summary>
        /// Initialise toutes les textBlock de l'interface
        /// </summary>
        /// <param name="i"></param>
        void InitTxt(int i)
        {
            string texte = "";
            if (i == 0) texte = "Liste Produits";
            if (i == 1) texte = "Liste Produits Manquants";
            if (i == 2) texte = "Liste Cdr";
            if (i == 3) texte = "Liste d'attente Cdr";
            if (i == 4) texte = "Liste recettes";
            if (i == 5) texte = "Liste recettes non validées";
            if (i == 6) texte = "";
            if (i == 7) texte = "";
            TextBlock txt = new TextBlock()
            {
                Text = texte,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };
            int ligne = 2;
            if (i % 2 == 0)
            {
                ligne = 0;
            }
            Grid.SetRow(txt, ligne + 1);
            if (i <= 1)
            {
                GesProdGrid.Children.Add(txt);
            }
            else if (i <= 3)
            {
                cdrGrid.Children.Add(txt);
            }
            else if (i <= 5)
            {
                GesRecGrid.Children.Add(txt);
            }

        }

        /// <summary>
        /// Initialise le bouton d'export du fichier xml
        /// </summary>
        void InitXmlExporter()
        {
            Button btn1 = new Button()
            {
                Content = "Exporter la commande",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                               
            };
            btn1.Click += new RoutedEventHandler(ExportCom);
            Grid.SetRow(btn1,4+1);
            GesProdGrid.Children.Add(btn1);

            Button btn2 = new Button()
            {
                Content = "Importer une commande",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            btn2.Click += new RoutedEventHandler(ImportCom);
            Grid.SetRow(btn2, 5+1);
            GesProdGrid.Children.Add(btn2);
        }

        /// <summary>
        /// Event handler déclenché lors de l'export d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportCom(object sender, RoutedEventArgs e)
        {
            CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Produit.ExportProductToReload("");
        }

        /// <summary>
        /// Fonction déclenchée pour l'import d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImportCom(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                DefaultExt = ".xml",
                Filter = "Document XML (.xml)|*.xml"
                //Filter = "Fichier xml (.xml)|*.xml"
            };
            if (dlg.ShowDialog() == true)
            {
                Produit.ImportCommande(dlg.FileName);
            }
        }

        /// <summary>
        /// Crée un bouton de rafraichissement
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        private Button RefreshButton(string window)
        {
            WrapPanel wrapper = new WrapPanel();


            TextBlock txt = new TextBlock()
            {
                Text = "Refresh"
            };

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("Media/Refresh.png", UriKind.Relative);
            img.EndInit();
            Image refresh = new Image()
            {
                Source = img,
                Stretch = Stretch.Uniform
            };
            //Grid.SetRow(refresh, 0);

            wrapper.Children.Add(txt);
            wrapper.Children.Add(refresh);

            Button Btn = new Button()
            {
                Content = wrapper,
                Tag = window                
            };
            Btn.Click += new RoutedEventHandler(Refresh);
            Grid.SetRow(Btn, 0);

            return Btn;
        }

        /// <summary>
        /// Fonction appelée lors du click d'un resfresh button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, RoutedEventArgs e)
        {
            string window = (sender as Button).Tag.ToString();
            if(window == "prod")
            {
                GesProdGrid.Children.Clear();
                InitProd();
                InitProdManquant();
                InitXmlExporter();
            }else if(window == "rec")
            {
                GesRecGrid.Children.Clear();
                InitRecNonVal();
                InitRecVal();
            }
            else if (window == "cdr")
            {
                cdrGrid.Children.Clear();
                InitCdrNonVal();
                InitCdrVal();
            }
        }

        /// <summary>
        /// Initalise le DashBoard
        /// </summary>
        void InitDashBoard()
        {
            
            Grid grille = new Grid();
            for (int i = 0; i < 8; i++)
            {
                RowDefinition ligne = new RowDefinition()
                {
                    Height = new GridLength(0, GridUnitType.Auto)
                };
                grille.RowDefinitions.Add(ligne);
                
                string idTopCdr = "1";
                if (i % 2 == 0)
                {
                    TextBlock Txt = new TextBlock()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center
                    };
                    if (i == 0) Txt.Text = "Cdr of the week :";
                    if (i == 2) Txt.Text = "Top 5 recette";
                    if (i == 4) Txt.Text = "Meilleur Cdr :";
                    if (i == 6) Txt.Text = "Top 5 Recettes meilleur cdr";
                    Grid.SetRow(Txt, i);
                    grille.Children.Add(Txt);
                }
                else
                {
                    
                    if (i == 1)
                    {
                        Grid cdrWeek = new Utils.ItemContainer(CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.CdrOfTheWek().Split('§'), false, 1).grille;
                        Grid.SetRow(cdrWeek, i);
                        grille.Children.Add(cdrWeek);
                    }else if(i == 3)
                    {
                        string[] recettes = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.TopRecette().ToArray();
                        ScrollViewer topRec = new Utils.RecipeList(recettes, false).scroll;
                        Grid.SetRow(topRec, i);
                        grille.Children.Add(topRec);
                    }else if(i == 5)
                    {
                        string[] infos = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.CdrDor().Split('§');
                        idTopCdr = infos[0];
                        Grid topCdr = new Utils.ItemContainer(infos, false, 1).grille;
                        Grid.SetRow(topCdr, i);
                        grille.Children.Add(topCdr);
                    }
                    else if(i == 7)
                    {
                        string[] recettes = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Admin.TopRecetteCdr(idTopCdr).ToArray();
                        ScrollViewer topRec = new Utils.RecipeList(recettes, false).scroll;
                        Grid.SetRow(topRec, i);
                        grille.Children.Add(topRec);
                    }
                }
                ScrollViewer scroller = new ScrollViewer();
                scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scroller.Content = grille;
                dashboard.Children.Add(scroller);
            }
        }
    }
}
