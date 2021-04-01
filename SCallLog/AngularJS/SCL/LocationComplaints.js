app.controller('LocationComplaintCTRL', function ($scope, $http, $localStorage) {
    $scope.remaingDays = Math.abs($localStorage.RemaingDays);
    $scope.isDeaprtments = true;
    $scope.isCategory = false;
    $scope.isSubCategory = false;
    $scope.isShowTable = false;
    $scope.isMap = false;
    $scope.selectedDepartment = '';
    $scope.selectedCategoryColor = '';
    $scope.selectedCategory = '';
    $scope.selectedCategoryID = '';
    $scope.searchDepartment = '';
    $scope.searchColumn = '';
    $scope.compDeptLocations = {};
    $scope.compCatLocations = {};
    $scope.compSCatLocations = {};
    $scope.colors = [
        "bg-red",
        "bg-green",
        "bg-aqua",
        "bg-yellow",
        "bg-maroon",
        "bg-orange",
        "bg-blue",
        "bg-olive",
        "bg-purple",
        "bg-navy"
    ];
    $scope.DepartmentCounts = [];

    $scope.category = function () {
        $scope.isDeaprtments = true;
        $scope.isCategory = false;
        $scope.isSubCategory = false;
        $scope.isMap = true;
        $scope.isShowTable = false;
        if (map != undefined || map != null) {
            map.remove();
            $("#map1").html("");
            //$("#preMap").empty();
            //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
        }
        $scope.showLeafletMap($scope.compDeptLocations);
    }
    $scope.subCategory = function () {
        $scope.isDeaprtments = false;
        $scope.isCategory = true;
        $scope.isSubCategory = false;
        $scope.isMap = true;
        $scope.isShowTable = false;
        if (map != undefined || map != null) {
            map.remove();
            $("#map1").html("");
            //$("#preMap").empty();
            //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
        }
        $scope.showLeafletMap($scope.compCatLocations);
    }

    $scope.SearchLocationWise = function (Location) {
        //alert(Location)
        if (Location == undefined || Location == "") {
            alertify.alert('Error', "Location Field Mandatory", function () {
            });
        } else {
            $scope.SearchLocation = Location;
            $scope.TotalCatCounts = {}
            $scope.DepartmentCounts = []

            $scope.isDeaprtments = true;
            $scope.isMap = true;
            $scope.isCategory = false;
            $scope.isSubCategory = false;
            if (map != undefined || map != null) {
                map.remove();
                $("#map1").html("");
                //$("#preMap").empty();
                //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
            }
            $http.get("../SCall/getLocationComplaintsCount?Location=" + Location).then(
                function (response) {
                    if (response.data.success == true) {
                        var i = 0;
                        $scope.TotalCounts = response.data.deptCount;
                        $scope.TotalCounts.forEach(function (a) {
                            $scope.DepartmentCounts.push({
                                DEPARTMENT_DESC: a.DEPARTMENT_DESC,
                                DEPARTMENT_ID: a.DEPARTMENT_ID,
                                assCount: a.assCount,
                                compCount: a.compCount,
                                count: a.count,
                                recCount: a.recCount,
                                color: $scope.colors[i % 10]
                            })
                            i++;
                        });
                        $scope.compDeptLocations = response.data.CompLocations;

                        $scope.showLeafletMap(response.data.CompLocations);

                        console.log($scope.DepartmentCounts);

                    } else if (response.data.expire) {
                        window.location.href = '../Home/Login';
                    }
                },
                function (err) {
                    var error = err;
                });
        }
    }
    


    $scope.showCategory = function (department, DeptID, color) {
        //alert(color)

        $scope.selectedDepartment = department;
        $scope.selectedCategoryColor = color;
        $scope.isDeaprtments = false;
        $scope.isMap = true;
        $scope.isCategory = true;
        $scope.isSubCategory = false;
        $scope.TotalCatCounts = {}
        if (map != undefined || map != null) {
            map.remove();
            $("#map1").html("");
            //$("#preMap").empty();
            //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
        }
        $http.get("../SCall/getLocationComplaintsCategorywiseCount?Department=" + department + "&DeptID=" + DeptID + "&Location=" + $scope.SearchLocation).then(
            function (response) {
                if (response.data.success == true) {
                    console.log(response.data);
                    $scope.TotalCatCounts = response.data.catCount;
                    $scope.compCatLocations = response.data.CompLocations;

                    $scope.showLeafletMap(response.data.CompLocations);

                } else if (response.data.expire) {
                    window.location.href = '../Home/Login';
                }
            },
            function (err) {
                var error = err;
            });
    }

    $scope.showSubCategory = function (Category, CatID) {
        //alert($scope.SearchLocation);
        $scope.selectedCategory = Category;
        $scope.selectedCategoryID = CatID;
        $scope.isDeaprtments = false;
        $scope.isCategory = false;
        $scope.isMap = true;
        $scope.isSubCategory = true;
        $scope.TotalCatCounts = {}

        if (map != undefined || map != null) {
            map.remove();
            $("#map1").html("");
            //$("#preMap").empty();
            //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
        }
        $http.get("../SCall/getLocationComplaintsSubCategorywiseCount?Category=" + Category + "&CatID=" + CatID + "&Location=" + $scope.SearchLocation).then(
            function (response) {
                if (response.data.success == true) {
                    console.log(response.data);
                    $scope.TotalSCatCounts = response.data.scatCount;
                    $scope.compSCatLocations = response.data.CompLocations;

                    $scope.showLeafletMap(response.data.CompLocations);
                } else if (response.data.expire) {
                    window.location.href = '../Home/Login';
                }
            },
            function (err) {
                var error = err;
            });
    }

    $scope.viewDashBoard = function () {
        $scope.pageIndex = 1;
        if ($scope.searchColumn == 'Department') {
            $scope.isDeaprtments = true;
            $scope.isCategory = false;
            $scope.isSubCategory = false;
            $scope.isShowTable = false;
            $scope.isMap = true;
            if (map != undefined || map != null) {
                map.remove();
                $("#map1").html("");
                //$("#preMap").empty();
                //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
            }
            $scope.showLeafletMap($scope.compDeptLocations);

        } else if ($scope.searchColumn == 'Category') {
            $scope.isDeaprtments = false;
            $scope.isCategory = true;
            $scope.isSubCategory = false;
            $scope.isMap = true;
            $scope.isShowTable = false;
            if (map != undefined || map != null) {
                map.remove();
                $("#map1").html("");
                //$("#preMap").empty();
                //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
            }
            $scope.showLeafletMap($scope.compCatLocations);

        } else if ($scope.searchColumn == 'SubCategory') {
            $scope.isDeaprtments = false;
            $scope.isCategory = false;
            $scope.isMap = true;
            $scope.isSubCategory = true;
            $scope.isShowTable = false;
            if (map != undefined || map != null) {
                map.remove();
                $("#map1").html("");
                //$("#preMap").empty();
                //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
            }
            $scope.showLeafletMap($scope.compSCatLocations);

        } else {
            $scope.isDeaprtments = true;
            $scope.isCategory = false;
            $scope.isSubCategory = false;
            $scope.isMap = true;
            $scope.isShowTable = false;
            if (map != undefined || map != null) {
                map.remove();
                $("#map1").html("");
                //$("#preMap").empty();
                //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
            }
            $scope.showLeafletMap($scope.compDeptLocations);

        }

    }

    $scope.showLocationComplaintsTableDepartmentWise = function (Department, Search) {
        //alert(Search)
        $scope.searchDepartment = Department;
        $scope.searchColumn = Search;

        $scope.isDeaprtments = false;
        $scope.isCategory = false;
        $scope.isSubCategory = false;
        $scope.isShowTable = true;
        $http.get("../SCall/getAllLocationDepartmentComplaints?pageIndex=" + $scope.pageIndex
            + "&pageSize=" + $scope.pageSizeSelected
            + "&sorting=" + $scope.sorting
            + "&search=" + $scope.search
            + "&SearchLocation=" + $scope.SearchLocation
            + "&SelectedDepartment=" + $scope.selectedDepartment
            + "&SelectedCategory=" + $scope.selectedCategoryID
            + "&Department=" + Department
            + "&SColumn=" + Search).then(
                function (response) {
                    console.log('Pagination')
                    console.log(response.data.success)
                    if (response.data.success) {
                        $scope.complaints = response.data.success.complaints;
                        $scope.totalCount = response.data.success.totalCount;
                        $scope.showLoader = false;
                        $scope.showTable = true;
                    } else {
                        alertify.alert('Error', "No Data Found", function () {
                        });
                    }

                },
                function (err) {
                    var error = err;
                });
    }


    //Start Table Format

    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "Complaint_ReferenceNo";
    $scope.showLoader = true;
    $scope.showTable = false;
    $scope.totalCount = 0;

    // sort ordering (Ascending or Descending). Set true for desending
    $scope.reverse = false;
    $scope.column = "Complaint_ReferenceNo";
    $scope.sort = function (val) {
        $scope.column = val;
        if ($scope.reverse) {
            $scope.reverse = false;
            $scope.reverseclass = 'arrow-up';
        } else {
            $scope.reverse = true;
            $scope.reverseclass = 'arrow-down';
        }
        //alert(val)
        $scope.sorting = val;
        //alert($scope.sorting)
        $scope.showComplaintsTableDepartmentWise($scope.searchDepartment, $scope.searchColumn);
    }
    // remove and change class
    $scope.sortClass = function (col) {
        if ($scope.column == col) {
            if ($scope.reverse) {
                return 'arrow-down';
            } else {
                return 'arrow-up';
            }
        } else {
            return '';
        }
    }

    //Search Method
    $scope.SerachMethod = function (val) {
        $scope.search = val;
        $scope.showComplaintsTableDepartmentWise($scope.searchDepartment, $scope.searchColumn);
    }


    //This method is calling from pagination number
    $scope.pageChanged = function () {
        $scope.showComplaintsTableDepartmentWise($scope.searchDepartment, $scope.searchColumn);
    };

    //This method is calling from dropDown
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.showComplaintsTableDepartmentWise($scope.searchDepartment, $scope.searchColumn);
    };

    //End Table Format

    //Start Map

    var map;

    $scope.showLeafletMap = function (mapData) {



        var kk = "", zoomvalue = 0;
        if (screen.width < 991) {
        }
        else {
            zoomvalue = 2;
        }

        var i = 0;
        map = L.map('map1', {
            center: [10.0, 5.0],
            //minZoom: 2,
            zoom: zoomvalue
        });

        L.tileLayer('http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="">Six Step Solution</a>',
            subdomains: ['a', 'b', 'c']
        }).addTo(map);

        //var myURL = jQuery('script[src$="leaf-demo.js"]').attr('src').replace('leaf-demo.js', '');

        var myIcon = L.icon({
            iconUrl: '../Assets/icons8-marker-48.png',
            iconRetinaUrl: '../Assets/icons8-marker-48.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });
        var elecIcon = L.icon({
            iconUrl: '../Assets/map_elec.png',
            iconRetinaUrl: '../Assets/map_elec.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var adminIcon = L.icon({
            iconUrl: '../Assets/map_admin.png',
            iconRetinaUrl: '../Assets/map_admin.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var billIcon = L.icon({
            iconUrl: '../Assets/map_bill.png',
            iconRetinaUrl: '../Assets/map_bill.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });


        var parkIcon = L.icon({
            iconUrl: '../Assets/map_park.png',
            iconRetinaUrl: '../Assets/map_park.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var policeIcon = L.icon({
            iconUrl: '../Assets/map_police.png',
            iconRetinaUrl: '../Assets/map_police.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var propertyIcon = L.icon({
            iconUrl: '../Assets/map_property.png',
            iconRetinaUrl: '../Assets/map_property.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var roadIcon = L.icon({
            iconUrl: '../Assets/map_road.png',
            iconRetinaUrl: '../Assets/map_road.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var transportIcon = L.icon({
            iconUrl: '../Assets/map_transport.png',
            iconRetinaUrl: '../Assets/map_transport.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var wasteIcon = L.icon({
            iconUrl: '../Assets/map_waste.png',
            iconRetinaUrl: '../Assets/map_waste.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var waterIcon = L.icon({
            iconUrl: '../Assets/map_water.png',
            iconRetinaUrl: '../Assets/map_water.png',
            iconSize: [24, 24],
            iconAnchor: [12, 24],
            //popupAnchor: [0, -14]
        });

        var markerClusters = L.markerClusterGroup();
        var popup;
        var comment = '';
        for (i = 0; i < mapData.length; i++) {

            if (mapData[i].automatic_complaint != null) {
                comment = mapData[i].automatic_complaint;
            }
            popup = '<b style="font-size: 14px;">Complaint Details:</b>'
                + '<br/><b>Department:</b>' + mapData[i].Dept_Desc
                + '<br/><b>Category:</b>' + mapData[i].Category_Desc
                + '<br/><b>SubCategory:</b>' + mapData[i].SubCategory_Desc
                + '<br/><b>Comments:</b>' + comment;
            + '<br/><b>Status:</b>' + mapData[i].ComplaintStatus
                + '<br/><b>Location:</b>' + mapData[i].Address
                + '<br/><b>Complaint Date:</b>' + mapData[i].Complaint_Date;

            console.log(mapData[i].Lattitude.substring(0, 9));
            console.log(mapData[i].Longitude.substring(0, 9));

            var m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: myIcon }).bindPopup(popup);

            switch (mapData[i].Dept_Desc) {
                case "Electricity": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: elecIcon }).bindPopup(popup);
                    break;
                case "Water and Sanitation": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: waterIcon }).bindPopup(popup);
                    break;
                case "Property Rates": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: propertyIcon }).bindPopup(popup);
                    break;
                case "Solid Waste": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: wasteIcon }).bindPopup(popup);
                    break;
                case "Administration": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: adminIcon }).bindPopup(popup);
                    break;
                case "Metro Police": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: policeIcon }).bindPopup(popup);
                    break;
                case "Roads and Strom Water": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: roadIcon }).bindPopup(popup);
                    break;
                case "City Parks": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: parkIcon }).bindPopup(popup);
                    break;
                case "My Bills": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: billIcon }).bindPopup(popup);
                    break;
                case "Transport": m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: transportIcon }).bindPopup(popup);
                    break;

                default: m = L.marker([mapData[i].Lattitude.substring(0, 9), mapData[i].Longitude.substring(0, 9)], { icon: myIcon }).bindPopup(popup);
            }
            markerClusters.addLayer(m);

        }
        map.addLayer(markerClusters);

    }
    //End Map

});
