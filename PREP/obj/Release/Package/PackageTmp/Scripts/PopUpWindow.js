var PopUpWindow = function (action) {
    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "dialogoverlay";
    popUpObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    popUpObj.id = "popUp";
    popUpObj.className = "popUp";
}