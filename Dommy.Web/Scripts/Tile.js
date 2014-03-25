﻿/// <reference path="Scripts/jquery-2.0.3.js" />

$(document).ready(function () {
    $(".gridSection").each(function () {
        var maxWidth = $(this).width();
        var x = 0;
        var y = 0;
        var maxHeight = 0;
        $(this).css({ position: 'relative' })
            .find(".tile").each(function () {
                $(this).css({ position: 'absolute' });

                if ((x + $(this).outerWidth()) > maxWidth) {
                    x = 0;
                    y += $(this).outerHeight();
                }

                $(this).css({ left: x, top: y });

                x += $(this).outerWidth();
                if (y + $(this).outerHeight() > maxHeight) {
                    maxHeight = y + $(this).outerHeight();
                }
            });

        $(this).outerHeight(maxHeight);
        $(this).outerWidth(maxWidth);
    });

    $(".tile")
        .mousedown(function (e) {
            if (e.which === 3) {
                $(".grid").addClass("edit");
            } 

            var editMode = $(".grid").hasClass("edit");

            if (editMode) {
                $(".tile").removeClass("selected");
                $(".tile").each(function () {
                    if ($(this).css("animationIterationCount") != 'infinite') {
                        $(this).css(
                            {
                                animation: 'floatAnimation' + (Math.floor(Math.random() * 4) + 1) + ' 10s',
                                animationIterationCount: 'infinite'
                            });
                    }
                });

                $(this).addClass("selected")
                    .css(
                        {
                            animationIterationCount: '1'
                        });
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
        }).bind("contextmenu", function (e) {
            return false;
        });

    $(document).mousedown(function (event) {
        $(".grid").removeClass("edit");
        $(".grid .tile").removeClass("selected").css(
                        {
                            animation: '',
                            animationIterationCount: '0'
                        });
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
    tile.client.UpdateTile = function (t) {
        var name = "UpdateTile" + t.Id;

        var f = window[name];

        if (f) {
            f(t);
        }
    };

    $.connection.hub.start();
});