app.controller('MainDashBoardCtrl', function ($scope, $http, $localStorage) {
    $scope.Receieved = "0";
    $scope.Assigned = "0";
    $scope.InProgress = "0";
    $scope.Hold = "0";
    $scope.Completed = "0";
    $scope.DashboardCounts = [];
    $scope.year = "2021";
   // $scope.showLoader = true;

    $scope.getAreaChartByYear = function (val) {
        $scope.customchart()
        $scope.year = val;
        $http.get("../SCall/getDashboardAreaCahrtData?year="+val).then(
            function (response) {
                var tReceived = [];
                var tAssigned = [];
                var tInProgress = [];
                var tHold = [];
                var tCompleted = [];
                if (response.data.success == true) {
                    console.log(response.data);


                    if (response.data.ReceivedCount.length > 0) {
                        var j = 0;
                        for (var i = 1; i <= 12; i++) {
                            if (response.data.ReceivedCount[j].month == i) {
                                tReceived.push(response.data.ReceivedCount[j].count);

                                if (response.data.ReceivedCount.length - 1 > j) {
                                    j++;
                                }
                            } else {
                                tReceived.push(0);
                            }
                        }
                    }
                    else {
                        for (var i = 1; i <= 12; i++) {
                            tReceived.push(0);
                        }
                    }

                    if (response.data.AssignedCount.length > 0) {
                        var j = 0;
                        for (var i = 1; i <= 12; i++) {
                            if (response.data.AssignedCount[j].month == i) {
                                tAssigned.push(response.data.AssignedCount[j].count);

                                if (response.data.AssignedCount.length - 1 > j) {
                                    j++;
                                }
                            } else {
                                tAssigned.push(0);
                            }
                        }
                    }
                    else {
                        for (var i = 1; i <= 12; i++) {
                            tAssigned.push(0);
                        }
                    }

                    if (response.data.InProgressCount.length > 0) {
                        var j = 0;
                        for (var i = 1; i <= 12; i++) {
                            if (response.data.InProgressCount[j].month == i) {
                                tInProgress.push(response.data.InProgressCount[j].count);

                                if (response.data.InProgressCount.length - 1 > j) {
                                    j++;
                                }
                            } else {
                                tInProgress.push(0);
                            }
                        }
                    }
                    else {
                        for (var i = 1; i <= 12; i++) {
                            tInProgress.push(0);
                        }
                    }

                    if (response.data.HoldCount.length > 0) {
                        var j = 0;
                        for (var i = 1; i <= 12; i++) {
                            if (response.data.HoldCount[j].month == i) {
                                tHold.push(response.data.HoldCount[j].count);

                                if (response.data.HoldCount.length - 1 > j) {
                                    j++;
                                }
                            } else {
                                tHold.push(0);
                            }
                        }
                    }
                    else {
                        for (var i = 1; i <= 12; i++) {
                            tHold.push(0);
                        }
                    }

                    if (response.data.CompletedCount.length > 0) {
                        var j = 0;
                        for (var i = 1; i <= 12; i++) {
                            if (response.data.CompletedCount[j].month == i) {
                                tCompleted.push(response.data.CompletedCount[j].count);

                                if (response.data.CompletedCount.length - 1 > j) {
                                    j++;
                                }
                            } else {
                                tCompleted.push(0);
                            }
                        }
                    }
                    else {
                        for (var i = 1; i <= 12; i++) {
                            tCompleted.push(0);
                        }
                    }

                    $scope.customchart(tReceived, tAssigned, tInProgress, tHold, tCompleted);

                    // $scope.showLoader = false;

                } else if (response.data.expire) {
                    window.location.href = '../Home/Login';
                }
            },
            function (err) {
                var error = err;
            });
    }
    
    $http.get("../SCall/getDashboardData?year=" + $scope.year).then(
        function (response) {
            var tReceived = [];
            var tAssigned = [];
            var tInProgress = [];
            var tHold = [];
            var tCompleted = [];
            if (response.data.success == true) {
                console.log(response.data);

                $scope.DashboardCounts = response.data.DashboardCount;
                console.log('DashBoard Data')
                console.log($scope.DashboardCounts)
                angular.forEach($scope.DashboardCounts, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        
                        $scope.Receieved = value.count;
                        
                    } else if (value.ComplaintStatus == 'Assigned') {
                        $scope.Assigned = value.count;
                        

                    } else if (value.ComplaintStatus == 'InProgress') {
                        $scope.InProgress = value.count;
                        
                    } else if (value.ComplaintStatus == 'Hold') {
                        $scope.Hold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        $scope.Completed = value.count;
                    }
                    
                });
                $scope.Mainpiechart($scope.Receieved, $scope.Assigned, $scope.InProgress, $scope.Hold, $scope.Completed);

                var cpReceived = 0;
                var cpAssigned = 0;
                var cpInprogress = 0;
                var cpHold = 0;
                var cpCompleted = 0;
                angular.forEach(response.data.CityParksCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {

                        cpReceived = value.count;

                    } else if (value.ComplaintStatus == 'Assigned') {
                        cpAssigned = value.count;


                    } else if (value.ComplaintStatus == 'InProgress') {
                        cpInprogress = value.count;

                    } else if (value.ComplaintStatus == 'Hold') {
                        cpHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        cpCompleted = value.count;
                    }

                });
                $scope.CityParkschart(cpReceived, cpAssigned, cpInprogress, cpHold, cpCompleted);

                var EReceived = 0;
                var EAssigned = 0;
                var EInprogress = 0;
                var EHold = 0;
                var ECompleted = 0;
                angular.forEach(response.data.ElectricityCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        EReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        EAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        EInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        EHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        ECompleted = value.count;
                    }

                });
                $scope.Electricitychart(EReceived, EAssigned, EInprogress, EHold, ECompleted);

                var MPReceived = 0;
                var MPAssigned = 0;
                var MPInprogress = 0;
                var MPHold = 0;
                var MPCompleted = 0;
                angular.forEach(response.data.MetroPoliceCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        MPReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        MPAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        MPInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        MPHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        MPCompleted = value.count;
                    }

                });
                $scope.Metropolicechart(MPReceived, MPAssigned, MPInprogress, MPHold, MPCompleted);

                var RSWReceived = 0;
                var RSWAssigned = 0;
                var RSWInprogress = 0;
                var RSWHold = 0;
                var RSWCompleted = 0;
                angular.forEach(response.data.RoadsandStromWaterCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        RSWReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        RSWAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        RSWInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        RSWHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        RSWCompleted = value.count;
                    }

                });
                $scope.RoadsandStromWaterchart(RSWReceived, RSWAssigned, RSWInprogress, RSWHold, RSWCompleted);

                var SWReceived = 0;
                var SWAssigned = 0;
                var SWInprogress = 0;
                var SWHold = 0;
                var SWCompleted = 0;
                angular.forEach(response.data.SolidWasteCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        SWReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        SWAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        SWInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        SWHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        SWCompleted = value.count;
                    }

                });
                $scope.SolidWastechart(SWReceived, SWAssigned, SWInprogress, SWHold, SWCompleted);

                var WSReceived = 0;
                var WSAssigned = 0;
                var WSInprogress = 0;
                var WSHold = 0;
                var WSCompleted = 0;
                angular.forEach(response.data.WaterandSanitationCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        WSReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        WSAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        WSInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        WSHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        WSCompleted = value.count;
                    }

                });
                $scope.WaterandSanitationchart(WSReceived, WSAssigned, WSInprogress, WSHold, WSCompleted);

                var TReceived = 0;
                var TAssigned = 0;
                var TInprogress = 0;
                var THold = 0;
                var TCompleted = 0;
                angular.forEach(response.data.TransportCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        TReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        TAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        TInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        THold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        TCompleted = value.count;
                    }

                });
                $scope.Transportchart(TReceived, TAssigned, TInprogress, THold, TCompleted);

                var AReceived = 0;
                var AAssigned = 0;
                var AInprogress = 0;
                var AHold = 0;
                var ACompleted = 0;
                angular.forEach(response.data.AdministrationCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        AReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        AAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        AInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        AHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        ACompleted = value.count;
                    }

                });
                $scope.Administrationchart(AReceived, AAssigned, AInprogress, AHold, ACompleted);

                var MBReceived = 0;
                var MBAssigned = 0;
                var MBInprogress = 0;
                var MBHold = 0;
                var MBCompleted = 0;
                angular.forEach(response.data.MyBillsCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        MBReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        MBAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        MBInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        MBHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        MBCompleted = value.count;
                    }

                });
                $scope.MyBillschart(MBReceived, MBAssigned, MBInprogress, MBHold, MBCompleted);

                var PRReceived = 0;
                var PRAssigned = 0;
                var PRInprogress = 0;
                var PRHold = 0;
                var PRCompleted = 0;
                angular.forEach(response.data.PropertyRatesCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        PRReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        PRAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        PRInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        PRHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        PRCompleted = value.count;
                    }

                });
                $scope.PropertyRateschart(PRReceived, PRAssigned, PRInprogress, PRHold, PRCompleted);

                var ACReceived = 0;
                var ACAssigned = 0;
                var ACInprogress = 0;
                var ACHold = 0;
                var ACCompleted = 0;
                angular.forEach(response.data.AutometicCount, function (value) {
                    if (value.ComplaintStatus == 'Received') {
                        ACReceived = value.count;
                    } else if (value.ComplaintStatus == 'Assigned') {
                        ACAssigned = value.count;
                    } else if (value.ComplaintStatus == 'InProgress') {
                        ACInprogress = value.count;
                    } else if (value.ComplaintStatus == 'Hold') {
                        ACHold = value.count;
                    } else if (value.ComplaintStatus == 'Completed') {
                        ACCompleted = value.count;
                    }

                });
                $scope.Autometicchart(ACReceived, ACAssigned, ACInprogress, ACHold, ACCompleted);


                if (response.data.ReceivedCount.length > 0) {
                    var j = 0;
                    for (var i = 1; i <= 12; i++) {
                        if (response.data.ReceivedCount[j].month == i) {
                            tReceived.push(response.data.ReceivedCount[j].count);
                            
                            if (response.data.ReceivedCount.length-1 > j) {
                                j ++;
                            }
                        } else {
                            tReceived.push(0);
                        }
                    }
                }
                else {
                    for (var i = 1; i <= 12; i++) {
                        tReceived.push(0);
                    }
                }

                if (response.data.AssignedCount.length > 0) {
                    var j = 0;
                    for (var i = 1; i <= 12; i++) {
                        if (response.data.AssignedCount[j].month == i) {
                            tAssigned.push(response.data.AssignedCount[j].count);

                            if (response.data.AssignedCount.length - 1 > j) {
                                j++;
                            }
                        } else {
                            tAssigned.push(0);
                        }
                    }
                }
                else {
                    for (var i = 1; i <= 12; i++) {
                        tAssigned.push(0);
                    }
                }

                if (response.data.InProgressCount.length > 0) {
                    var j = 0;
                    for (var i = 1; i <= 12; i++) {
                        if (response.data.InProgressCount[j].month == i) {
                            tInProgress.push(response.data.InProgressCount[j].count);

                            if (response.data.InProgressCount.length - 1 > j) {
                                j++;
                            }
                        } else {
                            tInProgress.push(0);
                        }
                    }
                }
                else {
                    for (var i = 1; i <= 12; i++) {
                        tInProgress.push(0);
                    }
                }

                if (response.data.HoldCount.length > 0) {
                    var j = 0;
                    for (var i = 1; i <= 12; i++) {
                        if (response.data.HoldCount[j].month == i) {
                            tHold.push(response.data.HoldCount[j].count);

                            if (response.data.HoldCount.length - 1 > j) {
                                j++;
                            }
                        } else {
                            tHold.push(0);
                        }
                    }
                }
                else {
                    for (var i = 1; i <= 12; i++) {
                        tHold.push(0);
                    }
                }

                if (response.data.CompletedCount.length > 0) {
                    var j = 0;
                    for (var i = 1; i <= 12; i++) {
                        if (response.data.CompletedCount[j].month == i) {
                            tCompleted.push(response.data.CompletedCount[j].count);

                            if (response.data.CompletedCount.length - 1 > j) {
                                j++;
                            }
                        } else {
                            tCompleted.push(0);
                        }
                    }
                }
                else {
                    for (var i = 1; i <= 12; i++) {
                        tCompleted.push(0);
                    }
                }

                $scope.customchart(tReceived, tAssigned, tInProgress, tHold, tCompleted);

               // $scope.showLoader = false;

            } else if (response.data.expire) {
                window.location.href = '../Home/Login';
            }
        },
        function (err) {
            var error = err;
        });

    $scope.customchart = function (tReceived, tAssigned, tInProgress, tHold, tCompleted) {
        //-----------------------
        //- MONTHLY COMPLAINTS CHART -
        //-----------------------

        // Get context with jQuery - using jQuery's .get() method.
        var salesChartCanvas = $("#ComplaintByMonthChart").get(0).getContext("2d");
        // This will get the first returned node in the jQuery collection.
        var salesChart = new Chart(salesChartCanvas);

        var salesChartData = {
            labels: ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"],
            datasets: [
                {
                    label: "Received",
                    fillColor: "rgb(0, 173, 215)",
                    strokeColor: "rgb(0, 173, 215)",
                    pointColor: "rgb(0, 173, 215)",
                    pointStrokeColor: "rgb(0, 173, 215)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgb(0, 173, 215)",

                    data: tReceived
                },
                {
                    label: "Assigned",
                    fillColor: "rgb(0, 149, 81,0.9)",
                    strokeColor: "rgb(0, 149, 81)",
                    pointColor: "rgb(0, 149, 81)",
                    pointStrokeColor: "rgb(0, 149, 81)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgb(218, 140, 16)",
                    data: tAssigned
                },
                {
                    label: "InProgress",
                    fillColor: "rgb(218, 140, 16,0.7)",
                    strokeColor: "rgb(218, 140, 16)",
                    pointColor: "rgb(218, 140, 16)",
                    pointStrokeColor: "rgb(218, 140, 16)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgb(218, 140, 16)",
                    data: tInProgress
                },
                {
                    label: "Hold",
                    fillColor: "rgb(199, 67, 51,0.5)",
                    strokeColor: "rgb(199, 67, 51)",
                    pointColor: "rgb(199, 67, 51)",
                    pointStrokeColor: "rgb(199, 67, 51)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgb(199, 67, 51)",
                    data: tHold
                },
                {
                    label: "Completed",
                    fillColor: "rgb(199, 199, 200,0.5)",
                    strokeColor: "rgb(199, 199, 200)",
                    pointColor: "rgb(199, 199, 200)",
                    pointStrokeColor: "rgb(199, 199, 200)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgb(199, 199, 200)",
                    data: tCompleted
                }
            ]
        };

        var salesChartOptions = {
            //Boolean - If we should show the scale at all
            showScale: true,
            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines: false,
            //String - Colour of the grid lines
            scaleGridLineColor: "rgba(0,0,0,.05)",
            //Number - Width of the grid lines
            scaleGridLineWidth: 1,
            //Boolean - Whether to show horizontal lines (except X axis)
            scaleShowHorizontalLines: true,
            //Boolean - Whether to show vertical lines (except Y axis)
            scaleShowVerticalLines: true,
            //Boolean - Whether the line is curved between points
            bezierCurve: true,
            //Number - Tension of the bezier curve between points
            bezierCurveTension: 0.3,
            //Boolean - Whether to show a dot for each point
            pointDot: false,
            //Number - Radius of each point dot in pixels
            pointDotRadius: 4,
            //Number - Pixel width of point dot stroke
            pointDotStrokeWidth: 1,
            //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
            pointHitDetectionRadius: 20,
            //Boolean - Whether to show a stroke for datasets
            datasetStroke: true,
            //Number - Pixel width of dataset stroke
            datasetStrokeWidth: 2,
            //Boolean - Whether to fill the dataset with a color
            datasetFill: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%=datasets[i].label%></li><%}%></ul>",
            //Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true
        };

        //Create the line chart
        salesChart.Line(salesChartData, salesChartOptions);

        //---------------------------
        //- END MONTHLY SALES CHART -
        //---------------------------
    }
    
    $scope.Mainpiechart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.CityParkschart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart1").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.Electricitychart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart2").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.Metropolicechart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart3").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.RoadsandStromWaterchart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart4").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.SolidWastechart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart5").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.WaterandSanitationchart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart6").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.Transportchart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart7").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.Administrationchart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart8").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.MyBillschart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart9").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.PropertyRateschart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart10").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

    $scope.Autometicchart = function (received, assigned, inProgress, hold, completed) {
        //-------------
        //- PIE CHART -
        //-------------
        // Get context with jQuery - using jQuery's .get() method.

        var pieOptions = {
            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,
            //String - The colour of each segment stroke
            segmentStrokeColor: "#fff",
            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,
            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts
            //Number - Amount of animation steps
            animationSteps: 100,
            //String - Animation easing effect
            animationEasing: "easeOutBounce",
            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,
            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //Boolean - whether to make the chart responsive to window resizing
            responsive: true,
            // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
            maintainAspectRatio: true,
            //String - A legend template
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<segments.length; i++){%><li><span style=\"background-color:<%=segments[i].fillColor%>\"></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>"
        };

        var pieChartCanvas = $("#pieChart11").get(0).getContext("2d");
        var pieChart = new Chart(pieChartCanvas);
        var PieData = [
            {
                value: received,
                color: "#f56954",
                highlight: "#f56954",
                label: "Received"
            },
            {
                value: assigned,
                color: "#3c8dbc",
                highlight: "#3c8dbc",
                label: "Assigned"
            },
            {
                value: inProgress,
                color: "#f39c12",
                highlight: "#f39c12",
                label: "InProgress"
            },
            {
                value: hold,
                color: "#00c0ef",
                highlight: "#00c0ef",
                label: "Hold"
            },
            {
                value: completed,
                color: "#00a65a",
                highlight: "#00a65a",
                label: "Completed"
            },

        ];
        //Create pie or douhnut chart
        // You can switch between pie and douhnut using the method below.
        pieChart.Doughnut(PieData, pieOptions);

    }

});
