$(document).ready(function () {


    $('#List').dataTable({
    });
});


function PopUp() {
    $('#ModalTransporter').modal("show");
}

function SubmitMasterTransporter() {
    var DataPost = {
        //tanggal_mulai = $('#ddlContractNumber').val(),

        name: $('#txtname').val()
    }
    $.ajax({
        type: "post",
        url: UrlCreate,
        data: DataPost,
        success: function (Result) {
            if (Result != "") {
                $.ajax({
                    success: function (Result) {
                       
                        location.reload();
                    },
                    error: function (event, textStatus, errorThrown) {
                        location.reload();
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
function OpenModal(Type, ID) {
    debugger;
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

                debugger;
                if (Result != "") {
                    var DataEE = JSON.parse(Result);
                    $('#txtname').val(DataEE[0].name);                  
                    $('#id').val(ID);
                }
            }
        })
        //var test = $('#txtarea').val();
        //alert(test);

        $('#id').val(ID);
        $("#hideEdit").show();
        $("#hideCreate").hide();
        $('#ModalTransporter').modal('show');

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
function EditMasterTransporter() {
    var DataPost = {
        name: $('#txtname').val(),
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
