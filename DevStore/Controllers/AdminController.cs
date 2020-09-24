using Microsoft.AspNetCore.Mvc;
using DevStore.Models;
using DevStore.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using DevStore.Data;
using DevStore.ActionFilters;

namespace DevStore.Controllers
{

    public class AdminController : Controller
    {

        //Variable d'environnement
        private IWebHostEnvironment Environment;

        //Constructeur
        public AdminController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        #region regionNavigationView

        //Méthode de Controller de navigation pour aller sur la page d'adiministration
        [SessionAdminOut]
        public IActionResult adminView()
        {

            CategorieModel categorieModel = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();

            //recuperation des details en session sur l'utilisateur connecté -> "SessionUser";
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            StatistiqueDao statistique = new StatistiqueDao();
            viewModel.nbArticlesGlobal = statistique.getNbArticles();
            viewModel.listNbArticlesByCategories = statistique.getNbArticlesByCategories();
            viewModel.listNbArticlesByUtilisateurs = statistique.getNbArticlesByUtilisateurs();

            return View("~/Views/Admin/DashboardView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page d'ajout d'un article
        [SessionAdminOut]
        public IActionResult newArticleView()
        {
          
            CategorieModel categorieModel = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.listCategorie = categorieModel.getAllCategories();

            //recuperation des details en session sur l'utilisateur connecté -> "SessionUser";
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            return View("~/Views/Admin/newArticleView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page de vision et d'aministration des articles
        [SessionAdminOut]
        public IActionResult viewAndAdminArticlesView()
        {

            ArticleModel articleModel = new ArticleModel();
            CategorieModel categorieModel = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.listArticle = articleModel.getAllArticles();
            viewModel.listCategorie = categorieModel.getAllCategories();

            return View("~/Views/Admin/viewAndAdminArticleView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour la page de modification d'un article
        [SessionAdminOut]
        public IActionResult updateArticleView(int ArticleId)
        {
            CategorieModel categorieModel = new CategorieModel();
            ArticleModel articleModel = new ArticleModel();

            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.listCategorie = categorieModel.getAllCategories();
            viewModel.article = articleModel.getArticleById(ArticleId);

            return View("~/Views/Admin/updateArticleView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page de vision et d'aministration des categories
        [SessionAdminOut]
        public IActionResult viewAndAdminCategoriesView()
        {
            CategorieModel categorieModel = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.listCategorie = categorieModel.getAllCategories();

            return View("~/Views/Admin/viewAndAdminCategoriesView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page d'ajout d'un article
        [SessionAdminOut]
        public IActionResult newCategorieView()
        {
            CategorieModel categorieModel = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.listCategorie = categorieModel.getAllCategories();

            //recuperation des details en session sur l'utilisateur connecté -> "SessionUser";
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            //return View("~/Views/Home/NouvelArticleView.cshtml", viewModel);
            return View("~/Views/Admin/newCategorieView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour la page de modification d'une categorie
        [SessionAdminOut]
        public IActionResult updateCategorieView(int categorieId)
        {
            CategorieModel categorie = new CategorieModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.categorie = categorie.getCategorieById(categorieId);

            return View("~/Views/Admin/updateCategorieView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page de vision et d'aministration des Membres
        [SessionAdminOut]
        public IActionResult viewAndAdminMembresView()
        {
            MembreModel membreModel = new MembreModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.listMembre = membreModel.getAllMembre();

            return View("~/Views/Admin/viewAndAdminMembre.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page d'ajout d'un Membre
        [SessionAdminOut]
        public IActionResult newMembreView()
        {
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            return View("~/Views/Admin/newMembreView.cshtml", viewModel);
        }

        //Méthode de Controller de navigation pour la page de modification d'un Membre
        [SessionAdminOut]
        public IActionResult updateMembreView(int MembreId)
        {
            MembreModel membre = new MembreModel();
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.membre = membre.getMembreById(MembreId);
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            return View("~/Views/Admin/updateMembreView.cshtml",viewModel);
        }

        //Méthode de Controller de navigation pour aller sur la page d'aministration de son profil utilisateur
        [SessionAdminOut]
        public IActionResult updateUtilisateurView()
        {
            //recuperation ds information user
            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            return View("~/Views/Admin/updateUtilisateurView.cshtml",viewModel);
        }

        #endregion

        //Méthode de Controller pour poster un article
        [SessionAdminOut]
        public IActionResult newArticle(string Titre, string SousTitre, string Texte, int CategorieId)
        {
            //recuperation du fichier pdf passé dans formData
            var FichierPdf = Request.Form.Files["nomPdf"];

            string NomPdf = null;
            if (FichierPdf != null)
            {
                NomPdf = FichierPdf.FileName;
            }

            //recuperation du fichier img passé dans formData
            var FichierMiniature = Request.Form.Files["imageMiniature"];

            string NomImage = null;
            if (FichierMiniature != null)
            {
                NomImage = FichierMiniature.FileName;
            }


            ArticleModel articleModel = new ArticleModel();

            //recuperation des details en session sur l'utilisateur connecté -> "SessionUser";
            UtilisateurModel utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            ArticleModel nouvelArticle = articleModel.postArticle(Titre, SousTitre, Texte, NomPdf, NomImage, CategorieId, utilisateur.Id);


            bool retourNewArticle = false;
            if (nouvelArticle.Id != null && nouvelArticle.Id != 0)
            {
                try
                {
                    string dossierArticle_n = "article_" + nouvelArticle.Id;

                    // creation du nom de dossier parent en fonction de l'id de l'article
                    string srcArticle_n = this.Environment.WebRootPath + $@"/assets/articles/" + dossierArticle_n;
                    Directory.CreateDirectory(srcArticle_n);
                    // creation du sous dossier miniature
                    string srcMiniature = this.Environment.WebRootPath + $@"/assets/articles/" + dossierArticle_n + "/miniature";
                    Directory.CreateDirectory(srcMiniature);
                    // creation du sous dossier downloadDocument
                    string srcDownloadDocument = this.Environment.WebRootPath + $@"/assets/articles/" + dossierArticle_n + "/downloadDocument";
                    Directory.CreateDirectory(srcDownloadDocument);
                    // creation du sous dossier images
                    string srcImages = this.Environment.WebRootPath + $@"/assets/articles/" + dossierArticle_n + "/images";
                    Directory.CreateDirectory(srcImages);


                    if (nouvelArticle.Documentation != null)
                    {
                        string linkName = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/downloadDocument/{FichierPdf.FileName}";
                        //flush PDF
                        using (FileStream fs = System.IO.File.Create(linkName))
                        {
                            FichierPdf.CopyTo(fs);
                            fs.Flush();
                        }

                    }

                    if (nouvelArticle.Miniature != null)
                    {
                        string linkName = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/miniature/{FichierMiniature.FileName}";
                        //flush image miniature
                        using (FileStream fs = System.IO.File.Create(linkName))
                        {
                            FichierMiniature.CopyTo(fs);
                            fs.Flush();
                        }
                    }

                    retourNewArticle = true;
                }
                catch (Exception)
                {
                    retourNewArticle = false;
                }

            }

            if (retourNewArticle != true)
            {
                ViewBag.ErrMsg = "Error: Impossible d'ajouter l'article";
            }
            else
            {
                ViewBag.ValidationMsg = "Validation: Article ajouté avec succés";
            }

            return newArticleView();
        }

        //Méthode de Controller pour modifier un article
        [SessionAdminOut]
        public IActionResult updateArticle(int Id, string Titre, string SousTitre, string Texte, string miniatureActuelle, string documentationActuelle, int CategorieId)
        {
            string dossierArticle_n = "article_" + Id;

            //recuperation du fichier pdf passé dans formData
            var FichierDocumentation = Request.Form.Files["documentation"];
            string documentation = null;
            if (FichierDocumentation != null)
            {
                documentation = FichierDocumentation.FileName;
                //suppression de l'ancienne documentation
                string path = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/downloadDocument/{documentationActuelle}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                string linkName = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/downloadDocument/{FichierDocumentation.FileName}";
                //flush documentation
                using (FileStream fs = System.IO.File.Create(linkName))
                {
                    FichierDocumentation.CopyTo(fs);
                    fs.Flush();
                }
            }
            else
            {
                documentation = documentationActuelle;
            }

            //recuperation du fichier miniature passé dans formData
            var FichierMiniature = Request.Form.Files["miniature"];
            string miniature = null;
            if (FichierMiniature != null)
            {
                miniature = FichierMiniature.FileName;
                //suppression de l'ancienne miniature
                string path = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/miniature/{miniatureActuelle}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                string linkName = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}/miniature/{FichierMiniature.FileName}";
                //flush miniature
                using (FileStream fs = System.IO.File.Create(linkName))
                {
                    FichierMiniature.CopyTo(fs);
                    fs.Flush();
                }

            }
            else
            {
                miniature = miniatureActuelle;
            }

            CategorieModel categorieModel = new CategorieModel();
            ArticleModel articleModel = new ArticleModel();

            if (!articleModel.updateArticle(Id, Titre, SousTitre, Texte, miniature, documentation, CategorieId))
            {
                ViewBag.ErrMsg = "Error: Impossible de modifier l'article";
            }
            else
            {
                ViewBag.ValidationMsg = "Validation: Article modifié avec succés";
            }


            AdminViewModel viewModel = new AdminViewModel();
            viewModel.utilisateur = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));
            viewModel.listCategorie = categorieModel.getAllCategories();
            viewModel.article = articleModel.getArticleById(Id);

            return View("~/Views/Admin/UpdateArticleView.cshtml", viewModel);
        }

        //Méthode de Controller pour supprimer une categorie
        [SessionAdminOut]
        public IActionResult deleteArticle(int ArticleId)
        {
            ArticleModel articleModel = new ArticleModel();

            //suppresion du dossier de ressouces associés
            if (!articleModel.deleteArticleById(ArticleId))
            {
                ViewBag.ErrMsg = "Impossible de supprimer cet article";
            }
            else
            {
                string dossierArticle_n = "article_" + ArticleId;

                string path = this.Environment.WebRootPath + $@"/assets/articles/{dossierArticle_n}";

                //obtenir le contenu du dossier "article_*"
                string[] dirs = Directory.GetDirectories(path);

                //suppresion des sous dossiers
                foreach (string dir in dirs)
                {
                    Directory.Delete(dir,true);
                }

                //suppresion du dossier "article_*"
                Directory.Delete(path);
                ViewBag.ValidationMsg = "Article Supprimé";
            }

            return viewAndAdminArticlesView();
        }

        //Méthode de Controller pour modifier une categorie
        [SessionAdminOut]
        public IActionResult updateCategorie(int Id, string Intitule, string ImageActuelle)
        {
            //recuperation du fichier miniature passé dans formData
            var FichierImage = Request.Form.Files["image"];
            string image = null;
            if (FichierImage != null)
            {
                image = FichierImage.FileName;
                //suppression de l'ancienne miniature
                string path = this.Environment.WebRootPath + $@"/assets/miniatureCategorie/{ImageActuelle}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                string linkName = this.Environment.WebRootPath + $@"/assets/miniatureCategorie/{FichierImage.FileName}";
                //flush miniature
                using (FileStream fs = System.IO.File.Create(linkName))
                {
                    FichierImage.CopyTo(fs);
                    fs.Flush();
                }

            }
            else
            {
                image = ImageActuelle;
            }

            CategorieModel categorieModel = new CategorieModel();

            if (!categorieModel.updateCategorie(Id, Intitule, image))
            {
                ViewBag.ErrMsg = "Error: Impossible de modifier la categorie";
            }
            else
            {
                ViewBag.ValidationMsg = "Validation: Categorie modifiée avec succés";
            }


            return updateCategorieView(Id);
        }

        //Méthode de Controller pour ajouter une categorie
        [SessionAdminOut]
        public IActionResult newCategorie(string Intitule)
        {
            //recuperation du fichier pdf passé dans formData
            var FichierImage = Request.Form.Files["ImageCategorie"];

            string linkName = this.Environment.WebRootPath + $@"/assets/miniatureCategorie/{FichierImage.FileName}";

            using (FileStream fs = System.IO.File.Create(linkName))
            {
                FichierImage.CopyTo(fs);
                fs.Flush();
            }

            string NomImage = FichierImage.FileName;

            CategorieModel categorieModel = new CategorieModel();
            if (!categorieModel.postNewCategorie(Intitule, NomImage))
            {
                ViewBag.ErrMsg = "Impossible d'ajouter cette categorie";
                //supression du fichier chargé
                System.IO.File.Delete(linkName);
            }
            else
            {
                ViewBag.ValidationMsg = "Nouvelle catégorie ajoutée";
            }


            return RedirectToAction("viewAndAdminCategoriesView");
        }

        //Méthode de Controller pour supprimer une categorie
        [SessionAdminOut]
        public IActionResult deleteCategorie(int categorieId)
        {
            CategorieModel categorieModel = new CategorieModel();

            categorieModel = categorieModel.getCategorieById(categorieId);

            if (!categorieModel.postDeleteCategorie(categorieId))
            {
                ViewBag.ErrMsg = "Error: Impossible de supprimer cette catégorie";
            }
            else
            {
                //suppresion de son image associée
                string path = this.Environment.WebRootPath + $@"/assets/miniatureCategorie/{categorieModel.Image}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                ViewBag.ValidationMsg = "Validation: Catégorie supprimée";

            }

            return viewAndAdminCategoriesView();
        }

        //Méthode de Controller pour ajouter un membre
        [SessionAdminOut]
        public IActionResult newMembre(string Nom, string Prenom, string Profession)
        {
            MembreModel membreModel = new MembreModel();

            //recuperation du fichier bitmoji passé dans formData
            var FichierBitmoji = Request.Form.Files["Bitmoji"];

            string path = this.Environment.WebRootPath + $@"/assets/bitmoji/{FichierBitmoji.FileName}";

            if (System.IO.File.Exists(path))
            {
                ViewBag.ErrMsg = "Error: Impossible d'ajouter ce membre car le nom du fichier bitmoji existe deja";
            }
            else
            {
                if (membreModel.postMembre(Nom, Prenom, Profession, FichierBitmoji.FileName))
                {
                    using (FileStream fs = System.IO.File.Create(path))
                    {
                        FichierBitmoji.CopyTo(fs);
                        fs.Flush();
                    }
                    ViewBag.ValidationMsg = "Membre ajouté";
                }
                else
                {
                    ViewBag.ErrMsg = "Error: Impossible d'ajouter ce membre";
                }
            }

            return newMembreView();
        }

        //Méthode de Controller pour ajouter un membre
        [SessionAdminOut]
        public IActionResult updateMembre(int Id, string Nom, string Prenom, string Profession,string BitmojiActuel)
        {
            MembreModel membreModel = new MembreModel();

            //recuperation du fichier bitmoji passé dans formData
            var FichierBitmoji = Request.Form.Files["Bitmoji"];

            if (FichierBitmoji != null)
            {
               string path = this.Environment.WebRootPath + $@"/assets/bitmoji/{FichierBitmoji.FileName}";

                if (System.IO.File.Exists(path))
                {
                    ViewBag.ErrMsg = "Error: Impossible d'ajouter ce membre car le nom du fichier bitmoji existe deja";
                }
                else
                {
                    if (membreModel.updateMembre(Id, Nom, Prenom, Profession, FichierBitmoji.FileName))
                    {
                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            FichierBitmoji.CopyTo(fs);
                            fs.Flush();
                        }
                        string pathDelete = this.Environment.WebRootPath + $@"/assets/bitmoji/{BitmojiActuel}";
                        System.IO.File.Delete(pathDelete);
                        ViewBag.ValidationMsg = "Membre modifié";
                    }
                    else
                    {
                        ViewBag.ErrMsg = "Error: Impossible de modifier ce membre";
                    }
                }

            }
            else
            {
                if(membreModel.updateMembre(Id, Nom, Prenom, Profession, BitmojiActuel))
                {
                    ViewBag.ValidationMsg = "Membre modifié";
                }
                else
                {
                    ViewBag.ErrMsg = "Error: Impossible de modifier ce membre";
                }
            }

           
            return updateMembreView(Id);
        }

        //Méthode de Controller pour supprimer un membre
        [SessionAdminOut]
        public IActionResult deleteMembre(int membreId)
        {
            MembreModel membreModel = new MembreModel();

            membreModel = membreModel.getMembreById(membreId);

            if (!membreModel.deleteMembre(membreId))
            {
                ViewBag.ErrMsg = "Error: Impossible de supprimer ce membre";

            }
            else
            {
                //suppresion de son image associée
                string path = this.Environment.WebRootPath + $@"/assets/bitmoji/{membreModel.Image}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                ViewBag.ValidationMsg = "Membre supprimé";
            }

            return viewAndAdminMembresView();
        }

        //Méthode de Controller pour modifier un utilisateur
        [SessionAdminOut]
        public IActionResult updateUtilisateur(string Nom, string Prenom, string Email)
        {
            UtilisateurModel utilisateurModel = new UtilisateurModel();
            utilisateurModel = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            if (!utilisateurModel.updateUtilisateur(utilisateurModel.Id, Nom, Prenom, Email))
            {
                ViewBag.ErrMsg = "Error: Impossible de modifier le profil utilisateur";
            }
            else
            {
                ViewBag.ValidationMsg = "Profil utilisateur modifié";

                //mise a jour de la session avec les  nouvelles informations
                utilisateurModel = utilisateurModel.getUtilisateurById(utilisateurModel.Id);
                HttpContext.Session.SetString("SessionUser", JsonSerializer.Serialize(utilisateurModel));
            }

            return updateUtilisateurView();
        }

        //Méthode de Controller pour modifier le mot de passe utilisateur
        [SessionAdminOut]
        public IActionResult updatePasswordUtilisateur(string passwordActuel, string newPassword, string confirmNewPassword)
        {
            UtilisateurModel utilisateurModel = new UtilisateurModel();
            utilisateurModel = JsonSerializer.Deserialize<UtilisateurModel>(HttpContext.Session.GetString("SessionUser"));

            if(newPassword == confirmNewPassword)
            {
                if (passwordActuel == utilisateurModel.Password)
                {
                    if (!utilisateurModel.updatePasswordUtilisateur(utilisateurModel.Id, newPassword))
                    {
                        ViewBag.ErrMsg = "Error: Impossible de modifier le mot de passe";
                    }
                    else
                    {

                        ViewBag.ValidationMsg = "Mot de passe modifié";

                        //mise a jour de la session avec le nouveau mot de passe
                        utilisateurModel = utilisateurModel.getUtilisateurById(utilisateurModel.Id);
                        HttpContext.Session.SetString("SessionUser", JsonSerializer.Serialize(utilisateurModel));
                    }
                }
                else
                {
                    ViewBag.ErrMsg = "Error: Mot de passe erroné";
                }
            }
            else
            {
                ViewBag.ErrMsg = "Error: le nouveau mot de passe et la confirmation ne sont pas identique";
            }

            return updateUtilisateurView();
        }
    }
}
