
function tris(id) {

    var container = document.getElementById("containerArticle");

    //suppresion des articles affichés
    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }

    var obj_tris = {
        categorieId: id
    }

    $.ajax({
        type: "POST",
        url: "/Home/FilterArticles",
        data: obj_tris,
        async: false,
        success: function (data) {
            retour = data.listeArticle;
        },
        error: function (data) {
            alert(JSON.stringify(data));
        }
    });


    for (var i = 0; i < retour.length; i++) {
        //template d'article raporter au dom

        var backgroundPath = "";
        if (retour[i].miniature != null) {
            backgroundPath = "/assets/articles/article_" + retour[i].id + "/miniature/" + retour[i].miniature;
        } else {
            backgroundPath = "/assets/miniatureCategorie/" + retour[i].categorie.image;
        }

        container.insertAdjacentHTML('beforeend',

            "<div style='background-image:url(" + backgroundPath + "); background-size: cover;  background-repeat: no-repeat; background-position: center;' class='col-lg-2 col-md-4 col-sm-4 col-6 mt-2 mt-2 ml-1 mr-1 articleGrand'>" +
            "<p>" + retour[i].titre + "</p>" +
            "<p>" + retour[i].sousTitre + "</p>" +
            "<input type='button' value='Lire la suite' class='btn btn-primary btn-sm' onclick='lire(" + retour[i].id + ")' />" +
            "<input type='button' value='Modifier' class='btn btn-primary btn-sm mt-2' onclick='modifier(" + retour[i].id + ")' />" +
            "<input type='button' value='Supprimer' class='btn btn-danger btn-sm mt-2' onclick='supprimer(" + retour[i].id + ")' />" +
            "</div>");
    }
}

function lire(id) {
    window.location.href = "/Home/ViewArticle?ArticleId=" + id;
}

function modifier(id) {
    window.location.href = "/Admin/UpdateArticleView?ArticleId=" + id;
}

function supprimer(id) {
    window.location.href = "/Admin/DeleteArticle?ArticleId=" + id;
}