using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }

        /// <summary>
        /// Fonction utilisée pour remplir la bdd va cherhcer le clients dans le csv et les insère dans la bdd
        /// </summary>
        static void RemplirClient()
        {
            string fichier = File.ReadAllText("data.csv");
            string[] data = fichier.Split('\n');
            foreach (string pers in data)
            {
                if (pers != "")
                {
                    Console.WriteLine(pers);
                    string[] val = pers.Split(',');
                    Client.Create(val[0], val[1], val[2].Replace(" ", ""), "1234".GetHashCode().ToString());
                }
            }
        }
    }
}
