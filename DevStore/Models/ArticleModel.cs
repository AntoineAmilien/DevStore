using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using MySql.Data.MySqlClient;

namespace DevStore.Models
{
    public class ArticleModel
    {

        public int Id { get; set; }
        public string Titre { get; set; }
        public string SousTitre { get; set; }
        public DateTime Date { get; set; }
        public string Texte { get; set; }
        public string Miniature { get; set; }
        public string Documentation { get; set; }
        public CategorieModel Categorie { get; set; }
        public UtilisateurModel Auteur { get; set; }

        private MySqlConnection connection;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ArticleModel));

        //construteur avec pool de connexion
        public ArticleModel()
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

        //Obetenir tous les articles dans une liste
        public List<ArticleModel> getAllArticles()
        {
            List<ArticleModel> listeArticle = new List<ArticleModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Article.Id,Titre,SousTitre,Date,Texte,Article.Image,NomPdf," +
               " Utilisateur.Id,Nom,Prenom,Email," +
               " Categorie.Id,Intitule,Categorie.Image" +
               " FROM Article" +
               " INNER JOIN Categorie ON Fk_Categorie_Id = Categorie.Id" +
               " LEFT JOIN Utilisateur ON Fk_Utilisateur_Id = Utilisateur.Id ORDER BY Article.Date DESC";

                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    ArticleModel article = new ArticleModel();
                    UtilisateurModel auteur = new UtilisateurModel();
                    CategorieModel categorie = new CategorieModel();

                    article.Id = rdr.GetInt32(0);
                    article.Titre = rdr.GetString(1);
                    article.SousTitre = rdr.GetString(2);
                    article.Date = rdr.GetDateTime(3);
                    article.Texte = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                    {
                        article.Miniature = rdr.GetString(5);
                    }
                    if (!rdr.IsDBNull(6))
                    {
                        article.Documentation = rdr.GetString(6);
                    }

                    if (!rdr.IsDBNull(7))
                    {
                        auteur.Id = rdr.GetInt32(7);
                        auteur.Nom = rdr.GetString(8);
                        auteur.Prenom = rdr.GetString(9);
                        auteur.Email = rdr.GetString(10);

                        article.Auteur = auteur;
                    }

                    categorie.Id = rdr.GetInt32(11);
                    categorie.Intitule = rdr.GetString(12);
                    categorie.Image = rdr.GetString(13);

                    article.Categorie = categorie;


                    listeArticle.Add(article);
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

            return listeArticle;
        }

        //Obetenir les 3 derniers articles dans une liste
        public List<ArticleModel> getRecentsArticles()
        {
            List<ArticleModel> listeArticle = new List<ArticleModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Article.Id,Titre,SousTitre,Date,Texte,Article.Image,NomPdf," +
                " Utilisateur.Id,Nom,Prenom,Email," +
                " Categorie.Id,Intitule,Categorie.Image"+
                " FROM Article"+
                " INNER JOIN Categorie ON Fk_Categorie_Id = Categorie.Id"+
                " LEFT JOIN Utilisateur ON Fk_Utilisateur_Id = Utilisateur.Id ORDER BY Article.Date DESC LIMIT 3";

                MySqlDataReader rdr = command.ExecuteReader();


                while (rdr.Read())
                {
                    ArticleModel article = new ArticleModel();
                    UtilisateurModel auteur = new UtilisateurModel();
                    CategorieModel categorie = new CategorieModel();

                    article.Id = rdr.GetInt32(0);
                    article.Titre = rdr.GetString(1);
                    article.SousTitre = rdr.GetString(2);
                    article.Date = rdr.GetDateTime(3);
                    article.Texte = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                    {
                        article.Miniature = rdr.GetString(5);
                    }
                    if (!rdr.IsDBNull(6))
                    {
                        article.Documentation = rdr.GetString(6);
                    }

                    if (!rdr.IsDBNull(7))
                    {
                        auteur.Id = rdr.GetInt32(7);
                        auteur.Nom = rdr.GetString(8);
                        auteur.Prenom = rdr.GetString(9);
                        auteur.Email = rdr.GetString(10);

                        article.Auteur = auteur;
                    }

                    categorie.Id = rdr.GetInt32(11);
                    categorie.Intitule = rdr.GetString(12);
                    categorie.Image = rdr.GetString(13);

                    article.Categorie = categorie;

                    listeArticle.Add(article);
                }


            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation des articles recents. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return listeArticle;
        }

        //Obetenir tous les articles d'une catégorie dans une liste
        public List<ArticleModel> getAllArticlesByCategorie(int CategorieId)
        {
            List<ArticleModel> listeArticle = new List<ArticleModel>();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Article.Id,Titre,SousTitre,Date,Texte,Article.Image,NomPdf," +
                " Utilisateur.Id,Nom,Prenom,Email," +
                " Categorie.Id,Intitule,Categorie.Image" +
                " FROM Article" +
                " INNER JOIN Categorie ON Fk_Categorie_Id = Categorie.Id" +
                " LEFT JOIN Utilisateur ON Fk_Utilisateur_Id = Utilisateur.Id " +
                " WHERE Fk_Categorie_Id = @CategorieId" +
                " ORDER BY Article.Date DESC";


                command.Parameters.AddWithValue("@CategorieId", CategorieId);

                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    ArticleModel article = new ArticleModel();
                    UtilisateurModel auteur = new UtilisateurModel();
                    CategorieModel categorie = new CategorieModel();

                    article.Id = rdr.GetInt32(0);
                    article.Titre = rdr.GetString(1);
                    article.SousTitre = rdr.GetString(2);
                    article.Date = rdr.GetDateTime(3);
                    article.Texte = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                    {
                        article.Miniature = rdr.GetString(5);
                    }
                    if (!rdr.IsDBNull(6))
                    {
                        article.Documentation = rdr.GetString(6);
                    }

                    if (!rdr.IsDBNull(7))
                    {
                        auteur.Id = rdr.GetInt32(7);
                        auteur.Nom = rdr.GetString(8);
                        auteur.Prenom = rdr.GetString(9);
                        auteur.Email = rdr.GetString(10);

                        article.Auteur = auteur;
                    }

                    categorie.Id = rdr.GetInt32(11);
                    categorie.Intitule = rdr.GetString(12);
                    categorie.Image = rdr.GetString(13);

                    article.Categorie = categorie;

                    listeArticle.Add(article);

                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation des articles par categorie. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return listeArticle;
        }

        //Obetenir un article par son Id
        public ArticleModel getArticleById(int ArticleId)
        {
            ArticleModel article = new ArticleModel();

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "SELECT Article.Id,Titre,SousTitre,Date,Texte,Article.Image,NomPdf," +
                " Utilisateur.Id,Nom,Prenom,Email," +
                " Categorie.Id,Intitule,Categorie.Image" +
                " FROM Article" +
                " INNER JOIN Categorie ON Fk_Categorie_Id = Categorie.Id" +
                " LEFT JOIN Utilisateur ON Fk_Utilisateur_Id = Utilisateur.Id " +
                " WHERE Article.Id = @ArticleId";

                command.Parameters.AddWithValue("@ArticleId", ArticleId);

                MySqlDataReader rdr = command.ExecuteReader();

                if (rdr.Read())
                {
                    UtilisateurModel auteur = new UtilisateurModel();
                    CategorieModel categorie = new CategorieModel();

                    article.Id = rdr.GetInt32(0);
                    article.Titre = rdr.GetString(1);
                    article.SousTitre = rdr.GetString(2);
                    article.Date = rdr.GetDateTime(3);
                    article.Texte = rdr.GetString(4);
                    if (!rdr.IsDBNull(5))
                    {
                        article.Miniature = rdr.GetString(5);
                    }
                    if (!rdr.IsDBNull(6))
                    {
                        article.Documentation = rdr.GetString(6);
                    }

                    if (!rdr.IsDBNull(7))
                    {
                        auteur.Id = rdr.GetInt32(7);
                        auteur.Nom = rdr.GetString(8);
                        auteur.Prenom = rdr.GetString(9);
                        auteur.Email = rdr.GetString(10);

                        article.Auteur = auteur;
                    }

                    categorie.Id = rdr.GetInt32(11);
                    categorie.Intitule = rdr.GetString(12);
                    categorie.Image = rdr.GetString(13);

                    article.Categorie = categorie;
                }

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la recuperation de l'article. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return article;
        }

        //Creer un article
        public ArticleModel postArticle(string Titre, string SousTitre, string Texte, string NomPdf, string NomImage, int CategorieId, int UtilisateurId)
        {

            ArticleModel article = new ArticleModel();
            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "INSERT INTO `Article` (`Id`, `Titre`, `SousTitre`, `Date`, `Texte`, `Image`, `NomPdf`, `Fk_Categorie_Id`, `Fk_Utilisateur_Id`)" +
                    " VALUES ('0', @Titre, @SousTitre, CURRENT_TIMESTAMP, @Texte, @NomImage, @NomPdf, @CategorieId,@UtilisateurId);" +
                    "SELECT LAST_INSERT_ID();";
                command.Parameters.AddWithValue("@Titre", Titre);
                command.Parameters.AddWithValue("@SousTitre", SousTitre);
                command.Parameters.AddWithValue("@Texte", Texte);
                command.Parameters.AddWithValue("@NomImage", NomImage);
                command.Parameters.AddWithValue("@NomPdf", NomPdf);
                command.Parameters.AddWithValue("@CategorieId", CategorieId);
                command.Parameters.AddWithValue("@UtilisateurId", UtilisateurId);

                MySqlDataReader rdr = command.ExecuteReader();

                int newArticleId = 0;
                if (rdr.Read())
                {
                    newArticleId = rdr.GetInt32(0);
                }

                this.connection.Close();
                article = this.getArticleById(newArticleId);

            }
            catch (Exception error)
            {
                _logger.Error("Erreur pendant la creation de l'article. Motif : " + error.StackTrace);
            }

            return article;
        }

        //Modifier un article
        public bool updateArticle(int Id, string Titre, string SousTitre, string Texte, string miniature, string documentation, int CategorieId)
        {
            bool retourUpdateArticle = true;

            try
            {
                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "UPDATE `Article` SET " +
                        "`Titre` = @Titre , `SousTitre` = @SousTitre, `Date` = CURRENT_TIMESTAMP," +
                        " `Texte`= @Texte,`Image`= @miniature, `NomPdf`= @documentation, `Fk_Categorie_Id` = @CategorieId WHERE `Article`.Id = @Id;";

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Titre", Titre);
                command.Parameters.AddWithValue("@SousTitre", SousTitre);
                command.Parameters.AddWithValue("@Texte", Texte);
                command.Parameters.AddWithValue("@miniature", miniature);
                command.Parameters.AddWithValue("@documentation", documentation);
                command.Parameters.AddWithValue("@CategorieId", CategorieId);

                MySqlDataReader rdr = command.ExecuteReader();
            }
            catch (Exception error)
            {
                retourUpdateArticle = false;
                _logger.Error("Erreur pendant la mise à jour de l'article. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourUpdateArticle;
        }

        //Supprimer un article
        public bool deleteArticleById(int ArticleId)
        {
            bool retourSuppresion = true;
            try
            {

                this.connection.Open();

                MySqlCommand command = this.connection.CreateCommand();

                command.CommandText = "DELETE FROM Article WHERE Id = @ArticleId";
                command.Parameters.AddWithValue("@ArticleId", ArticleId);

                MySqlDataReader rdr = command.ExecuteReader();

                //le delete peu se faire sur un id inexistant sans retoruner d'erreur.
                if (rdr.RecordsAffected == 0)
                {
                    retourSuppresion = false;
                }

            }
            catch (Exception error)
            {
                retourSuppresion = false;
                _logger.Error("Erreur pendant la suppression de l'article. Motif : " + error.StackTrace);
            }
            finally
            {
                this.connection.Close();
            }

            return retourSuppresion;
        }

    }
}
