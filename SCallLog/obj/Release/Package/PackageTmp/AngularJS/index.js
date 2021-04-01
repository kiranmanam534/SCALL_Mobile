app.controller('MapCTRL', function ($scope, $http, $filter) {
    $scope.ErrorResult = "";
    $http.get("../User/getMapAssignedComplaints").then(
        
            function (response) {
                console.log('Pagination');
                console.log(response.data);
                if (response.data.success) {
                    $scope.ErrorResult = "";
                var complaints = response.data.success.complaints;
                if (complaints.length > 0) {
                    initMap(complaints);

                }
            }
                else {
                    $scope.ErrorResult = "No result found!";                
            }
                
            },
            function (err) {
                var error = err;
        });

    function initMap(complaints) {

        var myOptions = {
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            center: {
                lat: parseFloat(complaints[0].LAT),
                lng: parseFloat(complaints[0].LONG)
            }
        };
        map = new google.maps.Map(document.getElementById("map"), myOptions);
        var j = 0;
        var routecolor = ['#FF5733', 'red', 'blue', 'green', 'orange', 'yellow', 'black'];
        for (var i = 0; i < complaints.length - 1; i++) {
            var start = complaints[i].LAT + "," + complaints[i].LONG;
            var end = complaints[i+1].LAT + "," + complaints[i+1].LONG;
            var method = 'DRIVING';
            var colorcode = '';
            colorcode = routecolor[j];
            j = j + 1;
            if (i == 6) {
                j = 0;
            }
            if (i == 0) {
                drawRoute(start, end, method, animate = true, color = colorcode, i) // if color variable not passed, defaults to preset color

            } else {
                drawRoute(start, end, method, animate = true, color = colorcode, i) // if color variable not passed, defaults to preset color

            }
        }

        //angular.forEach(complaints, function (value, i) {
        //    if (complaints.length - 1 >= i) {

        //    }
            

        //});

        
        // Repeat the following lines to draw multiple routes on map.
        //var start = "-29.857884700000003,31.025781600000002";
        //var end = "-29.860912499999994,31.02630078125002";
        //var method = 'DRIVING';
        //drawRoute(start, end, method, animate = true, color = '#4caf50') // pass animate = false to animate dotted line

        //  var start = "-29.860912499999994,31.02630078125002";
        //var end = "Neyyanttinkara";
        //var method = 'DRIVING';
        //drawRoute(start, end, method,animate = true, color = '#4caf50') // pass animate = false to animate dotted line

    }
    
    const labels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    let labelIndex = 0;
    function drawRoute(start, end, method, animate = true, color = '#e53935', index) {
        var directionsService = new google.maps.DirectionsService();
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.DirectionsTravelMode[method]
        };
        
        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                var lineSymbol = {
                    // path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
                    path: google.maps.SymbolPath.CIRCLE,

                    fillOpacity: 1,
                    scale: 3
                };

                var routePath = new google.maps.Polyline({
                    path: response.routes[0].overview_path,
                    geodesic: true,
                    strokeColor: color,
                    strokeOpacity: 0,
                    fillOpacity: 0,
                    icons: [{
                        icon: lineSymbol,
                        offset: '0',
                        repeat: '10px'
                    }],
                });
                if (animate) {
                    animateLine(routePath);
                }
                routePath.setMap(map);

                const contentString =
                    '' + index;
                const infowindow = new google.maps.InfoWindow({
                    content: contentString,
                });
                
                

                var marker = new google.maps.Marker({
                    position: response.routes[0].overview_path[0],
                    label: labels[labelIndex++ % labels.length],
                    map: map,
                    title: index


                });
                marker.addListener("click", () => {
                    infowindow.open(map, marker);
                });
                var marker1 = new google.maps.Marker({
                    position: response.routes[0].overview_path[response.routes[0].overview_path.length - 1],
                    label: labels[labelIndex++ % labels.length],
                    map: map,
                    title: index + 1
                });
                marker1.addListener("click", () => {
                    infowindow.open(map, marker);
                });

            }
        });
    }

    function animateLine(line) {

        var count = 0;
        var zoomLevel;
        var markSpeed;
        var multiPointer = 10;

        window.setInterval(function () {


            count = (count + 1) % 200;
            var icons = line.get('icons');
            icons[0].offset = (count / markSpeed) + '%';
            line.set('icons', icons);


            var getZoom0 = line.get('map');
            var getZoom1 = getZoom0.getZoom();

            zoomLevel = getZoom1;

            if (zoomLevel >= 21) {
                // markSpeed = 120;
                markSpeed = multiPointer * zoomLevel / 0.2;
            }
            else if (zoomLevel >= 19) {
                // markSpeed = 120;
                markSpeed = multiPointer * zoomLevel / 0.5;
            }
            else if (zoomLevel >= 16) {
                // markSpeed = 60;
                markSpeed = multiPointer * zoomLevel / 2;

            }
            else {
                // markSpeed = 10;
                markSpeed = multiPointer * zoomLevel / 20;

            }

            console.log("Zoom Level :" + zoomLevel);
            console.log("Mark Speed :" + markSpeed);


        }, 100);

    }

});

