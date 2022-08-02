// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadFavorite(ISBN_13, htmlThis, isFavorite) {
    var unfavorite = '<a asp-controller="UserFavorites" asp-action="Delete" asp-route-ISBN_13="@ViewData[" ISBN_13"]" onclick = "loadFavorite(@ViewData["ISBN_13"], this, false)" onmouseover = "UnfavoriteOver(this)" onmouseout = "UnfavoriteOut(this)" style = "margin-bottom: 2px; padding-left: 30px; padding-right: 30px;" class="btn btn-danger" > <i class="bi bi-heart-fill"></i></a > ';
    var favorite = '<a asp-controller="UserFavorites" asp-action="Create" asp-route-ISBN_13="@ViewData["ISBN_13"]" onclick = "loadFavorite(@ViewData["ISBN_13"], this, true)" onmouseover = "FavoriteOver(this)" onmouseout = "FavoriteOut(this)" style = "margin-bottom: 2px; padding-left: 30px; padding-right: 30px;" class="btn btn-light" > <i class="bi bi-heart"></i></a >';
    if (htmlThis != undefined) {
        if (isFavorite) {
            document.getElementById("FavoriteButton" + ISBN_13).innerHTML = unfavorite;
        }
        else {
            document.getElementById("FavoriteButton" + ISBN_13).innerHTML = favorite;
        }
        setTimeout(checkFavoriteUpdate, 5000, ISBN_13);
    }
    else {
        checkFavoriteUpdate(ISBN_13);
    }
}

function checkFavoriteUpdate(ISBN_13) {
    const xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/UserFavorites/_Favorite?ISBN_13=" + ISBN_13, true);
    xhttp.send();
    xhttp.onerror = function () {

    }
    xhttp.onload = function () {
        document.getElementById("FavoriteButton" + ISBN_13).innerHTML = this.responseText;
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