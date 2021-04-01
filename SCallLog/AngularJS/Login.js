app.controller('LoginCtrl', function ($scope, $http,$localStorage) {
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
        }else if (data.Email == undefined || data.Email == "") {
            $scope.imgLoader_Save = false;
            alertify.alert('Error', "Email Mandatory", function () {
            });
        } else if (data.Password == undefined || data.Password == "") {
            $scope.imgLoader_Save = false;
            alertify.alert('Error', "Password Mandatory", function () {
            });
        } else {
            $http.get("../Home/SCLLogin?Username=" + data.Email
               + "&Password=" + data.Password).then(
                      function (res) {
                          //$scope.imgLoader_Save = false;
                          console.log(res)
                          if (res.data.success) {
                              if (res.data.Type == "ADMIN") {
                                  $localStorage.RemaingDays = res.data.Days.Days;
                                  //alert($localStorage.RemaingDays);
                                  window.location.href = '../SCall/MainIndex';
                              } else {
                                  window.location.href = '../User';
                              }
                              

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