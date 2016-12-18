var IsDraft = true;

var Url = {
    viewStatus: "/Status/ViewStatus",
    publishCP: "/Status/publishCP",
    preparePublicationMail: "/Status/PreparePublicationMail",
    prepareOutlook: "/Status/openOutlook",
    sendPublicationMail: "/Status/SendPublicationMail",
    DeleteFile: "/Status/deletePublicationMailIMG"
}


function saveStatus(IsPublish, IsDraft) {
    var data = $("#form-status").serializeArray();
    if (IsPublish == true && $('#StatusText_HighLightText').val() == "")
        alert("oops, Readiness & Highlight Summary text is missing.", warning);
    else {
        action = "/Status/SaveStatus";
        $.post(action, data)
            .done(function (result) {
                if (IsPublish && result >= 0) {
                    var ReleaseID = $('#StatusText_ReleaseID').val();
                    PopUpWindow("/Status/GetReleaseCP", {
                        'ReleaseID': ReleaseID, "IsDraft": IsDraft
                    });
                    $('#popUp').addClass('popUpPusblish');
                }
                else result >= 0 ? alert("The status have been saved successfully", success) : alert("oops, error occurred in saving data process.", error);
            });
    }
};


var PrepareMail = function () {
    var CPID = parseInt($('#drop-cp option:selected').val());
    var ReleaseID = parseInt($('#StatusText_ReleaseID').val());


    var data = {
        CPID: CPID,
        ReleaseID: ReleaseID,
        ChangeRiskLevel: false
    }

    $.post(Url.publishCP, data)
        .done(function (data) {
            if (data.Result == "ExistQuestions") {
                confirm("Please note that some questions are belong to the selected CP but are not effective yet, These questions will now become effective and its Risk Level will set to ”High” ", information, ChangeRiskLevel);
            }
            else {
                exitPopUpWindow();
                if (data.TotalCPQuestion > 0)
                    uploadIMG(ReleaseID, CPID);
                else
                    alert("Please note that there are no questions to publish.", error);
            }
        })
        .error(function () {
        });
}

var ChangeRiskLevel = function () {
    var CPID = parseInt($('#drop-cp option:selected').val());
    var ReleaseID = parseInt($('#StatusText_ReleaseID').val());
    var data = {
        CPID: CPID, ReleaseID: ReleaseID, ChangeRiskLevel: true
    };

    var urlhref = window.location.href;
    $.post(Url.publishCP, data)
        .done(
            function (data) {
                if (data.IsUpdated == true) {
                    exitPopUpWindow();
                    var loadingSave = (JSON.parse(JSON.stringify(loading)));
                    loadingSave.title = "Update Status";
                    CustomConfirm("Calculating updated Status, Please wait....  ", loadingSave);

                    var data = {
                        ReleaseID: ReleaseID,
                        CPID: CPID,
                        IsDraft: IsDraft
                    }

                    post(Url.viewStatus, data);
                }
                else
                    alert("oops, error occurred in saving data process.", error);
            }
        ).error(function () {
            console.log("Change Risk Level failed");
        });
}


function post(path, params, method) {
    method = method || "post"; // Set method to post by default if not specified.

    // The rest of this code assumes you are not using a library.
    // It can be made less wordy if you use one.
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}


var uploadIMG = function (ReleaseID, CPID) {
    html2canvas(document.body,
        {
            onrendered: function (canvas) {
                var canvasMail = canvas;
                var imageData = canvas.toDataURL('image/png', 1.0);
                var data = {
                    IsDraft: IsDraft,
                    CPID: CPID,
                    ReleaseID: ReleaseID,
                    base64string: imageData.split(',')[1],

                };
                var loadingSave = (JSON.parse(JSON.stringify(loading)));
                loadingSave.title = "Prepare Mail";
                CustomConfirm("Preparing your Mail, Please wait....  ", loadingSave);
                // PopUpMailWindow(Url.preparePublicationMail, data);
                PopUpMailWindow(Url.prepareOutlook, data);
            },
        });
}

var SendMail = function () {
    $('#CPID').val($('#cp-mail').val());
    var path = $('#MailForm > img').attr('src');
    var data = $("#MailForm, #form-status").serializeArray();
    data.push({
        name: 'IsDarft', value: IsDraft
    });


    exitPopUpWindow();
    sendingAlert = alert("Your mail is sending now. Please wait...", Sending)
    $.post(Url.sendPublicationMail, data)
       .done(function (result) {
           if (result.IsSent) {
               DeleteMailIMG(path);
               console.log("Mail send successfuly!!!!!");
               sendingAlert.Close();

               alert("The Mail sent successfully", success);
           }
           else alert("oops, error occurred in saving data process.", error);

       })
        .error(function () {
            console.log("Sent mail failed :(")
        });
    return false;
}

var DeleteMailIMG = function (path) {
    var path = path ? path : $('#MailForm > img').attr('src');
    //$.post(Url.DeleteFile, { FilePath: path })
    //  .done(console.log("The IMG deleted successfuly"))
    //  .error(console.log("delete Failed..."));
}



$(document).ready(function () {

    // reload page with new scores
    if ($('#CPID').val() != 0) {
        IsDraft = $('#IsDraft').val();
        uploadIMG($('#ReleaseID').val(), $('#CPID').val());
    }

    $("#save-status").click(function () {
        saveStatus(false)
    });
    $("#draft-button").click(function () {
        IsDraft = true;
        saveStatus(true, IsDraft)
    });

    $("#formal-button").click(function () {
        IsDraft = false;
        saveStatus(true, IsDraft)
    });

    $('.header-center').each(function () {
        if( this.scrollWidth> $('.header-center').innerWidth())
        {
            $(this).css("width","69%")
        }
    })
});







