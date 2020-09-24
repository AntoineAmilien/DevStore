using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DevStore.Models;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Data
{
    public class StatistiqueDao 
    {

        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConnexionData));

        public StatistiqueDao()
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

        //obtenir le nombre total d'article sur le site
        public int getNbArticles()
        {
            int NombreArticles = 0 ;

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT COUNT(*) FROM Article";

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    NombreArticles = rdr.GetInt32(0);
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

            return NombreArticles;
        }

        //Obtenir le nombre total d'article par categorie
        //retourne une liste d'object dynamique : categorie -> categorieModel
        //nombreArticles -> le nombre d'articles pour la catégorie
        public List<dynamic> getNbArticlesByCategories()
        {
            CategorieModel categorieModel = new CategorieModel();
            List<CategorieModel> listCategories = new List<CategorieModel> (categorieModel.getAllCategories());

            //Creation d'une liste d'objet dynamique
            List<dynamic> listObjectDyn = new List<dynamic>();

            foreach (CategorieModel categorie in listCategories)
            {
                int index = listCategories.IndexOf(categorie);


                dynamic dynObject = new ExpandoObject();
                dynObject.categorie = listCategories[index];

                try
                {
                    int NombreArticles = 0;
                    this.connection.Open();

                    MySqlCommand command = this.connection.CreateCommand();

                    command.CommandText = "SELECT COUNT(*) FROM Article WHERE Fk_Categorie_Id = @Id";
                    command.Parameters.AddWithValue("@Id", listCategories[index].Id);

                    MySqlDataReader rdr = command.ExecuteReader();

                    if (rdr.Read())
                    {
                        NombreArticles = rdr.GetInt32(0);
                    }

                    dynObject.nombreArticles = NombreArticles;

                    rdr.Close();
                }
                catch (Exception error)
                {
                    _logger.Error("Erreur pendant la recuperation des informations de l'utilisateur " + error.StackTrace);
                }
                finally
                {
                    this.connection.Close();
                }

                listObjectDyn.Add(dynObject);

            }

            listObjectDyn = listObjectDyn.OrderByDescending(dynObject => dynObject.nombreArticles).ToList();

            return listObjectDyn;
        }

        //Nombre d'articles poster pour chaque utilisateur
        //retourne une liste d'object dynamique : utilisteur -> utilisateurModel
        //nombreArticles -> le nombre d'articles poster par l'utilisateur
        public List<dynamic> getNbArticlesByUtilisateurs()
        {
            UtilisateurModel utilisateurModel = new UtilisateurModel();
            List<UtilisateurModel> listUtilisateurs = new List<UtilisateurModel>(utilisateurModel.getAllUtilisateur());

            //Creation d'une liste d'objet dynamique
            List<dynamic> listObjectDyn = new List<dynamic>();
            
            foreach (UtilisateurModel utilisateur in listUtilisateurs)
            {
                int index = listUtilisateurs.IndexOf(utilisateur);
                dynamic dynObject = new ExpandoObject();
                dynObject.utilisateur = listUtilisateurs[index];

                try
                {
                    int NombreArticles = 0;
                    this.connection.Open();

                    MySqlCommand command = this.connection.CreateCommand();

                    command.CommandText = "SELECT COUNT(*) FROM Article WHERE Fk_Utilisateur_Id = @Id";
                    command.Parameters.AddWithValue("@Id", listUtilisateurs[index].Id);

                    MySqlDataReader rdr = command.ExecuteReader();

                    if (rdr.Read())
                    {
                        NombreArticles = rdr.GetInt32(0);
                    }

                    dynObject.nombreArticles = NombreArticles;

                    rdr.Close();
                }
                catch (Exception error)
                {
                    _logger.Error("Erreur pendant la recuperation des informations de l'utilisateur " + error.StackTrace);
                }
                finally
                {
                    this.connection.Close();
                }

                listObjectDyn.Add(dynObject);

            }

            listObjectDyn = listObjectDyn.OrderByDescending(dynObject => dynObject.nombreArticles).ToList();

            return listObjectDyn;
        }


        //Nombre de j'aimes totale
        //Nombre de j'aimes par article
        //Nombre de fois ou il y a un telechargment de doc total
        //Nombre de fois ou il y a telechargment de doc par article


    }
}
