$(document).ready(function () {
    $("#ddlarea").on("change", function () {
        var Name = $(this).val();
        $.getJSON("../LaporanRekapData/GetDDLRig", { name: Name },
            function (data) {

                var select = $("#ddlrig");
                select.empty();
                select.append(new Option(""));

                if (Name != "") {

                    $.each(data, function (index, row) {
                        select.append("<option value='" + row.name + "'>" + row.name + "</option>")
                    });
                }
                else {
                    select.select2({
                        placeholder: "",

                    });
                }

            });
    });



    $("#txttanggal2").datetimepicker({
        minDate: new Date(),
        format: "yyyy-mm-dd hh:ii:ss",
        autoclose: true
    });

    $("#txtbiaya").inputFilter(function (value) {
        return /^[0-9,]*$/.test(value);    // Allow digits only, using a RegExp
    });
    $("#txtmoveout").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    });
    $("#txtjarak").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    });
    $("#txtharipjp").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    });
    $("#txttrip").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    });

});


function SubmitRigMovement() {

    if (ValidationParent()) {
        let biaya = $('#txtbiaya').val();
        let newBiaya = biaya.replace(/\,/g, '');

        var hostname = location.pathname;
        var res = hostname.replace("/Create", "");
        var full = res + '/Edit'

        var DataPost = {
            tanggal_mulai: $('#txttanggal2').val(),
            area: $('#ddlarea').val(),
            rig: $('#ddlrig').val(),
            rute_dari: $('#txtrutedari').val(),
            rute_ke: $('#txtke').val(),
            jarak: $('#txtjarak').val(),
            transporter_id: $('#ddltransporter').val(),
            biaya: newBiaya,
            target_hari: $('#txtharipjp').val(),
            target_trip: $('#txttrip').val(),
            jam: $('#txtjam').val(),
            commandline: 'CreateNew'
        }

        $.ajax({
            type: "post",
            url: UrlCreate,
            data: DataPost,
            success: function (Result) {
                if (Result != "") {
                    $.ajax({
                        success: function (Result) {

                            window.location.href = full;
                          
                        },
                        error: function (event, textStatus, errorThrown) {

                            window.location.href = full;
                          

                        }
                    })
                } else {

                }
            },
            error: function (event, textStatus, errorThrown) {
                alert("Message : Javasript or your connection is disabled or not connected ");
            }
        })
    }
}

function ValidationParent() {


    if (!$("#txtrutedari").val()) {
        $("#txtrutedari").focus();
        return false;
    }
    if (!$("#txtke").val()) {
        $("#txtke").focus();
        return false;
    }
    if (!$("#txtjarak").val()) {
        $("#txtjarak").focus();
        return false;
    }

    if (!$("#txtharipjp").val()) {
        $("#txtharipjp").focus();
        return false;
    }
    if (!$("#txtbiaya").val()) {
        $("#txtbiaya").focus();
        return false;
    }
    if (!$("#txttrip").val()) {
        $("#txttrip").focus();
        return false;
    }
    if (!$("#txttanggal2").val()) {
        $("#txttanggal2").focus();
        return false;
    }
    return true;
}
(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));