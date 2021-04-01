app.controller('WorkCTRL', function ($scope, $http, $filter) {
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
    $http.get("../Complaint/getUserStatus").then(
        function (response) {
            if (response.data.success) {
                console.log("status");
                console.log(response.data.status);
                $scope.status = response.data.status;
            } else if (response.data.expire) {
                window.location.href = '../Home/Login';
            }
        },
        function (err) {
            var error = err;
        });

    $scope.getComplaintList = function () {
        $http.get("../User/getWorkOrders?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
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

    $scope.changeComplaintStatus = function (ID, Status) {
        $scope.CompUpdate = {};
        $scope.CompUpdateID = ID;
        $scope.PStatus = Status;
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

            $http.post("../User/updateComplaintStatus?ID=" + $scope.CompUpdateID + "&Status=" + compData.compStatus +
                "&PStatus=" + $scope.PStatus + "&SDate=" + compData.StartDate +
                "&EDate=" + compData.EndDate + "&SHours=" + compData.ServiceHours + "&Comments=" + compData.Comments).then(
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

    $scope.isExist = function (id) {
        return $scope.complaints.map(function (type) { return type.COMPLAINT_ID; }).indexOf(id);
    }


    var checkedComplaintsList = [];

    $scope.addCheckedComplaints = function (complaint) {
        console.log(complaint)
        var ischecked = $('#chk_' + complaint.COMPLAINT_ID).is(':checked');


        if (ischecked) {
            checkedComplaintsList.push({
                COMPLAINT_ID: complaint.COMPLAINT_ID,
                JC_REFERENCE: complaint.JC_REFERENCE,
                isComplaintChecked: ischecked

            });
        } else {
            var index = checkedComplaintsList.map(function (item) {
                return item.COMPLAINT_ID;
            }).indexOf(complaint.COMPLAINT_ID);

            checkedComplaintsList.splice(index, 1)
        }
    }


    $scope.SaveWorkOrder = function () {
        $http.post("../User/addWorkOrder", checkedComplaintsList).then(
            function (response) {
                if (response.data.success) {
                    console.log(response.data.success)
                    alertify.alert('Success', "Complaint Updated Successfully", function () {
                        window.location.reload();
                    });
                } else if (response.data.error) {

                    alertify.alert('Error', "Try Again Later", function () {
                    });
                }
            },
            function (err) {
                var error = err;
            });

    }

});

//app.directive('datepicker', function () {
//    return {
//        require: 'ngModel',
//        link: function (scope, el, attr, ngModel) {
//            $(el).datepicker({
//                dateFormat: 'yy-mm-dd',
//                autoSize: true,
//                numberOfMonths: 1,
//                minDate:0,
//                showOptions: { origin: ["top", "center"] },
//                onSelect: function (dateText) {
//                    scope.$apply(function () {
//                        ngModel.$setViewValue(dateText);
//                    });
//                }
//            });
//        }
//    };
//});


