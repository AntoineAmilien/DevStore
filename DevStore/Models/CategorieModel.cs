using System;
using System.Collections.Generic;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Models
{
    public class CategorieModel
    {
        
        public int Id { get; set; }
        public string Intitule { get; set; }
        public string Image { get; set; }

        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CategorieModel));

        //contructeur avec pool de connexion
        public CategorieModel()
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

        //Lister les categories
        public List<CategorieModel> getAllCategories()
        {
            List<CategorieModel> listCategorie = new List<CategorieModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id,Intitule,Image FROM Categorie";

                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    listCategorie.Add(new CategorieModel()
                    {
                        Id = rdr.GetInt32(0),
                        Intitule = rdr.GetString(1),
                        Image = rdr.GetString(2)
                    });
                }


            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation de toutes les categories. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }
            return listCategorie;
        }

        //Obetenir une categorie par son Id
        public CategorieModel getCategorieById(int Id)
        {
            CategorieModel categorie = new CategorieModel();

            try{

                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Id,Intitule,Image FROM Categorie WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", Id);

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    categorie.Id = rdr.GetInt32(0);
                    categorie.Intitule = rdr.GetString(1);
                    categorie.Image = rdr.GetString(2);
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation d'une categorie par son id. Motif : " + error.StackTrace);

            }
            finally
            {
                this.connection.Close();
            }


            return categorie;
        }

        //Ajouter une Categorie
        public bool postNewCategorie(string Intitule,string Image)
        {


            bool retourPostNewCategories;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "INSERT INTO `Categorie`(`Intitule`,`Image`) VALUES (@Intitule,@Image)";
                command.Parameters.AddWithValue("@Intitule", Intitule);
                command.Parameters.AddWithValue("@Image", Image);

                MySqlDataReader rdr = command.ExecuteReader();

                retourPostNewCategories = true;

            }
            catch (Exception error)
            {
                retourPostNewCategories = false;
                _logger.Error("Erreur pendant la creation de la nouvelle catégorie. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourPostNewCategories;
        }

        //Supprimer une Categorie
        public bool postDeleteCategorie(int CategorieId)
        {

            bool retourPostDeleteCategorie;
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "DELETE FROM `Categorie` WHERE Categorie.Id = @CategorieId;";
                command.Parameters.AddWithValue("@CategorieId", CategorieId);

                MySqlDataReader rdr = command.ExecuteReader();

                retourPostDeleteCategorie = true;

            }
            catch (Exception error)
            {
                retourPostDeleteCategorie = false;
                _logger.Error("Erreur pendant la suppression de la catégorie. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourPostDeleteCategorie;
        }

        //Modifier une Categorie
        public bool updateCategorie(int Id, string Intitule, string Image)
        {
            bool retourUpdateCategorie = true;

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "UPDATE `Categorie` SET" +
                        " `Intitule` = @Intitule , `Image` = @Image " +
                        "  WHERE Categorie.Id = @Id;";

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Intitule", Intitule);
                command.Parameters.AddWithValue("@Image", Image);

                MySqlDataReader rdr = command.ExecuteReader();
            }
            catch (Exception error)
            {
                retourUpdateCategorie = false;
                _logger.Error("Erreur pendant la mise à jour de la categorie. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourUpdateCategorie;
        }

        
    }
}
