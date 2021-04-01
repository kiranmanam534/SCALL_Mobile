app.controller('AddCategoriesCntrl', function ($scope, $http) {
    $scope.ComplaintDepartmentsList = [];
    $scope.ComplaintCategoryList = [];
    $scope.ComplaintSubCategoryList = [];
    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "ContractName";
    $scope.showLoader = true;
    $scope.showTable = false;
    $scope.totalCount = 0;
    $scope.save = "Add Category"
    $scope.Status = "Active";
    $scope.getEmployeeList = function () {
        $http.get("../Master/getAllCategories?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
            function (response) {
                console.log('Pagination')
                console.log(response.data.success)
                $scope.employees = response.data.success.complaints;
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
    $scope.column = "CID";
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
        $scope.getEmployeeList();
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
        $scope.getEmployeeList();
    }
    //Loading employees list on first time
    $scope.getEmployeeList();

    //This method is calling from pagination number
    $scope.pageChanged = function () {
        $scope.getEmployeeList();
    };

    //This method is calling from dropDown
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        $scope.getEmployeeList();
    };

    $http.get('../Master/getComplaintDepartments').then(function (reasons) {
        if (reasons.data.success) {
            $scope.ComplaintDepartmentsList = reasons.data.ComplaintDepartments;
            console.log('DATA....');
            console.log(reasons.data);
        } else if (reasons.data.expire) {
            window.location.href = '../Home/Login';
        }
        else if (reasons.data.error) {
            alertify.alert('Error', reasons.data.error, function () {
            });
        }
    });

    //SaveNewDepartment(CreateNewDepartmentData)
    $scope.SaveCategory = function (deptId,category, status) {
        //alert(deptId)
        if (deptId == undefined || deptId == "") {
            alertify.alert('<b class="text-warning">Warning</b>', "Please select department", function () {
            });
        }
        else if (category == undefined || category == "") {
            alertify.alert('<b class="text-warning">Warning</b>', "Please Enter category", function () {
            });
        } else if (status == undefined || status == "") {
            alertify.alert('<b class="text-warning">Warning</b>', "Please Choose Status", function () {
            });
        } else {
            $scope.imgLoader_Save = true;
            $scope.save = "Please Wait...";
            $http.get("../Master/addCategory?deptId=" + deptId + "&cat=" + category + "&status=" + status).then(
                function (response) {
                    if (response.data.success) {
                        $scope.imgLoader_Save = false;
                        $scope.save = "Add Category";
                        delete $scope.CATEGORY_DESC;
                        alertify.alert('<b class="text-success">Success</b>', response.data.success, function () {
                        });
                        $scope.Status = "Active";
                        $scope.getEmployeeList();

                    } else if (response.data.error) {
                        $scope.imgLoader_Save = false;
                        $scope.save = "Add Category";
                        alertify.alert('Error', response.data.error, function () {
                        });
                    }
                },
                function (err) {
                    $scope.imgLoader_Save = false;
                    $scope.save = "Add Category";
                    var error = err;
                    alertify.alert('Error', "Please choose your Network Connection", function () {
                    });
                });


        }
    };
    $scope.editCategory = function (CID, DeptName, Status) {
        //alert(DeptName)
        $scope.Depname = DeptName;
        $scope.Sts = Status;
        $scope.DeptID = CID;
        $scope.button = "Update";
        //alert($scope.update.DeptName)
    }
    $scope.updateCategory = function (DeptID, Depname, Sts) {
        //int outID,int kpaID, int objectiveID,string objectiveName
        if (Depname == undefined || Depname == "") {
            alertify.alert('Error', "Please Enter Category", function () {
            });
        } else if (Sts == undefined || Sts == "") {
            alertify.alert('Error', "Please Choose Status", function () {
            });
        } else {
            $scope.imgLoader_Save2 = true;
            $scope.button = "Please Wait..";
            $http.get("../Master/updateCategory?catID=" + DeptID + "&catname=" + Depname + "&status=" + Sts).then(
                function (response) {
                    if (response.data.success) {
                        delete $scope.DeptID;
                        delete $scope.Depname;
                        delete $scope.Sts;
                        $scope.imgLoader_Save2 = false;
                        $scope.button = "Update"
                        $('#myModal').modal('hide');
                        alertify.alert('Success', "Updated Successful", function () {
                        });
                        $scope.getEmployeeList();

                    } else if (response.data.error) {
                        $scope.imgLoader_Save2 = false;
                        $scope.button = "Update"
                        alertify.alert('Error', "Try Again Later", function () {
                        });
                    }
                },
                function (err) {
                    var error = err;
                    $scope.imgLoader_Save2 = false;
                    $scope.button = "Update"
                    alertify.alert('Error', "Please Check your Internet Connection", function () {
                    });
                });


        }

    };
});








