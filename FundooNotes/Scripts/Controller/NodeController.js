/// <reference path="../scripts/app.js" />



app.controller('notectrl', function ($scope, $mdDialog, $mdSidenav, $window, $filter, $http) {

    //$http.post("/Labelshow",).then(function (response)
    //{
    //    debugger;
    //    console.log("Get Response" + res);
    //});

    //$window.onload = function () {
    //    alert("Called on page load..");
    //};

    $http.post("https://localhost:44326/Labelshow").then(function (res) {
        debugger;
        //  var abc = JSON.stringify(res);
        //console.log("Get Response", [res.data.Item2[0].NotesID]);
        //console.log("Get Response", res.data.Item1);
        //console.log("Get Response", abc.data);
        $scope.mir = [];
        $scope.coll = [];

        for (var i = 0; i < res.data.Item1.length; i++) {

            if (parseInt(res.data.Item2[i].LabelID) == res.data.Item1[i].ID) {
                $scope.person = {
                    UserID: res.data.Item2[i].UserID,
                    NotesID: res.data.Item2[i].NotesID,
                    LabelID: res.data.Item1[i].Label
                };
                $scope.mir.push($scope.person);

            }
        }


        for (var i = 0; i < res.data.Item3.length; i++) {
            $scope.person = {
                SharID: res.data.Item3[i].SharID,
                NotesID: res.data.Item3[i].NotesID,
                Firstalph: res.data.Item3[i].SharID.charAt(0)
            };
            $scope.coll.push($scope.person);
        }

        $scope.img = "/Images/ic_view_list_black_24px.svg";
        $scope.labels = res.data.Item1;
        $scope.noteid = res.data.Item2;
        $scope.shareid = res.data.Item3;
    });



    $scope.toggleLeft = buildToggler('left');
    $scope.NoteAdd = 1;
    $scope.Notedelete = 2
    $scope.NoteUpdate = 3;
    var time = "";
    var optionVal = new Array();
    var jScriptArray = new Array();
    var globaltime = "";
    var reminderdate;


    (function () {


        function checkTime(i) {
            return (i < 10) ? "0" + i : i;

        }

        function startTime() {
            var d = new Date();
            if (checkTime(d.getHours()).toString().length == 1) {
                hours = '0' + checkTime(d.getHours());
            }
            else {
                hours = checkTime(d.getHours());
                if (hours > 12) {
                    hours -= 12;

                } else if (hours === 0) {
                    hours = 12;
                }
            }
            minutes = checkTime(d.getMinutes()).toString().length == 1 ? '0' + checkTime(d.getMinutes()) : checkTime(d.getMinutes()),

                ampm = checkTime(d.getHours()) >= 12 ? 'PM' : 'AM',
                months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

            //document.getElementById('time').innerHTML = checkTime(d.getDate()) + " " + months[d.getMonth()] + " " + hours + " " + minutes + " " + ampm ;

            var time = months[d.getMonth()] + " " + checkTime(d.getDate()) + " " + hours + ":" + minutes + " " + ampm;
            globaltime = months[d.getMonth()] + " " + checkTime(d.getDate() + 1);


            for (var i = 0; i < jScriptArray.length; i++) {

                optionVal[i] = jScriptArray[i];
                if (optionVal[i].Reminder == time) {
                    alert(optionVal[i].Content);
                }

            }
            t = setTimeout(function () {

                startTime()
            }, 60000);
        }
        startTime();
    })();
    //prashant
    $(document).ready(function () {

        $('[name="date"]').datetimepicker();

        $('[name="date"]').on('dp.change', function (e) {
            reminderdate = $(this).val();
        });
    });


    $scope.GetnotesPopup = function (ev, note) {
        console.log(note);
        $scope.customFullscreen = false;

        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'Notes/GetNotesPopup',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
        })

        function DialogController($scope, $mdDialog) {
            $scope.hide = function () {
                $mdDialog.hide();
            };

            $scope.NoteAdd = 1;
            $scope.Notedelete = 2
            $scope.NoteUpdate = 3;
            $scope.note =
                {
                    ID: note.ID,
                    UserID: note.UserID,
                    Title: note.Title,
                    Content: note.Content,
                    ColorCode: note.ColorCode,
                    Reminder: note.Reminder,
                    DisplayOrde: note.DisplayOrde,
                    IsPin: note.IsPin,
                    IsArchive: note.IsArchive,
                    IsActive: note.IsActive,
                    IsDelete: note.IsDelete,
                    IsTrash: note.IsTrash,
                    ImageUrl: note.ImageUrl,
                    Mode: note.Mode,
                    Label: note.Label,
                    owner: note.owner,
                    share: note.share
                }


            if (note.IsPin == 1) {
                $scope.IsPin = 'Blue';
            }

            $scope.cancel = function () {
                $mdDialog.cancel();
            };


            $scope.Pin = function (val) {
                if (val.IsPin == 0) {
                    val.IsPin = 1;
                }
                else {
                    val.IsPin = 0;
                }
                val.Mode = $scope.NoteUpdate
                debugger;
                $http.post("/GetNotes", JSON.stringify(val)).then(function (response) {
                    //if (response != 0) {
                    //    debugger;
                    //    if (!obj.IsPin) {
                    //        localStorage.setItem("result", "Note Pinned");
                    //    }
                    //    else {
                    //        localStorage.setItem("result", "Note UnPinned");
                    //    }


                    //}

                    //else {
                    //    localStorage.setItem("result", "Failed to Pinned");
                    //}
                    window.location.reload();
                });
            }


            $scope.remToday = function (obj) {
                debugger;
                var date = new Date();
                $scope.ddMMMMyyyy = $filter('date')(new Date(), 'MMMM dd');

                var data = {
                    Id: obj.ID,
                    Mode: $scope.NoteUpdate,
                    IsArchive: (obj.IsArchive),
                    UserID: obj.UserID,
                    Content: obj.Content,
                    Title: (obj.Title == 'null') ? "" : obj.Title,
                    ColorCode: obj.ColorCode,
                    IsPin: obj.IsPin,
                    IsTrash: obj.IsTrash,
                    Reminder: $scope.ddMMMMyyyy + " " + "8:00 PM",
                    ImageUrl: obj.ImageUrl,
                    owner: obj.owner,
                    share: obj.share,
                    Label: obj.Label
                }

                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });
            }

            // Reminde Tommorrow 
            $scope.remTommorrow = function (obj) {
                debugger;
                var Mode = $scope.update;
                var date = new Date();
                var NextDay = new Date(date);
                NextDay.setDate(date.getDate() + 1);

                var MMMM = $filter('date')(new Date(NextDay), 'dd MMMM');

                var data = {
                    Id: obj.ID,
                    Mode: $scope.NoteUpdate,
                    IsArchive: (obj.IsArchive),
                    UserID: obj.UserID,
                    Content: obj.Content,
                    Title: (obj.Title == 'null') ? "" : obj.Title,
                    ColorCode: obj.ColorCode,
                    IsPin: obj.IsPin,
                    IsTrash: obj.IsTrash,
                    Reminder: globaltime + " " + "8:00 AM",
                    ImageUrl: obj.ImageUrl,
                    owner: obj.owner,
                    share: obj.share,
                    Label: obj.Label
                }


                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });
            }

            // Reminde next week 
            $scope.remWeek = function (obj) {
                debugger;
                var Mode = $scope.update;
                //var DD = new Date();
                //var NextWeek = new Date(DD);
                //NextWeek.setDate(DD.getDate() + 7);
                //var DD1 = $filter('date')(new Date(NextWeek), 'dd MMMM');

                function checkTime(i) {

                    return (i < 10) ? "0" + i : i;

                }


                //getting next monday date 
                var da = new Date();
                da.setDate(da.getDate() + (1 + 7 - da.getDay()) % 7);
                var nextweekmon = months[da.getMonth()] + " " + checkTime(da.getDate()) + " " + "8:00 PM";




                var data = {
                    Id: obj.ID,
                    Mode: $scope.NoteUpdate,
                    IsArchive: (obj.IsArchive),
                    UserID: obj.UserID,
                    Content: obj.Content,
                    Title: (obj.Title == 'null') ? "" : obj.Title,
                    ColorCode: obj.ColorCode,
                    IsPin: obj.IsPin,
                    IsTrash: obj.IsTrash,
                    Reminder: nextweekmon,
                    ImageUrl: obj.ImageUrl,
                    owner: obj.owner,
                    share: obj.share,
                    Label: obj.Label
                }

                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });
            }

            // Reminde by date
            $scope.saveRem = function (obj) {
                debugger;
                function checkTime(i) {
                    return (i < 10) ? "0" + i : i;

                }

                var mydate = new Date(reminderdate);
                months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                var date = mydate.getDate();
                var months = months[mydate.getMonth()];


                if (checkTime(mydate.getHours()).toString().length == 1) {
                    hours = '0' + checkTime(mydate.getHours());
                }
                else {
                    hours = checkTime(mydate.getHours());
                    if (hours > 12) {
                        hours -= 12;

                    } else if (hours === 0) {
                        hours = 12;
                    }
                }
                minutes = checkTime(mydate.getMinutes()).toString().length == 1 ? '0' + checkTime(mydate.getMinutes()) : checkTime(mydate.getMinutes());

                ampm = checkTime(mydate.getHours()) >= 12 ? 'PM' : 'AM';

                var data = {



                    Id: obj.ID,
                    Mode: $scope.NoteUpdate,
                    IsArchive: obj.IsArchive,
                    UserID: obj.UserID,
                    Content: obj.Content,
                    Title: (obj.Title == 'null') ? "" : obj.Title,
                    ColorCode: obj.ColorCode,
                    IsPin: obj.IsPin,
                    IsTrash: obj.IsTrash,
                    Reminder: months + " " + date + " " + hours + ":" + minutes + " " + ampm,
                    ImageURL: obj.ImageUrl,
                    owner: obj.owner,
                    share: obj.share,
                    Label: obj.Label

                }


                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });
            }


            /// Remove Reminder.
            $scope.remove = function (obj) {
                debugger;
                var data = {
                    Id: obj.ID,
                    Mode: $scope.NoteUpdate,
                    IsArchive: obj.IsArchive,
                    UserID: obj.UserID,
                    Content: obj.Content,
                    Title: (obj.Title == 'null') ? "" : obj.Title,
                    ColorCode: obj.ColorCode,
                    IsPin: obj.IsPin,
                    IsTrash: obj.IsTrash,
                    Reminder: null,
                    ImageUrl: obj.ImageUrl,
                    owner: obj.owner,
                    share: obj.share,
                    Label: obj.Label
                }

                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });
            }



            $scope.Editcontent = function (obj) {
                obj.Mode = $scope.NoteUpdate
                debugger;
                $http.post("/GetNotes", JSON.stringify(obj)).then(function (response) {
                    //if (response != 0) {
                    //    debugger;
                    //    if (!obj.IsPin) {
                    //        localStorage.setItem("result", "Note Pinned");
                    //    }
                    //    else {
                    //        localStorage.setItem("result", "Note UnPinned");
                    //    }


                    //}

                    //else {
                    //    localStorage.setItem("result", "Failed to Pinned");
                    //}
                    window.location.reload();
                });
            }

            $scope.color = function (id, color) {
                debugger;
                var colorcode = color;
                var id = id;
                var obj = {
                    ColorCode: colorcode,
                    ID: id.ID
                }

                $http.post("/UpdateColor", JSON.stringify(obj)).then(function (response) {
                    //GetNote();
                    window.location.reload();

                    console.log(response);
                });


            }

            $scope.addImage = function (obj) {
                debugger;
                $("#imagefile").click();
                var fileUpload = $('#imagefile').get(0);
                var files = fileUpload.files;
                var test = new FormData();
                for (var i = 0; i < files.length; i++) {
                    test.append(files[i].name, files[i]);
                }


                test.append("Title", (obj.Title == 'null') ? "" : obj.Title, );
                test.append("Content", obj.Content);
                test.append("ColorCode", obj.ColorCode);
                test.append('ID', obj.ID);
                test.append('IsPin', obj.IsPin);
                test.append('IsArchive', obj.IsArchive);
                test.append('IsTrash', obj.IsTrash);
                test.append('Label', obj.Label);
                test.append('owner', obj.owner);
                test.append('share', obj.share);
                test.append('UserID', obj.UserID);

                var imagedata =
                    {

                        "url": "/api/uploadimage",
                        "method": "POST",
                        "processData": false,
                        "contentType": false,
                        "mimeType": "multipart/form-data",
                        "data": test,
                        success: function (response) {
                            console.log(response);
                        },
                        error: function (response) {
                            console.log(response);
                        }

                    };
                $.ajax(imagedata).then(function (response) {

                    console.log(response);
                    $scope.imgsrc = response;
                    localStorage.setItem("url", $scope.imgsrc);
                    //getNotes();
                    window.location.reload();
                    $scope.smallcard = true;
                    $scope.largecard = false;
                    $scope.note.title = "";
                    $scope.note.notes = "";
                    $scope.color = "transparent";
                    //$scope.imgsrc = null;
                });


            }

            $scope.Archive = function (obj) {
                debugger;
                var data =
                    {

                        Id: obj.ID,
                        Mode: $scope.NoteUpdate,
                        IsArchive: !(obj.IsArchive),
                        UserID: obj.UserID,
                        Content: obj.Content,
                        Title: (obj.Title == 'null') ? "" : obj.Title,
                        ColorCode: obj.ColorCode,
                        IsPin: 0,
                        IsTrash: obj.IsTrash,
                        Reminder: null,
                        ImageUrl: obj.ImageUrl,
                        owner: obj.owner,
                        share: obj.share,
                        Label: obj.Label

                    }

                $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

                    window.location.reload();
                });

            }
        }

    };

    function buildToggler(componentId) {


        return function () {

            $mdSidenav(componentId).toggle();
        };
    }

    $scope.AddNote = function () {
        debugger;

        var objtbl =
            {

                Mode: $scope.NoteAdd,
                Content: $scope.noteContent,
                ColorCode: $scope.ColorSave,
                Title: $scope.Title,
                //Title: (obj.Title == 'null') ? "" : obj.Title,
                IsPin: pinned
            }
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }
        $http.post("/GetNotes", JSON.stringify(objtbl)).then(function (respones) {
            window.location.reload();
            if (respones != 0) {
                localStorage.setItem("result", "Note Added");
            }
        });
    }
    $scope.newcopy = function (obj) {


        var objtbl =
            {
                Mode: $scope.NoteAdd,
                UserID: obj.UserID,
                Content: obj.Content,
                Title: (obj.Title == 'null') ? "" : obj.Title,
                ColorCode: obj.ColorCode,
                IsPin: 0,
                ImageUrl: obj.ImageUrl,
                Label: obj.Label
            };

        var objtbl1 =
            {

                UserID: obj.UserID,
                ID: obj.ID,
                Mode: 3
            }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }
        $http.post("/GetNotes", JSON.stringify(objtbl)).then(function (respones) {
            debugger;
            //window.location.reload();
            //if (respones != 0) {
            //    localStorage.setItem("result", "Note Added");
            //}
            $http.post("/addnotelabel", JSON.stringify(objtbl1)).then(function (respones) {
                debugger;
                window.location.reload();
                if (respones != 0) {
                    localStorage.setItem("result", "Note Added");
                }
            });
        });


    }

    /// Reminde today
    $scope.remToday = function (obj) {
        debugger;
        var date = new Date();
        $scope.ddMMMMyyyy = $filter('date')(new Date(), 'MMMM dd');

        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: (obj.IsArchive),
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: $scope.ddMMMMyyyy + " " + "8:00 PM",
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label
        }

        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    // Reminde Tommorrow 
    $scope.remTommorrow = function (obj) {
        debugger;
        var Mode = $scope.update;
        var date = new Date();
        var NextDay = new Date(date);
        NextDay.setDate(date.getDate() + 1);

        var MMMM = $filter('date')(new Date(NextDay), 'dd MMMM');

        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: (obj.IsArchive),
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: globaltime + " " + "8:00 AM",
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label
        }


        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    // Reminde next week 
    $scope.remWeek = function (obj) {
        debugger;
        var Mode = $scope.update;
        //var DD = new Date();
        //var NextWeek = new Date(DD);
        //NextWeek.setDate(DD.getDate() + 7);
        //var DD1 = $filter('date')(new Date(NextWeek), 'dd MMMM');

        function checkTime(i) {

            return (i < 10) ? "0" + i : i;

        }


        //getting next monday date 
        var da = new Date();
        da.setDate(da.getDate() + (1 + 7 - da.getDay()) % 7);
        var nextweekmon = months[da.getMonth()] + " " + checkTime(da.getDate()) + " " + "8:00 PM";




        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: (obj.IsArchive),
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: nextweekmon,
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label
        }

        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    // Reminde by date
    $scope.saveRem = function (obj) {
        debugger;
        function checkTime(i) {
            return (i < 10) ? "0" + i : i;

        }

        var mydate = new Date(reminderdate);
        months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var date = mydate.getDate();
        var months = months[mydate.getMonth()];


        if (checkTime(mydate.getHours()).toString().length == 1) {
            hours = '0' + checkTime(mydate.getHours());
        }
        else {
            hours = checkTime(mydate.getHours());
            if (hours > 12) {
                hours -= 12;

            } else if (hours === 0) {
                hours = 12;
            }
        }
        minutes = checkTime(mydate.getMinutes()).toString().length == 1 ? '0' + checkTime(mydate.getMinutes()) : checkTime(mydate.getMinutes());

        ampm = checkTime(mydate.getHours()) >= 12 ? 'PM' : 'AM';

        var data = {



            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: obj.IsArchive,
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: months + " " + date + " " + hours + ":" + minutes + " " + ampm,
            ImageURL: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label

        }


        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }


    /// Remove Reminder.
    $scope.remove = function (obj) {
        debugger;
        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: obj.IsArchive,
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: null,
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label
        }

        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }


    $scope.removelabel = function (obj, lblname) {
        debugger;
        var data = {
            Id: obj.ID,
            Mode: $scope.Notedelete,
            IsArchive: obj.IsArchive,
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: obj.IsPin,
            IsTrash: obj.IsTrash,
            Reminder: obj.Reminder,
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: lblname
        }

        $http.post("/addnotelabel", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    // Pin the note or unpin the note.
    $scope.ispin = function (obj) {
        debugger
        var data = {

            ID: obj.ID,
            Mode: $scope.NoteUpdate,
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: !(obj.IsPin),
            IsArchive: (obj.IsArchive),
            Reminder: obj.Reminder,
            IsTrash: (obj.IsTrash),
            ImageURL: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label
        }
        $http.post("/GetNotes", JSON.stringify(data)).then(function (response) {
            if (response != 0) {
                debugger;
                if (!obj.IsPin) {
                    localStorage.setItem("result", "Note Pinned");
                }
                else {
                    localStorage.setItem("result", "Note UnPinned");
                }


            }

            else {
                localStorage.setItem("result", "Failed to Pinned");
            }
            window.location.reload();
        });

    }


    var pinned = 0;

    $scope.color = function (id, color) {
        debugger;
        var colorcode = color;
        var id = id;
        var obj = {
            ColorCode: colorcode,
            ID: id.ID
        }

        $http.post("/UpdateColor", JSON.stringify(obj)).then(function (response) {
            //GetNote();
            window.location.reload();

            console.log(response);
        });


    }

    $scope.addImage = function (obj) {
        debugger;
        $("#imagefile").click();
        var fileUpload = $('#imagefile').get(0);
        var files = fileUpload.files;
        var test = new FormData();
        for (var i = 0; i < files.length; i++) {
            test.append(files[i].name, files[i]);
        }


        test.append("Title", (obj.Title == 'null') ? "" : obj.Title, );
        test.append("Content", obj.Content);
        test.append("ColorCode", obj.ColorCode);
        test.append('ID', obj.ID);
        test.append('IsPin', obj.IsPin);
        test.append('IsArchive', obj.IsArchive);
        test.append('IsTrash', obj.IsTrash);
        test.append('Label', obj.Label);
        test.append('owner', obj.owner);
        test.append('share', obj.share);
        test.append('UserID', obj.UserID);

        var imagedata =
            {

                "url": "/api/uploadimage",
                "method": "POST",
                "processData": false,
                "contentType": false,
                "mimeType": "multipart/form-data",
                "data": test,
                success: function (response) {
                    console.log(response);
                },
                error: function (response) {
                    console.log(response);
                }

            };
        $.ajax(imagedata).then(function (response) {

            console.log(response);
            $scope.imgsrc = response;
            localStorage.setItem("url", $scope.imgsrc);
            //getNotes();
            window.location.reload();
            $scope.smallcard = true;
            $scope.largecard = false;
            $scope.note.title = "";
            $scope.note.notes = "";
            $scope.color = "transparent";
            //$scope.imgsrc = null;
        });


    }

    $scope.Archive = function (obj) {
        debugger;
        var data =
            {
                Id: obj.ID,
                Mode: $scope.NoteUpdate,
                IsArchive: !(obj.IsArchive),
                UserID: obj.UserID,
                Content: obj.Content,
                Title: (obj.Title == 'null') ? "" : obj.Title,
                ColorCode: obj.ColorCode,
                IsPin: 0,
                IsTrash: obj.IsTrash,
                Reminder: null,
                ImageUrl: obj.ImageUrl,
                owner: obj.owner,
                share: obj.share,
                Label: obj.Label
            }

        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });

    }

    $scope.Delete = function (obj) {
        debugger;
        var data =
            {
                Id: obj.ID,
                Mode: $scope.NoteUpdate,
                IsArchive: !(obj.IsArchive),
                UserID: obj.UserID,
                Content: obj.Content,
                Title: (obj.Title == 'null') ? "" : obj.Title,
                ColorCode: obj.ColorCode,
                IsPin: 0,
                IsTrash: 1,
                Reminder: null,
                ImageUrl: obj.ImageUrl,
                owner: obj.owner,
                share: obj.share,
                Label: obj.Label
            }
        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    $scope.Deleteforever = function (obj) {
        debugger;
        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: !(obj.IsArchive),
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: 0,
            IsTrash: 3,
            Reminder: null,
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label

        }
        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    $scope.Restore = function (obj) {
        debugger;
        var data = {
            Id: obj.ID,
            Mode: $scope.NoteUpdate,
            IsArchive: !(obj.IsArchive),
            UserID: obj.UserID,
            Content: obj.Content,
            Title: (obj.Title == 'null') ? "" : obj.Title,
            ColorCode: obj.ColorCode,
            IsPin: 0,
            IsTrash: 0,
            Reminder: null,
            ImageUrl: obj.ImageUrl,
            owner: obj.owner,
            share: obj.share,
            Label: obj.Label

        }
        $http.post("/GetNotes", JSON.stringify(data)).then(function (respones) {

            window.location.reload();
        });
    }

    $scope.CreateLabel = function (ev) {
        debugger;
        $("#tbl1 tbody tr").remove();


        $.ajax({
            method: "POST",
            contentType: "application/json; ",
            url: "/AddLabel",
            data: {

            },
            datatype: "json",
            success: function (result) {

                var jsonHtml = "";
                var jsondata = JSON.parse(result);
                $("#tbl1 tbody tr").remove();
                for (var i = 0; i < jsondata.length; i++) {
                    jsonHtml += "<tr><td style='  border: none;  text-align:left; '><img src='../Images/labels.svg' style='padding: 0 10px 0 10px;' id='img" + i + "' onmouseover='bigImg(" + i + ")' onmouseout='normalImg(" + i + ")'   onclick='deletelable(this," + i + ")' /><label id='lbl" + i + "' Class='mytxt'> " + jsondata[i].Label + "</label> </td><td style ='border:none'><img src='../Images/edit.svg' Class='mytxt'  id='img1" + i + "' onclick='bigImg1(this," + i + ")'  />   <input type='hidden' id='custId" + i + "' name='custId' value=" + jsondata[i].ID + "> </td></tr>";
                    //jsonHtml += "<tr><td style='  border: none;  text-align:left; '><img src='../Images/labels.svg' id='img" + i + "' onmouseover='bigImg(" + i + ")' onmouseout='normalImg(" + i + ")' />" + "      " + jsondata[i].Label + "</td><td style ='border:none'><img src='../Images/edit.svg' id='img1" + i + "' onclick='bigImg1(this," + i + ")'  /></td></tr>";

                }
                jsonHtml += "<tr><td colspan=2><input type='submit' value='DONE' class='btn .btn-default' onclick='refresh()' /></td></tr>";
                $('#tbl1 tbody').append(jsonHtml);
            },
            error: function (result) {
                alert('error');
            }
        });
        var modal = document.getElementById('myModal');

        var span = document.getElementsByClassName("close")[0];
        modal.style.display = "block";
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

    }




    //$scope.CreateLabel = function (ev) {
    //    debugger;
    //    $mdDialog.show({
    //        controller: DialogController,
    //        templateUrl: 'Labelpopup.html',
    //        parent: angular.element(document.body),
    //        targetEvent: ev,
    //        clickOutsideToClose: true,
    //        fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
    //    })



    //    //$http.post("http://localhost:53342/Account/verifycode").then(function (res) {
    //    //    console.log("Get Response" + res);
    //    //    alert(JSON.stringify(res));
    //    //});

    //};
    //function DialogController($scope, $mdDialog) {
    //    $scope.hide = function () {
    //        $mdDialog.hide();
    //    };
    //    $scope.cancel = function () {
    //        $mdDialog.cancel();
    //    };

    //    var objtbllabel = { Label: "-1" };

    //    $http.post("https://localhost:44326/AddLabel", objtbllabel).then(function (response) {
    //        console.log("Response is :", response);
    //        $scope.labels = response.data;


    //    });
    //}


    //Sideniv
    $scope.toggleLeft = buildToggler('left');
    $scope.NoteAdd = 1;
    $scope.Notedelete = 2;
    $scope.NoteUpdate = 3;
    $scope.NoteLabel = 4;
    $scope.NoteCollaborator = 5;
    $scope.Label = "";
    $scope.labels = "";
    // Add labels for perticular notes by checkbox.
    var arr = new Array();
    var labeldata = "";
    $scope.select = function (note) {
        debugger;
        $mdDialog.show
            ({
                controller: function () {
                    var $ctrl = this;
                    $ctrl.Title = note.Title;
                    $ctrl.Content = note.Content;
                    $ctrl.ColorCode = note.ColorCode;
                    $ctrl.imgsrc = note.Image;
                    $ctrl.ID = note.ID;
                    $ctrl.UserID = note.UserID
                    $ctrl.IsPin = note.IsPin;
                    $ctrl.IsArchive = note.IsArchive;
                    $ctrl.IsTrash = note.IsTrash;
                    $ctrl.Delete = note.IsDelete;
                    $ctrl.ImageUrl = note.ImageUrl;
                    $ctrl.Reminder = note.Reminder;
                    $ctrl.Label = note.Label;
                    $ctrl.owner = note.owner;
                    $ctrl.share = note.share;




                    $ctrl.toggle = function (label) {
                        debugger;
                        $scope.Label = label.Label

                        arr.push($scope.Label);
                        console.log("Array Data is :", arr);
                        labeldata = arr.toString();


                    }
                    $ctrl.AddSelected = function () {

                        var Label =
                            {

                                Title: note.Title,
                                Content: note.Content,
                                ColorCode: note.ColorCode,
                                imgsrc: note.Image,
                                ID: note.ID,
                                UserID: note.UserID,
                                IsPin: note.IsPin,
                                IsArchive: note.IsArchive,
                                IsTrash: note.IsTrash,
                                Delete: note.IsDelete,
                                ImageUrl: note.ImageUrl,
                                Reminder: note.Reminder,
                                Label: labeldata,
                                Mode: $scope.NoteAdd,
                                owner: note.owner,
                                share: note.share
                            }

                        $http.post("/addnotelabel", JSON.stringify(Label)).then(function (respones) {

                            if (respones != 0) {
                                localStorage.setItem("result", "Label Added");

                            }
                            window.location.reload();
                        });

                    }

                },

                templateUrl: "/Notes/SelectLabel",
                parent: angular.element(document.body),
                //  targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: $scope.customFullscreen,
                controllerAs: '$ctrl'
            })

    }

    $scope.checkbox1 = function () {
        debugger;
    }

    // Display notes of perticular label. 
    $scope.GetLabel = function (label) {
        debugger;
        var Displaylabel = label.Label;

        window.top.location.href = "/Notes/Displaylabel?Displaylabel=" + Displaylabel;

    }


    /// collaborator popup
    $scope.collaborator = function (note) {
        debugger;
        $scope.own = [];
        $http.post("https://localhost:44326/OwnerLabel", note).then(function (res) {
            debugger;


            for (var i = 0; i < res.data.length; i++) {
                if (i == 0) {
                    $scope.person = {
                        SharID: res.data[i].SharID,
                        NotesID: res.data[i].NotesID,
                        OwnerID: res.data[i].OwnerID,
                        Firstalph: res.data[i].OwnerID.charAt(0),
                        Firstshar: res.data[i].SharID.charAt(0)
                    };

                } else {
                    $scope.person = {
                        SharID: res.data[i].SharID,
                        NotesID: res.data[i].NotesID,
                        //OwnerID: res.data[i].OwnerID,
                        //Firstalph: res.data[i].OwnerID.charAt(0),
                        Firstshar: res.data[i].SharID.charAt(0)
                    };
                }

                $scope.own.push($scope.person);
            }
        }).then(function () {
            $mdDialog.show({
                locals: {
                    own: $scope.own
                },
                controller: function (own) {
                    var $ctrl = this;
                    $ctrl.own = own;
                    $ctrl.Title = note.Title;
                    $ctrl.Content = note.Content;
                    $ctrl.ColorCode = note.ColorCode;
                    $ctrl.imgsrc = note.Image;
                    $ctrl.ID = note.ID;
                    $ctrl.UserID = note.UserID;
                    $ctrl.IsPin = note.IsPin;
                    $ctrl.IsArchive = note.IsArchive;
                    $ctrl.IsTrash = note.IsTrash;
                    $ctrl.Delete = note.IsDelete;
                    $ctrl.ImageUrl = note.ImageUrl;
                    $ctrl.Reminder = note.Reminder;
                    $ctrl.Label = note.Label;
                    $ctrl.owner = note.owner;
                    $ctrl.share = note.share;

                    $ctrl.Collaborator = function (pick) {
                        debugger;
                        var shareWith = pick.Email;
                        var Label =
                            {
                                UserID: note.UserID,
                                SharID: shareWith,
                                NotesID: note.ID,
                                Mode: 1,
                                OwnerID: note.owner
                            }

                        $http.post("/AddCollaborator", Label).then(function (responses) {
                            if (responses != 0) {
                                localStorage.setItem("result", "Note Shared");

                            }
                            //window.location.reload();
                        });

                    }

                    $ctrl.Deletemail = function (note) {
                        debugger;
                        var Emailinfo = {
                            NotesID: note.data.NotesID,
                            OwnerID: note.data.OwnerID,
                            SharID: note.data.SharID,
                            Mode: 2,
                        }
                        $http.post("/AddCollaborator", Emailinfo).then(function (responses) {
                            if (responses != 0) {
                                localStorage.setItem("result", "Note Shared");

                            }
                            window.location.reload();
                        });
                    }

                },

                templateUrl: "/Notes/Collaborators",
                parent: angular.element(document.body),
                //  targetEvent: ev,9++
                clickOutsideToClose: true,
                fullscreen: $scope.customFullscreen,
                controllerAs: '$ctrl'
            })

            $("#Email").fadeOut(220);
        });



    }

    
    $scope.view=function()
    {
        var abc = $scope.layout;
        if ($scope.layout == "column") {
            $scope.layout = "row";
            $scope.width = "250px";
            $scope.img = "/Images/ic_view_list_black_24px.svg";


        }
        else
        {
            $scope.layout = "column";
            $scope.width = "615px";
            $scope.img = "/Images/gridview.svg";
        }
        
    }



});


