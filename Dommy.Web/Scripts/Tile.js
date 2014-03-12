/// <reference path="Scripts/jquery-2.0.3.js" />

$(document).ready(function () {
    $(".gridSection").each(function () {
        var maxWidth = $(this).width();
        var x = 0;
        var y = 0;
        $(this).css({ position: 'relative' })
            .find(".tile").each(function () {
                $(this).css({ position: 'absolute'});

                if ((x + $(this).outerWidth()) > maxWidth) {
                    x = 0;
                    y += $(this).outerHeight();
                }

                $(this).css({ left: x, top: y });

                x += $(this).outerWidth();
            });

        $(this).outerHeight(y);
        $(this).outerWidth(maxWidth);
    });

    $(".tile")
        .mousedown(function (e) {
        if (e.which === 3) {
            $(".grid").addClass("edit");
            $(this).addClass("selected");
            $(".tile").each(function () {
                $(this).css(
                    {
                        animation: 'floatAnimation' + Math.floor(Math.random() * 5) + ' 10s',
                        animationIterationCount: 'infinite'
                    });
            });
            return false;
        } else {
            var editMode = $(".grid").hasClass("edit");

            if (editMode) {
                $(".tile").removeClass("selected");
                $(this).addClass("selected");
                return false;
            } else {
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
            }
        }
    }).bind("contextmenu", function (e) {
        return false;
    });

    $(document).mousedown(function (event) {
        $(".grid").removeClass("edit");
        $(".grid .tile").removeClass("selected");
    })

    $('a[href^="#"]').click(function () {
        var the_id = $(this).attr("href");
        $('html, body').animate({
            scrollTop: $(the_id).offset().top - ($(".gridMenu").offset().top - $(window).scrollTop())
        }, 'slow');
        return false;
    });

    $(window).scroll(function () {
        var scrollValue = $(window).scrollTop();
        //var scrollValueMax = $(window).outerHeight() - ($(".gridMenu").offset().top -scrollValue);
        var scrollValueMax = $(document).outerHeight() - $(window).outerHeight();// - $(window).height();
        var v = 0;

        $(".menu").each(function () {
            var top = $(this).offset().top + $(this).outerHeight() - scrollValue;
            var the_id = $(this).attr("href");

            var h = $(the_id).outerHeight();

            //$(this).find(".back").text('scrollValue : ' + scrollValue + ' max : ' + scrollValueMax + ' heightSec ' + v);
            var arrow = $(this).find(".arrowRight");
            if ((scrollValue + 10 >= v
                && scrollValue + 10 < v + h
                && scrollValue + 10 < scrollValueMax)
                || (scrollValue + 10 > scrollValueMax && v > scrollValueMax)) {
                arrow.addClass('selected');
                arrow.removeClass('unselected');
            } else {
                arrow.removeClass('selected');
                arrow.addClass('unselected');
            }
            v += h;
        });
    });


    // Proxy created on the fly
    var tile = $.connection.tile;

    // Declare a function on the chat hub so the server can invoke it
    tile.client.UpdateTile = function (m) {
    };

    $.connection.hub.start();
});