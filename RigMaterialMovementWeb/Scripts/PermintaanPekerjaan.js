$(document).ready(function () {
    $("#txttglpekerjaansrcstart").datepicker({
        dateFormat: 'dd-mm-yy'
    });
    $("#txttglpekerjaansrcend").datepicker({
        dateFormat: 'dd-mm-yy'
    });
    
    });


function PopUp() {
    $('ModalRig').modal("show");
}



function SubmitPermintaanPekerjaan() {

}

function SearchPermintaanPekerjaan() {
    if (Validation()) {
        var DataPost = {
            no_wo: $('#txtwosrc').val(),
            no_activity: $('#txtactivitysrc').val(),
            judul_pekerjaan: $('#txtjudulsrc').val(),
            lokasi_asal: $('#txtlokasiasalsrc').val(),
            lokasi_tujuan: $('#txtlokasitujuansrc').val(),
            tanggal_pekerjaandari: $('#txttglpekerjaansrcstart').val(),
            tanggal_pekerjaansampai: $('#txttglpekerjaansrcend').val(),
            status: $('#ddlstatussrc').val(),
            commandline: 'Search'
        }
        $.ajax({
            type: "post",
            url: UrlSearch,
            data: DataPost,
            success: function (Result) {
                if (Result != "") {
                    $.ajax({
                        success: function (Result) {
                        },
                        error: function (event, textStatus, errorThrown) {
                            debugger;
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

function CheckID(ID) {
    alert(ID);
}

function Validation() {

    if (!$("#txttglpekerjaansrcstart").val()) {
        $("#txttglpekerjaansrcstart").focus();
        return false;
    }
    if (!$("#txttglpekerjaansrcend").val()) {
        $("#txttglpekerjaansrcend").focus();
        alert("All field must be filled");
        return false;
    }

    return true;
}
