using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class Produit
    {
        string idFournisseur;
        string idProd;
        string nom;
        string quantite;

        /// <summary>
        /// Constructeur pour sérialiser
        /// </summary>
        /// <param name="idF"></param>
        /// <param name="idPro"></param>
        /// <param name="nom"></param>
        /// <param name="quantity"></param>
        public Produit(string idF, string idPro, string nom, string quantity)
        {
            idFournisseur = idF;
            idProd = idPro;
            this.nom = nom;
            quantite = quantity;
        }
        /// <summary>
        /// COnstructeur pour désérialiser
        /// </summary>
        public Produit()
        {

        }


        //===================================Accesseurs pour le serialiser============================//
        public string IdFournisseur
        {
            get { return idFournisseur; }
            set { idFournisseur = value; }
        }
        public string IdProduit
        {
            get { return idProd; }
            set { idProd = value; }
        }
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public string Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }
        //===================================Accesseurs Fin============================//

        /// <summary>
        /// Retourne les produits dont le stock est inférieur au minimum
        /// </summary>
        /// <returns>retourne un tableau avec les champs séparés par des §</returns>
        public static string[] GetProductToReload()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Produits where quantite_stock < min_stock order by id_fournisseur, Numero_Produit; ");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] result = new string[size];
            sql.Request("select id_fournisseur, Numero_Produit, nom_produit,Categorie_Produit,max_stock-quantite_stock from Produits where quantite_stock<min_stock order by id_fournisseur,Numero_Produit;");
            int i = 0;
            while (sql.reader.Read())
            {
                result[i] = sql.reader.GetString(0) + "§" + sql.reader.GetString(1) + "§" + sql.reader.GetString(2) + "§" + sql.reader.GetString(3)+"§"+ sql.reader.GetString(4);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Exporte le fichier de commande
        /// </summary>
        /// <param name="path">Non du fichier à exporter</param>
        public static void ExportProductToReload(string path)
        {
            DivideUnusedProducts();
            string[] myProducts = Produit.GetProductToReload();
            List<Produit> prod = new List<Produit>();
            foreach(string e in myProducts)
            {
                Console.WriteLine(e);
                string[] vals = e.Split('§');
                prod.Add(new Produit(vals[0], vals[1], vals[2], vals[4]));
            }
            

            XmlSerializer serializer = new XmlSerializer(prod.GetType(), new XmlRootAttribute("Commande"));
            path = path + "Commande";
            string finalpath = path+ ".xml";
            int i = 1;
            while (File.Exists(finalpath))
            {
                finalpath = path + "(" + i.ToString() + ").xml";
                i++;
            }           
            StreamWriter writer = new StreamWriter(finalpath);
            serializer.Serialize(writer.BaseStream, prod);
            writer.Close();
        }
        /// <summary>
        /// Retourne une liste de tous les produits 
        /// </summary>
        /// <returns>Tableau produit par produit les champs sont séparés par des §</returns>
        public static string[] GetAll()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Produits");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] tab = new string[size];
            sql.Request("select Numero_Produit,nom_produit,Categorie_Produit,unite from Produits");
            int i = 0;
            while (sql.reader.Read())
            {

                //string substring = "";
                tab[i] = sql.reader.GetString(0);
                for (int j = 1; j < sql.reader.FieldCount; j++)
                {
                    tab[i] = tab[i] + "§" + sql.reader.GetString(j);
                }
                tab[i] = tab[i] + "§0";
                i++;
            }
            sql.Close();
            return tab;
        }

        /// <summary>
        /// Retourne la liste de tous les produits avec la quantité en stock
        /// </summary>
        /// <returns>Tableau produit par produit les champs sont séparés par des §</returns>
        public static string[] GetAllWithQuantity()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Produits");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] tab = new string[size];
            sql.Request("select Numero_Produit,nom_produit,Categorie_Produit,unite,quantite_stock from Produits");
            int i = 0;
            while (sql.reader.Read())
            {
                //string substring = "";
                tab[i] = "";
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    tab[i] = tab[i] + "§" + sql.reader.GetString(j);
                }
                ;
                i++;
            }
            sql.Close();
            return tab;
        }

        /// <summary>
        ///  Ajoute une quantité de produit à la bdd
        /// </summary>
        /// <param name="idProduit">id du produit a recharger</param>
        /// <param name="quantite">quantité à ajouter</param>
        public static void AjouterStock(string idProduit,int quantite)
        {

            SQLUser sql = new SQLUser();
            sql.Request("update Produits set quantite_stock = quantite_stock + " + quantite + " , derniere_util = NOW() where Numero_Produit = \""+idProduit+"\"");
            sql.Close();
        }

        public static string[] GetAllAdmin()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Produits");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] tab = new string[size];
            sql.Request("select Numero_Produit,nom_produit,Categorie_Produit,unite,quantite_stock,min_stock,max_stock,derniere_util from Produits");
            int i = 0;
            while (sql.reader.Read())
            {
                //string substring = "";
                tab[i] = "";
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    tab[i] = tab[i] + "§" + sql.reader.GetString(j);
                }
                ;
                i++;
            }
            sql.Close();
            return tab;
        }

        /// <summary>
        /// Retire une quantité de produit de la bdd
        /// </summary>
        /// <param name="idProduit">id du produit à retirer</param>
        /// <param name="quantite">quantité à retirer</param>
        public static void RetirerStock(string idProduit, int quantite)
        {

            SQLUser sql = new SQLUser();
            sql.Request("update Produits set quantite_stock = quantite_stock - " + quantite + " , derniere_util = NOW() where Numero_Produit = \"" + idProduit + "\"");
            sql.Close();
        }

        /// <summary>
        /// Actualise le limite de stock d'un produit
        /// </summary>
        /// <param name="idProduit">id du produit dont il faut recalculer la limite de stock</param>
        public static void ActualiserLimiteStock(string idProduit)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select sum(quantite_ingredient) from Utilise where numero_produit = \"" + idProduit+"\" group by numero_produit");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            int min = 3 * result;
            int max = 6 * result;
            sql.Request("update Produits set min_stock=\"" + min + "\" , max_stock=\""+max+ "\" where numero_produit =\"" + idProduit + "\"");
            sql.Close();
        }

        /// <summary>
        /// Permet de recharger le stock de produits avec le fichier précisé en paramètre
        /// </summary>
        /// <param name="filepath">chemin d'accès au fichier</param>
        public static void ImportCommande(string filepath)
        {
            List<Produit> custList = new List<Produit>();

            XmlSerializer serializer = new XmlSerializer(custList.GetType(), new XmlRootAttribute("Commande"));
            StreamReader re = new StreamReader(filepath);
            custList = (List<Produit>)serializer.Deserialize(re);

            foreach(Produit produit in custList)
            {
                int stock = GetStock(produit.idProd);
                
                stock = stock + Convert.ToInt32(produit.quantite);
                Console.WriteLine(produit.quantite +" "+stock );
                RefreshStock(produit.idProd, stock.ToString());
            }
        }

        /// <summary>
        /// Actualise la quantité d'un produit à la valeur précisée
        /// </summary>
        /// <param name="idProduit">id du produti dont le stock doit être modifié</param>
        /// <param name="quantite">quantité du produit</param>
        static void RefreshStock(string idProduit,string quantite)
        {
            ActualiserDateUtil(idProduit);
            SQLUser sql = new SQLUser();
            sql.Request("update Produits set quantite_stock =\"" + quantite + "\" where Numero_Produit=\"" + idProduit + "\"");
            sql.Close();
        }

        /// <summary>
        /// Permet de récupérer le stock d'un produit
        /// </summary>
        /// <param name="idProduit">id du produit</param>
        /// <returns></returns>
        public static int GetStock(string idProduit)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select quantite_stock from Produits where Numero_Produit =\"" + idProduit + "\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Actualise la date d'utilisation d'un produit à maintenant
        /// </summary>
        /// <param name="idProduit"></param>
        static void ActualiserDateUtil(string idProduit)
        {
            SQLUser sql = new SQLUser();
            sql.Request("update Produits set derniere_util =NOW() where Numero_Produit=\"" + idProduit + "\"");
            sql.Close();
        }

        /// <summary>
        /// Divise la quantité de produit par 2 si il n' a pas été utilisé depuis 2 semaines
        /// </summary>
        static void DivideUnusedProducts()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select Numero_Produit from Produits where datediff(NOW(),date(derniere_util))>13");
            string id = "";
            string idPro = "";
            while (sql.reader.Read())
            {
                idPro = sql.reader.GetString(0);
            }
            sql.Close();
            sql.Request("update Produits set quantite_stock = quantite_stock / 2 where Numero_Produit =\""+idPro+"\"");
            sql.Close();
            
        }

        /// <summary>
        /// Retourne toutes les recettes qui composent un produit
        /// </summary>
        /// <param name="idPro"></param>
        /// <returns></returns>
        public static string[] getRecettes(string idPro)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select R.nom_recette, P.nom_produit, U.quantite_ingredient from Utilise U join Produits P on P.numero_produit = U.numero_produit join Recette R on R.numero_recette = U.numero_recette where P.numero_Produit = \""+idPro+"\"");
            List<string> prods = new List<string>();
            while (sql.reader.Read())
            {
                string result = "";
                for(int i = 0; i < sql.reader.FieldCount; i++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
                prods.Add(result);
            }
            sql.Close();
            return prods.ToArray();
        }
    }
}
