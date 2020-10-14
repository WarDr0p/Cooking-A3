using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class Client
    {
        /// <summary>
        /// Retounr le nombres de clients inscrits
        /// </summary>
        /// <returns></returns>
        public static int GetNumber()
        {
            SQLUser sql = new SQLUser();
            sql.Request("Select count(*) from Client");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            return result;
        }

        /// <summary>
        /// Crée un client avec le informations précisées en paramètre
        /// </summary>
        /// <param name="nom">Nom du client</param>
        /// <param name="prenom">Prénom du client</param>
        /// <param name="tel">Téléphone duclient</param>
        /// <param name="mdpHache">Mot de passe</param>
        public static void Create(string nom, string prenom, string tel, string mdpHache)
        {
            SQLUser sql = new SQLUser();
            sql.Request("insert into Client(nom_client, prenom_client, telephone_client, mdp) values(\""+ nom + "\",\"" + prenom + "\",\"" + tel + "\",\"" + mdpHache + "\")");
            sql.Close();
        }
        /// <summary>
        /// Retoune l'id associé aux infos clients placées en paramètre
        /// </summary>
        /// <param name="nom">Nom du client</param>
        /// <param name="prenom">Prénom du client</param>
        /// <param name="tel">Téléphone client </param>
        /// <returns>Retounre l'id client correspondant ou 0</returns>
        public static int GetID(string nom, string prenom,string tel)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select id_client from Client where nom_client = \"" + nom + "\" and prenom_client =\"" + prenom + "\" and telephone_client = \"" + tel + "\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Retoune true si le couple id/mdp est valide.
        /// </summary>
        /// <param name="id">Id du client</param>
        /// <param name="mdpHache">Mot de passe du client déjà haché</param>
        /// <returns></returns>
        public static bool Login(int id, string mdpHache)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Client where id_client = \"" + id + "\" and mdp =\"" + mdpHache + "\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            if(result != 0)
            {
                return true;
            }
            sql.Close();
            return false;
        }
        /// <summary>
        /// Permet de récupérer les infos d'un client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getCredentials(string id)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select id_client,nom_client, prenom_client, solde_client, telephone_client from Client where id_client = \"" + id + "\"");
            string result = "";
            while (sql.reader.Read())
            {
                for (int i = 0; i < sql.reader.FieldCount; i++)
                {
                    if (i != 0)
                    {
                        result = result + "§";
                    }
                    result = result + sql.reader.GetString(i);
                }
            }
            sql.Close();
            return result;
            
        }

        /// <summary>
        /// Permet de savoir si un client a fait une demande pour devenir cdr
        /// </summary>
        /// <param name="id">id du client à tester</param>
        /// <returns></returns>
        public static bool hasRequestedCdr(string id)
        {
            SQLUser sql = new SQLUser();
            sql.connection.Open();
            MySqlCommand commande = sql.connection.CreateCommand();
            commande.CommandText = "select count(*) from Valide_Cdr where id_client = \"" +id+ "\"";
            MySqlDataReader reader;
            reader = commande.ExecuteReader();
            int result = 0;
            while (reader.Read())
            {
                result = reader.GetInt32(0);
            }
            sql.connection.Close();
            if(result == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Rejoute le client dans la liste des cdr en attente
        /// </summary>
        /// <param name="id_client">id du client à ajouter</param>
        public static void RequestCdr(string id_client)
        {
            SQLUser sql = new SQLUser();
            sql.Request("INSERT INTO Valide_Cdr(id_client) values(\"" + id_client+"\")");
            sql.Close();
        }

        /// <summary>
        /// Vérifie si le client précisé en paramètre est cdr ou pas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsCdr(string id)
        {
            SQLUser  sql = new SQLUser();
            sql.Request("select count(*) from Client natural join Cdr where id_client=\"" + id + "\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            Console.WriteLine(result);
            if(result <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        /// <summary>
        /// Retire le montant précisé en paramètre du compte du client
        /// </summary>
        /// <param name="id">Client à débiter</param>
        /// <param name="montant">Montant à débiter</param>
        public static void Debiter(string id, int montant)
        {
            SQLUser sql = new SQLUser();
            sql.Request("update Client set solde_client = solde_client - "+montant+" where id_client =\""+id+"\"");
            sql.Close();
        }

        /// <summary>
        /// retourne le solde du client passé en paramètre
        /// </summary>
        /// <param name="id">client</param>
        /// <returns></returns>
        public static int GetSolde(string id)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select solde_client from Client where id_client=\""+id+"\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Crédite un cleint
        /// </summary>
        /// <param name="montant">montant à créditer</param>
        /// <param name="idClient">id du client à créditer</param>
        public static void CrediterClient(int montant, string idClient)
        {
            SQLUser sql = new SQLUser();
            sql.Request("update Client set solde_client = solde_client + " + montant + " where id_client = \"" + idClient + "\"");
            sql.Close();
        }

        /// <summary>
        /// Permet de récupérer toute ses commandes associées un client
        /// </summary>
        /// <param name="idClient"></param>
        /// <returns>numero_commande,date_commande,id_client,montant from Commande_Client</returns>
        public static List<string> GetCommandes(string idClient)
        {
            SQLUser sql = new SQLUser();
            List<string> list = new List<string>();
            sql.Request("Select numero_commande,date_commande,id_client,montant from Commande_Client where id_client = \"" + idClient + "\"");
            
            while (sql.reader.Read()){
                string result = "";
                for (int i = 0; i < sql.reader.FieldCount; i++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
                list.Add(result);
            }
            sql.Close();
            return list;
        }
    }
}
