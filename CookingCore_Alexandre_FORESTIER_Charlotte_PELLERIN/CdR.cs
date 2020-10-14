using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class CdR
    {
        /// <summary>
        /// Retoune les recettes validées par un Administrateur d'un cdr
        /// </summary>
        /// <param name="idCdr">cdr dont on veut les recette validées</param>
        /// <returns></returns>
        public static string[] getValidRecipe(string idCdr)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Recette natural join Valide_Recette where id_cdr =\"" + idCdr + "\"");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette  from Recette natural join Valide_Recette where id_cdr =\"" + idCdr+"\"");
            string[] result = new string[size];
            int i = 0;
            while (sql.reader.Read())
            {
                for (int j = 0;j < sql.reader.FieldCount; j++)
                {
                    result[i] = result[i] + "§" + sql.reader.GetValue(j).ToString();
                }
                i++;
            }
            sql.Close();
            if (result == null)
            {
                return new string[] { "0§0§0§0§0§0§0"};
            }
            return result;
        }

        /// <summary>
        /// Retourne le nombre de cdr validés
        /// </summary>
        /// <returns>Nombre de cdr validés</returns>
        public static int GetNumber()
        {
            SQLUser sql = new SQLUser();
            sql.Request("Select count(*) from Cdr");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            return result;
        }
        /// <summary>
        /// Retourne les recettes non validées d'un cdr
        /// </summary>
        /// <param name="idCdr"></param>
        /// <returns></returns>
        public static string[] getUnvalidRecipe(string idCdr)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Recette where id_cdr = \"" + idCdr + "\" and numero_recette not in (select numero_recette from Recette natural join Valide_Recette where id_cdr=\"" + idCdr + "\")");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            sql.Request("select numero_recette, nom_recette, type_recette, description_recette, prix_recette, nombre_com_recette, photo_recette from Recette where id_cdr = \"" + idCdr + "\" and numero_recette not in (select numero_recette from Recette natural join Valide_Recette where id_cdr=\"" + idCdr + "\")");
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
        /// Retourne les nombre de cdr validés
        /// </summary>
        /// <returns></returns>
        public static string[] getValidCdr()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Client natural join Cdr");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] result = new string[size];
            sql.Request("select id_cdr, id_client, nom_client, prenom_client from Client natural join Cdr");
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
        /// Retourne la liste des cdr avec le total de leur commandes
        /// </summary>
        /// <returns></returns>
        public static string[] getValidCdrWithQuantity()
        {
            SQLUser sql = new SQLUser();
            List<string> cdr = new List<string>();
            sql.Request("select C.id_cdr,C.id_client, nom_client, prenom_client,sum(Con.nombre_recettes) from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande group by C.id_cdr");
            while (sql.reader.Read())
            {
                string result = "";
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result = result + "§" + sql.reader.GetValue(j).ToString();
                }
                cdr.Add(result);

            }
            sql.Close();
            
            sql.Request("select id_cdr, id_client, nom_client, prenom_client, 0 from Cdr natural join Client where id_cdr not in (select C.id_cdr from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande group by C.id_cdr)");
            while (sql.reader.Read())
            {
                string result = "";
                for (int j = 0; j < sql.reader.FieldCount; j++)
                {
                    result = result + "§" + sql.reader.GetValue(j).ToString();
                }
                cdr.Add(result);

            }
            sql.Close();

            return cdr.ToArray();
        }
        /// <summary>
        /// retourne la liste des cdr en attente
        /// </summary>
        /// <returns></returns>
        public static string[] getPendingCdr()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Client C join Valide_Cdr V on C.id_client = V.id_client");
            int size = 0;
            while (sql.reader.Read())
            {
                size = sql.reader.GetInt32(0);
            }
            sql.Close();
            string[] result = new string[size];
            sql.Request("select C.id_client,nom_client, prenom_client from Client C join Valide_Cdr V on C.id_client = V.id_client");
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
        /// retourne l'id cdr associé à à un client s'il existe
        /// </summary>
        /// <param name="idClient">0 si le client n'est pas cdr ou l'idcdr du client s'il est cdr</param>
        /// <returns></returns>
        public static int GetId(string idClient)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select id_cdr from Cdr where id_client =\""+idClient+"\"");
            int id = 0;
            while (sql.reader.Read())
            {
                id = sql.reader.GetInt32(0);
            }
            sql.Close();
            return id;
        }

        /// <summary>
        /// Crédite le cdr entré en paramètre sur son compte client
        /// </summary>
        /// <param name="idCdr">id du cdr à créditer</param>
        /// <param name="montant">montant à créditer</param>
        public static void Créditer(string idCdr, int montant)
        {
            SQLUser sql = new SQLUser();

            sql.Request("select id_client from Cdr where id_cdr =\"" + idCdr + "\"");
            string id = "";
            while (sql.reader.Read())
            {
                id = sql.reader.GetString(0);
            }
            sql.Close();
            Client.CrediterClient(montant, id);
        }
    }
}
