app.controller('crctrl', function ($scope, $http) {
    $scope.imgLoader_Save = false;
    $scope.passwordFormat = /^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$/;
    $scope.mobileFormat = /^[0-9]{10,10}$/;
    $scope.SUB = "Register Now";
    $scope.isFreetrail = false;
    $scope.company = {};
    //alert()
    //var queryString = window.location.href;
    //var url = new URL(queryString);
    //var subProcess = url.searchParams.get("FreeTrail");
    //alert(subProcess)
    var subProcess = "NO";
    if (subProcess == "YES") {
        $scope.SUB = "Free Trail"
        $scope.isFreetrail = true;

    }

    $http.get("../Home/getCompanyList").then(
        function (response) {
            //alert('ok')
            console.log('Pagination')
            $scope.companyList = response.data.success;
            console.log($scope.companyList)

        },
        function (err) {
            var error = err;
            //alert(error)
        });


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


    $scope.company_registration = function (company,cpassword) {
        var count = 0;
        var type = '';
        var formData = new FormData();
        //alert($scope.attachFiles)
        if (company == undefined || company == "") {
            alertify.alert('Warning', "Please Enter Company Name", function () {
            });
        } else if (company.company_name == undefined || company.company_name == "") {
            alertify.alert('Warning', "Please Enter Company Name", function () {
            });
        }else if (company.first_name == undefined || company.first_name == "") {
            alertify.alert('Warning', "Please Enter  First Name", function () {
            });
        }
        else if (company.last_name == undefined || company.last_name == "") {
            alertify.alert('Warning', "Please Enter  Last Name", function () {
            });
        } else if (company.company_email == undefined || company.company_email == "") {
            alertify.alert('Warning', "Please Enter Valid Email", function () {
            });
        } else if (company.company_email != undefined || company.company_email != "") {
            if ($scope.companyList != undefined) {
                $scope.companyList.forEach(function (e) {
                    if (e.username == company.email) {
                        count = count + 1;
                        type = e.type;
                    }
                })
            }

            if (count > 0) {
                alertify.alert('Warning', "Your Email Already Exist for " + type + " type in Database ", function () {
                });
            } else if (company.company_mobile == undefined || company.company_mobile == "") {
                alertify.alert('Warning', "Please Enter Company Mobile", function () {
                });
            } else if (company.company_address == undefined || company.company_address == "") {
                alertify.alert('Warning', "Please Enter Company Address", function () {
                });
            } else if (company.company_password == undefined || company.company_password == "") {
                alertify.alert('Warning', "Please Enter valid Password", function () {
                });
            } else if (cpassword == undefined || cpassword == "") {
                alertify.alert('Warning', "Please Enter Confirm password", function () {
                });
            } else if (company.company_password != cpassword) {
                alertify.alert('Warning', "Password and Confirm password didn't match", function () {
                });
            } else if ($scope.attachFiles == 0) {
                alertify.alert('Warning', "Please Upload Picture", function () {
                });
            } else if ($scope.AttachmentFiles.length == 0) {
                alertify.alert('Warning', "Please Upload One Picture", function () {
                });
            } else if ($scope.AttachmentFiles.length > 1) {
                alertify.alert('Warning', "Please Upload One Picture", function () {
                });
            } else if ($scope.AttachmentFiles.length == 1) {
                //alert()
                var files = $scope.AttachmentFiles;
                $scope.imgLoader_Save = true;

                $http({
                    method: 'post',
                    url: '../Home/addCompany',
                    headers: { 'Content-Type': undefined },

                    transformRequest: function () {

                        formData.append("UserFormData", angular.toJson($scope.company));
                        formData.append("FreeTrail", angular.toJson(subProcess));

                        for (var i = 0; i < files.length; i++) {
                            formData.append("UploadedImage", files[i]);
                        }

                        return formData;
                    }
                }).then(function (res) {

                    $scope.imgLoader_Save = false;

                    if (res.data.success) {
                        alertify.alert('Success', "Company Registration Successful. Login Credential Send to your Registered mail Id", function () {
                            delete $scope.company;
                            delete cpassword;
                            $scope.AttachmentFiles = [];
                            window.location.href ='../Home/Login';
                        });
                    } else if (res.data.expire) {
                        $scope.imgLoader_Save = false;
                        window.location.href = '../Home/Home';
                    }
                    else {
                        $scope.imgLoader_Save = false;
                        alertify.alert('Error', "Company Registration was failed. ", function () {
                        });
                    }

                });

                
            }
        }
        $scope.checkEmail = function (email) {
            count = 0;
            $scope.companyList.forEach(function (e) {
                if (e.company_email == email) {
                    count = count + 1;
                }
            })
            if (count > 0) {
            }
        }
        $scope.checkCompanyName = function (name) {
            count = 0;
            $scope.companyList.forEach(function (e) {
                if (e.company_email == name) {
                    count = count + 1;
                }
            })
            if (count > 0) {
            }
        }
        $scope.checkCompanyUnique = function (UniqueID) {
            count = 0;
            $scope.companyList.forEach(function (e) {
                if (e.company_email == UniqueID) {
                    count = count + 1;
                }
            })
            if (count > 0) {
            }
        }
    }
});