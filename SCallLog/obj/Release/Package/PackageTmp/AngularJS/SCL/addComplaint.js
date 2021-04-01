app.controller('LocationComplaintCTRL', function ($scope, $http, $localStorage) {
    $scope.ComplaintDepartmentsList = [];
    $scope.ComplaintCategoryList = [];
    $scope.ComplaintSubCategoryList = [];
    $scope.isFormValid = false;
    $scope.Submitted = false;
    $scope.isloading = false;
    $scope.$watch('AddComplaintForm.$valid', function (newValue) {
        $scope.isFormValid = newValue;
        //alert(newValue)
    });

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
    $scope.getComplaintCategoriesbyDeptID = function (DeptID) {
        //alert(DeptID)
        $scope.ComplaintCategoryList = [];
        $scope.ComplaintSubCategoryList = [];
        $http.get('../Master/getComplaintCategoriesbyDeptID?DeptID=' + DeptID).then(function (reasons) {
            if (reasons.data.success) {
                $scope.ComplaintCategoryList = reasons.data.ComplaintCategories;
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
    }
    //$scope.getComplaintCategories = function () {
    //    //alert();
    //    $scope.ComplaintCategoryList = [];
    //    $scope.ComplaintSubCategoryList = [];
    //    $http.get('../Master/getAllComplaintCategories').then(function (reasons) {
    //        if (reasons.data.success) {
    //            $scope.ComplaintCategoryList = reasons.data.ComplaintCategories;
    //            console.log('DATA....');
    //            console.log(reasons.data);
    //        } else if (reasons.data.expire) {
    //            window.location.href = '../Home/Login';
    //        }
    //        else if (reasons.data.error) {
    //            alertify.alert('Error', reasons.data.error, function () {
    //            });
    //        }
    //    });
    //}
    //$scope.getComplaintCategories();
    $scope.getComplaintSubCategoriesbyCatID = function (CatID) {
        //alert(CatID)
        $scope.ComplaintSubCategoryList = [];
        $http.get('../Master/getComplaintSubCategoriesbyCatID?CatID=' + CatID).then(function (reasons) {
            if (reasons.data.success) {
                $scope.ComplaintSubCategoryList = reasons.data.ComplaintSubCategories;
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
    }
    $scope.Save = function () {
        var formData = new FormData();
        var files = $('#Attachment').get(0).files;

        console.log(files);
        $scope.Submitted = true;
        //alert($scope.isFormValid)
        if ($scope.isFormValid) {
            $scope.isloading = true;
            $http({
                method: 'post',
                url: '../Complaint/SaveComplaint',
                headers: { 'Content-Type': undefined },

                transformRequest: function () {
                    formData.append("FormData", angular.toJson($scope.AddComplaintModel));

                    for (var i = 0; i < files.length; i++) {
                        formData.append("UploadedImage", files[i]);
                    }

                    return formData;
                }
            }).then(function (res) {
                //
                if (res.data.success) {
                    //alertify
                    //    .alert("Item saved successfully.", function () {

                    //    });
                    $scope.isloading = false;
                    alertify.alert('<b class="text-success">Success</b>', "Complaint saved successfully", function () {
                    });
                    $scope.resetForm();
                } else {
                    //alertify
                    //    .alert("Item not saved successfully.", function () {

                    //    });
                    alertify.alert('<b class="text-danger">Error</b>', "Complaint saving failed!", function () {
                    });
                    //alert("Complaint saving failed!");
                    $scope.isloading = false;
                }
                console.log(res);

            });
        }
    }
    google.maps.event.addDomListener(window, 'load', function () {
        //alert('map');
        var places = new google.maps.places.Autocomplete(document.getElementById('Address'));
        google.maps.event.addListener(places, 'place_changed', function () {
            var place = places.getPlace();
            var address = place.formatted_address;
            var latitude = place.geometry.location.lat();
            var longitude = place.geometry.location.lng();
            $scope.AddComplaintModel.Address = address;
            //alert(place.geometry.location.lat());
            $scope.AddComplaintModel.Lattitude = parseFloat(place.geometry.location.lat()) + (Math.random() - 0.5) / 3000;
            $scope.AddComplaintModel.Longitude = parseFloat(place.geometry.location.lng()) + (Math.random() - 0.5) / 3000;
        });
    });
    $scope.resetForm = function () {
        $scope.Submitted = false;
        $scope.AddComplaintModel = {};
        $('#Attachment').val('');
        // Set the field values back to the original default values   
        $scope.AddComplaintForm.$setPristine();
        //$scope.VolumeBasedForm.$setValidity();
        $scope.AddComplaintForm.$setUntouched();
        // in my case I had to call $apply to refresh the page, you may also need this.

    };
});
