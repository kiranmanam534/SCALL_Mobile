app.controller('MasterCtrl', function ($scope, $http) {

    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "ContractName";
    $scope.showLoader = true;
    $scope.showTable = false;
    $scope.totalCount = 0;
    $scope.save = "Add Role"
    $scope.Status = "Active";
    $scope.getEmployeeList = function () {
        $http.get("../Master/getAllRoles?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
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
    
    //SaveNewDepartment(CreateNewDepartmentData)
    $scope.SaveNewRole = function (RoleName,Status) {
        //alert(status)
        if (RoleName == undefined || RoleName == "") {
            alertify.alert('Error', "Please Enter Role", function () {
            });
        } else if (Status == undefined || Status == "") {
            alertify.alert('Error', "Please Choose Status", function () {
            });
        } else {
            $scope.imgLoader_Save = true;
            $scope.save = "Please Wait...";
            $http.get("../Master/addRole?RoleName=" + RoleName + "&Status=" + Status).then(
                       function (response) {
                           if (response.data.success) {
                               $scope.imgLoader_Save = false;
                               $scope.save = "Add Role";
                               delete $scope.RoleName;
                               $scope.Status ="Active";
                               $scope.getEmployeeList();

                           } else if (response.data.error) {
                               $scope.imgLoader_Save = false;
                               $scope.save = "Add Department";
                               alertify.alert('Error', "Try Again Later", function () {
                               });
                           }
                       },
                       function (err) {
                           $scope.imgLoader_Save = false;
                           $scope.save = "Add Department";
                           var error = err;
                           alertify.alert('Error', "Please choose your Network Connection", function () {
                           });
                       });


        }
    };
    $scope.editRole = function (CID, DeptName, Status) {
        //alert(DeptName)
        $scope.Role = DeptName;
        $scope.Sts = Status;
        $scope.RoleID = CID;
        $scope.button = "Update";
        //alert($scope.update.DeptName)
    }
    $scope.updateRole = function (RoleID, Role, Sts) {
        //int outID,int kpaID, int objectiveID,string objectiveName
        if (Role == undefined || Role == "") {
            alertify.alert('Warning', "Please Enter Role", function () {
            });
        } else if (Sts == undefined || Sts == "") {
            alertify.alert('Warning', "Please Choose Status", function () {
            });
        } else {
            $scope.imgLoader_Save2 = true;
            $scope.button = "Please Wait.."
            $http.get("../Master/updateRole?RoleID=" + RoleID + "&Role=" + Role + "&Status=" + Sts).then(
                       function (response) {
                           if (response.data.success) {
                               delete $scope.RoleID;
                               delete $scope.Role;
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



