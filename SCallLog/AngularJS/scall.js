var app = angular.module('SCALL', ['ngRoute', 'ngStorage', 'ui.bootstrap']);

app.directive("datepicker1", function () {
    //alert('ok');
    return {
        restrict: "A",
        link: function (scope, el, attr) {
            el.datepicker({
                dateFormat: 'yy-mm-dd',
                changeMonth: true,
                changeYear: true,
                yearRange: "1900:Y",
                showButtonPanel: true,
                //onSelect: function (date) {
                //    //document.getElementById("MQ_OCCU_END").setAttribute("min", date);
                //}
            });
        }
    };
});