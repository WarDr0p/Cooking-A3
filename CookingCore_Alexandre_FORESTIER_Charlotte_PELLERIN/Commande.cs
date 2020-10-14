using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class Commande
    {
        Commande()
        {

        }
        /// <summary>
        /// Récupère les recettes contenues dans une commande
        /// </summary>
        /// <param name="idCom">commande dont on veut récupérer les recettes</param>
        /// <returns>numero_recette,nombre_recettes</returns>
        public static List<string> getRecettes(string idCom)
        {
            SQLUser sql = new SQLUser();
            List<string> liste = new List<string>();
            sql.Request("Select numero_recette,nombre_recettes from Contient where numero_commande = \"" + idCom + "\"");
            string result = "";
            while (sql.reader.Read())
            {
                 liste.Add( sql.reader.GetString(0)+ "§" + sql.reader.GetString(1));
            }
            sql.Close();
            return liste;
        }

        /// <summary>
        /// Calcule le prix associés à la recette et à sa quantité
        /// </summary>
        /// <param name="id">id de la recette dont on veut le prix</param>
        /// <param name="quantite">quantité</param>
        /// <returns></returns>
        public static int GetPrice(string id, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select nombre_com_recette, prix_recette from Recette where \"" + id + "\"");
            int nbCOm = 0;
            int prix = 0;
            while (sql.reader.Read())
            {
                nbCOm = sql.reader.GetInt32(0);
                prix=  sql.reader.GetInt32(1);
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
            return prix * quantite;
        }

        /// <summary>
        /// Crée une recette du montant précisé en paramètre et lié au client précisé en paramètre et retourne l'id associé à la commande
        /// </summary>
        /// <param name="montant">montant de la commande</param>
        /// <param name="id_client">id du client</param>
        /// <returns>id associé à la commande</returns>
        public static string Create(int montant, string id_client)
        {
            SQLUser sql = new SQLUser();
            sql.Request("insert into Commande_Client(date_commande,montant,id_client) values(NOW(),\""+montant+"\",\""+id_client+"\")");
            sql.Close();
            sql.Request("select max(numero_commande) from Commande_Client");
            string id = "";
            while (sql.reader.Read())
            {
                id = sql.reader.GetString(0);
            }
            sql.Close();
            return id; 
        }

        /// <summary>
        /// Rajoute le prix précisé à la commande entrée en paramètre
        /// </summary>
        /// <param name="idCom">id de la commande</param>
        /// <param name="prix">pris à ajouter</param>
        public static void RefreshPrix(string idCom, int prix)
        {
            SQLUser sql = new SQLUser();
            sql.Request("update Commande_Client set montant = montant +" + prix + " where numero_commande = \"" + idCom + "\""); 
            sql.Close();
        }
        /// <summary>
        /// Ajoute une certaine quantité d'une recette à la une commande
        /// </summary>
        /// <param name="numCom">Numéro de la commande</param>
        /// <param name="numRec">Numéro de la recette</param>
        /// <param name="quantite">Quantité à ajouter</param>
        public static void AddRecette(string numCom, string numRec, int quantite)
        {
            SQLUser sql = new SQLUser();
            sql.Request("insert into Contient(numero_commande,numero_recette,nombre_recettes) values(\"" + numCom+"\",\""+ numRec + "\",\"" +quantite+"\")");
            sql.Close();
        }

        /// <summary>
        /// Fonction gérant tous le système de commande débite le crédite le client si la recette est impossible sinon actualise les stock des produits, le nombre de commande et le prix des recettes, rémunère les cdr crée la commande et associe les recettes
        /// </summary>
        /// <param name="id_client">id du client qui a passé la commande</param>
        /// <param name="id">liste des id recettes</param>
        /// <param name="quanti">liste des quantités des recettes</param>
        public static void Process(string id_client,List<string> id, List<int> quanti)
        {
            int prix = 0;
            string numCom = "0";
            for (int i = 0; i < quanti.Count; i++)
            {
                string idRec = id[i];
                int quantiRec = quanti[i];
                bool estRealisable = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Recette.isPossible(idRec, quantiRec);
                if (estRealisable)
                {
                    if (numCom == "0")
                    {
                        numCom = Commande.Create(0, id_client);
                    }
                    
                    
                    Commande.AddRecette(numCom, idRec, quantiRec);                    
                    prix = prix + Commande.GetPrice(idRec, quantiRec);
                    Recette.RefreshPriceAndQuantity(idRec, quantiRec);
                    Recette.RefreshProducts(idRec, quantiRec);
                    Recette.RemunererCdr(idRec, quantiRec);
                }               
                else
                {
                   Client.CrediterClient(Commande.GetPrice(idRec,quantiRec), id_client);
                }

                if (numCom != "0")
                {
                    Commande.RefreshPrix(numCom, prix);
                }
            }
        }
    }
}
