using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    public class Admin
    {
        /// <summary>
        /// Vérifie la validité du couple mots de passe identifiant
        /// </summary>
        /// <param name="id">UUID admin</param>
        /// <param name="mdp">mot de passe admin</param>
        /// <returns></returns>
        public static bool Login(int id, string mdp)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select count(*) from Administrateur where id_admin = \"" + id + "\" and mdp =\"" + mdp + "\"");
            int result = 0;
            while (sql.reader.Read())
            {
                result = sql.reader.GetInt32(0);
            }
            sql.Close();
            if(result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        /// <summary>
        /// Supprime un client de la liste des cdr et les recettes qui lui sont associées
        /// </summary>
        /// <param name="id_cdr">Identifiant cdr</param>
        public static void RevokeCdr(string id_cdr)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select numero_recette from Recette where id_cdr =\""+id_cdr+"\"");
            string result = "";
            while (sql.reader.Read())
            {
                result += "§" + sql.reader.GetString(0);
            }
            string[] ids = result.Split(new string[] { "§" }, StringSplitOptions.RemoveEmptyEntries  );
            sql.Close();
            foreach(string id in ids)
            {
                DeleteRecette(id);
            }
            sql.Request("delete from Cdr where id_cdr =\"" + id_cdr + "\"");
            sql.Close();
        }
        /// <summary>
        /// Supprime la recette en paramètre
        /// </summary>
        /// <param name="idRecette">identifiant recette</param>
        public static void DeleteRecette(string idRecette)
        {
            SQLUser sql = new SQLUser();
            sql.Request("delete from Recette where numero_recette =\"" + idRecette + "\"");
            sql.Close();
            sql.Request("delete from Contient where numero_recette =\"" + idRecette + "\"");
            sql.Close();
        }
        /// <summary>
        /// Permet d'ajouter un client dans la liste des Cdr
        /// </summary>
        /// <param name="id_client">numéro client</param>
        public static void PromoteCdr(string id_client)
        {           
            SQLUser sql = new SQLUser();
            sql.Request("insert into Cdr(id_client) values(\""+id_client+"\")");
            sql.Close();
            sql.Request("delete from Valide_Cdr where id_client=\"" + id_client + "\"");
            sql.Close();
        }

        /// <summary>
        /// Valide la recette précisée en paramètre
        /// </summary>
        /// <param name="idRecette">numéro de la recette</param>
        /// <param name="idAdmin">identifiant de l'admin effectuant la commande</param>
        public static void ValiderRecette(string idRecette,string idAdmin)
        {
            SQLUser sql = new SQLUser();
            //Console.WriteLine("insert into Valide_Recette(numero_recette,id_admin) values(\"" + idRecette + "\",\"" + idAdmin + "\")");
            sql.Request("insert into Valide_Recette(numero_recette,id_admin) values(\""+idRecette+"\",\""+idAdmin+"\")");
            sql.Close();

        }
        /// <summary>
        /// Récupère le nom associé à l'admin précisé en paramètre
        /// </summary>
        /// <param name="id">numéro de l'admin</param>
        /// <returns></returns>
        public static string GetName(string id)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select nom_admin from Administrateur where id_admin = \"" + id + "\"");
            string result = "";
            while (sql.reader.Read())
            {
                result = sql.reader.GetString(0);
            }
            sql.Close();
            return result;
        }

        /// <summary>
        /// Retourne la liste de tout les cdr non validés
        /// </summary>
        /// <returns></returns>
        public static string[] GetCdrNotValidated()
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
            sql.Request("select C.id_client, nom_client, prenom_client from Client C join Valide_Cdr V on C.id_client = V.id_client");
            int i = 0;
            while (sql.reader.Read())
            {
                result[i] = sql.reader.GetString(0)+"§"+sql.reader.GetString(1) + "§"+ sql.reader.GetString(2);
                i++;
            }
            return result;
            sql.Close();
        }

        /// <summary>
        /// Ajoute un admin dans la bdd
        /// </summary>
        /// <param name="nom">Nom de l'admin</param>
        /// <param name="mdppasHache">Mot de passe de l'admin</param>
        public static void Create(string nom, string mdppasHache)
        {
            SQLUser sql = new SQLUser();
            sql.Request("insert into Administrateur(nom_admin,mdp) value(\"" + nom + "\",\"" + mdppasHache.GetHashCode().ToString() + "\")");
            sql.Close();
        }

        /// <summary>
        /// Retourne le cdr de la semaine avec ses infos perso
        /// </summary>
        /// <returns></returns>
        public static string CdrOfTheWek()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select C.id_cdr, C.id_client, nom_client, sum(Con.nombre_recettes), prenom_client from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande where datediff(NOW(), date(date_commande)) <= 7 group by C.id_cdr having sum(Con.nombre_recettes) >= ALL(select sum(Con.nombre_recettes) from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande where datediff(NOW(), date(date_commande)) <= 7 group by C.id_cdr); ");
            string result = "";
            while (sql.reader.Read())
            {
                for(int i = 0; i < sql.reader.FieldCount; i ++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
            }
            return result;
        }
        /// <summary>
        /// Retourne le Cdr d'or
        /// </summary>
        /// <returns></returns>
        public static string CdrDor()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select C.id_cdr, C.id_client, nom_client,sum(Con.nombre_recettes), prenom_client from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande group by C.id_cdr having sum(Con.nombre_recettes) >= ALL(select sum(Con.nombre_recettes) from Cdr C join Client Cl on Cl.id_client = C.id_client join Recette R on R.id_cdr = C.id_cdr join Contient Con on Con.numero_recette = R.numero_recette join Commande_Client Com on Com.numero_commande = Con.Numero_Commande group by C.id_cdr); ");
            string result = "";
            while (sql.reader.Read())
            {
                for (int i = 0; i < sql.reader.FieldCount; i++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
            }
            return result;
        }
        /// <summary>
        /// Retoune les 5 recettes plus commandées
        /// </summary>
        /// <returns></returns>
        public static List<string> TopRecette()
        {
            SQLUser sql = new SQLUser();
            sql.Request("select numero_recette,nom_recette,nom_recette,description_recette,prix_recette,nombre_com_recette,photo_recette from Recette order by nombre_com_recette desc limit 5;");
            List<string> recette = new List<string>();
            while (sql.reader.Read())
            {
                string result = "";
                for (int i = 0; i < sql.reader.FieldCount; i++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
                recette.Add(result);
            }
            return recette;
        }

        /// <summary>
        /// Retourne les 5 recttes les plus commandées d'un Cdr
        /// </summary>
        /// <param name="idCdr">id du cdr</param>
        /// <returns></returns>
        public static List<string> TopRecetteCdr(string idCdr)
        {
            SQLUser sql = new SQLUser();
            sql.Request("select numero_recette,nom_recette,nom_recette,description_recette,prix_recette,nombre_com_recette,photo_recette from Recette where id_cdr = \""+idCdr+"\" order by nombre_com_recette desc limit 5;");
            List<string> recette = new List<string>();
            while (sql.reader.Read())
            {
                string result = "";
                for (int i = 0; i < sql.reader.FieldCount; i++)
                {
                    result = result + "§" + sql.reader.GetString(i);
                }
                recette.Add(result);
            }
            return recette;
        }
    }
}
