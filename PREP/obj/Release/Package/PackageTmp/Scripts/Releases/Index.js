

$(document).ready(function () {

    //var oTable = $('.table').dataTable({
    //    "oLanguage": {
    //        'sSearch': "_INPUT_",
    //        'sSearchPlaceholder': "Search...",
    //        "oPaginate": {
    //            "sFirst": "<< First",
    //            "sLast": "Last >>",
    //            "sPrevious": "< Previous",
    //            "sNext": "Next >",
    //        }
    //    },
    //    "sDom": '<t>irp',
    //    "bServerSide": true,
    //    "sAjaxSource": "/Releases/AjaxHandler",
    //    "bProcessing": true,
    //    "iDisplayLength": 10,
    //    "bSort": true,
    //    "bpaginate": true,
    //    //add hide column for sorting the ID as num without the ">>"
    //    "aoColumnDefs": [{ "iDataSort": 1, "aTargets": [0] }, { "bVisible": false, "aTargets": [1] }],
    //    "sPaginationType": "full_numbers",
    //});
    $.fn.DataTable.ext.pager.numbers_length = 7;
    oTable = $('.table').DataTable(
        {
            "oLanguage": {
                'sSearch': "_INPUT_",
                'sSearchPlaceholder': "Search...",
                "oPaginate": {
                    "sFirst": "<< First",
                    "sLast": "Last >>",
                    "sPrevious": "< Previous",
                    "sNext": "Next >",
                }
            },
            "drawCallback": function (settings) {
                $(".tdReleaseIndex").mouseenter(function () {
                    addTitleIfREquired(this);
                });
            },
            "oSearch": {"sSearch": $('#myInputTextField').val()},
            "sDom": '<t>irp',
            "bSort": true,
            "iDisplayLength": 10,
            "bpaginate": true,
            "pagingType": "full_numbers",
            //add hide column for sorting the ID as num without the ">>"
            "aoColumnDefs": [{ "iDataSort": 1, "aTargets": [0] }, { "bVisible": false, "aTargets": [1] }],
            "sPaginationType": "two_button",
            "Processing": true,
            "bServerSide": true,
            "sAjaxSource": "/Releases/AjaxHandler",
            "aoColumns": [

                    {
                        "mData": "ReleaseID",
                        "sName": "ReleaseID",
                        "sClass":"tdID",
                        "mRender": function (innerData, sSpecific, oData) {
                                                      return '<a href=\"/Releases/Details/View/' + innerData + '\">' + innerData + " >>"+'</a>';
                        }
                    },
                    {
                        "mData": "ReleaseID",
                        "sName": "ReleaseID",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "AccountName",
                        "sName": "AccountName",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "PrepFPName",
                        "sName": "PrepFPName",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "ReleaseName",
                        "sName": "ReleaseName",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "PrepReviewMode",
                        "sName": "PrepReviewMode",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "SPNameEmployee",
                        "sName": "SPNameEmployee",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "ProgramMeEmployee",
                        "sName": "ProgramMeEmployee",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "ProductionStartDate",
                        "sName": "ProductionStartDate",
                        "sClass": "tdReleaseIndex",
                    },
                    {
                        "mData": "ReleaseID",
                        "sName": "ReleaseID",
                        "bSearchable": false,
                        "bSortable": false,
                        
                        "mRender": function (innerData, sSpecific, oData) {
                            if (oData["IsExistCheckList"] == true)
                               return '<a  class="CheckListFont" href=\"/CheckList/ViewCheckList/' + innerData + '\">CheckList</a>';
                            else 
                            return '<a   class="CheckListFont" href=\"#\" title="No checklist was initiated." >CheckList</a>';
                          
                        },
                        "sClass": "trandtd"
                        }
            ]
           
            ,
        }

        );
  

    $('#myInputTextField').on('blur keyup', function (event) {
        if (event.type == 'blur' || event.keyCode == '13') {
        oTable.search($(this).val()).draw();
        }
    })

  
    $('.dataTables_info').addClass('span6')
    $('.dataTables_paginate').addClass('span6')
    if (!$.browser.chrome) {
        $('.span6').wrapAll('<div class="row-fluid"></div>')
    }

    $('#DataTables_Table_0_filter').children().attr('ID', 'search_Label')
    $('.dataTables_filter input').attr("placeholder", "Search...");
    $('.dataTables_filter input').addClass('input')

    $('.dataTables_filter input').wrapAll('<div Id="SearchDiv" class="right-inner-addon"></div>')
    $('.dataTables_filter input').append($('<span class="glyphicon glyphicon-search"></span>'));

    $('.dataTables_info').addClass('span6')
    $('.dataTables_paginate').addClass('span6')
    if (!$.browser.chrome) {
        $('.span6').wrapAll('<div class="row-fluid"></div>')
    }
});
$("#releases").addClass("active");

var addTitleIfREquired = function (elem) {
    if (elem.offsetWidth < elem.scrollWidth && !$(elem).attr('title')) {
        $(elem).attr('title', $(elem).text())
    }
}
