
//Employee search model declaration
var oTable;
$("#dialogEmployeeSearch").dialog({
    autoOpen: false,
    closeText: "",
    title: "Employees",
    height: 750,
    width: 700,
    resizable: false,
    modal: true,
    dialogClass: "dialogEmployeeSearch-dialog be-over",
    buttons:

         [
             {
                 id: "button-ok",
                 text: "Ok",
                 click: function () {
                     var empNames = [];
                     var empIds = [];

                     var data = $('#hdnSelectedSearchEmployee').data('record');
                     if (!data) data = oTable.fnGetData($('#gridEmployeeSearch tbody tr input:radio:checked').closest('tr')[0]);

                     if ($("." + jq(SearchType) + "_txtBox").length == 0) {
                         $("[data-hidden=" + SearchTypeIdetinifier + "]").val(data["EmployeeID"]);
                         $("[data-displayed=" + SearchTypeIdetinifier + "]").val(data["FirstName"] + " " + data["LastName"])
                         $(this).dialog("close");
                         $("#dialogoverlay").remove();
                         $("[data-displayed=" + SearchTypeIdetinifier + "]").trigger('change');
                     }
                     else {
                         $("." + jq(SearchType) + "_txtBox").val(data["FirstName"] + " " + data["LastName"]);
                         $("." + jq(SearchType) + "_hdnId").val(data["EmployeeID"]);
                         $(this).dialog("close");
                         $("#dialogoverlay").remove();
                     }
                 }
             },
                     {
                         id: "button-cancel",
                         text: "Cancel",
                         click: function () {
                             $(this).dialog("close");
                             $("#dialogoverlay").remove();
                         }
                     }
         ]
    ,
    close: function () {
        $("#dialogoverlay").remove();
        $("#txtempName").val("");
        $("#txtCodeID").val("");
    }
});




//Open Employee serach dialog
//$("#openerEmployeeSearch,#EmpSearchVpName,#EmpSearchCdmName,#EmpSearchVmName,#EmpSearchCbeName,#EmpSearchCemName,#EmpSearchQbpName,#EmpSearchQeName,#EmpSearchPrFpName,#EmpSearchDisApprovedBy").click(function () {
var SearchType = "";
$(".userIcon , .user-icon, .userDialog").click(employeeDialogClick);

var employeeDialogClick = function () {//#EmpSearchVpName,#EmpSearchCdmName
    SearchType = $(this).attr('id');
    SearchTypeIdetinifier = $(this).data('displayed');


    $("#gridEmployeeSearch").DataTable().clear().destroy();
    $("#button-ok").prop("disabled", true).addClass("ui-state-disabled");
    $("#gridEmployeeSearch").hide();
    $('#dialogEmployeeSearch').css('height', 'auto');
    $('.ui-dialog.dialogEmployeeSearch-dialog').css('margin-top', '-250');
    d = document;
    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "dialogoverlay";
    $("#dialogEmployeeSearch").dialog("open");

    $('.dataTables_filter input').val('').keyup();
    return false;
}

//search emplyee details by name or id

//var expertpostUrl = '@Url.Action("GetContentEmployee", "Base")';
$("#BtnSearch").click(performEmployeeSearch);
$("#txtempName , #txtCodeID").keypress(function (e) {
    if (e.which == 13) {
        $("#BtnSearch").click();
    }
});

var performEmployeeSearch = function () {

    $('#gridEmployeeSearch').DataTable().clear().destroy();
    var name = $.trim($("#txtempName").val());
    var empcode = $.trim($("#txtCodeID").val());
    if (empcode == "" && name == "") {
        var $messageDiv = $('#dialogboxbodyEmpSearchAlert');
        $messageDiv.html("Please enter the ID or Name.");
        $("#MsgAlertdialog").dialog("open");
        $("#gridEmployeeSearch").hide();
        $("#button-ok").prop("disabled", true).addClass("ui-state-disabled");
        return false;
    }

    if (empcode != "" && empcode.match(/^\d+$/) == null) {
        var $messageDiv = $('#dialogboxbodyEmpSearchAlert');
        $messageDiv.show().html("Please enter the only number.");
        $("#MsgAlertdialog").dialog("open");
        $("#gridEmployeeSearch").hide();
        $("#button-ok").prop("disabled", true).addClass("ui-state-disabled");
        return false;
    }
    if (empcode != "" && empcode.length < 2) {
        var $messageDiv = $('#dialogboxbodyEmpSearchAlert');
        $messageDiv.show().html("Please enter the minimum two digits.");
        $("#MsgAlertdialog").dialog("open");
        $("#gridEmployeeSearch").hide();
        $("#button-ok").prop("disabled", true).addClass("ui-state-disabled");
        return false;
    }
    if (name != "" && name.length < 3) {
        var $messageDiv = $('#dialogboxbodyEmpSearchAlert');
        $messageDiv.show().html("Please enter the minimum three character.");
        $("#MsgAlertdialog").dialog("open");
        $("#gridEmployeeSearch").hide();
        $("#button-ok").prop("disabled", true).addClass("ui-state-disabled");
        return false;
    }


    $("#gridEmployeeSearch").show();


    var postdata = {
        name: name,
        code: empcode
    };



    oTable = $('#gridEmployeeSearch').dataTable({
        "bProcessing": true,
        "bFilter": true,
        "bSort": true,
        bAutoWidth: false,
        "oLanguage": {
            "sSearch": "Narrow Search:"
        },
        ajax: {
            data: postdata,
            url: urlpath,
        },
        "fnInitComplete": function () {
            $('.dataTables_scrollHeadInner').css('padding-left', '0px'); //changing the width
            $('#gridEmployeeSearch tbody tr').off('click').on('click', function () {
                var self = this;
                if ($(this).find('input:radio').attr('checked', true)) {
                    var aData = oTable.fnGetData(self);
                    $('#hdnSelectedSearchEmployee').data('record', aData);
                    $("#button-ok").prop("disabled", false).removeClass("ui-state-disabled");
                }
            });
        },
        "fnDrawCallback": function () {
            $('.dataTables_scrollHeadInner').css('padding-left', '0px'); //changing the width

        },
        "aoColumns": [
            {
                sClass: "center",
                mData: "EmployeeID",
                sWidth: "5%",
                mRender: function (data) {
                    return '<input type="radio" name="empId" id="rb' + data + '">';
                }
            },
           // "aoColumnDefs": [{ "iDataSort": 1, "aTargets": [0] }, { "bVisible": false, "aTargets": [1] }],

            { mData: "MDMCode", sClass: "left", sWidth: "5%" },
            { mData: "FirstName", sClass: "left", sWidth: "5%" },
            { mData: "LastName", sClass: "left", sWidth: "5%" },
            { mData: "UserName", sClass: "left", sWidth: "5%" },
        ],
        //"aoColumnDefs": [ { "bVisible": false, "aTargets": [5] }],
        "scrollY": "500px",
        "paging": false
    });





//Get selected row data
$(document).ready(function () {

    //var table = $('#gridEmployeeSearch').DataTable();


    dialog = $("#MsgAlertdialog").dialog({
        autoOpen: false,
        height: 200,
        width: 350,
        resizable: false,
        modal: true,
        dialogClass: "MsgAlert-dialog over-all"
    }).dialog("widget").find(".ui-dialog-titlebar-close").hide();

    $("#okBtnPopup").on("click", function () {
        window.parent.$('#MsgAlertdialog').dialog('close');
    });
});
}


function jq(myid) {
    return myid.replace(/(:|\.|\[|\]|,)/g, "\\$1");
}