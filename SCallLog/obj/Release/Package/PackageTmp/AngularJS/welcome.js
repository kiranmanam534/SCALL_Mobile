app.controller('WelcomeCtrl', function ($scope, $http) {
    $scope.SubScription = '';
    $scope.Amount = 0;
    $scope.Month = 0;
    $scope.showSubscribe1 = function (subscribe, amount, month) {
        $scope.SubScription = subscribe;
        $scope.Amount = amount;
        $scope.Month = month;
    }


});