var ReleaseDetails = function () {

    var $tabwrapper = $('#tabs').find('.navigation-bar ul');
    var $liTabs = $tabwrapper.find('li:not(.hide)');

    var $tabfrom = $('#tabs').find('form#formsave');
    var $tabcontent = $tabfrom.find('#tab-content');
    var $tabsbody = $tabcontent.find('.tab-pane');

    var $GeneralDetails = $tabsbody.filter('#GeneralDetails');
    var $ProductsInScope = $tabsbody.filter('#ProductsInScope');
    var $AreaOwners = $tabsbody.filter('#AreaOwners');
    var $ReleaseStakeholders = $tabsbody.filter('#ReleaseStakeholders');


    ViewEditMode = {}
    ViewEditMode.view = { id: "View", cssToShow: ".view-mode, .header-button-section" }
    ViewEditMode.edit = { id: "Edit", cssToShow: ".edit-mode, .header-button-section" }
    ViewEditMode.add = { id: "Add", cssToShow: ".edit-mode" }

    var Url = {
        ReleaseDetails: '/Releases/Details/',
        ExistCheckListAnswers: '/Releases/ExistCheckListAnswers/',
        Footer: '/Releases/_ReleaseFooter/',
        ViewCheckList: '/CheckList/ViewCheckList/',
        ReleaseIndex: '/Releases',
        GetModifiedDetails: '/Releases/GetModifiedByandDate/',
        SaveRelease: $tabfrom.attr("action"),
    };
    var CurrentViewEditMode = ViewEditMode[Object.keys(ViewEditMode).find(function (a) {
        return a.toUpperCase() == Mode.toUpperCase()
    })];
    var CurrentReleaseID = ReleaseID;
    var CurrentIndex = $tabcontent.data("currentindex");
    // $(CurrentViewEditMode.cssToShow).show();

    /*General Events*/

    //fire on document.ready
    var loadTabs = function () {
        //init mode
        //$(CurrentViewEditMode.cssToShow).show();

        //tabs li
        $liTabs.click(navigateTav);
        //set current index click
        if (!CurrentIndex || CurrentIndex > $liTabs.size() || CurrentIndex < 0) CurrentIndex = 0;
        $liTabs.eq(CurrentIndex).find('a').click();

        //binding function
        //edit release button
        $("#header-btn-mode").click(changeMode);
        //Initiate Checklist button
        $("#InitiateBtn").click(initiate);
        //view Checklist button
        $("#VieCLBtn").click(setLocationViewCL);
        //next prev buttons
        $(".footer-button-nevigate").click(setNectPrevTabs);
        //save button 
        $("#details-save").click(function () { save(false); });
        //cancel button 
        $("#details-cancel").click(cancel);

        //ProductsInScope tab
        $ProductsInScope.find(".family-cb").change(SetFamilyProduct);
        $ProductsInScope.find(".add-custom-product").click(function () {
            $(this).hide();
            $(this).siblings('.custom-product').show();
            $(this).siblings('.custom-product').children(".product-txt").keyup(function () {
                if ($(this).val() == '')
                    $(this).siblings('.product-cb').prop('checked', false);
                else
                    $(this).siblings('.product-cb').prop('checked', true);
            });
        });
        GetDataProducts();

        //AreaOwners tab
        $AreaOwners.find(".area-cb").click(function () { SetAreaOwner(this) });
        $AreaOwners.find(".deleteIconAFP").click(function () { deleteAFP(this.parentNode); });

        //ReleaseStakeholders tab
        $ReleaseStakeholders.find(".deleteIcon").click(function () { deleteEmp(this.parentNode); });
        $ReleaseStakeholders.find("._addUser").click(function () { setShow(this); });
        $ReleaseStakeholders.find(".minusIcon").click(function () { setConfirm(this.parentNode, hideEmployee); });
        initShowAndHide();

        setDatePicker();
        //loadFooter();***********

    }
    //fire on Save or switch mode
    //unload tabs details
    var unloadTabs = function () {
        SetDataProducts();
    };


    var loadFooter = function () {
        $(".checkpoint-link").click(openCheckpointDetailsPopUp);
    };

    /*set Employee*/

    function setShow(e) {
        $("." + jq($(e).attr("id"))).show();
        $("#" + jq($(e).attr("id"))).hide();
    }

    function setConfirm(e, f) {
        return confirm("Are you sure you want to delete the employee?", warning, f, e);//hideEmployee
    }
    function hideEmployee(e) {
        $("." + jq($(e).attr("class"))).hide();
        $("." + jq($(e).attr("class"))).children(":first").attr("value", "");
        $(e).children('.hidden-employeeId').attr("value", null);
        $("#" + jq($(e).attr("class"))).show();
    }
    function deleteEmp(e) {
        $(e).children(':first').attr("value", "");
        $(e).children('.hidden-employeeId').attr("value", null);
    }
    function initShowAndHide() {
        $("[class*='IsAddUser']").each(function () {
            if (!($(this).children(":first").val()))
                $(this).hide();
            else {
                $(this).show();
                $("#" + jq($(this).attr("class"))).hide();
            }

        });
    }
    /*end set Employee*/

    //set Next Prev to Tabs
    var setNectPrevTabs = function () {
        var nextIndex = getIndexActive();
        if ($(this).attr("id") == "details-next") {
            nextIndex++;
            if (nextIndex == $liTabs.size()) nextIndex = 0;
        }
        else {
            nextIndex--;
            if (nextIndex == -1) nextIndex = $liTabs.size() - 1;
        }
        $liTabs.eq(nextIndex).find('a').click();
    }

    var setLocation = function (_View, _ReleaseID) {
        window.location.href = ROOT + Url.ReleaseDetails + _View + '/' + _ReleaseID + "?TabIndex=" + getIndexActive();
    }
    var setLocationViewCL = function () {
        window.location.href = ROOT + Url.ViewCheckList + CurrentReleaseID;
    }
    var setLocationReleaseIndex = function () {
        window.location.href = ROOT + Url.ReleaseIndex;
    }

    var getIndexActive = function () {
        return $tabwrapper.find('li.active').index();
    }

    var navigateTav = function () {
        return $($(this).find('a').attr('href')).data('IsUpdated', true);
    }

    var changeMode = function () {
        NextViewEditMode = CurrentViewEditMode == ViewEditMode.view ? ViewEditMode.edit : ViewEditMode.view;
        setLocation(NextViewEditMode.id, CurrentReleaseID);
    }

    var save = function (IsInitiated) {
        if (!IsInitiated) {
            var loadingSave = (JSON.parse(JSON.stringify(loading)));
            loadingSave.title = "Save Release";
            var alertSave = CustomConfirm("Saving Release, Please wait....  ", loadingSave);
        }
        unloadTabs();
        var action = Url.SaveRelease;
        var mode = CurrentViewEditMode;
        var data = $tabfrom.serializeArray();
        //add IsInitiated
        data.push({ name: 'IsInitiated', value: IsInitiated ? 'true' : 'false' });
        data.push({ name: 'ReleaseID', value: CurrentReleaseID });
        //add listUpdated
        $tabsbody.each(function (index) {
            data.push({
                name: 'listUpdated[' + index + '].key',
                value: $(this).attr('id'),
            });
            data.push({
                name: 'listUpdated[' + index + '].value',
                value: $(this).data('IsUpdated') || 'false',
            });
        });
        $.post(action, data)
            .done(function (result) {
                if (IsInitiated) {
                    initiateAlert.Close();
                    confirm("Release Checklist generated successfully", success, setLocationViewCL, null, null, null);
                }
                else {
                    alertSave.Close();
                    if (mode == ViewEditMode.add) {
                        //result=ReleaseID
                        if (result.count > 0) {
                            setLocation(ViewEditMode.edit.id, result.count);
                        }
                    }
                    if (result.count >= 0) {
                        //remove list updated
                        $tabsbody.each(function (index) {
                            if (!$(this).hasClass('active'))
                                $(this).data('IsUpdated', false);
                        });
                        //set ReleaseName at page
                        var ReleaseName = $GeneralDetails.find('[name="GeneralDetails.Name"]').val();
                        if (ReleaseName != "") $('.ReleaseName').text(ReleaseName);
                        //set AccountName at page  
                        var AccountName = $GeneralDetails.find('[name="GeneralDetails.AccountID"] :selected').text();
                        if (AccountName != "") $('.header .AccountName').text(AccountName);
                        var massage = "The data have been saved successfully";

                        alert(massage, success)
                    }
                    else alert("oops, error occurred in saving data process.", error);
                    $.post(Url.GetModifiedDetails, { releaseID: CurrentReleaseID }).done(function (result) {
                        $("#GeneralDetails_ModifiedBy").val(result.ModifiedBy);
                        $("#ModifiedDate").text(result.ModifiedDate);
                    });
                }



                $.get(Url.Footer, { ReleaseID: CurrentReleaseID }).done(function (res) {
                    $('#details-footer-wrapper').html(res);
                    loadFooter();
                });

            })
        .error(function (result, textStatus, jqXHR) {
            alertSave.Close();
            alert("oops, error occurred in saving data process.", error);
        });
    }

    //cancelReleaseChanges
    var cancel = function () {
        if (Mode.toUpperCase() == ViewEditMode.add.id.toUpperCase())
            setLocationReleaseIndex()
        else {
            view = ViewEditMode.view;
            setLocation(view.id, CurrentReleaseID);
        }
    }

    /*end General Events*/

    /*initiate */
    var initiate = function () {
        var initiateFlag = false;

        $.ajax({
            //contentType: 'application/json',
            type: "POST",
            url: Url.ExistCheckListAnswers,
            data: {
                "releaseID": ReleaseID
            },
            // dataType: "json",
            //  async: true,
            success: function (data) {
                if (data.Result == true)
                    CustomConfirm("Release checklist answers will be deleted, are you sure you want to delete your answers and fill the checklist again?",
                        information,
                        function () {
                            CustomConfirm("A new checklist is going to be initiated now, Are you sure you want to continue?", information, function () { GetInitiateQuestion(); });
                        });
                else GetInitiateQuestion();
                console.log("initiate - success!!!!", data);
            },
            error: function (data, textStatus, jqXHR) {
                console.log("initiate - Failed :(")
            }
        });
    }
    var GetInitiateQuestion = function () {
        initiateAlert = CustomConfirm("Initiating Checklist, Please wait....  ", loading);
        save(true);
    }
    /*end initiate */

    /*ProductsInScope Tab*/
    var SetFamilyProduct = function () {
        if (this.checked)
            $(this).parent().siblings('.products').css("display", "block");
        else {
            $(this).parent().siblings('.products').find('.product-cb').attr('checked', this.checked);
            $(this).parent().siblings('.products').css("display", "none");
        }
    }

    var SetDataProducts = function () {
        additionalProducts = '';
        $ProductsInScope.find('.custom-product').each(function (index) {
            customProduct = $ProductsInScope.find('.custom-product:eq(' + index + ')');
            family = customProduct.children('.custom-txt').data('family');
            txt = customProduct.children('.custom-txt').val();
            isChecked = customProduct.children('.custom-cb').prop('checked');
            if (txt != '')
                additionalProducts += family + ':' + txt + ':' + isChecked + ";";
        });
        $ProductsInScope.find('#addedProducts').val(additionalProducts);
    }

    var GetDataProducts = function () {
        if ($ProductsInScope.find('#addedProducts').val() != undefined) {
            var arr = $ProductsInScope.find('#addedProducts').val().split(";")
            var obj = {};
            $.each(arr, function (index, item) {
                itemArr = item.split(":");
                $ProductsInScope.find('.custom-txt[data-family="' + itemArr[0] + '"] ').parent().show();
                $ProductsInScope.find('.custom-txt[data-family="' + itemArr[0] + '"] ').val(itemArr[1]);
                $ProductsInScope.find('.custom-cb[data-family="' + itemArr[0] + '"]').prop("checked", itemArr[2] == "true" ? true : false);
                $ProductsInScope.find('.add-custom-product[data-family="' + itemArr[0] + '"] ').hide();

            });
        }
    }
    /*end ProductsInScope Tab*/

    /*AreaOwners Tab*/
    var SetAreaOwner = function (e) {
        if (e.checked)
            $(e).parent().siblings('.area-div').css("display", "block");
        else {
            $(e).attr('checked', 'checked');
            return confirm("The Area is about to be removed.", warning, function () {
                $(e).parent().siblings('.area-div').css("display", "none"); $(e).attr('checked', false);
            });
        }
    }

    //delete Amdocs FocalPoint
    //fire in ReleaseAreaOwners
    var deleteAFP = function (e) {
        $(e).children(':first').attr("value", null);
        $(e).children('.amdocs-emp').attr("value", "");
    }
    /*end AreaOwners Tab*/

    return loadTabs()
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
}

$(document).ready(function () {
    ReleaseDetails();
});





