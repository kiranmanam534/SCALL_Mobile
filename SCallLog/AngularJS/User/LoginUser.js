app.controller('UsrLoginCtrl', function ($scope, $http) {
    $scope.LoginData = {};

    $scope.imgLoader_Save = false;

    $scope.LoginUser = function (data) {
        //alert()
        $scope.imgLoader_Save = true;
        console.log(data);
        //alert(data.email);
        if (data == undefined || data == "") {
            $scope.imgLoader_Save = false;
            alertify.alert('Error', "Please Enter Email and Password", function () {
            });
        } else if (data.Email == undefined || data.Email == "") {
            $scope.imgLoader_Save = false;
            alertify.alert('Error', "Email Mandatory", function () {
            });
        } else if (data.Password == undefined || data.Password == "") {
            $scope.imgLoader_Save = false;
            alertify.alert('Error', "Password Mandatory", function () {
            });
        } else {
            $http.get("../Home/LoginUser?Username=" + data.Email
                + "&Password=" + data.Password).then(
                    function (res) {
                        //$scope.imgLoader_Save = false;
                        if (res.data.success) {
                            window.location.href = '../User/UserDashBoard';

                        }
                        else {
                            $scope.imgLoader_Save = false;
                            alertify.alert('Error', res.data.error, function () {
                            });
                        }
                    },
                    function (err) {
                        $scope.imgLoader_Save = false;
                        //alert(err);
                        var error = err;
                    });
        }
    }


});