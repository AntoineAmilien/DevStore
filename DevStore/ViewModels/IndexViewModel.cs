using System;
using System.Collections.Generic;
using DevStore.Models;

namespace DevStore.ViewModels
{
    public class IndexViewModel
    {
        public List<ArticleModel> listArticle { get; set; }
        public List<MembreModel> listMembre { get; set; }
    }
}
