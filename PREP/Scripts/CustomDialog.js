function DialogInfo(title, Class, imageUrl, okBtnText, cancelBtnText) {
    this.title = title;
    this.Class = Class;
    this.imageUrl = imageUrl;
    this.okBtnText = okBtnText;
    this.cancelBtnText = cancelBtnText;
}

const danger = new DialogInfo("Danger", "danger", "/Content/Images/Danger.jpg");
const error = new DialogInfo("Error", "error", "/Content/Images/Error.png");
const information = new DialogInfo("Information", "information", "/Content/Images/Information.jpg");
const success = new DialogInfo("Success", "success", "/Content/Images/Success.png");
const warning = new DialogInfo("Warning", "warning", "/Content/Images/Warning.png");
const loading = new DialogInfo("Initiate CheckList", "loading", "/Content/Images/ajax-loader.gif");
const Sending = new DialogInfo("Sending", "loading", "/Content/Images/ajax-loader.gif");
const waiting = new DialogInfo("", "waiting", "/Content/Images/ajax-loader.gif");
const leaveScenario = new DialogInfo("Attention", "Do you want to save your changes?", "/Content/Images/Information.jpg", "Yes", "No");

if (document.getElementById) {
    //override confirm() method
    window.confirm = function (message, DialogInfo, okBtnFunc, param, cnclBtnFunc, param2) {
        return CustomConfirm(message, DialogInfo, okBtnFunc, param, cnclBtnFunc, param2);
    }
    //override alert() method
    window.alert = function (message, DialogInfo) {

        return CustomConfirm(message, DialogInfo, null);
    }
}
//create custom dialog box according to the parameters
function CustomConfirm(message, DialogInfo, okBtnFunc, param, cnclBtnFunc, param2) {

    message = message || '';
    DialogInfo = DialogInfo || information;
    DialogInfo.title = DialogInfo.title || 'Message';
    DialogInfo.okBtnText = DialogInfo.okBtnText || 'Ok';
    DialogInfo.cancelBtnText = DialogInfo.cancelBtnText || 'Cancel';
    DialogInfo.Class = DialogInfo.Class || '';

    d = document;
    if (d.getElementById("divCustomConfirm")) return;
    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "dialogoverlay";

    alertObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    alertObj.id = "dialogbox";
    alertObj.className = DialogInfo.Class;

    head = alertObj.appendChild(d.createElement("div"));
    if (DialogInfo.imageUrl != null) {
        icon = head.appendChild(d.createElement("img"));
        icon.src = DialogInfo.imageUrl;
    }
    text = head.appendChild(d.createElement("span"));
    text.innerText = DialogInfo.title;
    head.id = "dialogboxhead";

    msg = alertObj.appendChild(d.createElement("div"));
    msg.innerHTML = message;
    msg.id = "dialogboxbody";

    divBtns = alertObj.appendChild(d.createElement("div"));
    divBtns.id = "dialogboxfoot";
    divBtns.className = okBtnFunc != null ? "dialogconfirm" : "dialogalert";

    okBtn = divBtns.appendChild(d.createElement("button"));
    okBtn.id = "okBtn";
    okBtn.focus();
    okBtn.innerText = DialogInfo.okBtnText;
    okBtn.className = "button";
    okBtn.onclick = function () {
        RemoveCustomElementById("dialogoverlay");
        RemoveCustomElementById("dialogbox");
        if (okBtnFunc != null)
            okBtnFunc(param);
        return true;
    }

    if (typeof (cnclBtnFunc) == 'undefined' && okBtnFunc != null || cnclBtnFunc != null) {
        cancelBtn = divBtns.appendChild(d.createElement("button"));
        cancelBtn.id = "cancelBtn";
        cancelBtn.focus();
        cancelBtn.innerText = DialogInfo.cancelBtnText;
        cancelBtn.className = "button";
        cancelBtn.onclick = function () {
            RemoveCustomElementById("dialogoverlay");
            RemoveCustomElementById("dialogbox");
            if (cnclBtnFunc != null)
                cnclBtnFunc(param2);
            return false;
        }
        // okBtn.onclick = function () { RemoveCustomElementById("dialogoverlay"); RemoveCustomElementById("dialogbox"); if (okBtnFunc != null) okBtnFunc(param, okBtnFunc()); return true; }
    }
    alertObj.style.display = "block";
    var CurrentAlert = {}
    CurrentAlert.Close = function () {
        RemoveCustomElementById("dialogoverlay");
        RemoveCustomElementById("dialogbox");
    };

    return CurrentAlert;
}

function RemoveCustomElementById(mId) {
    document.getElementsByTagName("body")[0].removeChild(document.getElementById(mId));
    return true;
}


//=========================popUpWindow===================
var PopUpWindow = function (action, param, callback) {

    $.ajax({
        type: "POST",
        url: action,
        async: true,
        data: param,
        success: function (data) {
            if (!data) {
                alert("oops, error occurred in loading data.", error);
                return;
            }
            //if (true) {
            d = document;
            mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
            mObj.className = "dialogoverlay";
            popUpObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
            popUpObj.id = "popUp";
            popUpObj.className = "popUp";
            //popUpObjx = d.getElementById("popUp").appendChild(d.createElement("input"));
            //popUpObjx.className = "exit-popup-btn";
            //popUpObjx.type = "button";
            //  popUpObjx.va

            //} else {
            //    $("#popUp").show();
            //    $("#dialogoverlay").show();
            //}
            // console.log(data);
            $(".popUp").html(data);

            //$(".popUp").html("");

        },
        error: function (data, textStatus, jqXHR) {
            console.log(data);
            console.log(textStatus);
            console.log(jqXHR);
            alert("oops, error occurred in loading data.", error);
        }
    });
}



var PopUpMailWindow = function (action, data, alertSave) {
    $.ajax({
        type: "POST",
        url: action,
        async: true,
        data: data,
        success: function (data) {
            if (alertSave != null)
                alertSave.Close();
            if (!data) {
                alert("oops, error occurred in loading data.", error);
                return;
            }
            //if (true) {
            d = document;
            mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
            mObj.className = "dialogoverlay";
            popUpObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));

            popUpObj.id = "popUp";
            popUpObj.className = "popUp";
             $(".popUp").html(data);
        },
        error: function (data, textStatus, jqXHR) {
            console.log(data);
            console.log(textStatus);
            console.log(jqXHR);
            alert("oops, error occurred in loading data.", error);
        }
    });
}





var exitPopUpWindow = function () {
    //RemoveCustomElementById("dialogoverlay");
    //RemoveCustomElementById("popUp");
    $(".popUp").empty();
    $(".popUp").remove();
    $(".dialogoverlay").remove();
}

var ChangeContentPopUpWindow = function (action, param) {
    $.ajax({
        type: "GET",
        url: action,
        async: false,
        data: param,
        success: function (data) {
            if (!data) {
                alert("oops, error occurred in loading data.", error);
                return;
            }
            $(".popUp").html(data);

        },
        error: function (data, textStatus, jqXHR) {
            console.log(data);
            console.log(textStatus);
            console.log(jqXHR);
            alert("oops, error occurred in loading data.", error);
        }
    });
}