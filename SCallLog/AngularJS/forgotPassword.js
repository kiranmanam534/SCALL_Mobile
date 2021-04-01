app.controller('fpctrl', function ($scope, $http) {
    $scope.imgLoader_Save = false;
    
    $scope.ForgotUSer = function (PMSL) {

    }
    $scope.getPassword = function (username) {
        //alert(username)
        if (username == undefined || username == "") {
            alertify.alert('Warning', "Please Enter Registered mail ID", function () {
            });
        } else {
            $scope.imgLoader_Save = true;
            $http.get("../Home/getPassword?Company_Email=" + username).then(
                function (response) {
                    alertify.alert('Success', response.data.success, function () {
                        window.location.href="../Home/Login"
                   });
                    $scope.Email = "";
                    $scope.imgLoader_Save = false;

                },
            function (err) {
                $scope.imgLoader_Save = false;
                var error = err;
            });
        }
    }
});