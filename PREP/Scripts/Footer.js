$(document).ready(function () {
    $(".checkpoint-link").click(openCheckpointDetailsPopUp);
});

var openCheckpointDetailsPopUp = function () {
    console.log("openCheckpointDetailsPopUp")
    var UrlReleaseCP = '/Releases/releaseCPDetails/';
    var CPID = $(event.target).closest("p").data("cpid");
    $("#milstone-strip-wrapper").data("selectedmilstone", CPID);
    var data = { "CPID": CPID };
    PopUpWindow(UrlReleaseCP, data)

}

var UrlSaveReleaseCPDetails = '/Releases/SaveReleaseCPDetails/';




var cpDetailsSave = function () {
    //console.log("cpDetailsSave")
    releaseCPObjectSerilized = getFormData($('.popUpViewWrapper'));
  //  console.log("releaseCPObjectSerilized====", releaseCPObjectSerilized);
    $.ajax({
        contentType: 'application/json',
        type: "POST",
        url: UrlSaveReleaseCPDetails,
        dataType: "json",
        data: JSON.stringify(releaseCPObjectSerilized),
        async: true,
        success: function (data) {
            console.log("cpDetailsSave")
            if (data >= 0) {
                exitPopUpWindow();
            }
            else
                alert("oops, error occurred in saving data process. Please try again.", error);
        },
        error: function (data, textStatus, jqXHR) {
            console.log(data);
            console.log(textStatus);
            console.log(jqXHR);
            alert("oops, error occurred in saving data process. Please try again.", error);
        }
    });
}

var setDatePicker = function () {
    $('.form_datetime .datetimepickerinput').datetimepicker({
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0,
        format: "dd-M-yyyy",
        pickerPosition: "bottom-right"
    });

    $(".datetimepickercalendar").click(function () {
        var nameinput = this.id.substring(1);
        $("[name='" + nameinput + "']").data('datetimepicker').show();
        $("[name='" + nameinput + "']").focus();
    });
};