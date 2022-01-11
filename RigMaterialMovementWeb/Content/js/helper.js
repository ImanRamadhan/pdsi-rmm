function isEmptyString(str) {
    return ($.trim(str) === "");
}

function getDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    return today = dd + '-' + mm + '-' + yyyy;
}

function getDatePicker() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    return today = mm + '/' + dd + '/' + yyyy;
}

function getDatePickerMin() {
    var today = new Date();
    var dd = today.getDate() - 7;
    var mm = today.getMonth() + 1; //January is 0!
    if (dd <= 0) {
        dd = 24;
        mm = mm - 1;
    }
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    if (mm <= 0) {
        mm = 12;
        yyyy = yyyy - 1;
    }
    return today = mm + '/' + dd + '/' + yyyy;
}

function getDateMerge() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    return today = dd + mm + yyyy;
}

function getTime() {
    var today = new Date();
    var hh = today.getHours();
    var mi = today.getMinutes();

    return today = hh + ':' + mi;
}

function refreshTable(table, url, callback) {
    if (url) {
        var obj = table.dataTable().fnSettings();
        obj.sAjaxSource = url;
    }
    var dtable = table.dataTable({ bRetrieve: true });
    dtable._fnDraw();
}

function renderDate(data, type, val) {
    if (!isEmptyString(data)) {
        return data.substr(0, 10).split('-').reverse().join('-');
    }
    return '';
}

function renderDateYear(data, type, val) {
    if (!isEmptyString(data)) {
        return data.substr(0, 10).split('-').join('-');
    }
    return '';
}

function renderDateDatePicker(data, type, val) {
    if (!isEmptyString(data)) {
        var split = [];
        split = data.substr(0, 10).split('-');
        result = split[1] + "/" + split[2] + "/" + split[0];
        return result;
    }
    return '';
}

function updateDate(data, type, val) {
    if (!isEmptyString(data)) {
        var split = [];
        split = data.substr(0, 10).split('/');
        result = split[2] + "-" + split[0] + "-" + split[1];
        return result;
    }
    return null;
}
function updateintordecimal(data) {
    if (!isEmptyString(data)) {
        
        return data.replace(/,/g, '');
    }
    return null;
}

function GetSeverity(data) {
    if (!isEmptyString(data)) {
        return data.substr(0, 1);
    }
    return '';
}

function GetProbability(data) {
    if (!isEmptyString(data)) {
        return data.substr(2, 1);
    }
    return '';
}

function refreshDropzone(group_id, elm) {
    //thisDropzone = Dropzone.forElement(elm);
    $.ajax({
        type: "GET",
        url: routePath + "FileUpload/GetFiles?GROUP_ID=" + group_id,
        // data: null,
        async: false,
        contentType: "application/json",
        success: function (data) {
            var rows = [];
            rows.push("<table id='TableDropzone' class='table table-borderless'><tbody>");
            $(data.Data).each(function (index, element) {
                var row = [];
                row.push("<tr>");
                row.push("<td>" + element.FILE_NAME + "</td>");
                row.push("<td><a alt='Download' title='Download' href='#' onclick=downloadFile('" + element.FILE_ID + "') ><span class='glyphicon glyphicon-download'></span></a></td>");
                row.push("<td><a alt='Remove' title='Remove' href='#' onclick=removeFile('" + element.FILE_ID + "','" + element.FILE_NAME + "','" + group_id + "','" + elm + "')  ><span class='glyphicon glyphicon-trash'></span></a></td>");
                row.push("</tr>");
                rows.push(row.join(''));
            });
            rows.push("</tbody></table>");
            var table = rows.join('');
            $(elm).html(table);
        },
        error: function (event, textStatus, errorThrown) {
            alert("Message : Javasript or your connection is disabled or not connected ");
        }
    });
}

window.beginWait = function () {
    $('.loadingscreen').css('opacity', 0.8).css('height', '100%');
    $('.windows8').css('display', '');
};

window.endWait = function () {
    $('.loadingscreen').css('opacity', 0).css('height', '0');
    $('.windows8').css('display', 'none');
    $(window).scrollTop(0);
};

/*using by function ThausandSeperator!!*/
function removeCharacter(v, ch) {
    var tempValue = v + "";
    var becontinue = true;
    while (becontinue === true) {
        var point = tempValue.indexOf(ch);
        if (point >= 0) {
            var myLen = tempValue.length;
            tempValue = tempValue.substr(0, point) + tempValue.substr(point + 1, myLen);
            becontinue = true;
        } else {
            becontinue = false;
        }
    }
    return tempValue;
}

function characterControl(value) {
    var tempValue = "";
    var len = value.length;
    for (i = 0; i < len; i++) {
        var chr = value.substr(i, 1);
        if ((chr < '0' || chr > '9') && chr !== '.' && chr !== ',') {
            chr = '';
        }
        tempValue = tempValue + chr;
    }
    return tempValue;
}

function decimalHandler(e) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((e.which != 46 || $(this).val().indexOf('.') != -1) && (e.which < 48 || e.which > 57)) {
        e.preventDefault();
    }
}

function integerHandler(e) {
    $(this).val($(this).val().replace(/[^0-9]/g, ''));
    if ((e.which != 46 || e.which != 190) && (e.which < 48 || e.which > 57)) {
        e.preventDefault();
    }
}

function textValidator(e) {
    var char = String.fromCharCode(e.keyCode);
    return !/[~!@#$%\^&*()+=\_\[\]\\';/{}|\\":<>\?]/g.test(char);
}

//function percentHandler(e) {
    //var key = e.key;
    //var re = /^((0|[1-9]\d?)(\.\d{1,2})?|100(\.00?)?)/g;
    //if (!re.test(key)) {
    //    e.preventDefault();
    //    return false;
    //}
    //var char = String.fromCharCode(e.keyCode);
    //return /^((0|[1-9]\d?)(\.\d{1,2})?|100(\.00?)?)/g.test(char);
//}

function ThausandSeperator(hidden, value, digit) {
    //debugger;
    var thausandSepCh = ",";
    var decimalSepCh = ".";
    var tempValue = "";
    var realValue = value + "";
    var devValue = "";
    if (digit === "") digit = 3;
    realValue = characterControl(realValue);
    var comma = realValue.indexOf(decimalSepCh);
    if (comma !== -1) {
        tempValue = realValue.substr(0, comma);
        devValue = realValue.substr(comma);
        devValue = removeCharacter(devValue, thausandSepCh);
        devValue = removeCharacter(devValue, decimalSepCh);
        devValue = decimalSepCh + devValue;
        if (devValue.length > digit) {
            devValue = devValue.substr(0, digit + 1);
        }
    } else {
        tempValue = realValue;
    }
    tempValue = removeCharacter(tempValue, thausandSepCh);
    var result = "";
    var len = tempValue.length;
    while (len > 3) {
        result = thausandSepCh + tempValue.substr(len - 3, 3) + result;
        len -= 3;
    }
    result = tempValue.substr(0, len) + result;
    if (hidden !== "") {
        $("#" + hidden).val(tempValue + devValue);
    }
    return result + devValue;
}

function clearForm(form) {
    document.getElementById(form).reset();
    $("#" + form + " select").val(null).trigger('change');
    $("#" + form + "").validate().resetForm();
};

function currencyConverter(code) {
    var message = '';

    $.ajax({
        method: "GET",
        async: false,
        url: "http://free.currencyconverterapi.com/api/v5/convert?q=" + code + "_IDR&compact=ultra",
        success: function (data) {
            message = JSON.stringify(data);
        },
        error: function (event, status, error) {
        }
    })

    return message;
}

function toCommas(value) {
    if (value != null) {
        arr = [];
        arr = parseFloat(value).toFixed(2).toString().split(".");
        return arr[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "." + arr[1];
    }
}

function toCommasFixed4(value) {
    if (value != null) {
        arr = [];
        arr = parseFloat(value).toFixed(4).toString().split(".");
        return arr[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "." + arr[1];
    }
}

function toCommasFixed2(value) {
    if (value != null) {
        arr = [];
        arr = parseFloat(value).toFixed(2).toString().split(".");
        return arr[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "." + arr[1];
    }
}

function fixedCommas(value) {
    return value.replace(/,/g, '');
}

$(document).ready(function () {
    //validate form
    $('.form_required').each(function () {
        $(this).validate({
            'ignore': [],
            'rules': {
                table_required: {
                    required: function () {
                        return !$('.table_required_data').DataTable().data().any();
                    }
                }
            },
            'messages': {
                table_required: {
                    required: "Required data detil"
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).parents('.form-control').removeClass('has-success').addClass('has-error');
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).parents('.form-control').removeClass('has-error').addClass('has-success');
            },
            errorPlacement: function (error, element) {
                if (element.hasClass('select2') && element.next('.select2-container').length) {
                    error.insertAfter(element.next('.select2-container'));
                } else if (element.parent('.input-group').length) {
                    error.insertAfter(element.parent());
                }
                else if (element.prop('type') === 'radio' && element.parent('.radio-inline').length) {
                    error.insertAfter(element.parent().parent());
                }
                else if (element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
                    error.appendTo(element.parent().parent());
                }
                else {
                    error.insertAfter(element);
                }
            }
        });
    });

    //datetime now
    $('.dateNow').val(getDate());

    $('.select2').select2({
        allowClear: true,
        placeholder: ""
    }).on("change", function (e) {
        $(this).valid();
    });

    $('.datepicker').datepicker({
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true
    }).on("change", function (e) {
        $(this).valid();
    });

    $('.datepicker').on("paste", function (e) {
        e.preventDefault();
    });

    $('input[type=eu-time]').w2field('time', {
        format: 'h24'
    });

    $('input.numberDecimal').val('0.0000');

    $('textarea').attr('maxlength', '500');

    //$('input[type=text]').attr('maxlength', '100');
});

function LoadingScreenStart(callback) {
    $('.loadingscreen').css('opacity', 0.8).css('height', '100%');
    $('.windows8').css('display', '');
    //debugger;
    setTimeout(function () { callback(); LoadingScreenEnd()},1000)
}
function LoadingScreenEnd() {
    $('.loadingscreen').css('opacity', 0).css('height', '0');
    $('.windows8').css('display', 'none');
    $(window).scrollTop(0);
}
