/// <reference path="../scripts/app.js" />

app.controller('AppCtrl', function ($scope, $http, $window) {
    $scope.gotoregister = function () {
        $window.location.href = "Register";
    };
    $scope.signin1 = function () {
        alert("Email is " + $scope.Username + "\n\nPassword" + $scope.Password);
        var model =
            {
                Email: $scope.Username,
                Password: $scope.Password
            }

        //$http.post("http://localhost:50374/Account/Signin", model).then(function (res) {
        //    console.log(res);
        //});


        //$http.get("http://localhost:53342/Account/GetData?email=" + $scope.Email).then(function (res) {
        //    console.log("Get Response" + res);
        //    alert(JSON.stringify(res));
        //});
    };

});


app.controller('Forgotpassword', function ($scope, $mdDialog) {

    $scope.customFullscreen = false;
    $scope.Forgotpassword = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'ForgotPassword',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
        })

    };
    function DialogController($scope, $mdDialog) {
        $scope.hide = function () {
            $mdDialog.hide();
        };
        $scope.cancel = function () {
            $mdDialog.cancel();
        };
    }
});



app.controller('Verifyotp', function ($scope, $mdDialog) {
    
    $scope.customFullscreen = false;
    $scope.Verifyotp = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'Verify',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
        })

    };
    function DialogController($scope, $mdDialog) {
        $scope.hide = function () {
            $mdDialog.hide();
        };
        $scope.cancel = function () {
            $mdDialog.cancel();
        };
    }
});


//app.controller('Verifyotp1', function ($scope, $mdDialog) {
//    debugger;
//    $scope.customFullscreen = false;
//    $scope.Verifyotp = function (ev) {
//        $mdDialog.show({
//            controller: DialogController,
//            templateUrl: 'Verify',
//            parent: angular.element(document.body),
//            targetEvent: ev,
//            clickOutsideToClose: true,
//            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
//        })

//    };
//    function DialogController($scope, $mdDialog) {
//        $scope.hide = function () {
//            $mdDialog.hide();
//        };
//        $scope.cancel = function () {
//            $mdDialog.cancel();
//        };
//    }
//});



app.controller('Registerctrl', function ($scope, $http) {

    //$scope.submitall = function () {
    //    debugger;
    //    var model = {
    //        Firstname: $scope.FirstName,
    //        Lastname: $scope.LastName,
    //        Gender: $scope.Gender,
    //        Dateofbirth: $scope.Date,
    //        Adhar: $scope.Adhar,
    //        Countrycode: $scope.selectedCountry,
    //        Phoneno: $scope.Phone,
    //        Address: $scope.Address,
    //        line2: $scope.Line2,
    //        state: $scope.state,
    //        city: $scope.city,
    //        postalcode: $scope.postalcode,
    //        Username: $scope.Username,
    //        password1: $scope.password1
    //    }

    //    $http.post("http://localhost:50374/Account/Register", model).then(function (res) {

    //    });
    //}
});


//app.controller('Verify', function ($scope, $http) {

//    $scope.Verifyotp = function () {
//        debugger;
//        var model = {
            
//        }

//        $http.post("http://localhost:50374/Account/Verifycode", model).then(function (res) {

//        });
//    }
//});


