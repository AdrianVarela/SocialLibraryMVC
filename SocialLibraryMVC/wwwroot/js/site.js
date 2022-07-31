// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadFavorite(ISBN_13) {
    const xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/UserFavorites/_Favorite?ISBN_13="+ISBN_13, true);
    xhttp.send();
    xhttp.onerror = function () {

    }
    xhttp.onload = function () {
        document.getElementById("FavoriteButton"+ISBN_13).innerHTML = this.responseText;
        //return this.responseText;
    }
    
}

function FavoriteOver(x) {
    x.innerHTML = "<i class='bi bi-heart-fill'></i>";
    
}
function UnfavoriteOver(x) {
    x.innerHTML = "<i class='bi bi-heart'></i>";
}
function FavoriteOut(x) {
    x.innerHTML = "<i class='bi bi-heart'></i>";
}

function UnfavoriteOut(x) {
    x.innerHTML = "<i class='bi bi-heart-fill'></i>"
}