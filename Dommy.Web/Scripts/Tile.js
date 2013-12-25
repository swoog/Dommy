/// <reference path="Scripts/jquery-2.0.3.js" />

$(document).ready(function () {
    $(".tile").on("click", function () {
        var url = $(this).find(".url").html();
        window.location = url;

        //$.ajax("/home/tile/" + $(this).find(".id").html(), function (result) {
        //    if (result.redirectUrl != null) {
        //        window.location = result.redirectUrl;
        //    }
        //});
    });
});