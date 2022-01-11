$(document).ready(function () {

    $("#txtmovein").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
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

    $("#ddlarea").change(function () {
        var end = this.value;
        $('#ddlrig').html('');
        var a = $(this).val();
        if (a == "" || a == null) {
            b = 0;
        }
        else {
            var b = a.toString();
        }


        var selected = $(this).find("option:selected");
        var arrSelected = [];


        var ParamMainData = {
            area: end
        }
        $.ajax({
            data: ParamMainData,
            url: GetDDL,
            type: 'GET',
            success: function (result) {

                 
                var s = $("#ddlrig").html();
                var arr = [];

                var test = result.data.length;

                for (var i = 0; i < test; i++) {
                    if (arr.includes(result.data[i].name)) {
                        continue;
                    }
                    else {
                        if (result.data[i].name != null) {
                            s += '<option value="' + result.data[i].name + '">' + result.data[i].name + '</option>'
                        }
                        arr.push(result.data[i].name);
                    }
                }
                $("#ddlrig").html(s);
                selected.each((idx, val) => {
                    arrSelected.push(val.value);
                });
            }

        })

    });



    var tanggal = $("#txttanggal2").val();
    
    $("#txttanggal2").datetimepicker({
    

        format: "yyyy-mm-dd hh:ii:ss",
        autoclose: true
    });


    $("#txttglmove").datetimepicker({

        startDate: tanggal,
        format: "yyyy-mm-dd hh:ii:ss",
   
        autoclose: true
    });

 
  
    $('#txttanggal').prop('readonly', true);
 

});
function SubmitRigMovementDetails() {
    if (Validation()) {
        debugger
        var DataPost = {
            //tanggal_mulai = $('#ddlContractNumber').val(),
            rig_material_movement_id: $('#txtviewbag').val(),
            trip_move_out: $('#txtmoveout').val(),
            trip_move_in: $('#txtmovein').val(),
            kendala: $('#txtkendala').val(),
            tindaklanjut: $('#txttindaklanjut').val(),
            faktorketerlambatan: $('#ddlketerlambatan').val(),
            tanggal_move: $('#txttglmove').val(),
            test_tanggal_move: $('#txttglmove').val()
        }
      
        debugger
        $.ajax({
            type: "post",
            url: UrlCreateDetails,
            data: DataPost,
            success: function (Result) {
                if (Result != "") {
                    $.ajax({
                        success: function (Result) {

                            location.reload();
                        },
                        error: function (event, textStatus, errorThrown) {

                            location.reload();
                             
                        }
                    })
                } else {

                }
            },
            error: function (event, textStatus, errorThrown) {
                alert("Message : Javasript or your connection is disabled or not connected ");
            }
        })
        debugger
        location.reload();
    }
}
function PopUp() {
    $('#hideEdit').hide();
    $('#hideCreate').show();
    $('#ModalRig').modal('show');
    
}


function OpenModal(Type, ID) {
     
    if (Type == "EditRigMovement") {
        $('#id').val(ID);
        var ParamMainData = {
            id: ID
        }
        $.ajax({
            data: ParamMainData,
            url: UrlGetDetail,
            type: 'POST',
            success: function (Result) {

                 
                if (Result != "") {
                    var DataEE = JSON.parse(Result);
                    $('#txtmoveout').val(DataEE[0].trip_move_out);
                    $('#txtmovein').val(DataEE[0].trip_move_in);
                    $('#txtkendala').val(DataEE[0].kendala);
                    $('#txttindaklanjut').val(DataEE[0].tindaklanjut);
                    $('#ddlketerlambatan').val(DataEE[0].faktorketerlambatan);
                    var StartDateServer = DataEE[0].tanggalmove;
                    var parsedDate = new Date(parseInt(StartDateServer.substr(6)));
                    var tanggal = parsedDate;
                    var tanggalan = new Date(tanggal);
                    var date = tanggalan.getDate();
                    var month = tanggalan.getMonth() + 1;
                    var year = tanggalan.getFullYear();
                    var hour = tanggalan.getHours();
                    var minute = tanggalan.getMinutes();

                    var actualDate = year + "-" + month + "-" + date + " " + hour + ":" + minute
                 
                    $('#txttglmove').val(actualDate);
                    $('#id').val(ID);
                }
            }
        })
   
        $('#id').val(ID);
        $('#hideEdit').show();
        $('#hideCreate').hide();
        $('#ModalRig').modal('show');

    }
    if (Type == "DeleteRigMovement") {
        if (confirm("Are you sure?")) {
            //Change Value ID
            $('#id').val(ID);
            var ParamMainData = {
                id: ID
            }
            $.ajax({
                data: ParamMainData,
                url: UrlDelete,
                type: 'POST',
                success: function (Result) {
                    location.reload();
                }
            })


        }
        return false;
    }


}
function EditRigMovement() {
    if (Validation()) {
        var DataPost = {
            trip_move_in: $('#txtmovein').val(),
            trip_move_out: $('#txtmoveout').val(),
            kendala: $('#txtkendala').val(),
            tindaklanjut: $('#txttindaklanjut').val(),
            faktorketerlambatan: $('#ddlketerlambatan').val(),
            tanggal_move: $('#txttglmove').val(),
            id: $('#id').val()
        };
        $.ajax({
            type: "post",
            url: UrlEdit,
            data: DataPost,
            success: function (Result) {
                if (Result != "") {

                    $.ajax({
                        success: function (Result) {

                           
                            location.reload();
                        },
                        error: function (event, textStatus, errorThrown) {
                            alert("Message : Javasript or your connection is disabled or not connected ");
                            location.reload();
                             
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
function SaveParent() {
     
    var hostname = location.pathname;
    var res = hostname.replace("/Edit", "");
    var full = res + '/RigMaterialList'
 
    if (ValidationParent()) {
        var ParamMainData = {
            id: $('#txtviewbag').val(),
            area: $('#ddlarea').val(),
            rig: $('#ddlrig').val(),
            rute_dari: $('#txtrutedari').val(),
            rute_ke: $('#txtke').val(),
            jarak: $('#txtjarak').val(),
            transporter_id: $('#ddltransporter').val(),
            target_hari: $('#txtharipjp').val(),
            target_trip: $('#txttrip').val(),
            tanggal_mulai: $('#txttanggal2').val(),
            biaya: $('#txtbiaya').val(),
            test_tanggal_move: $('#txttanggal2').val()
        }
         
        $.ajax({
            type: "post",
            url: UrlEditParent,
            data: ParamMainData,
            success: function (Result) {
                if (Result != "") {

                    $.ajax({
                        success: function (Result) {

                           
                            window.location.href = full;
                        },
                        error: function (event, textStatus, errorThrown) {
                            alert("Message : Javasript or your connection is disabled or not connected ");
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

function Validation() {

    if (!$("#txtmovein").val()) {
        $("#txtmovein").focus();
        return false;
    }
    if (!$("#txtmoveout").val()) {
        $("#txtmoveout").focus();
        alert("All field must be filled");
        return false;
    }

    if (!$("#txttglmove").val()) {
        $("#txttglmove").focus();
        alert("All field must be filled");
        return false;
    }
    return true;
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

function StrToDate(StrDate) {
     
    var date = new Date(StrDate),
        yr = date.getFullYear(),
        month = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1,
        day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate(),
        newDate = day + '-' + month + '-' + yr;
    return newDate;
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