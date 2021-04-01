app.controller('ComplaintCTRL', function ($scope, $http, $filter) {
    $scope.isShowTable = true;
    $scope.CompUpdate = {};
    $scope.CompUpdateID = 0;

    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "ID";
    $scope.showLoader = true;
    $scope.showTable = false;
    $scope.totalCount = 0;

    $http.get("../Complaint/getUsers").then(
        function (response) {
            if (response.data.success) {
                console.log("Users");
                console.log(response.data.Users);
                $scope.Users = response.data.Users;
                

            } else if (response.data.expire) {
                window.location.href = '../Home/Login';
            }
        },
        function (err) {
            var error = err;
        });

    //$http.get("../Mail/ReadImap").then(
    //    function (response) {
    //        console.log('mail')
    //        console.log(response)
    //    },
    //    function (err) {
    //        var error = err;
    //        console.log('mail')
    //        console.log(err)
    //    });


    $scope.getComplaintList = function () {
        $http.get("../Complaint/getAllComplaints?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
            function (response) {
                console.log('Pagination')
                console.log(response.data.success)
                $scope.complaints = response.data.success.complaints;
                $scope.totalCount = response.data.success.totalCount;
                $scope.showLoader = false;
                $scope.showTable = true;
            },
            function (err) {
                var error = err;
            });
    }
    //Sorting 

    // sort ordering (Ascending or Descending). Set true for desending
    $scope.reverse = false;
    $scope.column = "ID";
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

    $scope.AssignComplaint = function (ID) {
        $scope.CompUpdate = {};
        $scope.CompUpdateID = ID;
    }
    $scope.viewComplaint = function (item) {
        //alert(item)
        $scope.viewSelectedComplaint = item;
    }

    $scope.viewComplaintHistory = function (refNumber) {
        $scope.complaintHistory = {};
        $http.get("../Complaint/getComplaintHistory?refNumber=" + refNumber).then(
            function (response) {
                console.log(response.data.success)
                $scope.HistoryRefNumber = refNumber;
                $scope.complaintHistory = response.data.ComplaintHistory;
                
            },
            function (err) {
                var error = err;
            });
    }

    $scope.ComplaintAssignedToUser = function (compData) {
        if (compData == undefined || compData == "") {
            alertify.alert('Error', "Please Enter Required Fields", function () {
            });
        } else if (compData.compUserID == undefined || compData.compUserID == "") {
            alertify.alert('Error', "Please Select user", function () {
            });
        } else if (compData.Comments == undefined || compData.Comments == "") {
            alertify.alert('Error', "Please Enter Comments", function () {
            });
        } else {
            $scope.imgLoader_Save1 = true;
            $http.post("../Complaint/AssignedComplaintUser?ID=" + $scope.CompUpdateID + "&UserID=" + compData.compUserID + "&Comments=" + compData.Comments).then(
                function (response) {
                    console.log(response);
                    if (response.data.success) {
                        delete $scope.CompUpdate;
                        $('#myModal').modal('hide');
                        $scope.imgLoader_Save1 = false;
                        var ref = response.data.JCREF;
                        alertify.alert('Success', "Job Card Created Successfully. Assigned Job Card Reference NO: "+ref, function () {
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

    

});

app.directive('datepicker6', function () {
    //alert('d');
    return {
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).datepicker({
                dateFormat: 'yy-mm-dd',
                autoSize: true,
                numberOfMonths: 1,
                // minDate: 0,
                //beforeShowDay: $.datepicker.noWeekends,
                showOptions: { origin: ["top", "center"] },
                onSelect: function (dateText) {
                    //alert(dateText);
                    //alert(this.id);
                    scope.$apply(function () {
                        ngModel.$setViewValue(dateText);
                    });

                }

            });
        }
    };
});

app.directive('datepicker1', function () {

    return {
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).datepicker({
                dateFormat: 'yy-mm-dd',
                autoSize: true,
                numberOfMonths: 1,
                minDate: 0,
                //beforeShowDay: $.datepicker.noWeekends,
                showOptions: { origin: ["top", "center"] },
                onSelect: function (dateText) {
                    //alert(dateText)
                    //alert(this.id)
                    scope.$apply(function () {
                        ngModel.$setViewValue(dateText);
                    });
                    // alert(this.id)
                    if (this.id === "cendDate") {

                        $("#cstartDate").datepicker("option", "maxDate", dateText);
                    }
                    if (this.id === "pendDate") {

                        $("#pstartDate").datepicker("option", "maxDate", dateText);
                    }


                }

            });
        }
    };
});
