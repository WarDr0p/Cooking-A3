using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class Recette
    {
        /// <summary>
        /// Calcule le nombre de recettes présentes dans la bdd
        /// </summary>
        /// <returns>nombre de recttes dans la bdd</returns>
        public static int GetNumer()
        {
            SQLUser sql = new SQLUser();
            sql.Request("Select count(*) from Recette");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            return result;
        }



        /// <summary>
        /// Récupère toutes les recettes validées ainsi que leurs informations
        /// </summary>
        /// <returns></returns>
        public static string[] GetAll()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Recette natural join Valide_Recette");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette  from Recette natural join Valide_Recette");
            string[] result = new string[size];
            int i = 0;
            while (sql.reader.Read())
            {
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result[i] = result[i] + "§" + sql.reader.GetValue(j).ToString();
                }
                i++;
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Récupère les données de la recette précisée en paramètre
        /// </summary>
        /// <param name="idRec"></param>
        /// <returns></returns>
        public static string GetRecipe(string idRec)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette  from Recette natural join Valide_Recette where numero_recette = \""+idRec+"\"");
            string result = "";
            int i = 0;
            while (sql.reader.Read())
            {
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result = result + "§" + sql.reader.GetValue(j).ToString();
                }
                i++;
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Crée la recette avec les infos précisées en paramètre
        /// </summary>
        /// <param name="nom">nom de la recette</param>
        /// <param name="type">type de la recette</param>
        /// <param name="desc">description de la recette</param>
        /// <param name="prix">prix de la recette</param>
        /// <param name="url_photo">photo de la recette</param>
        /// <param name="idCdr">id du cdr ayant créé la recette</param>
        /// <returns></returns>
        public static int Create(string nom,string type, string desc, string prix, string url_photo, string idCdr)
        {
            SQLUser sql = new SQLUser();
            sql.Request("insert into Recette(nom_recette,type_recette,description_recette,prix_recette,photo_recette,id_cdr) values(\"" + nom+ "\",\"" + type+ "\",\"" + desc+ "\",\"" + prix+ "\",\"" + url_photo+ "\",\"" + idCdr+"\")");
            sql.Close();
            sql.Request("select max(numero_recette) from Recette");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Ajoute le produit précisé en paramètre dans la composition de la recette
        /// </summary>
        /// <param name="idRecette">id de la recette</param>
        /// <param name="idProduit">id du produit contenant la recette</param>
        /// <param name="quantite">quantité du produit</param>
        public static void AjouterProduit(int idRecette, int idProduit, int quantite)
        {
            SQLUser sql = new SQLUser();           
            sql.Request("insert into Utilise(numero_recette,numero_produit,quantite_ingredient) values(\""+idRecette+ "\",\"" + idProduit+ "\",\"" + quantite+ "\")");
            sql.Close();
        }

        /// <summary>
        /// Retourne la liste des recette validées
        /// </summary>
        /// <returns></returns>
        public static string[] getValidRecipe()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Recette natural join Valide_Recette");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette  from Recette natural join Valide_Recette");
            string[] result = new string[size];
            int i = 0;
            while (sql.reader.Read())
            {
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result[i] = result[i] + "§" + sql.reader.GetValue(j).ToString();
                }
                i++;
            }
            sql.Close();
            if (result == null)
            {
                return new string[] { "0§0§0§0§0§0§0" };
            }
            return result;
        }

        /// <summary>
        /// Retourne la liste des recette non validées
        /// </summary>
        /// <returns></returns>
        public static string[] getUnvalidRecipe()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Recette where numero_recette not in (select numero_recette from Recette natural join Valide_Recette)");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette from Recette where numero_recette not in (select numero_recette from Recette natural join Valide_Recette)");
            string[] result = new string[size];
            int i = 0;
            while (sql.reader.Read())
            {
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result[i] = result[i] + "§" + sql.reader.GetValue(j).ToString();
                }
                i++;
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Vérifie que la recette peut être réalisée dans la quantité indiquée
        /// </summary>
        /// <param name="idRecette">id de la recette</param>
        /// <param name="quantite">quantité de la recettte</param>
        /// <returns></returns>
        public static bool isPossible(string idRecette, int quantite)
        {
            List<string> idPro = new List<string>();
            List<int> quantitePro = new List<int>();
            SQLUser sql = new SQLUser();
            sql.Request("select numero_produit,quantite_ingredient from Utilise where numero_recette = \"" + idRecette + "\"");
            while (sql.reader.Read())
            {
                Console.WriteLine(sql.reader.GetString(0) + " , " + sql.reader.GetInt32(1) * quantite);
                idPro.Add(sql.reader.GetString(0));
                quantitePro.Add(sql.reader.GetInt32(1)*quantite);
            }
            for(int i = 0; i < idPro.Count; i++)
            {
                int stock = Produit.GetStock(idPro[i]);
                Console.WriteLine(quantitePro[i] + " , " + stock);
                if (quantitePro[i] > stock)
                {
                    Console.WriteLine("false");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retire la quantité correspondante de tous les ingrédients d'une recette 
        /// </summary>
        /// <param name="idRecette">id de la recette</param>
        /// <param name="quantite">quantité</param>
        public static void RefreshProducts(string idRecette, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select numero_produit,quantite_ingredient from Utilise where numero_recette = \"" + idRecette + "\"");
            List<string> liste = new List<string>();
            while (sql.reader.Read())
            {
                liste.Add(sql.reader.GetString(0) + "§"+sql.reader.GetString(1));
            }
            sql.Close();
            foreach(string prod in liste)
            {
                string[] vals = prod.Split('§');
                Produit.RetirerStock(vals[0], Convert.ToInt32(vals[1])*quantite);
            }
        }

        /// <summary>
        /// Rajoute la quantité correspondante de commande associées à une recette
        /// </summary>
        /// <param name="idRec">id de la recette</param>
        /// <param name="quantite">quantité à ajouter</param>
        public static void ActualiserNbCom(string idRec, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("update Recette set nombre_com_recette = nombre_com_recette +"+quantite+ " where numero_recette = \""+idRec+"\"");
            sql.Close();
        }

        /// <summary>
        /// Rémunère le dcr de la recette
        /// </summary>
        /// <param name="idRec">id recette</param>
        /// <param name="quantite">quatité commandée</param>
        public static void RemunererCdr(string idRec, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select id_cdr, nombre_com_recette from Recette where numero_recette = \"" + idRec+"\"");
            string idCdr = "";
            int nbCom = 0;
            while (sql.reader.Read())
            {
                idCdr = sql.reader.GetString(0);
                nbCom = sql.reader.GetInt32(1);
            }
            sql.Close();
            int montant = 0;
            if (nbCom < 50)
            {
                montant = 2 * quantite;
            }
            else
            {
                montant = 4 * quantite;
            }
            sql.Request("update Client set solde_client = solde_client + " + montant + " where id_client =\"" + idCdr + "\"");
            sql.Close();
        }

        /// <summary>
        /// Actualise le prix de la recette et la qauntité commandée
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantite"></param>
        public static void RefreshPriceAndQuantity(string id, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select nombre_com_recette, prix_recette from Recette where numero_recette =  \"" + id + "\"");
            int nbCOm = 0;
            int prix = 0;
            while (sql.reader.Read())
            {
                nbCOm = sql.reader.GetInt32(0);
                prix = sql.reader.GetInt32(1);
            }
            if (nbCOm < 10 && nbCOm + quantite >= 10)
            {
                prix = prix + 2;
            }
            else if (nbCOm < 50 && nbCOm + quantite >= 50)
            {
                prix = prix + 4;
            }
            sql.Close();
            sql.Request("update Recette set prix_recette ="+prix+ ", nombre_com_recette = nombre_com_recette + "+quantite+ " where numero_recette = \""+id+"\"");
            sql.Close();
        }




    }
}
