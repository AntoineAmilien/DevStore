using System;
using System.Collections.Generic;
using DevStore.Models;

namespace DevStore.ViewModels
{
    public class AdminViewModel : CategorieViewModel
    {

        public ArticleModel article { get; set; }
        public List<ArticleModel> listArticle { get; set; }
        public UtilisateurModel utilisateur { get; set; }
        public List<MembreModel> listMembre { get; set; }
        public MembreModel membre;

        //statistiques :
        public int nbArticlesGlobal { get; set; }
        public List<dynamic> listNbArticlesByCategories { get; set; }
        public List<dynamic> listNbArticlesByUtilisateurs  { get; set; }

    }
}
