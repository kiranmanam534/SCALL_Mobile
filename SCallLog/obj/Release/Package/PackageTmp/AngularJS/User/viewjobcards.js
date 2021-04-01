app.controller('JobcardCTRL', function ($scope, $http, $filter) {
    $scope.isShowTable = true;

    $scope.CompUpdate = {};
    $scope.CompUpdateID = 0;

    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "Complaint_ReferenceNo";
    $scope.showLoader = true;
    $scope.showTable = false;
    $scope.totalCount = 0;
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

    $scope.getComplaintList = function () {
        $scope.complaints = [];
        $http.get("../User/getUserJobCards?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
            function (response) {
                console.log('Pagination')
                console.log(response.data.success)
                if (response.data.success) {
                    $scope.complaints = response.data.success.complaints;
                    $scope.totalCount = response.data.success.totalCount;
                    $scope.showLoader = false;
                    $scope.showTable = true;
                }
                else {
                    //alert();
                    $scope.complaints = [];
                    $scope.totalCount = 0;
                    $scope.showLoader = false;
                    $scope.showTable = true;
                }

            },
            function (err) {
                var error = err;
            });
    }
    //Sorting 

    // sort ordering (Ascending or Descending). Set true for desending
    $scope.reverse = false;
    $scope.column = "JCID";
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
        $scope.getComplaintList();
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
        $scope.getComplaintList();
    }
    //Loading employees list on first time
    $scope.getComplaintList();

    //This method is calling from pagination number
    $scope.pageChanged = function () {
        $scope.getComplaintList();
    };

    //This method is calling from dropDown
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.getComplaintList();
    };


    //Assigned Complaints

    $scope.changeComplaintStatus = function (ID) {
        $scope.CompUpdate = {};
        $scope.CompUpdateID = ID;
    }
    $scope.viewJobCardHistory = function (compRef) {
        $scope.complaintHistory = {};
        $http.get("../User/getJobCardHistory?refNumber=" + compRef).then(
            function (response) {
                console.log(response.data.success)
                $scope.HistoryRefNumber = compRef;
                $scope.JobHistory = response.data.jobCardHistory;

            },
            function (err) {
                var error = err;
            });
    }

    $scope.viewJobCardMapDetails = function (mapData) {
        $scope.LoadComplaintsOnMap(mapData);
        //alert();
        $("#myModalMap").modal('show');
    }

    $scope.ComplaintStatusChange = function (compData) {
        if (compData == undefined || compData == "") {
            alertify.alert('Error', "Please Enter Required Fields", function () {
            });
        } else if (compData.compStatus == undefined || compData.compStatus == "") {
            alertify.alert('Error', "Please Select Status", function () {
            });
        } else if (compData.Comments == undefined || compData.Comments == "") {
            alertify.alert('Error', "Please Enter Comments", function () {
            });
        } else {
            $scope.imgLoader_Save1 = true;

            $http.post("../User/updateComplaintStatus?ID=" + $scope.CompUpdateID + "&Status=" + compData.compStatus + "&Comments=" + compData.Comments).then(
                function (response) {
                    if (response.data.success) {
                        delete $scope.CompUpdate;
                        $('#myModal').modal('hide');
                        $scope.imgLoader_Save1 = false;
                        alertify.alert('Success', "Complaint Updated Successfully", function () {
                            window.location.reload();
                        });

                    } else if (response.data.error) {
                        $scope.imgLoader_Save1 = false;
                        alertify.alert('Error', "Try Again Later", function () {
                        });
                    }
                },
                function (err) {
                    var error = err;
                });
        }
    }

    //$scope.ErrorResult = "";
    //$scope.LoadComplaintsOnMap = function () {
    //    $http.get("../User/getMapAssignedComplaints").then(

    //        function (response) {
    //            console.log('Pagination');
    //            console.log(response.data);
    //            if (response.data.success) {
    //                $scope.ErrorResult = "";
    //                var complaints = response.data.success.complaints;
    //                if (complaints.length > 0) {
    //                    alert('map');
    //                    alert(complaints[0].LAT);
    //                    initMap(complaints);

    //                }
    //            }
    //            else {
    //                $scope.ErrorResult = "No result found!";
    //            }

    //        },
    //        function (err) {
    //            var error = err;
    //        });
    //}
    

   


    //Start Map

    var map;
    $scope.ErrorResult = "";
    $scope.LoadComplaintsOnMap = function (complaintId) {
        $http.get("../User/getJobcardLocationDetails?complaintId=" + complaintId).then(

            function (response) {
                console.log('Pagination');
                console.log(response.data);
                if (response.data.success) {
                    $scope.ErrorResult = "";
                    var complaints = response.data.CompLocations;
                    if (complaints.length > 0) {
                        //alert('map');
                        //alert(complaints[0].LAT);
                        //initMap(complaints);
                        if (map != undefined || map != null) {
                            map.remove();
                            $("#map1").html("");
                            //$("#preMap").empty();
                            //$("<div id=\"map\" style=\"height: 500px;\"></div>").appendTo("#preMap");
                        }
                        $scope.showLeafletMap(complaints);
                    }
                }
                else {
                    $scope.ErrorResult = "No result found!";
                }

            },
            function (err) {
                var error = err;
            });
    }
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
            popup = '<b style="font-size: 14px;">Jobcard Details:</b>'
                + '<br/><b>Department:</b>' + mapData[i].Dept_Desc
                + '<br/><b>Category:</b>' + mapData[i].Category_Desc
                + '<br/><b>SubCategory:</b>' + mapData[i].SubCategory_Desc
                + '<br/><b>Comments:</b>' + comment
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
