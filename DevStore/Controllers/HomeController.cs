using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DevStore.Models;
using DevStore.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using DevStore.Data;
using DevStore.ActionFilters;

namespace DevStore.Controllers
{
    public class HomeController : Controller 
    {
        //Controller initial
        public IActionResult index()
        {
            ArticleModel articleModel = new ArticleModel();
            MembreModel membreModel = new MembreModel();
            IndexViewModel viewModel = new IndexViewModel();

            viewModel.listArticle = articleModel.getRecentsArticles();
            viewModel.listMembre = membreModel.getAllMembre();

            return View("Index", viewModel);
        }

        //Méthode de Controller pour lister tout les articles
        public IActionResult articlesView()
        {

            ArticleModel articleModel = new ArticleModel();

            ArticleViewModel viewModel = new ArticleViewModel();
            viewModel.listArticle = articleModel.getAllArticles();

            CategorieModel categorieModel = new CategorieModel();
            viewModel.listCategorie = categorieModel.getAllCategories();

            return View("ArticlesView", viewModel);
        }

        //Méthode de Controller pour le tris des articles
        public IActionResult filterArticles(int categorieId)
        {
            ArticleModel articleModel = new ArticleModel();

            List<ArticleModel> listeArticle = new List<ArticleModel>();

            if (categorieId == 0)
            {
               listeArticle = articleModel.getAllArticles();
            }
            else
            {
                 listeArticle = articleModel.getAllArticlesByCategorie(categorieId);
            }
           

            return new JsonResult(new
            {
                listeArticle = listeArticle
            });
        }

        //Méthode de Controller detail d'un article
        public IActionResult viewArticle(int articleId)
        {
            ArticleModel articleModel = new ArticleModel();
            ArticleViewModel viewModel = new ArticleViewModel();

            viewModel.article = articleModel.getArticleById(articleId);
            return View("DetailsArticleView", viewModel);
        }

        //Méthode de Controller telechargment du pdf d'un article
        public ActionResult downloadArticlePdf(int articleId,string nomPdf)
        {

            string dossierArticle = "article_" + articleId;

            string filePath = $"~/assets/articles/{dossierArticle}/downloadDocument/" + nomPdf;
            Response.Headers.Add("Content-Disposition", "inline; filename="+ nomPdf);
            return File(filePath, "application/pdf");
        }

        //Méthode de Controller de navigation pour aller sur la page connexion 
        public IActionResult connexionView()
        {
            return View("ConnexionView");
        }

        //Méthode de Controller de connexion
        public IActionResult connexion(string EmailLogin, string PasswordLogin)
        {

            ConnexionData connexionData = new ConnexionData();
            UtilisateurModel utilisateur = connexionData.getUtilisateur(EmailLogin, PasswordLogin);

            if (utilisateur.Id != 0)
            {
                HttpContext.Session.SetString("SessionUser", JsonSerializer.Serialize(utilisateur));
                return RedirectToAction("AdminView","Admin");
            }
            else
            {
                ViewBag.ErrMsg = "Merci de renseigner un Email et un Mot de passe valide";
                return View("ConnexionView");
            }

        }

        //Méthode de Controller de deconnexion
        public IActionResult deconnexion()
        {
            //remove session user
            HttpContext.Session.Remove("SessionUser");

            return connexionView();
        }

        //Méthode de Controller de redirection apres un echec de session admin
        public IActionResult TimeOut()
        {
            ViewBag.ErrMsg ="Session expirée, veuillez vous reconnecter.";

            return connexionView();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
