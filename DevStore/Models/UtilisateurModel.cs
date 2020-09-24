using System;
using System.Collections.Generic;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Models
{
    public class UtilisateurModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UtilisateurModel));

        //construteur avec pool de connexion
        public UtilisateurModel()
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

        //Lister tout les utilisateurs
        public List<UtilisateurModel> getAllUtilisateur (){
            List<UtilisateurModel> listUtilisateurs = new List<UtilisateurModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id, Nom, Prenom FROM Utilisateur"; 

                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    UtilisateurModel utilisateur = new UtilisateurModel();

                    utilisateur.Id = rdr.GetInt32(0);
                    utilisateur.Nom = rdr.GetString(1);
                    utilisateur.Prenom = rdr.GetString(2);

                    listUtilisateurs.Add(utilisateur);
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la récupération des utilisateurs. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return listUtilisateurs;
        }

        //Avoir les information d'un utilisateur par son Id
        public UtilisateurModel getUtilisateurById(int id)
        {
            UtilisateurModel utilisateur = new UtilisateurModel();
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id, Nom, Prenom,Email,Password FROM Utilisateur WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id",id);

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    utilisateur.Id = rdr.GetInt32(0);
                    utilisateur.Nom = rdr.GetString(1);
                    utilisateur.Prenom = rdr.GetString(2);
                    utilisateur.Email = rdr.GetString(3);
                    utilisateur.Password = rdr.GetString(4);
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la récupération de l'utilisateur par son id. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return utilisateur;
        }

        //Ajouter un utilisateur
        public bool postUtilisateur(string Nom, string Prenom, string Email, int Password)
        {
            bool retourPostUtilisateur = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "INSERT INTO `Utilisateur` ( `Id`,`Nom`, `Prenom`, `Email`, `Password`)" +
                    " VALUES ('0', @Nom, @Prenom, @Email, @Password);";
                command.Parameters.AddWithValue("@Nom", Nom);
                command.Parameters.AddWithValue("@Prenom", Prenom);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", Password);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourPostUtilisateur = false;
                _logger.Error("Erreur pendant la creation de l'utilisateur. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourPostUtilisateur;
        }

        //Supprimer un utilisateur
        public bool deleteUtilisateur(int Id)
        {
            bool retourSuppresion = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "DELETE FROM Utilisateur WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourSuppresion = false;
                _logger.Error("Erreur pendant la suppression de l'utilisateur. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourSuppresion;
        }

        //Modifier un utilisateur
        public bool updateUtilisateur(int Id, string Nom, string Prenom, string Email)
        {
            bool retourupdateUtilisateur = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "UPDATE `Utilisateur` SET `Nom`=@Nom,`Prenom`=@Prenom,`Email`=@Email WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Nom", Nom);
                command.Parameters.AddWithValue("@Prenom", Prenom);
                command.Parameters.AddWithValue("@Email", Email);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourupdateUtilisateur = false;
                _logger.Error("Erreur pendant la mise à jour de l'utilisateur. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourupdateUtilisateur;
        }

        //Modifier le mot de passe de l'utilisateur
        public bool updatePasswordUtilisateur(int Id, string Password)
        {
            bool retourupdateUtilisateur = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "UPDATE `Utilisateur` SET `Password`=@Password WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Password", Password);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourupdateUtilisateur = false;
                _logger.Error("Erreur pendant la mise à jour du mot de passe utilisateur. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourupdateUtilisateur;
        }

     
    }
}
