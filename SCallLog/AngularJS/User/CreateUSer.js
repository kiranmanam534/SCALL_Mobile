app.controller('MasterCtrl', function ($scope, $http, $filter) {

    $scope.capturePanel = true;

    $scope.EditUserID = null;


    $scope.uTable = true;
    $scope.uForm = false;
    $scope.SaveType = 'Add';
    $scope.passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$/

    $scope.CreateNewUserData = {};
    $scope.password = "";
    $scope.cpassword = "";
    $scope.CreateNewUserData.Status = "Active";

    

    //Gets Dropdown lists
    $http.get('../Master/getDepartmentsAndRoles').then(function (reasons) {
        if (reasons.data.success) {
            $scope.DepartmentsList = reasons.data.Departments;
            $scope.RolesList = reasons.data.Roles;
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


    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.
    $scope.sorting = "FirstName";
    $scope.showLoader = true;
    $scope.showTable = false;

    $scope.getEmployeeList = function () {
       // alert()
        $http.get("../Master/getUsers?pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected + "&sorting=" + $scope.sorting + "&search=" + $scope.search).then(
                       function (response) {
                              if (response.data.success) {
                                  $scope.users = response.data.success.Users;
                                  $scope.totalCount = response.data.success.totalCount;
                                  $scope.showLoader = false;
                                  $scope.showTable = true;
                              } else if (response.data.expire) {
                                  window.location.href = '../Home/Login';
                              }
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
    $scope.SearchMethod = function (val) {
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

    //$scope.showAddUserForm = function () {

    //    $scope.SaveType = 'Add';

    //    $scope.uTable = false;
    //    $scope.uForm = true;

    //    $scope.EditUserID = null;
    //    $scope.CreateNewUserData

    //}

    //$scope.showUsersTable = function () {

    //    $scope.uTable = true;
    //    $scope.uForm = false;

    //    $scope.CreateNewUserData = null;

    //}

    //Attachments
    $scope.UploadAttachmentLoader = false;

    $scope.AttachmentFiles = [];

    $scope.RemoveAttachment = function (index) {
        $scope.AttachmentFiles.splice(index, 1);
    }
    $scope.attachFiles = 0;
    $scope.UploadAttachment = function () {

        var files = $("#Attachment").get(0).files;
        $scope.attachFiles = files.length;
        if (files.length < 1) {
            alertify.alert('Error', 'Select files to upload.', function () {
            });
        } else {

            for (var i = 0; i < files.length; i++) {
                $scope.AttachmentFiles.push(files[i]);
            }

            alertify.alert('Success', 'Uploaded successfully.', function () {
            });

            $('#Attachment').val('').clone(true);
        }
    }
    
    //Save User
    $scope.SaveNewUser = function (CreateNewUserData, password, cpassword) {
        //alert($scope.CreateNewUserData.password);
        var formData = new FormData();
        var filecount = $("#Attachment").get(0).files;
        if (CreateNewUserData == undefined || CreateNewUserData == "") {
                    alertify.alert('Warning', "Please Enter First Name", function () {
                    });
        } else if (CreateNewUserData.FirstName == undefined || CreateNewUserData.FirstName == "") {
            alertify.alert('Warning', "Please Enter First Name", function () {
            });
        } else if (CreateNewUserData.LastName == undefined || CreateNewUserData.LastName == "") {
            alertify.alert('Warning', "Please Enter Last Name", function () {
            });
        } else if (CreateNewUserData.DepartmentID == undefined || CreateNewUserData.DepartmentID == "") {
            alertify.alert('Warning', "Please Select Department", function () {
            });
        } else if (CreateNewUserData.RoleID == undefined || CreateNewUserData.RoleID == "") {
            alertify.alert('Warning', "Please Select Role", function () {
            });
        } else if (CreateNewUserData.EmailID == undefined || CreateNewUserData.EmailID == "") {
            alertify.alert('Warning', "Please Enter Email", function () {
            });
        } else if (password == undefined || password == "") {
            alertify.alert('Warning', "Please Enter valid Password", function () {
            });
        } else if (cpassword == undefined || cpassword == "") {
            alertify.alert('Warning', "Please Enter Confirm password", function () {
            });
        } else if (password != cpassword) {
            alertify.alert('Warning', "Password and Confirm password didn't match", function () {
            });
        } else if ($scope.attachFiles ==0) {
            alertify.alert('Warning', "Please Upload Picture", function () {
            });
        } else if ($scope.AttachmentFiles < 1) {
            //alert('hi')
               // $scope.showMsgs = false;
                $scope.imgLoader_Save = true;
                $http.post('../Master/CreateNewUser',
                    {
                        UserFormData: $scope.CreateNewUserData,
                        password: $scope.password,
                        SaveType: $scope.EditUserID == null ? "Add" : "Update"
                    }).then(function (res) {
                        //alert('okk')
                        $scope.imgLoader_Save = false;

                        if (res.data.success) {
                            
                            alertify.alert('Success', res.data.success, function () {
                                // window.location.href = '../Master/AddNewUser';
                                window.location.reload();
                            });
                        }
                        else {
                            alertify.alert('Error', res.data.error, function () {
                            });
                        }
                    });
        } else {
            //alert('hiii')
            var files = $scope.AttachmentFiles;
            $scope.imgLoader_Save = true;
                $http({
                    method: 'post',
                    url: '../Master/CreateNewUserWithAttachment',
                    headers: { 'Content-Type': undefined },

                    transformRequest: function () {
                        formData.append("UserFormData", angular.toJson($scope.CreateNewUserData));
                        formData.append("SaveType", angular.toJson($scope.EditUserID == null ? "Add" : "Update"));
                        formData.append("password", angular.toJson($scope.password));

                        for (var i = 0; i < files.length; i++) {
                            formData.append("UploadedImage", files[i]);
                        }

                        return formData;
                    }
                }).then(function (res) {

                    $scope.imgLoader_Save = false;

                    if (res.data.success) {
                        alertify.alert('Success', res.data.success, function () {
                            delete $scope.CreateNewUserData;
                            delete password;
                            delete cpassword;
                            $scope.AttachmentFiles = [];
                            $scope.getEmployeeList();
                        });
                    } else if (res.data.expire) {
                        window.location.href = '../Home/Home';
                    }
                    else {
                        alertify.alert('Error', res.data.error, function () {
                        });
                    }

                });
            }
    }

    $scope.updateUser = function (CreateNewUserData) {
        //alert($scope.CreateNewUserData.password);
        var formData = new FormData();
        var filecount = $("#Attachment").get(0).files;
        if (CreateNewUserData == undefined || CreateNewUserData == "") {
            alertify.alert('Warning', "Please Enter First Name", function () {
            });
        } else if (CreateNewUserData.FirstName == undefined || CreateNewUserData.FirstName == "") {
            alertify.alert('Warning', "Please Enter First Name", function () {
            });
        } else if (CreateNewUserData.LastName == undefined || CreateNewUserData.LastName == "") {
            alertify.alert('Warning', "Please Enter Last Name", function () {
            });
        } else if (CreateNewUserData.DepartmentID == undefined || CreateNewUserData.DepartmentID == "") {
            alertify.alert('Warning', "Please Select Department", function () {
            });
        } else if (CreateNewUserData.RoleID == undefined || CreateNewUserData.RoleID == "") {
            alertify.alert('Warning', "Please Select Role", function () {
            });
        } else if (CreateNewUserData.EmailID == undefined || CreateNewUserData.EmailID == "") {
            alertify.alert('Warning', "Please Enter Email", function () {
            });
        }  else if ($scope.AttachmentFiles < 1) {
            //alert('hi')
            // $scope.showMsgs = false;
            $scope.imgLoader_Save = true;
            $http.post('../Master/CreateNewUser',
                {
                    UserFormData: $scope.CreateNewUserData,
                    password: $scope.password,
                    SaveType: $scope.EditUserID == null ? "Add" : "Update"
                }).then(function (res) {
                    //alert('okk')
                    $scope.imgLoader_Save = false;

                    if (res.data.success) {

                        alertify.alert('Success', res.data.success, function () {
                            // window.location.href = '../Master/AddNewUser';
                            window.location.reload();
                        });
                    }
                    else {
                        alertify.alert('Error', res.data.error, function () {
                        });
                    }
                });
        } else {
            //alert('hiii')
            var files = $scope.AttachmentFiles;
            $scope.imgLoader_Save = true;
            $http({
                method: 'post',
                url: '../Master/CreateNewUserWithAttachment',
                headers: { 'Content-Type': undefined },

                transformRequest: function () {
                    formData.append("UserFormData", angular.toJson($scope.CreateNewUserData));
                    formData.append("SaveType", angular.toJson($scope.EditUserID == null ? "Add" : "Update"));
                    formData.append("password", angular.toJson($scope.password));

                    for (var i = 0; i < files.length; i++) {
                        formData.append("UploadedImage", files[i]);
                    }

                    return formData;
                }
            }).then(function (res) {

                $scope.imgLoader_Save = false;

                if (res.data.success) {
                    alertify.alert('Success', res.data.success, function () {
                        delete $scope.CreateNewUserData;
                        delete password;
                        delete cpassword;
                        $scope.AttachmentFiles = [];
                        $scope.getEmployeeList();
                    });
                } else if (res.data.expire) {
                    window.location.href = '../Home/Home';
                }
                else {
                    alertify.alert('Error', res.data.error, function () {
                    });
                }

            });
        }
    }
       
    

    //Show Edit User Form
    $scope.EditUser = function (UserID) {

        $http.post('../Master/getUserDetails',
            {
                UserID: UserID
            }).then(function (res) {

                if (res.data.success) {

                    $scope.EditUserID = UserID;

                    $scope.CreateNewUserData = res.data.UserDetails[0];
                    console.log('EDIT DATA');
                    console.log(res);

                    //$scope.EditProjectNumber = res.data.UserDetails[0].ID;

                    //display form
                    $scope.SaveType = 'Update';
                    

                } else if (res.data.error) {
                    alertify.alert('Error', "Problem loading Update User form.", function () {
                    });
                } else if (res.data.expire) {
                    window.location.href = '../Home/Home';
                }

            });
    }
    $scope.setDeptData = function () {
        $('#myModal1').modal('show');
    }
    $scope.setDepartment = function (dept) {
        $scope.CreateNewUserData.DepartmentID = dept.ID;
        $scope.CreateNewUserData.DeptName = dept.DEPARTMENT_DESC;
        $('#myModal1').modal('hide');
    }


});
