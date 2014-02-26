/// <reference path="Scripts/jquery-2.0.3.js" />

$(document).ready(function () {
    $(".tile").on("click", function () {
        var url = $(this).find(".url").html();

        if (url) {
            window.location = url;
        } else {
            $.post("/home/tile/" + $(this).find(".id").html(), null, function (result) {
                if (result.redirectUrl != null) {
                    window.location = result.redirectUrl;
                }
            });
        }
    });

    // Proxy created on the fly
    var tile = $.connection.tile;

    // Declare a function on the chat hub so the server can invoke it
    tile.client.UpdateTile = function (m) {
    };

    $.connection.hub.start();
});