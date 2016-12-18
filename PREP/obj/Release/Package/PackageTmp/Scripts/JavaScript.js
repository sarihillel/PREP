//var selector, elems, makeActive;

//selector = '.nav li';

//elems = document.querySelectorAll(selector);

//makeActive = function () {
//    for (var i = 0; i < elems.length; i++) {
//        elems[i].classList.remove('active');
//    }

//    this.classList.add('active');
//};


//for (var i = 0; i < elems.length; i++)
//    elems[i].addEventListener('mousedown', makeActive);
//Array.prototype.filter = function (obj, predicate) {
//    var result = {}, key;
//    for (key in obj) {
//        if (obj.hasOwnProperty(key) && !predicate(obj[key])) {
//            result[key] = obj[key];
//        }
//    }
//    return result;
//};
if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        if (this == null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (obj, fromIndex) {
        if (fromIndex == null) {
            fromIndex = 0;
        } else if (fromIndex < 0) {
            fromIndex = Math.max(0, this.length + fromIndex);
        }
        for (var i = fromIndex, j = this.length; i < j; i++) {
            if (this[i] === obj)
                return i;
        }
        return -1;
    };
}
// make jquery contains not case sensitive
$.expr[":"].contains = $.expr.createPseudo(function (arg) {
    return function (elem) {
        return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
    };
});

//function on date
var DateObj = {
    convertJsonToDate: function (date, NotIncludeTime) {
        NotIncludeTime = NotIncludeTime || true;
        if (date == null || date == "")
            return date
        else {
            var dateTime = new Date(date.match(/\d+/)[0] * 1);
            if (NotIncludeTime) dateTime.setHours(0, 0, 0, 0);
            return dateTime.getTime();
        }
    },
    convertJsonToString: function (date) {
        return date == null || date == "" ? date : new Date(date.match(/\d+/)[0] * 1).toISOString();
    },
    convertToJSONDate: function (getTime) {
        return '/Date(' + getTime + ')/';
    },
    checkDateInRange: function (date, minDate, maxDate) {
        return date.getTime() >= minDate.getTime() && date.getTime() <= maxDate.getTime();
    },
    getdatetimepicker: function ($elem) {
        return $elem.val() == "" ? "" : $elem.datetimepicker('getDate').setHours(0, 0, 0, 0);
    },
    getdatetimepickerJson: function ($elem) {
        return $elem.val() == "" ? null : this.convertToJSONDate($elem.datetimepicker('getDate').setHours(0, 0, 0, 0));
    },
}

function jq(myid) {
    return myid.replace(/(:|\.|\[|\]|,)/g, "\\$1");
}
// ------ serilize release tab-content ------
var objectSerilized = {}

function getFormData($content) {
    var unindexed_array = $content.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    $("form input:checkbox").each(function () {
        indexed_array[this.name] = this.checked;
    })

    return indexed_array;
}

///---------serilize checkbox------------------


//----- end serilize release tab-content ------
