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

        $http.post("http://localhost:53342/Account/verifycode").then(function (res) {
            console.log("Get Response" + res);
            alert(JSON.stringify(res));
        });

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

app.controller('contopenotp', function ($scope, $mdDialog, $http) {

    //$scope.customFullscreen = false;
    //$scope.openotp = function (ev) {
    //    debugger;
    //    $mdDialog.show({
    //        controller: DialogController,
    //        templateUrl: 'Verify',
    //        parent: angular.element(document.body),
    //        targetEvent: ev,
    //        clickOutsideToClose: true,
    //        fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
    //    })

    //};
    //function DialogController($scope, $mdDialog) {
    //    $scope.hide = function () {
    //        $mdDialog.hide();
    //    };
    //    $scope.cancel = function () {
    //        $mdDialog.cancel();
    //    };
    //}
    //$scope.openotp = function (ev) {
    //    var model =
    //        {
    //            Email: $scope.Username

    //        }
    //    $http.get("http://localhost:53342/Account/CheckCode" + model).then(function (res) {
    //        console.log("Get Response" + res);
    //        alert(JSON.stringify(res));
    //    });
    //}


    $scope.openotp = function (ev) {

        debugger;
        //var model =
        //    {
        //        Email: $scope.Username

        //    }

        var post = $http({
            method: "POST",
            url: "/Account/CheckCodeAsync",
            dataType: 'json',
            data: { name: $scope.Username },
            headers: { "Content-Type": "application/json" }
        });

        //$http.post("http://localhost:53342/Account/CheckCode").then(function (res) {
        //    console.log("Get Response" + res);
        //    alert(JSON.stringify(res));
        //});
    };
});



app.controller('Registerctrl', function ($scope, $http) {
    //$scope.PassPhone = function () {
    //    debugger;
    //    var data = {"PhoneNumber": $scope.PhoneNumber }

    //    $http.post("http://localhost:50374/Account/Verifycode", data).then(function (respone)
    //    {

    //    })
    //}

});

