//function pour naviguer sur la partie "equipe" de IndexView.cshtml
function scrollEquipe() {
    $('html, body').animate({
        scrollTop: $("#containerEquipe").offset().top
    }, 2000);
}

//function pour naviguer sur la partie "contact" de IndexView.cshtml
function scrollContact() {
    $('html, body').animate({
        scrollTop: $("#containerContact").offset().top
    }, 2000);
}
