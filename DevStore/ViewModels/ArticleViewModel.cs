using System;
using System.Collections.Generic;
using DevStore.Models;

namespace DevStore.ViewModels
{
    public class ArticleViewModel : CategorieViewModel
    {

        public List<ArticleModel> listArticle { get; set; }
        public ArticleModel article { get; set;}
        public UtilisateurModel utilisateur { get; set; }

    }
}
