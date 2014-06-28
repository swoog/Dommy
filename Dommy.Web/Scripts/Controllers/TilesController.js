(function () {
    var app = angular.module("dommy", []);

    var TilesController = function ($scope, $http) {

        var tileMarge = 10;


        $http.get("/home/tiles")
            .success(function (response) {
                $scope.sections = response;

                angular.forEach(response, function (value, key) {
                    var maxWidth = 1024; // TODO : Dynamic
                    var x = tileMarge / 2;
                    var y = tileMarge / 2;
                    var maxHeight = 0;

                    angular.forEach(value.Tiles, function (value, key) {

                        if ((x + value.Width) > maxWidth) {
                            x = tileMarge / 2;
                            y += value.Height;
                        }

                        value.Left = x;
                        value.Top = y;

                        x += value.Width + tileMarge;
                        if (y + value.Height > maxHeight) {
                            maxHeight = y + value.Height + tileMarge;
                        }
                    });

                    value.Height = maxHeight;
                    value.Width = maxWidth;
                });
            });

        // Proxy created on the fly
        var tile = $.connection.tile;

        // Declare a function on the chat hub so the server can invoke it
        tile.client.UpdateTile = function (t) {

            for (var i = 0; i < $scope.sections.length; i++) {
                for (var j = 0; j < $scope.sections[i].Tiles.length; j++) {
                    var tile = $scope.sections[i].Tiles[j];

                    if (tile.Id == t.Id) {
                        $scope.sections[i].Tiles[j] = t;
                    }
                }
            }
        };

        $.connection.hub.start();
    }

    var ColorCss = function () {
        return function (input) {
            return input.charAt(0).toLowerCase() + input.substring(1);
        };
    }

    app.controller("TilesController", TilesController);
    app.filter("ColorCss", ColorCss);
}())