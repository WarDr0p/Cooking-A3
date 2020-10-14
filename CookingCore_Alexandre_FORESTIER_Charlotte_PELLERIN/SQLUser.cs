using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{/// <summary>
/// Classe servant à intéragir avec la base de donnée
/// </summary>
    class SQLUser
    {
        public MySqlConnection connection;
        public MySqlDataReader reader;

        /// <summary>
        /// Classe servant à intéragir avec la base de donnée
        /// </summary>
        public SQLUser()
        {
            string connectionString = "SERVER=remotemysql.com;PORT=3306;DATABASE=kpWXAJSoGV;UID=kpWXAJSoGV;PASSWORD=TJrQPmTWwP;SSLMODE=none;";
            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Effectue une requète auprès de la BDD
        /// </summary>
        /// <param name="request">requète à effectuer</param>
        public void Request(string request)
        {
            connection.Open();
            MySqlCommand commande = connection.CreateCommand();
            commande.CommandText = request;
            reader = commande.ExecuteReader();
        }
        /// <summary>
        /// Permet de fermer la connection, à appeler après chaque requête
        /// </summary>
        public void Close()
        {
            connection.Close();
        }
    }
}
