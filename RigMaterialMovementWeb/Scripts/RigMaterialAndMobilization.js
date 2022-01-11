$(document).ready(function () {
    var d = new Date();
    var n = d.getDate() + "/" + d.getMonth() + "/" + d.getFullYear()
    $('#txttanggal').val(n);
    $('#txttanggal2').val(n);
});


function CreatePopUp() {
    $('#ModalRig').modal('show'); 
    var d = new Date();
    var n = d.getDate() + "/" + d.getMonth() + "/" + d.getFullYear()
    $('#txttanggal').val(n);
    $('#txttanggal2').val(n);
    $('#txttanggal').prop('readonly', true);
    $('#txttanggal2').prop('readonly', true);
}
function SubmitRigMovement() {
     
    var DataPost = {
        //tanggal_mulai = $('#ddlContractNumber').val(),
        
        area : $('#txtarea').val(),
        rig : $('#txtrig').val(),
        rute_dari : $('#txtrutedari').val(),
        rute_ke : $('#txtke').val(),
        jarak : $('#txtjarak').val(),
        transporter_id : $('#ddltransporter').val(),
        target_hari : $('#txtharipjp').val(),
        target_trip : $('#txttrip').val()
}
    $.ajax({
        type: "post",
        url: UrlCreate,
        data: DataPost,
        success: function (Result) {
            if (Result != "") {
                var DataForm = new FormData($('#formModalRig')[0]);
                $.ajax({
                    success: function (Result) {

                        $('#ModalRig').modal('hide');
                        //location.reload();
                    },
                    error: function (event, textStatus, errorThrown) {
                        alert("Message : Javasript or your connection is disabled or not connected ");
                        //location.reload();
                         
                    }
                })
            } else {

            }
        },
        error: function (event, textStatus, errorThrown) {
            alert("Message : Javasript or your connection is disabled or not connected ");
        }
    })
    $('#ModalRigDetail').modal('show'); 
    
}

function SubmitRigMovementDetail() {
     
    var DataPost = {
        //tanggal_mulai = $('#ddlContractNumber').val(),

        trip_move_out: $('#txtmoveout').val(),
        trip_move_in: $('#txtmovein').val(),
        kendala: $('#txtkendala').val(),
        tindaklanjut: $('#txttindaklanjut').val(),

    }
    $.ajax({
        type: "post",
        url: UrlCreateDetail,
        data: DataPost,
        success: function (Result) {
            if (Result != "") {
               
                $.ajax({
                    success: function (Result) {

                       
                        //location.reload();
                    },
                    error: function (event, textStatus, errorThrown) {
                        alert("Message : Javasript or your connection is disabled or not connected ");
                        //location.reload();
                         
                    }
                })
            } else {

            }
        },
        error: function (event, textStatus, errorThrown) {
            alert("Message : Javasript or your connection is disabled or not connected ");
        }
    })
    $('#ModalRigDetail').modal('show');

}


function OpenModal(Type, ID) {
     
    if (Type == "EditRigMovement") {
        $('#id').val(ID);
        var ParamMainData = {
            id: ID
        }
        $.ajax({
            data: ParamMainData,
            url: UrlGet,
            type: 'POST',
            success: function (Result) {
                
                 
                if (Result != "") {
                    var DataEE = JSON.parse(Result);
                    $('#txtmoveout').val(DataEE[0].trip_move_out);
                    $('#txmovein').val(DataEE[0].trip_move_in);
                    $('#txtkendala').val(DataEE[0].kendala);
                    $('#txttindaklanjut').val(DataEE[0].tindaklanjut);
                    $('#id').val(ID);
                }
            }
        })
        //var test = $('#txtarea').val();
        //alert(test);
        
        $('#id').val(ID);
        $('#ModalRigDetail').modal('show');

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

function EditRigMovement(){
    var DataPost = {
        area: $('#txtarea').val(),
        rig: $('#txtrig').val(),
        rute_dari: $('#txtrutedari').val(),
        rute_ke: $('#txtke').val(),
        jarak: $('#txtjarak').val(),
        transporter_id: $('#ddltransporter').val(),
        target_hari: $('#txtharipjp').val(),
        target_trip: $('#txttrip').val(),
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

                       // $('#ModalEngineeringEstimate').modal('hide');
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
