app.controller('DashBoardCtrl', function ($scope, $http) {

    $scope.TotalComplaints = 0;
    $scope.AllocatedComplaints = 0;
    $scope.CompletedComplaints = 0;
    $scope.HoldComplaints = 0;
    $scope.ProgressComplaints = 0;
    $scope.LeadHistory = {};
    //$http.get("../User/getDashboardUsersAndStatus").then(
    //    function (response) {
    //        if (response.data.success) {
    //            console.log("Users");
    //            console.log(response.data.AllocatedStatus);
    //            $scope.AllocatedUsers = response.data.AllocatedUsers;
    //            $scope.AllocatedStatus = response.data.AllocatedStatus;
    //            $scope.ContactStatus = response.data.ContactStatus;
    //            $scope.OppStatus = response.data.OppStatus;
                

    //        } else if (response.data.expire) {
    //            window.location.href = '../Home/Login';
    //        }
    //    },
    //    function (err) {
    //        var error = err;
    //    });
    $http.get("../User/getComplaintsCount").then(
        function (response) {
            if (response.data.success) {
                $scope.TotalComplaints = response.data.TotalComplaints;
                $scope.AllocatedComplaints = response.data.AllocatedComplaints;
                $scope.CompletedComplaints = response.data.CompletedComplaints;
                $scope.HoldComplaints = response.data.HoldComplaints;
                $scope.ProgressComplaints = response.data.ProgressComplaints;
                

            } else if (response.data.expire) {
                window.location.href = '../Home/UserLogin';
            }
        },
        function (err) {
            var error = err;
        });

    $scope.viewHistory = function (LAID) {
        //alert(LAID)
        $http.get("../User/getLeadHistory?LID=" + LAID).then(
            function (response) {
                if (response.data.success) {
                    console.log("Leads");
                    console.log(response.data.LeadHistory);
                    $scope.LeadHistory = response.data.LeadHistory;
                } else if (response.data.expire) {
                    window.location.href = '../Home/Login';
                }
            },
            function (err) {
                var error = err;
            });
    }

    $scope.viewProfile = function (CompanyName, CompanyDescription, CompanyCategory) {
        $scope.CompanyName = CompanyName;
        $scope.CompanyDescription = CompanyDescription;
        $scope.CompanyCategory = CompanyCategory;
    }

    $scope.editLeadScreen = function (LAID) {
        //alert(LAID)
        $scope.LeadUpdate = {};
        $scope.LeadUpdateLID = LAID;
    }

    $scope.getUser = function (StatusID) {
        $scope.isQualify = false;
        $scope.isOpportunity = false;
        if (StatusID == 1) {
            $scope.isQualify = true;
            $scope.isOpportunity = false;
        } else if (StatusID == 3) {
            $scope.isQualify = false;
            $scope.isOpportunity = true;
        } else if (StatusID == 4) {
            $scope.isQualify = false;
            $scope.isOpportunity = true;
        } else {
            $scope.isQualify = false;
            $scope.isOpportunity = false;
        }
    }
    $scope.updateLead = function (ContactLead) {
        if (ContactLead == undefined || ContactLead == "") {
            alertify.alert('Error', "Please Enter Required Fields", function () {
            });
        } else if (ContactLead.StatusID == undefined || ContactLead.StatusID == "") {
            alertify.alert('Error', "Please Change Status", function () {
            });
        } else if (ContactLead.StatusID == 3 || ContactLead.StatusID == 4) {
            if (ContactLead.OwnerID == undefined || ContactLead.OwnerID == "") {
                alertify.alert('Error', "Please Select ContactLead Owner", function () {
                });
            } else if (ContactLead.ContactDate == undefined || ContactLead.ContactDate == "") {
                alertify.alert('Error', "Please Select Contact Date", function () {
                });
            } else if (ContactLead.ContactTime == undefined || ContactLead.ContactTime == "") {
                alertify.alert('Error', "Please Select Contact Time", function () {
                });
            } else if (ContactLead.ContactTo == undefined || ContactLead.ContactTo == "") {
                alertify.alert('Error', "Please Select Contact By", function () {
                });
            } else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
                alertify.alert('Error', "Please write your comments for change Lead Status", function () {
                });
            } else {
                $scope.imgLoader_Save1 = true;

                $http.post("../User/UpdateLeadOwner?LID=" + $scope.LeadUpdateLID + "&StatusID=" + ContactLead.StatusID + "&OwnerID=" + ContactLead.OwnerID + "&ContactDate=" + ContactLead.ContactDate + "&ContactTime=" + ContactLead.ContactTime + "&ContactTo=" + ContactLead.ContactTo + "&Comments=" + ContactLead.Comments).then(
                    function (response) {
                        if (response.data.success) {
                            delete $scope.ContactLead;
                            $('#myModal').modal('hide');
                            $scope.imgLoader_Save1 = false;
                            
                            alertify.alert('Success', "Lead Screen Updated Successfully", function () {
                                window.location.reload();
                            });

                        } else if (response.data.error) {
                            $scope.imgLoader_Save1 = false;
                            alertify.alert('Error', "Try Again Later", function () {
                            });
                        }
                    },
                    function (err) {
                        var error = err;
                    });
            }
        } else if (ContactLead.StatusID == 1) {

            if (ContactLead.ContactDate == undefined || ContactLead.ContactDate == "") {
                alertify.alert('Error', "Please Select Contact Date", function () {
                });
            } else if (ContactLead.ContactTime == undefined || ContactLead.ContactTime == "") {
                alertify.alert('Error', "Please Select Contact Time", function () {
                });
            } else if (ContactLead.ContactTo == undefined || ContactLead.ContactTo == "") {
                alertify.alert('Error', "Please Select Contact By", function () {
                });
            } else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
                alertify.alert('Error', "Please write your comments for change ContactLead Status", function () {
                });
            } else {
                $scope.imgLoader_Save1 = true;

                $http.post("../User/UpdateLeadStatus1?LID=" + $scope.LeadUpdateLID + "&StatusID=" + ContactLead.StatusID + "&ContactDate=" + ContactLead.ContactDate + "&ContactTime=" + ContactLead.ContactTime + "&ContactTo=" + ContactLead.ContactTo + "&Comments=" + ContactLead.Comments).then(
                    function (response) {
                        if (response.data.success) {
                            delete $scope.ContactLead;
                            $('#myModal').modal('hide');
                            $scope.imgLoader_Save1 = false;
                            
                            alertify.alert('Success', "ContactLead Screen Updated Successfully", function () {
                                window.location.reload();
                            });

                        } else if (response.data.error) {
                            $scope.imgLoader_Save1 = false;
                            alertify.alert('Error', "Try Again Later", function () {
                            });
                        }
                    },
                    function (err) {
                        var error = err;
                    });
            }
        } else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
            alertify.alert('Error', "Please write your comments for change ContactLead Status", function () {
            });
        } else {
            $scope.imgLoader_Save1 = true;
            $http.post("../User/UpdateLeadStatus?LID=" + $scope.LeadUpdateLID + "&StatusID=" + ContactLead.StatusID + "&Comments=" + ContactLead.Comments).then(
                function (response) {
                    if (response.data.success) {
                        delete $scope.ContactLead;
                        $scope.imgLoader_Save1 = false;
                        
                        $('#myModal').modal('hide');
                        alertify.alert('Success', "ContactLead Screen Updated Successfully", function () {
                            window.location.reload();

                        });

                    } else if (response.data.error) {
                        $scope.imgLoader_Save = false;
                        alertify.alert('Error', "Try Again Later", function () {
                            
                        });
                    }
                },
                function (err) {
                    var error = err;
                });
        }

    }

    $scope.editContactScreen = function (LAID) {
        $scope.ContactUpdate = {};
        $scope.ContactUpdateLID = LAID;
    }

    $scope.getUser1 = function (StatusID) {
        $scope.isQualify = false;
        $scope.isOpportunity = false;
        if (StatusID == 1) {
            $scope.isQualify = true;
            $scope.isOpportunity = false;
        }
        if (StatusID == 3) {
            $scope.isQualify = false;
            $scope.isOpportunity = true;
        }
    }
    $scope.updateContact = function (ContactLead) {
        if (ContactLead == undefined || ContactLead == "") {
            alertify.alert('Error', "Please Enter Required Fields", function () {
            });
        } else if (ContactLead.StatusID == undefined || ContactLead.StatusID == "") {
            alertify.alert('Error', "Please Change Status", function () {
            });
        } else if (ContactLead.StatusID == 3) {
            if (ContactLead.OwnerID == undefined || ContactLead.OwnerID == "") {
                alertify.alert('Error', "Please Select ContactLead Owner", function () {
                });
            } else if (ContactLead.ContactDate == undefined || ContactLead.ContactDate == "") {
                alertify.alert('Error', "Please Select Contact Date", function () {
                });
            } else if (ContactLead.ContactTime == undefined || ContactLead.ContactTime == "") {
                alertify.alert('Error', "Please Select Contact Time", function () {
                });
            } else if (ContactLead.ContactTo == undefined || ContactLead.ContactTo == "") {
                alertify.alert('Error', "Please Select Contact By", function () {
                });
            } else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
                alertify.alert('Error', "Please write your comments for change ContactLead Status", function () {
                });
            } else {
                $scope.imgLoader_Save1 = true;

                $http.post("../User/UpdateContactOwner?LID=" + $scope.ContactUpdateLID + "&StatusID=" + ContactLead.StatusID + "&OwnerID=" + ContactLead.OwnerID + "&ContactDate=" + ContactLead.ContactDate + "&ContactTime=" + ContactLead.ContactTime + "&ContactTo=" + ContactLead.ContactTo + "&Comments=" + ContactLead.Comments).then(
                    function (response) {
                        if (response.data.success) {
                            delete $scope.ContactLead;
                            $('#myModal1').modal('hide');
                            $scope.imgLoader_Save1 = false;
                            
                            alertify.alert('Success', "ContactLead Screen Updated Successfully", function () {
                                window.location.reload();
                            });

                        } else if (response.data.error) {
                            $scope.imgLoader_Save1 = false;
                            alertify.alert('Error', "Try Again Later", function () {
                            });
                        }
                    },
                    function (err) {
                        var error = err;
                    });
            }
        } else if (ContactLead.StatusID == 1) {

            if (ContactLead.ContactDate == undefined || ContactLead.ContactDate == "") {
                alertify.alert('Error', "Please Select Contact Date", function () {
                });
            } else if (ContactLead.ContactTime == undefined || ContactLead.ContactTime == "") {
                alertify.alert('Error', "Please Select Contact Time", function () {
                });
            } else if (ContactLead.ContactTo == undefined || ContactLead.ContactTo == "") {
                alertify.alert('Error', "Please Select Contact By", function () {
                });
            }  else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
                alertify.alert('Error', "Please write your comments for change ContactLead Status", function () {
                });
            } else {
                $scope.imgLoader_Save1 = true;

                $http.post("../User/UpdateContactStatus1?LID=" + $scope.ContactUpdateLID + "&StatusID=" + ContactLead.StatusID + "&ContactDate=" + ContactLead.ContactDate + "&ContactTime=" + ContactLead.ContactTime + "&ContactTo=" + ContactLead.ContactTo +  "&Comments=" + ContactLead.Comments).then(
                    function (response) {
                        if (response.data.success) {
                            delete $scope.ContactUpdate;
                            $('#myModal1').modal('hide');
                            $scope.imgLoader_Save1 = false;
                            
                            alertify.alert('Success', "ContactLead Screen Updated Successfully", function () {
                                window.location.reload();
                            });

                        } else if (response.data.error) {
                            $scope.imgLoader_Save1 = false;
                            alertify.alert('Error', "Try Again Later", function () {
                            });
                        }
                    },
                    function (err) {
                        var error = err;
                    });
            }
        } else if (ContactLead.Comments == undefined || ContactLead.Comments == "") {
            alertify.alert('Error', "Please write your comments for change ContactLead Status", function () {
            });
        } else {
            $scope.imgLoader_Save1 = true;
            $http.post("../User/UpdateContactStatus?LID=" + $scope.ContactUpdateLID + "&StatusID=" + ContactLead.StatusID + "&Comments=" + ContactLead.Comments).then(
                function (response) {
                    if (response.data.success) {
                        delete $scope.$scope.ContactUpdate;
                        $scope.imgLoader_Save1 = false;
                       
                        $('#myModal1').modal('hide');
                        alertify.alert('Success', "ContactLead Screen Updated Successfully", function () {

                            window.location.reload();
                        });

                    } else if (response.data.error) {
                        $scope.imgLoader_Save = false;
                        alertify.alert('Error', "Try Again Later", function () {
                            
                        });
                    }
                },
                function (err) {
                    var error = err;
                });
        }

    }

    $scope.editOppScreen = function (LAID) {
        $scope.OppUpdate = {};
        $scope.OppUpdateLID = LAID;
    }

    $scope.getUser2 = function (StatusID) {
        $scope.isOpportunity = false;
        if (StatusID == 1) {
            $scope.isOpportunity = true;
        }

    }
    $scope.updateOpportunity = function (OppLead) {
        if (OppLead == undefined || OppLead == "") {
            alertify.alert('Error', "Please Enter Required Fields", function () {
            });
        } else if (OppLead.StatusID == undefined || OppLead.StatusID == "") {
            alertify.alert('Error', "Please Change Status", function () {
            });
        } else if (OppLead.StatusID == 1) {

            if (OppLead.ContactDate == undefined || OppLead.ContactDate == "") {
                alertify.alert('Error', "Please Select Contact Date", function () {
                });
            } else if (OppLead.ContactTime == undefined || OppLead.ContactTime == "") {
                alertify.alert('Error', "Please Select Contact Time", function () {
                });
            } else if (OppLead.ContactTo == undefined || OppLead.ContactTo == "") {
                alertify.alert('Error', "Please Select Contact To", function () {
                });
            } else if (OppLead.Comments == undefined || OppLead.Comments == "") {
                alertify.alert('Error', "Please write your comments for change Opportunity Lead Status", function () {
                });
            } else {
                $scope.imgLoader_Save1 = true;

                $http.post("../User/UpdateOppStatus1?LCustID=" + $scope.OppUpdateLID + "&StatusID=" + OppLead.StatusID + "&ContactDate=" + OppLead.ContactDate + "&ContactTime=" + OppLead.ContactTime + "&ContactTo=" + OppLead.ContactTo + "&Comments=" + OppLead.Comments).then(
                    function (response) {
                        if (response.data.success) {
                            delete $scope.OppUpdate;
                            $('#myModal2').modal('hide');
                            $scope.imgLoader_Save1 = false;
                            $scope.isOpportunity = false;
                            alertify.alert('Success', "Opportunity Lead Screen Updated Successfully", function () {
                                window.location.reload();
                                
                            });

                        } else if (response.data.error) {
                            $scope.imgLoader_Save1 = false;
                            alertify.alert('Error', "Try Again Later", function () {
                            });
                        }
                    },
                    function (err) {
                        var error = err;
                    });
            }
        } else if (OppLead.Comments == undefined || OppLead.Comments == "") {
            alertify.alert('Error', "Please write your comments for change Opportunity Lead Status", function () {
            });
        } else {
            $scope.imgLoader_Save1 = true;
            $http.post("../User/UpdateOppStatus?LCustID=" + $scope.OppUpdateLID + "&StatusID=" + OppLead.StatusID + "&Comments=" + OppLead.Comments).then(
                function (response) {
                    if (response.data.success) {
                        delete $scope.OppUpdate;
                        $scope.imgLoader_Save1 = false;
                            $scope.isOpportunity = false;

                        $('#myModal2').modal('hide');
                        alertify.alert('Success', "OppLead Screen Updated Successfully", function () {
                            window.location.reload();
                        });

                    } else if (response.data.error) {
                        $scope.imgLoader_Save = false;
                        alertify.alert('Error', "Try Again Later", function () {
                            
                        });
                    }
                },
                function (err) {
                    var error = err;
                });
        }

    }

});

app.directive('datepicker1', function () {
    //alert()
    return {
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).datepicker({
                dateFormat: 'yy-mm-dd',
                autoSize: true,
                numberOfMonths: 1,
                minDate: 0,
                //beforeShowDay: $.datepicker.noWeekends,
                showOptions: { origin: ["top", "center"] },
                onSelect: function (dateText) {
                    //alert(dateText)
                    //alert(this.id)
                    scope.$apply(function () {
                        ngModel.$setViewValue(dateText);
                    });
                    // alert(this.id)
                    if (this.id === "cendDate") {

                        $("#cstartDate").datepicker("option", "maxDate", dateText);
                    }
                    if (this.id === "pendDate") {

                        $("#pstartDate").datepicker("option", "maxDate", dateText);
                    }


                }

            });
        }
    };
});
