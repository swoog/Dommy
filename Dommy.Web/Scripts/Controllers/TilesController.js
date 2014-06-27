(function () {
    var app = angular.module("dommy", []);

    var TilesController = function ($scope, $http) {

        $http.get("/home/tiles")
            .then(function (response) {
                $scope.sections = response.data;
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

    app.controller("TilesController", TilesController);
})