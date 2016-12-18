var checklistObj = null;
var continueLeavehref = "";
/*-
  options Properties Constructors:
        options.Record.key,
        options.Record.RiskLevelID,
        options.Record.HandlingStartDate,
        options.Record.QuestionOwnerID,
        options.JsonRecords,
        options.SaveUrl

   for example:
        var checklistObj= new checklist(
        {
            JsonRecords: yourJsonRecords ,
            Record:{
                key:"ReleaseChecklistAnswerID",
                RiskLevelID: "RiskLevelID",
                HandlingStartDate: "HandlingStartDate",
                Comments: "Comments",
                AreaID:"AreaID",
                AreaName:"AreaName",
                SubAreaID:"SubAreaID",
                SubAreaName:"SubAreaName",
                Responsibility:"Responsibility",
                QuestionOwnerID: "QuestionOwnerID",
                QuestionOwnerName:"QuestionOwnerName",
                ActualComplation:"ActualComplation",
                LastAutomaticUpdateDate:"LastAutomaticUpdateDate",
                LastModifiedDate:"LastModifiedDate"
            },
            SaveUrl: "yourSaveUrl", //fire on Save
            ViewUrl: "yourViewUrl", //fire on canecl
            UserID: yourUserID,


        });

      Note:
           Sort Value Use JsonRecords Properties - Value.
*/

var checklistRecords = function (options) {
    options = options || [];
    options.Record = options.Record || [];

    var self = this;
    var countFilter = 0;
    var _checklistFilter = new checklistFilter({
        checklist: self,
    });

    /*getall prop*/
    //all record of current release
    this.records = options.JsonRecords;
    //recordsChanged 
    this.recordsChanged = [];
    this.SaveUrl = options.SaveUrl;
    //url to return on click cancel
    this.ViewUrl = options.ViewUrl;
    //current user
    this.UserID = options.UserID;
    //list of data about filter chosen of all tabs
    //contain Array of Object{:
    // TabType: liTab.data('tab-type') || "list",
    // RecordName: liTab.data('record-name'),
    // RecordType: liTab.data('record-type') || "string",
    // TabID: TabID,
    // ListChecked: [],
    // From: "",
    // To: "",
    // GroupArray: null,
    // IsFilterTab: false,
    // Loaded: false
    //}
    this.listFilter = [];
    //flag that hire where ther's Filter
    this.IsFilter = false;
    //flag if Filter Initial
    //false: on change viewFilter
    //true: on click on FilterButton if false..
    this.IsFilterInitial = false;
    this.CountNotValidate = 0;
    this.IsChangedRecord = false;
    //total records
    this.ArrayTotalByriskLevel = [];
    this.TotalRecords = 0;
    //get record fields that used in checklist
    this.record = [];
    this.record.key = options.Record.key || "ReleaseChecklistAnswerID";
    this.record.RiskLevelID = options.Record.RiskLevelID || "RiskLevelID";
    this.record.HandlingStartDate = options.Record.HandlingStartDate || "HandlingStartDate";
    this.record.QuestionOwnerID = options.Record.QuestionOwnerID || "QuestionOwnerID";
    this.record.Comments = options.Record.Comments || "Comments";
    this.record.AreaID = options.Record.AreaID || "AreaID";
    this.record.AreaName = options.Record.AreaName || "AreaName";
    this.record.AreaID = options.Record.AreaID || "AreaID";
    this.record.SubAreaID = options.Record.SubAreaID || "SubAreaID";
    this.record.SubAreaName = options.Record.SubAreaName || "SubAreaName";
    this.record.Responsibility = options.Record.Responsibility || "Responsibility";
    this.record.QuestionOwnerID = options.Record.QuestionOwnerID || "QuestionOwnerID";
    this.record.QuestionOwnerName = options.Record.QuestionOwnerName || "QuestionOwnerName";
    this.record.ActualComplation = options.Record.ActualComplation || "ActualComplation";
    this.record.LastAutomaticUpdateDate = options.Record.LastAutomaticUpdateDate || "LastAutomaticUpdateDate";
    this.record.LastModifiedDate = options.Record.LastModifiedDate || "LastModifiedDate";
    /*end getall prop*/


    /*all objects in page*/
    this.objects = [];
    this.objects.sort = $('.checklist ._sort_by #SortBy');
    this.objects.tbodytable = $(".checklist div#table ._repeating_rows_");
    this.objects.ChecklistFilter = $(".ChecklistFilter");
    //this.objects.rows = this.objects.tbodytable.find("[data-record-key]");
    this.objects.Save = $(".checklist ._transparent_btns#Save");
    this.objects.Cancel = $(".checklist ._transparent_btns#Cancel");
    this.objects.divTotal = $(".checklist #divTotal");
    /*end objects*/

    /*all enums in page*/
    this.enums = [];
    this.enums.RiskLevelID = {
        Initiated: 0,
        NA: 1,
        OnHold: 2,
        NoneClosed: 3,
        NoneAsPlanned: 4,
        Low: 5,
        Med: 6,
        High: 7,
        ShowStopper: 8
    };
    this.enums.RiskLevelIDNames = {
        Initiated: "Initiated",
        NA: "NA",
        OnHold: "On Hold",
        NoneClosed: "None (Closed)",
        NoneAsPlanned: "None (As Planned)",
        Low: "Low",
        Med: "Medium",
        High: "High",
        ShowStopper: "Show Stopper"
    };
    this.enums.Responsibility = {
        Amdocs: 0,
        Customer: 1,
    };
    this.enums.ChecklistFilter = {
        ALLQuestions: 0,
        MyPendingQuestions: 1,
        NewEffectiveQuestions: 2,
        ReinitiatedQuestions: 3,
        AutomaticChangedToLow: 4,
        EffectiveQuestions: 5
    };
    /*end enums*/

    /*private functions*/
    //get two records and sort by type

    //try parse string text to number 
    //if not number return 0
    _ParseToNumber = function (stringNumber) {
        return isNaN(stringNumber) ? 0 : parseFloat(stringNumber);
    }
    _isNumeric = function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }
    _sortRecordByType = function (record1, record2, type) {
        if (type == "date") {
            var x = DateObj.convertJsonToDate(record1);
            var y = DateObj.convertJsonToDate(record2);
            x = isNaN(x) ? 0 : x;
            y = isNaN(y) ? 0 : y;
            return x - y;
        }
        else if (type == "number" || _isNumeric(record1) && _isNumeric(record2)) {
            record1 = isNaN(record1) ? 0 : parseFloat(record1);
            record2 = isNaN(record2) ? 0 : parseFloat(record2);
            return record1 - record2;
        }
        else {
            record1 = record1 == null ? "" : record1;
            record2 = record2 == null ? "" : record2;
            return record1.toUpperCase().localeCompare(record2.toUpperCase());
        }

    }
    _SaveRecords = function (param) {
        var allrecords = param[0];
        var callbackdone = param[1];
        //var CountNotValidate = param[2];
        var action = self.SaveUrl;
        var loadingSave = (JSON.parse(JSON.stringify(loading)));
        loadingSave.title = "Save CheckList";
        var alertSave = CustomConfirm("Saving Records, Please wait....  ", loadingSave);
        // allrecords = allrecords.toArray();
        $.post(action, { 'Checklist': allrecords })
           .done(function (result) {
               alertSave.Close();
               if (result != -1) {
                   self.ArrayTotalByriskLevel = [];
                   self.TotalRecords = 0;
                   //hide all records that is validate
                   //this code not relevant when additional fields are read only
                   if (self.CountNotValidate == 0) {
                       self.functions.cleanQuestionsContainer();
                   }
                   else {
                       // reset changed record etc
                       //if done change IsChangedRecord to false;
                       //TODO exec anyway
                       self.functions.EmptyFilter();
                       self.ArrayTotalByriskLevel = [];
                       self.TotalRecords = 0;
                       $.each(self.records, function (index, record) {
                           record.IsFilter = false;
                           record.IsView = !record.IsValidate;
                           if (!record.IsValidate) {
                               debugger;
                               self.ArrayTotalByriskLevel[record[self.record.RiskLevelID]] = (self.ArrayTotalByriskLevel[record[self.record.RiskLevelID]] || 0) + 1;
                               self.TotalRecords++;
                           }
                       });
                       //    mz??? var ChecklistFilter = self.objects.ChecklistFilter.val();
                       self.functions.cleanQuestionsContainer();
                       self.functions.loadtopscroll();
                   }

                   if (callbackdone != null && callbackdone != undefined && callbackdone != -1) {
                       callbackdone(); return;
                   }
                   self.functions.setTotal();
                   //self.functions.loadtopscroll();
                   alert("Checklist saved successfully", success);
               }
               else {
                   if (callbackdone) {
                       callbackdone(); return;
                   }
                   alert("oops, error occurred in saving data process.", error);
               }
               $.each(self.recordsChanged, function (index, element) {
                   element.IsChangedRecord = false;
               });
               self.recordsChanged = [];
               //initiate IsChangedRecord to false
               self.IsChangedRecord = false;
           })
           .error(function (result) {
               alertSave.Close();
               if (callbackdone) {
                   callbackdone(); return;
               }
               alert("oops, error occurred in saving data process.", error);
           });
    };
    //sort table
    _sortTable = function (order, sortby, type) {
        var asc = order === 'asc';
        //self.records.filter(self.functions.isViewFilter);
        if (type == "enum") {
            self.records.sort(function (a, b) {
                var record1 = _getEnumByValue(sortby, a[sortby]);
                var record2 = _getEnumByValue(sortby, b[sortby]);
                return _sortRecordByType(record1, record2, "string");
            });
        }
        else
            self.records.sort(function (a, b) {
                var record1 = a[sortby];
                var record2 = b[sortby];
                return _sortRecordByType(record1, record2, type);
            });
    };

    _filterRowsAndSort = function () {
        self1 = self;
        $.each(self.records, function (index, record) {
            self1.functions.ValidateRecord(record);

        });
        self.functions.sortRecords();
        self.functions.filterRecords();
    }
    _getEnumByValue = function (key, val) {
        var enumobj = self.enums[key];
        var keyname = Object.keys(enumobj)[val];
        if (self.enums[key + "Names"])
            keyname = self.enums[key + "Names"][keyname];
        return keyname;
    }
    //group array by key and sort it  
    _GroupBy = function (data, key, type) {
        return data.reduce(function (res, obj) {
            if (obj[key] || obj[key] == "0")
                if (!(obj[key] in res)) {
                    var Text = "";
                    if (type == "enum")
                        Text = _getEnumByValue(key, obj[key]);
                    else Text = obj[key];
                    res.__array.push(res[obj[key]] = {
                        Text: Text,
                        Value: obj[key],
                        Count: 1,
                        IsChecked: false
                    });
                }
                else {
                    res[obj[key]].Count++;
                }
            return res;
        }, {
            __array: []
        })
            .__array
            .sort(function (a, b) {
                return _sortRecordByType(a.Text, b.Text, type);
            });
    }
    /*end private functions*/


    /*public functions*/
    this.functions = []

    //load recrds to trs fire on load page
    this.functions.loadRecords = function () {
        /*events*/
        //on change Views input
        self.objects.ChecklistFilter.change(function () {
            if (self.IsFilter)
                confirm("Please note that Filter selection will be removed, would you like to continue?", warning, self.functions.filterRecords, true);
            else
                self.functions.filterRecords(true);
            return;
        });
        //on change sort input
        self.objects.sort.change(function () {
            event.preventDefault();
            self.functions.cleanQuestionsContainer();
            self.functions.sortRecords();
            self.functions.loadtopscroll();
            return;
        });
        self.objects.Save.click(function () {
            self.functions.SaveRecords(null);
        });
        self.objects.Cancel.click(function () {
            window.location.href = ROOT + self.ViewUrl;
        });
        /*end events*/

        _filterRowsAndSort();
    };
    //return all records that IsView
    this.functions.loadRecordsIsView = function () {
        return $.grep($(self.records), function (record) {
            return record.IsView;
        });
    };
    //group array by key and sort it  
    this.functions.GroupArray = function (data, key, type) {
        return _GroupBy(data, key, type);
    };
    //sort table by sortobj
    this.functions.sortRecords = function () {
        var sortby = self.objects.sort.val();
        type = $('option:selected', self.objects.sort).attr('type') || "string";
        //get sort by and sort all rows
        _sortTable('asc', sortby, type);
    };
    //call private sort record
    this.functions.sortRecordByType = function (record1, record2, type) {
        return _sortRecordByType(record1, record2, type);
    }
    //filter all records by all conditions
    //fire on change view or filter Save
    this.functions.filterRecords = function (SwEmptyFilter) {
        $('#loading').removeClass('hide');
        //empty filter values on change View 
        if (SwEmptyFilter) self.functions.EmptyFilter();
        self.ArrayTotalByriskLevel = [];
        self.TotalRecords = 0;
        var ChecklistFilter = self.objects.ChecklistFilter.val();
        self.functions.cleanQuestionsContainer();
        $.each(self.records, function (index, record) {
            var NotShow = record.IsFilter == false && self.IsFilter == true;
            var IsView = false;
            switch (parseInt(ChecklistFilter)) {
                case self.enums.ChecklistFilter.ALLQuestions:
                    IsView = true;
                    break;

                case self.enums.ChecklistFilter.MyPendingQuestions:
                    var RiskLevelID = parseInt(record[self.record.RiskLevelID]);
                    var HandlingStartDate = record[self.record.HandlingStartDate];
                    var QuestionOwnerID = record[self.record.QuestionOwnerID];
                    IsView = (QuestionOwnerID != null &&
                                    QuestionOwnerID == self.UserID &&
                                    HandlingStartDate != null &&
                                    DateObj.convertJsonToDate(HandlingStartDate) < new Date().getTime() &&
                                    [self.enums.RiskLevelID.High, self.enums.RiskLevelID.ShowStopper, self.enums.RiskLevelID.Med, self.enums.RiskLevelID.Low].indexOf(RiskLevelID) > -1);
                    break;

                case self.enums.ChecklistFilter.NewEffectiveQuestions:
                    var RiskLevelID = parseInt(record[self.record.RiskLevelID]);
                    var LastAutomaticUpdateDate = record[self.record.LastAutomaticUpdateDate];
                    var LastModifiedDate = record[self.record.LastModifiedDate];
                    IsView = ((LastAutomaticUpdateDate == LastModifiedDate) && (RiskLevelID == self.enums.RiskLevelID.High) && (LastAutomaticUpdateDate != null));
                    break;
                case self.enums.ChecklistFilter.ReinitiatedQuestions:
                    var RiskLevelID = parseInt(record[self.record.RiskLevelID]);
                    var LastAutomaticUpdateDate = record[self.record.LastAutomaticUpdateDate];
                    var LastModifiedDate = record[self.record.LastModifiedDate];
                    IsView = ((LastAutomaticUpdateDate == LastModifiedDate) && (RiskLevelID == self.enums.RiskLevelID.Initiated) && (LastAutomaticUpdateDate != null));
                    break;
                case self.enums.ChecklistFilter.AutomaticChangedToLow:
                    var RiskLevelID = parseInt(record[self.record.RiskLevelID]);
                    var LastAutomaticUpdateDate = record[self.record.LastAutomaticUpdateDate];
                    var LastModifiedDate = record[self.record.LastModifiedDate];
                    IsView = ((LastAutomaticUpdateDate == LastModifiedDate) && (RiskLevelID == self.enums.RiskLevelID.Low) && (LastAutomaticUpdateDate != null));
                    break;
                case self.enums.ChecklistFilter.EffectiveQuestions:
                    var RiskLevelID = parseInt(record[self.record.RiskLevelID]);
                    var HandlingStartDate = record[self.record.HandlingStartDate];
                    IsView = (DateObj.convertJsonToDate(HandlingStartDate) <= new Date().getTime() &&
                        [self.enums.RiskLevelID.ShowStopper, self.enums.RiskLevelID.High, self.enums.RiskLevelID.Med,
                            self.enums.RiskLevelID.NoneClosed, self.enums.RiskLevelID.NoneAsPlanned].indexOf(RiskLevelID) > -1
                    );
                    break;
            }
            if (IsView) {
                if (!NotShow) {
                    self.ArrayTotalByriskLevel[record[self.record.RiskLevelID]] = (self.ArrayTotalByriskLevel[record[self.record.RiskLevelID]] || 0) + 1;
                    self.TotalRecords++;
                    record.IsView = true;
                }
            }
            else
                record.IsView = false;
            record.IsAppended = false;
        });
        self.functions.setTotal();

        self.functions.loadtopscroll();
        $('#loading').addClass('hide');
    };
    this.functions.setTotal = function () {
        var divTotal = self.objects.divTotal;
        var other = 0;
        var Total = self.TotalRecords;
        var ArrayTotalByriskLevel = self.ArrayTotalByriskLevel;
        (divTotal.find('#total').text(Total || 0)).text();
        other += parseInt((divTotal.find('#showStopper').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.ShowStopper] || 0)).text());
        other += parseInt((divTotal.find('#high').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.High] || 0)).text());
        other += parseInt((divTotal.find('#medium').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.Med] || 0)).text());
        other += parseInt((divTotal.find('#low').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.Low] || 0)).text());
        other += parseInt((divTotal.find('#noneAsPlanned').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.NoneAsPlanned] || 0)).text());
        other += parseInt((divTotal.find('#NoneClosed').text(ArrayTotalByriskLevel[self.enums.RiskLevelID.NoneClosed] || 0)).text());
        divTotal.find('#other').text(parseInt(Total - other));
    }
    this.functions.ChangeTotalByriskLevel = function (oldRiskLevel, newRiskLevel) {
        self.ArrayTotalByriskLevel[oldRiskLevel]--;
        self.ArrayTotalByriskLevel[newRiskLevel] = (self.ArrayTotalByriskLevel[newRiskLevel] || 0) + 1;
        self.functions.setTotal();

    }


    this.functions.loadtopscroll = function () {
        var body = self.objects.tbodytable;
        body.unbind('scroll');
        $.each(self.records, function (index, record) {
            record.IsAppended = false;
        });
        rowCount = self.functions.showTopToFilter()
        if (rowCount > 0) {
            body.scroll(function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight - 200) {
                    rowCount = self.functions.showTopToFilter();
                    if (rowCount <= 0) body.unbind('scroll');
                } else {
                    return false;
                }
            }
            );
        }
    };
 this.functions.cleanQuestionsContainer = function () {
    $(".questions-container").empty();
};

this.functions.showTopToFilter = function () {
    var recordsToshow = self.records.filter(self.functions.isViewFilter) || [];
    unbindQuestionItemEvent();
    $.each(recordsToshow.slice(0, 30), function (index, item) {
        AppendRecord(item);
        item.IsAppended = true;
    });
    bindQuestionItemEvent();
    var rowCount = recordsToshow.length - 30;
    return rowCount;
};

this.functions.isViewFilter = function (record) {
    return (!self.IsFilter || record.IsFilter) && record.IsView && !record.IsAppended;
}

//EmptyFilter ,fire on change ViewFilter
this.functions.EmptyFilter = function () {
    self.IsFilterInitial = false;
    self.listFilter = [];
    self.IsFilter = false;
    _checklistFilter.functions.setFilterIcon(self.IsFilter);
}
//fire on change record values, get record object changed and tab of record
//set all new values at record
this.functions.changedRecord = function (recordChanged) {
    var record;
    $.each(self.records, function (index, item) {
        if (item[self.record.key] == recordChanged.recordKey)
            record = item;
    });
    self.IsChangedRecord = true;
    record.IsChangedRecord = true;
    if (record[self.record.RiskLevelID] != recordChanged.RiskLevelID)
        self.functions.ChangeTotalByriskLevel(record[self.record.RiskLevelID], recordChanged.RiskLevelID);

    record[self.record.HandlingStartDate] = recordChanged.HandlingStartDate;
    record[self.record.Comments] = recordChanged.Comments;
    //record[self.record.AreaID] = recordChanged.AreaID;
    //record[self.record.AreaName] = recordChanged.AreaName
    //record[self.record.SubAreaID] = recordChanged.SubAreaID;
    //record[self.record.SubAreaName] = recordChanged.SubAreaName;
    record[self.record.Responsibility] = recordChanged.Responsibility;
    //record[self.record.QuestionOwnerID] = recordChanged.QuestionOwnerID;
    //record[self.record.QuestionOwnerName] = recordChanged.QuestionOwnerName;
    record[self.record.ActualComplation] = parseInt(recordChanged.ActualComplation);
    record[self.record.RiskLevelID] = parseInt(recordChanged.RiskLevelID);
};
this.functions.ChangeValidationState = function (record, boolState) {
    if ((record.IsValidate == undefined || record.IsValidate == true) && boolState == false)
        self.CountNotValidate++;
    else if (record.IsValidate == false && boolState == true)
        self.CountNotValidate--;
    record.IsValidate = boolState;
};
//get record and find if it is validate 
//and set record.IsValidate 
//fire on click save
this.functions.ValidateRecord = function (record) {
    var risklevelenum = self.enums.RiskLevelID;
    var boolState = true;
    // record.IsValidate = true;
    if (
        //!record[self.record.SubAreaID] ||
        !record[self.record.HandlingStartDate] ||
        !record[self.record.ActualComplation] && record[self.record.ActualComplation] != "0" ||
        !record[self.record.RiskLevelID] && record[self.record.RiskLevelID] != "0"
        //||!record[self.record.Responsibility] && record[self.record.Responsibility] != "0"
        //|| !record[self.record.QuestionOwnerID]
        )
        boolState = false;
    else if (record[self.record.ActualComplation] == 100 && (
        record[self.record.RiskLevelID] != risklevelenum.NoneClosed &&
        record[self.record.RiskLevelID] != risklevelenum.NA &&
        record[self.record.RiskLevelID] != risklevelenum.OnHold
    ))
        boolState = false;
    else if (record[self.record.ActualComplation] != 100 && record[self.record.RiskLevelID] == risklevelenum.NoneClosed) {
        boolState = false;
    }
    self.functions.ChangeValidationState(record, boolState);
};

//saveRecords
this.functions.SaveRecords = function (callbackdone) {
    //return records to db
    var action = self.SaveUrl;
    var allrecords = [];
    self.recordsChanged = [];
    //var CountValidate = 0;
    var CountView = 0;
    //  allrecords = self.functions.GetAllRecords();
    $.each(self.records, function (index, element) {
        if (element.IsView) {
            CountView++;
            if (element.IsChangedRecord) {
                self.functions.ValidateRecord(element);
                //if (element.IsValidate) CountValidate++;
                //else CountNotValidate++;
                if (element.IsChangedRecord) {
                    self.recordsChanged.push(element);
                    if (element.IsValidate) {
                        //copy record to new record
                        var record = $.extend(true, {
                        }, $(element))[0];
                        //update date to date recognise by dot net
                        record[self.record.HandlingStartDate] = DateObj.convertJsonToString(record[self.record.HandlingStartDate]);
                        record[self.record.LastAutomaticUpdateDate] = DateObj.convertJsonToString(record[self.record.LastAutomaticUpdateDate]);
                        allrecords.push(record);
                    }
                }
            }
        }
    });

    //if there is no records in this view not need do anything
    if (CountView == 0) return;
    if (callbackdone)
        _SaveRecords([allrecords, callbackdone]);

    else
        // all records not validate
        if (self.TotalRecords - self.CountNotValidate == 0) {
            alert("Please verify that all mandatory fields are filled and that there is no mismatch between Actual Completion & Risk Level", warning);
        }//part of records not validate
        else if (self.CountNotValidate > 0) {
            confirm("Only part of your answers were saved - please review invalid answers, verify that all mandatory fields are filled and that there is no mismatch between Actual Completion & Risk Level",
                warning, _SaveRecords, [allrecords]);
        }
        else {
            //all records validate
            _SaveRecords([allrecords]);
        }

};
/*end public functions*/


//load all records in tr and sort and view them
self.functions.loadRecords();

};
//object of Filter functions and flags
var checklistFilter = function (options) {
    options = options || [];
    options.checklistRecords = options.checklist || [];
    var self = this;

    var _checklistRecords = options.checklist;
    /*getall prop*/
    //all recoreds Is In View 
    this.records = [];
    //current TabId Active
    this.TabID = null;

    //temp list of filter 
    //{TabType,ListChecked, RecordName ,RecordType,TabID}
    this.temp_listFilter = [];

    this.searchTabName = 'input._search';
    this.bodyTabName = '#body';
    this.btnFromTabName = '#btnFrom';
    this.btnToTabName = '#btnTo';
    /*end getall prop*/


    /*private functions*/
    _addOptionsToSelect = function (array, selectObj, selectedVal, IsOptional) {
        selectObj.html('');

        if (IsOptional) {
            sSelected = selectedVal == '' ? 'selected' : '';
            selectObj.append($('<option ' + sSelected + '></option>').val('').html(''));
        }
        $.each(array, function (index, element) {
            sSelected = selectedVal == element.Value.toString() ? 'selected' : '';
            selectObj.append($('<option ' + sSelected + '></option>').val(element.Value.toString()).html(element.Text))
        });
    }
    /*end private functions*/

    /*all objects in page*/
    this.objects = []
    //filter button
    this.objects.FilterButton = $(".checklist #Filter");
    this.objects.FilterButtonIconV = $(".checklist #Filter ._filterV");
    //FilterModal
    this.objects.FilterModal = $('#FilterModal');
    //li tabs
    this.objects.UlTabs = self.objects.FilterModal.find('ul.nav-tabs');
    //Save button
    this.objects.SelectAll = self.objects.FilterModal.find('.tab-pane .SelectAll');
    this.objects.ClearAll = self.objects.FilterModal.find('.tab-pane .ClearAll');
    this.objects.Search = self.objects.FilterModal.find(self.searchTabName);

    this.objects.Save = self.objects.FilterModal.find(':button._transparent_btns#Save');
    this.objects.Cancel = self.objects.FilterModal.find(':button._transparent_btns#Cancel');
    this.objects.ClearAllFilters = self.objects.FilterModal.find(':button._transparent_btns#ClearAll');
    /*end all objects in page*/


    /*public functions*/
    this.functions = [];
    //return List of Filtered items 
    //params
    //TabID: if TabID!=null not check on that TabID
    this.functions.getRecordsByFilter = function (TabID) {
        return $.grep(self.records, function (record) {
            var isMapped = true;

            $.each(self.temp_listFilter, function (index, obj) {
                if (obj.TabID != TabID) {
                    if (obj.TabType == "list") {
                        if (obj.ListChecked.length > 0) {
                            //if not in list filter it
                            if ($.inArray(record[obj.RecordName], obj.ListChecked) == -1) {
                                isMapped = false;
                                return;
                            }
                        }
                    }
                    else if (obj.TabType == "range") {
                        if (obj.From != "" && _checklistRecords.functions.sortRecordByType(record[obj.RecordName], obj.From, obj.RecordType) < 0) {
                            isMapped = false;
                            return;
                        }
                        if (obj.To != "" && _checklistRecords.functions.sortRecordByType(record[obj.RecordName], obj.To, obj.RecordType) > 0) {
                            isMapped = false;
                            return;
                        }
                    }
                }

            });
            return isMapped;

        });
    };

    this.functions.loadTab = function (TabID) {
        if (self.TabID == TabID) return;
        if (self.TabID != null) {
            self.functions.unloadTab(self.TabID);
        }

        self.TabID = TabID;

        //initial current tab checked
        var Tab = self.functions.getCurrentTab(TabID, true);
        //if not changed filter not need to load again
        if (Tab.Loaded) return;
        //empty search input of current tab
        self.objects.FilterModal.find(TabID + self.searchTabName).val('');

        //add to body the inputs
        var body = self.objects.FilterModal.find(TabID + ' ' + self.bodyTabName);

        //if (!Tab.GroupArray)
        self.functions.setGroupArrayToTab(Tab);

        if (Tab.TabType == "list") {
            body.html('');
            var ListCurrentTabChecked = Tab.ListChecked;
            $.each(Tab.GroupArray, function (index, element) {
                var isChecked = $.inArray(element.Value, ListCurrentTabChecked) > -1 ? 'checked' : '';

                var div = $('<div class="input-group width-100"></div>');
                var outerdiv = $('<div class="form-group tr_checkbox" ></div>');
                var input = $('<input type="checkbox" class="pull-left _checkbox ' + Tab.RecordName + '" id="' + Tab.RecordName + index + '" name="' + Tab.RecordName + index + '" ' + isChecked + ' >');
                var label = $('<label class="pull-left _lable_checkbox width-95" for="' + Tab.RecordName + index + '">' + element.Text + '</label>').data('value', element.Value);

                input
                    .click(self.functions.checboxClick)
                    .appendTo(div);
                label.appendTo(div);
                div.appendTo(outerdiv);
                outerdiv.appendTo(body);
            });
        }
        else if (Tab.TabType == "range") {
            var btnFrom = body.find(self.btnFromTabName);
            var btnTo = body.find(self.btnToTabName);
            if (Tab.RecordType == "date") {
                btnFrom.val('');
                btnTo.val('');
                //if there is no data to filter empty date inputs and disabled them
                if (Tab.GroupArray.length == 0) {
                    btnFrom.prop('disabled', true);
                    btnTo.prop('disabled', true);
                }
                else {
                    btnFrom.prop('disabled', false);
                    btnTo.prop('disabled', false);
                    /*set start date and end date to inputs
                     * check if date chosen in the  range if not empty it 
                     */
                    var minDate = new Date(DateObj.convertJsonToDate($(Tab.GroupArray).first()[0].Text));
                    var maxDate = new Date(DateObj.convertJsonToDate($(Tab.GroupArray).last()[0].Text));

                    self.functions.setdatetimepicker(btnFrom, Tab.From, minDate, maxDate);
                    self.functions.setdatetimepicker(btnTo, Tab.To, minDate, maxDate);
                }
            }
            else {
                _addOptionsToSelect(Tab.GroupArray, btnFrom, Tab.From || '', true);
                _addOptionsToSelect(Tab.GroupArray, btnTo, Tab.To || '', true);
            }

        }
        Tab.Loaded = true;
    };
    //on unload get list of checked||range and put in temp_listTab 
    this.functions.unloadTab = function (TabID) {
        var Tab = self.functions.getCurrentTab(TabID);
        var body = self.objects.FilterModal.find(TabID + ' ' + self.bodyTabName);
        var IsChangedFilter = false;
        if (Tab.TabType == "list") {
            //get list of checked
            var ListChecked = body.find('input[type="checkbox"]:checked+label').map(function () {
                return $(this).data('value');
            }).toArray();
            IsChangedFilter = !($(ListChecked).not(Tab.ListChecked).length === 0 && $(Tab.ListChecked).not(ListChecked).length === 0);
            Tab.ListChecked = ListChecked;
        }
        else if (Tab.TabType == "range") {
            //set From and To in Tab
            if (Tab.RecordType == "date") {
                var From = DateObj.getdatetimepickerJson(body.find(self.btnFromTabName)) || "";
                var To = DateObj.getdatetimepickerJson(body.find(self.btnToTabName)) || "";

                IsChangedFilter = From != Tab.From || To != Tab.To;
                Tab.From = From;
                Tab.To = To;
            }
            else {
                var From = body.find(self.btnFromTabName).val();
                var To = body.find(self.btnToTabName).val();
                IsChangedFilter = From != Tab.From || To != Tab.To;
                Tab.From = From;
                Tab.To = To;
            }
        }
        //if changed set Loaded temp_listFilter to false
        if (IsChangedFilter)
            $.each(self.temp_listFilter, function (index, obj) {
                obj.Loaded = false;
            });
        //set IsFilterTab and filter icon if is Filterd
        self.functions.setFilterTab(Tab);

    };
    //fire on clear all tabs - Clear temp_listFilter
    this.functions.Clear_temp_listFilter = function () {
        var TabID = self.functions.getCurrentTabID();
        //clear the list
        $.each($(self.temp_listFilter), function (index, Tab) {
            self.functions.setFilterTabClass(Tab.TabID, false);
        });
        self.temp_listFilter = [];
        self.TabID = null;
        self.functions.loadTab(TabID);
    };
    /* get tabId and return Current Tab from temp_listFilter 
      * if currenTab not exists create new Tab In List and return it*/
    this.functions.getCurrentTab = function (TabID, CrateIfNotExist) {
        var CrateIfNotExist = typeof CrateIfNotExist !== 'undefined' ? CrateIfNotExist : true;
        var Tab = self.temp_listFilter.find(function (tab) {
            return tab.TabID == TabID
        });
        if (!Tab) {
            var liTab = self.objects.UlTabs.find('li a[href=' + TabID + ']');
            Tab = {
                TabType: liTab.data('tab-type') || "list",
                RecordName: liTab.data('record-name'),
                RecordType: liTab.data('record-type') || "string",
                TabID: TabID,
                ListChecked: [],
                From: "",
                To: "",
                GroupArray: null,
                IsFilterTab: false,
                Loaded: false
            };
            if (CrateIfNotExist) self.temp_listFilter.push(Tab);
        }
        return Tab;
    }
    this.functions.getCurrentTabID = function () {
        return self.objects.UlTabs.find('li.active a').attr("href");
    }
    this.functions.getCurrentTabActive = function () {
        return self.objects.UlTabs.find('.tab-pane.active');
    }
    /*set GroupArray of recordsMapped to currentTab 
     *fire on load tab
     * and set ListChecked if exists in List Array */
    this.functions.setGroupArrayToTab = function (Tab) {
        Tab.recordsMapped = self.functions.getRecordsByFilter(Tab.TabID);
        Tab.GroupArray = _checklistRecords.functions.GroupArray(Tab.recordsMapped, Tab.RecordName, Tab.RecordType);
        Tab.ListChecked = $.grep(Tab.ListChecked, function () {
            return $.inArray($(this), Tab.GroupArray);
        });
    };
    //set  Tab.IsFilterTab and icon of tab
    //fire on unload
    this.functions.setFilterTab = function (Tab) {
        if (Tab.TabType == "list")
            Tab.IsFilterTab = Tab.ListChecked.length > 0;
        else if (Tab.TabType == "range") Tab.IsFilterTab = (Tab.From || Tab.To) ? true : false;
        self.functions.setFilterTabClass(Tab.TabID, Tab.IsFilterTab);
    }
    //set filter icon if is Filterd
    this.functions.setFilterTabClass = function (TabID, IsFilterTab) {
        var obj = self.objects.UlTabs.find('li a[href=' + TabID + ']');
        if (IsFilterTab)
            obj.addClass('filter');
        else obj.removeClass('filter');
    }
    //set filterButton V (out) on save changed
    this.functions.setFilterIcon = function (IsFilter) {
        if (IsFilter)
            self.objects.FilterButtonIconV.addClass('active')
        else
            self.objects.FilterButtonIconV.removeClass('active')
    }

    this.functions.setdatetimepicker = function ($input, val, minDate, maxDate) {
        $input
            .datetimepicker("setStartDate", minDate)
            .datetimepicker("setEndDate", maxDate);
        if (val != "") {
            var date = new Date(DateObj.convertJsonToDate(val));
            if (DateObj.checkDateInRange(date, minDate, maxDate)) $input.datetimepicker("setDate", date);
        }
    };
    this.functions.checboxClick = function () {
        var tab = $(this).closest('.tab-pane');
        self.functions.setFilterTabClass('#' + tab.attr('id'), tab.find('#body').find('input[type="checkbox"]:checked:first').length > 0)
    };
    this.functions.inputRangeChange = function () {
        var tab = $(this).closest('.tab-pane');
        btnFrom = tab.find('#body').find('#btnFrom').val();
        btnTo = tab.find('#body').find('#btnTo').val();
        self.functions.setFilterTabClass('#' + tab.attr('id'), btnFrom || btnTo);
    };
    this.functions.closeModal = function () {
        self.objects.FilterModal.modal("hide");
    };
    /*end public functions*/


    /*Events*/
    self.objects.FilterButton.click(function () {
        //copy all list to temp checked
        self.temp_listFilter = _checklistRecords.listFilter.slice();
        //load all records that in View 
        if (!_checklistRecords.IsFilterInitial) {
            self.records = _checklistRecords.functions.loadRecordsIsView();
            //run all tabs and add class filter if IsFilterTab=true
            self.objects.UlTabs.find('li a').each(function () {
                TabID = $(this).attr("href");
                self.functions.setFilterTabClass(TabID, false);
            });
            _checklistRecords.IsFilterInitial = true;
        }
        //run all tabs and add class filter if IsFilterTab=true
        self.objects.UlTabs.find('li a').each(function () {
            TabID = $(this).attr("href");
            Tab = self.functions.getCurrentTab(TabID, false);
            self.functions.setFilterTabClass(Tab.TabID, Tab.IsFilterTab);
        });

        //initial current TabID to null
        self.TabID = null;
        //get current TabID active
        var TabID = self.functions.getCurrentTabID();
        //load current Tab active
        self.functions.loadTab(TabID);
    });
    self.objects.UlTabs.find('li').click(function () {
        self.functions.loadTab($(this).find("a").attr("href"));
    });
    self.objects.Save.click(function () {
        //get current TabID active
        var TabID = self.functions.getCurrentTabID();
        //unload last TabID - to get checked list
        self.functions.unloadTab(TabID);
        //set recordsMapped by checked params To all params
        var recordsMapped = self.functions.getRecordsByFilter(null);
        //run on all records and set IsFilter true or false
        var recordkey = _checklistRecords.record.key;
        //get array keys of mapped records 
        var ArrayKey = recordsMapped.map(function (record) {
            return record[recordkey];
        });
        //run all records and find if it in the array set IsFilter=true
        $.each(_checklistRecords.records, function (index, element) {
            element.IsFilter = $.inArray(element[recordkey], ArrayKey) != -1;
        });
        //save listFilter changes
        _checklistRecords.listFilter = self.temp_listFilter.slice();

        //set isFilter obj
        _checklistRecords.IsFilter = false;
        $.each($(_checklistRecords.listFilter), function (index, Tab) {
            if (Tab.IsFilterTab == true) {
                _checklistRecords.IsFilter = true;
                return;
            }
        });
        self.functions.setFilterIcon(_checklistRecords.IsFilter);
        //filter rows 
        _checklistRecords.functions.filterRecords();

        self.functions.closeModal();
    });
    self.objects.Cancel.click(self.functions.closeModal);
    self.objects.ClearAllFilters.click(self.functions.Clear_temp_listFilter);
    $('[id=btnFrom]').change(self.functions.inputRangeChange);
    $('[id=btnTo]').change(self.functions.inputRangeChange);
    self.objects.Search.keyup(function () {
        var searchText = $(this).val();
        var bodyTr = $(this)
            .closest('.tab-pane').find('#body');
        bodyTr.find('.tr_checkbox').addClass('hide');
        bodyTr.find('input[type="checkbox"]+label:contains(' + searchText + ')').parents('.tr_checkbox').removeClass('hide');
    });
    self.objects.SelectAll.click(function () {
        var tab = $(this).closest('.tab-pane');
        //select all checkbox not hide
        tab.find('#body .tr_checkbox:not(.hide) input[type="checkbox"]').prop('checked', true);
        self.functions.setFilterTabClass('#' + tab.attr('id'), tab.find('#body input[type="checkbox"]:checked:first').length > 0);
        //self.functions.closeModal();
    });
    self.objects.ClearAll.click(function () {
        var tab = $(this).closest('.tab-pane');
        var body = tab.find('#body');
        var liTab = self.objects.UlTabs.find('li a[href=#' + tab.attr('id') + ']');
        var tabtype = liTab.data('tab-type') || "list";
        //var tabtype = liTab.data('tab-type') || "list";
        if (tabtype == "list") {
            //clear all checkbox not hide
            body.find('.tr_checkbox:not(.hide) input[type="checkbox"]:checked').prop('checked', false);
            self.functions.setFilterTabClass('#' + tab.attr('id'), body.find('input[type="checkbox"]:checked:first').length > 0);
        }
        else {
            body.find('input').val('');
            body.find('select').val('');
            self.functions.setFilterTabClass('#' + tab.attr('id'), false);
        }
    });
    bindDateTimePicker(self.objects.FilterModal.find('._date'));
    /*end Events*/


}






var BindPopoverDate = function () {
    $(this).closest("._date-calender").find(".HandlingStartDatePopoverInput").val($(this).val());
}



//var setPrevRiskLevelVal = function () {

//    prevRiskLevelVal = $(this).val();
//}

var viewLog = function () {
    if (this)
        releaseChecklistAnswer = $(this).data("releasechecklistanswer");

    thisObj = $(this);
    $.ajax({ //Not found in cache, get from server
        url: "/CheckList/GetActivityLogByReleaseCheckListId",
        data: {
            "releaseChecklistAnswer": releaseChecklistAnswer
        },
        type: 'POST',
        async: true,
        success: function (data) {
            if (data == "") {
                data = "No item to show"
            }

            thisObj.attr('data-content', data)
            console.log(data);

            //    thisObj.trigger('click');

            // thisObj.popover();
            thisObj.popover('show');
        }
    });
}

var setActualComplation = function () {
    setActualComplationUI($(this));
    setActualComplationBind($(this));
    savechanges(this);
    //get current and previous data
}

var setActualComplationBind = function (elem) {
    currActualComplationVal = elem.text();
    prevRiskLevelVal = elem.val();
    //  prevActualComplationVal = elem.closest("._width_risk_level").find("#actualComplation").val();
    //perform the change

    // Actual Completion <> Risk Level Behavior
    var ddl = elem.closest("._width_risk_level").children("select");
    if (elem.text() == "100") {
        // ddl.find(":selected").attr("selected", false)
        ddl.val("3");//children("[value=3]").attr("selected", "selected")
        //setRiskLevelColor(ddl);
    }
    else
        if (prevActualComplationVal == "100" && currActualComplationVal != "100") {
            //ddl.find(":selected").attr("selected", false)
            //ddl.children("[value=7]").attr("selected", "selected")
            ddl.val("7");
            // setRiskLevelColor(ddl);
            alert("Please note that Risk level Changed to High", information)
        }
    setRiskLevelUI(ddl);
    setHandlingDateBind(elem);
    ddl.data("selected", ddl.val());
}

var setActualComplationUI = function (elem) {
    prevActualComplationVal = elem.closest("._width_risk_level").find("#actualComplation").val();
    elem.closest(".btn-group").children(".selected-btn").removeClass("selected-btn");
    elem.addClass("selected-btn");
    elem.closest("._width_risk_level").children("#actualComplation").val((elem.text()));
}

var addCommentTitle = function () {
    $(event.target).closest("._comment").find(".comment-input").removeAttr("disabled");
    var displayDate = moment(new Date()).format("D-MMM-YYYY");
    var userName = $("#userName").val();
    var textarea = $(this).closest("._comment").children("textarea");
    var text = userName + " " + displayDate + ":"
    var isFirstComment = textarea.val() == ""
    text += isFirstComment ? "" : "\n\n";
    textarea.val(text + textarea.val());
    textarea.focus().setCursorPosition(isFirstComment ? text.length : text.length - 1);
    textarea.change();
}


//var getSubAreaList = function () {
//    var ddl = $(this).closest(".hide-row").find(".ddlSubArea");
//    $.ajax({
//        url: "/CheckList/getSubAreaListByAreaID",
//        data: { "areaId": $(this).find(":selected").val() },
//        type: 'POST',
//        dataType: 'json',
//        async: true,
//        success: function (data) {
//            ddl.children().remove();
//            $.each(data, function (idx, obj) {
//                ddl.append($("<option></option>").attr("value", obj.SubAreaID).text(obj.Name));
//            })
//            ddl.attr("data-isUpdated", "true")
//        },
//        failure: function () {
//            ddl.attr("data-isUpdated", "false");
//        }
//    });
//}

var setRiskLevelUI = function (ddl) {
    var selectedRiskLevel = ddl.find(":selected").val();
    setRiskLevelColor(ddl, selectedRiskLevel);
}

var setRiskLevelColor = function (ddl, selectedRiskLevel) {
    var color = 'black';
    switch (selectedRiskLevel) {
        case "7":
        case "8":
            color = '#CD0005 ';
            break;
        case "6":
            color = 'yellow';
            break;
        case "3":
        case "4":
            color = 'green';
            break;
        case "5":
            color = 'lightgreen';
    }
    ddl.attr('style', 'border-color: ' + color + ' !important');
}

var setRiskLevel = function () {
    setRiskLevelUI($(this));
    setRiskLevelBind($(this));
    savechanges(this);
}

var setRiskLevelBind = function (elem) {
    prevRiskLevelVal = elem.data("selected");
    currRiskLevelVal = elem.val();
    var btn;
    if (currRiskLevelVal == "3") {
        btn = elem.closest("._width_risk_level").find("[data-val='100']")[0];
    }
    if ((prevRiskLevelVal == "3" || prevRiskLevelVal == "4")
        && "786540".indexOf(currRiskLevelVal) != "-1") {
        btn = elem.closest("._width_risk_level").find("[data-val='0']")[0];
        alert("Please note that Actual Completion value is set to 0", information);
    }

    if (btn)
        setActualComplationUI($(btn));
    setHandlingDateBind(elem);
    elem.data("selected", currRiskLevelVal);
}

var setHandlingDateBind = function (elem) {

    var today = moment(new Date());
    var handlingDate = moment(elem.closest("._repeating_block").find(".HandlingStartDatePopover").val());
    if (handlingDate > today)
        elem.closest("._repeating_block").find(".HandlingStartDatePopover").val(moment(new Date()).format("D-MMM-YYYY"))
        .data('date', DateObj.convertToJSONDate(new Date().getTime()));
}

var ShowHideCheckListdetails = function () {
    debugger;
    var $this = $(this);
    var txt = $this.html();
    var $tr = $this.closest('._repeating_block');
    console.log(txt);
    //bindDateTimePicker($tr.find('._date'));

    if ($.trim(txt) == 'Show More Details +') {
        $(this).text('Hide Details -');
    }
    else {
        $(this).text('Show More Details +');
    }
    //$('#hide-row').slideToggle('slow');
   // console.log('display= '+$(this).css('display'));
   // console.log('visible= '+$(this).css('visiblility'));
    $(this).closest("div._border_bottom").find(".hide-row").slideToggle('slow'//, function () {
        //if ($(this).is(':visible'))
        //    $(this).css('display', 'inline');
//    }
);

    //renderSubArea($(this));
}

var formatDate = function (datetime) {
    if (!datetime)
        return "";
    return moment(datetime).format("D-MMM-YYYY");
}

var bindDateTimePicker = function (elem) {
    elem.datetimepicker({
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0,
        format: "dd-M-yyyy",
        pickerPosition: "bottom-right"

    });
}

$.fn.setCursorPosition = function (pos) {
    this.each(
        function (index, elem) {
            if (elem.setSelectionRange) {
                elem.setSelectionRange(pos, pos);
            } else if (elem.createTextRange) {
                var range = elem.createTextRange();
                range.collapse(true);
                range.moveEnd('character', pos);
                range.moveStart('character', pos);
                range.select();
            }
        });
    return this;
}

var addTitleIfREquired = function (elem) {
    if (elem.offsetWidth < elem.scrollWidth && !$(elem).attr('title')) {
        $(elem).attr('title', $(elem).text())
    }
}

var bindQuestionItemEvent = function () {
    $(".risk-level-ddl").each(function () {
        $(this).val($(this).data("selected"))
        setRiskLevelColor($(this), $(this).val())
    });
    $(".ddlResponsibility").each(function () {
        $(this).val($(this).data("selected"));
    });
    $(".question-code").mouseenter(function () {
        addTitleIfREquired(this);
    });
    $(".userDialog").click(employeeDialogClick);
    $('.hideShowRow').click(ShowHideCheckListdetails);
    $(".HandlingStartDatePopover").click(BindPopoverDate);
    // $(".risk-level-ddl").focus(setPrevRiskLevelVal);
    $(".risk-level-ddl").change(setRiskLevel);
    //$(".ddlArea").change(getSubAreaList);
    $(".actualComplationBtn").click(setActualComplation);
    $(".add-comment").click(addCommentTitle);
    $(".view-log").click(viewLog);
    $(".ddlArea, .ddlSubArea, .ddlResponsibility").change(function () {
        savechanges(this);
    });
    //$("#comment").blur(function () { savechanges(this); });
    $(".comment").change(function () {
        savechanges(this);
    });
    $("._user").change(function () {
        savechanges(this);
    });
    $('[data-toggle="popover"]').popover();
    $(".view-log").popover({
        html: true
    });
}

var unbindQuestionItemEvent = function () {
    $(".risk-level-ddl").unbind();
    $(".question-code").unbind();
    $(".userDialog").unbind();
    $('.hideShowRow').unbind();
    $(".HandlingStartDatePopover").unbind();
    $(".risk-level-ddl").unbind();
    $(".actualComplationBtn").unbind();
    $(".add-comment").unbind();
    $(".view-log").unbind();
    $(".ddlArea, .ddlSubArea, .ddlResponsibility").unbind();
    $(".comment").unbind();
    $("._user").unbind();
    $('[data-toggle="popover"]').unbind();
    $(".view-log").unbind();
}

var savechanges = function (elem) {
    var tab = $(elem).closest('._repeating_block');
    var recordChanged = [];
    recordChanged.HandlingStartDate = tab.find('.HandlingStartDatePopover').data('date');
    recordChanged.Comments = tab.find('.comment').val();
    //recordChanged.AreaID = tab.find('.ddlArea').val();//
    //recordChanged.AreaName = tab.find('.ddlArea option:selected').text();//
    //recordChanged.SubAreaID = tab.find('.ddlSubArea').val();//
    //recordChanged.SubAreaName = tab.find('.ddlSubArea option:selected').text();//
    recordChanged.Responsibility = tab.find('.ddlResponsibility').val();//
    //recordChanged.QuestionOwnerID = tab.find('#QuestionOwnerID').val();//
    //recordChanged.QuestionOwnerName = tab.find('.userDialog').val();//
    recordChanged.RiskLevelID = tab.find('.risk-level-ddl').val();//
    recordChanged.ActualComplation = tab.find('#actualComplation').val();//
    recordChanged.recordKey = tab.data('record-key');
    checklistObj.functions.changedRecord(recordChanged);
}

var leaveScreen = function () {
    var href;
    if (!($(this).find("a").attr("href")))
        href = $(this).attr("href");
    else href = $(this).find("a").attr("href");
    leaveScreenForScreen(href)
}

var leaveScreenForScreen = function (href, massage) {
    window.onbeforeunload = null;
    event.preventDefault();
    if (!massage)
        massage = "Do you want to save your changes?";
    if (checklistObj.IsChangedRecord)
        confirm(massage, leaveScenario, saveDB, href, continueLeave, href);
    else {
        continueLeave(href);
    }
    return false;
}

var saveDB = function (href) {
    continueLeavehref = href;
    checklistObj.functions.SaveRecords(continueLeave)
}

var continueLeave = function (href) {
    continueLeavehref = href || continueLeavehref;
    window.location.href = continueLeavehref;
}

var defaultLeaveScenario = function () {
    window.onbeforeunload = function () {
        return "Please note that you did not save your last changes, please Hit \"Stay on this Page\" button and hit the \"Save\" button. \n" +
                                                   "Clicking \"Leave this Page\" will discard your changes,";
    }
}

var cancelDefaultLeaveScenario = function () {
    window.onbeforeunload = null;
}

//var HandlingSrartDateClose = function (elem) {
//    if (!elem)
//        elem = $(event.target).closest("._date-calender").find("._calender");
//    elem.popover("hide");
//}

//var HandlingSrartDateSave = function () {
//    var inputDate = $(event.target).closest("._date-calender").find(".HandlingStartDatePopover");
//    var inputDatePopover = $(event.target).closest("._date-calender").find(".HandlingStartDatePopoverInput");
//    inputDate.val(inputDatePopover.val());
//    inputDate.data('date', DateObj.getdatetimepickerJson(inputDatePopover));
//  HandlingSrartDateClose($(event.target).closest("._date-calender").find("._calender"));
//    savechanges(event.target);
//}

//var HandlingSrartDateReset = function (elem) {
//    var inputDatePopover = $(elem).closest("._repeating_block").find(".HandlingStartDatePopoverInput");
//    var inputDate = $(elem).closest("._date-calender").find(".HandlingStartDatePopover");
//    $.ajax({ //Not found in cache, get from server
//        url: "/CheckList/GetStartHandlingDateCalculate",
//                data: { "releaseId": ReleaseID, "questionId": $(elem).data("qid")
//    },
//        type: 'POST',
//        async: true,
//        success: function (data) {
//            console.log(data);
//            inputDatePopover.val(formatDate(data));
//            inputDate.val(inputDatePopover.val());
//            inputDate.data('date', inputDatePopover.val() == "" ? '': inputDatePopover.datetimepicker('getDate').setHours(0, 0, 0, 0));
//           // HandlingSrartDateClose($(elem).closest("._date-calender").find("._calender"));
//            savechanges(elem);
//        }
//    });
//}

//var renderSubArea = function (elem) {
//    var ddl = elem.closest("._border_bottom").find(".ddlSubArea");

//    if (ddl.attr("data-isUpdated") == "true")
//        return;
//    var areaID = elem.closest("._border_bottom").find(".ddlArea").val()

//    $.ajax({ //Not found in cache, get from server
//        url: "/CheckList/getSubAreaListByAreaID",
//        data: { "areaId": areaID },
//        type: 'POST',
//        async: true,
//        success: function (data) {
//            ddl.children().remove();
//            $.each(data, function (idx, obj) {
//                //  ddl.append($("<option></option>").attr("value", obj.SubAreaID).text(obj.Name));
//                ddl.append($("<option></option>").attr("value", obj.SubAreaID).text(obj.Name));
//            });
//            ddl.attr("data-isUpdated", "true")
//        }
//    });
//}

