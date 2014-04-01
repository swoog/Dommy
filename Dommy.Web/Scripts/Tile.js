/// <reference path="Scripts/jquery-2.0.3.js" />

$(document).ready(function () {
    var tileMarge = 10;
    function getSize(element) {
        if (element.find('.small').length != 0) {
            return { type: 'small', width: 70, height: 70 };
        }

        if (element.find('.medium').length != 0) {
            return { type: 'medium', width: 150, height: 150 };
        }

        if (element.find('.largeLine').length != 0) {
            return { type: 'largeLine', width: 310, height: 150 };
        }

        if (element.find('.large').length != 0) {
            return { type: 'large', width: 310, height: 310 };
        }
    }

    function updatePosition() {
        $(".gridSection").each(function () {
            var maxWidth = $(this).width();
            var x = tileMarge / 2;
            var y = tileMarge / 2;
            var maxHeight = 0;
            $(this).css({ position: 'relative' })
                .find(".tile").each(function () {
                    var tileSize = getSize($(this));
                    $(this).css({ position: 'absolute' });

                    if ((x + tileSize.width) > maxWidth) {
                        x = tileMarge / 2;
                        y += tileSize.height;
                    }

                    $(this).css({ left: x, top: y });

                    x += tileSize.width + tileMarge;
                    if (y + tileSize.height > maxHeight) {
                        maxHeight = y + tileSize.height + tileMarge;
                    }
                });

            $(this).outerHeight(maxHeight);
            $(this).outerWidth(maxWidth);
        });
    }

    updatePosition();

    $(".flecheTile").mousedown(function (e) {
        var tiles = null;
        tiles = $(".tile.selected .small");
        if (tiles.length > 0) {
            tiles.removeClass("small").addClass("medium");
            positionFleche($(".tile.selected"));
            updatePosition();
            return false;
        }

        tiles = $(".tile.selected .medium");
        if (tiles.length > 0) {
            tiles.removeClass("medium").addClass("largeLine");
            positionFleche($(".tile.selected"));
            updatePosition();
            return false;
        }

        tiles = $(".tile.selected .largeLine");
        if (tiles.length > 0) {
            tiles.removeClass("largeLine").addClass("large");
            positionFleche($(".tile.selected"));
            updatePosition();
            return false;
        }

        tiles = $(".tile.selected .large");
        if (tiles.length > 0) {
            tiles.removeClass("large").addClass("small");
            positionFleche($(".tile.selected"));
            updatePosition();
            return false;
        }
    });


    function positionFleche(tile) {
        var size = getSize(tile);
        var offset = tile.offset()
        var left = offset.left + size.width - ($(".flecheTile").outerWidth() / 2);
        var top = offset.top + size.height - ($(".flecheTile").outerHeight() / 2);

        var classFleche = 'b';

        if (size.type == 'small') {
            classFleche = 'bd';
        } else if (size.type == 'medium') {
            classFleche = 'd';
        } else if (size.type == 'largeline') {
            classFleche = 'b';
        } else if (size.type == 'large') {
            classFleche = 'hg';
        }

        $(".flecheTile")
            .removeClass('b')
            .removeClass('bd')
            .removeClass('d')
            .removeClass('hg')
            .css({ position: 'absolute', left: left, top: top })
            .addClass(classFleche);
    }

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
                        //$(this).css(
                        //    {
                        //        animation: 'floatAnimation' + (Math.floor(Math.random() * 4) + 1) + ' 10s',
                        //        animationIterationCount: 'infinite'
                        //    });
                    }
                });

                $(this).addClass("selected")
                    .css(
                        {
                            animationIterationCount: '1'
                        });

                positionFleche($(this));
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