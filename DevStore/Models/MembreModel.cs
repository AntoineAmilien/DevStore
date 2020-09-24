using System;
using System.Collections.Generic;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Models
{
    public class MembreModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Profession { get; set; }
        public string Image { get; set; }


        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MembreModel));

        //construteur avec pool de connexion
        public MembreModel()
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

        //Lister les membre
        public List<MembreModel> getAllMembre()
        {
            List<MembreModel> listeMembre = new List<MembreModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id,Nom,Prenom,Profession,Image FROM Membre";

                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    MembreModel membre = new MembreModel();

                    membre.Id = rdr.GetInt32(0);
                    membre.Nom = rdr.GetString(1);
                    membre.Prenom = rdr.GetString(2);
                    membre.Profession = rdr.GetString(3);
                    membre.Image = rdr.GetString(4);

                    listeMembre.Add(membre);
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation de tous les articles. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return listeMembre;
        }

        //Obtenir un membre par son Id
        public MembreModel getMembreById(int Id)
        {
            MembreModel membre = new MembreModel();
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id,Nom,Prenom,Profession,Image FROM Membre WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);

                MySqlDataReader rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    membre.Id = rdr.GetInt32(0);
                    membre.Nom = rdr.GetString(1);
                    membre.Prenom = rdr.GetString(2);
                    membre.Profession = rdr.GetString(3);
                    membre.Image = rdr.GetString(4);
                }
            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation du membre par son id. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return membre;
        }
        //Ajouter un membre
        public bool postMembre(string Nom, string Prenom, string Profession, string Image)
        {
            bool retourPostMembre = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "INSERT INTO `Membre` ( `Id`,`Nom`, `Prenom`, `Profession`, `Image`)" +
                    " VALUES ('0', @Nom, @Prenom, @Profession, @Image);";
                command.Parameters.AddWithValue("@Nom", Nom);
                command.Parameters.AddWithValue("@Prenom", Prenom);
                command.Parameters.AddWithValue("@Profession", Profession);
                command.Parameters.AddWithValue("@Image", Image);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourPostMembre = false;
                _logger.Error("Erreur pendant la creation du membre. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourPostMembre;
        }

        //Supprimer un membre
        public bool deleteMembre(int Id)
        {
            bool retourSuppresion = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "DELETE FROM Membre WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourSuppresion = false;
                _logger.Error("Erreur pendant la suppression du membre. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourSuppresion;
        }

        //Modifier un Membre -> a modifier
        public bool updateMembre(int Id, string Nom, string Prenom, string Profession, string Image)
        {
            bool retourupdateUtilisateur = true;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "UPDATE `Membre` SET `Nom`=@Nom,`Prenom`=@Prenom,`Profession`=@Profession,`Image`=@Image WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Nom", Nom);
                command.Parameters.AddWithValue("@Prenom", Prenom);
                command.Parameters.AddWithValue("@Profession", Profession);
                command.Parameters.AddWithValue("@Image", Image);

                MySqlDataReader rdr = command.ExecuteReader();

            }
            catch (Exception error)
            {
                retourupdateUtilisateur = false;
                _logger.Error("Erreur pendant la mise à jour du membre. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourupdateUtilisateur;
        }
    }
}
