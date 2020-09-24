using System;
using DevStore.Models;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Data
{
    public class ConnexionData
    {
        public ConnexionData()
        {
            String connectString = Resources.connectString;
            if (!String.IsNullOrEmpty(connectString))
            {
                try
                {
                    this.connection = new MySqlConnection(connectString);
                }
                catch (Exception e)
                {
                    _logger.Error("Erreur durant la création de la connexion à la base de donnée dans le constructeur ArticleDao :" + e.StackTrace);
                }
            }
        }

        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConnexionData));


        //obtenir un utilisateur par son email et son mot de passe
        public UtilisateurModel getUtilisateur(string emailLogin, string passwordLogin)
        {
            UtilisateurModel user = new UtilisateurModel();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id,Nom,Prenom,Email,Password FROM Utilisateur WHERE Email = @Email AND Password = @Password";
                command.Parameters.AddWithValue("@Email", emailLogin);
                command.Parameters.AddWithValue("@Password", passwordLogin);

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    user.Id = rdr.GetInt32(0);
                    user.Nom = rdr.GetString(1);
                    user.Prenom = rdr.GetString(2);
                    user.Email = rdr.GetString(3);
                    user.Password = rdr.GetString(4);
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation des informations de l'utilisateur " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return user;
        }

    }
}
