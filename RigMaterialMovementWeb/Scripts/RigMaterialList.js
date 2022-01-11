$(document).ready(function () {
    $("#txttglpekerjaansrcstart").datepicker({
        //dateFormat: 'dd-mm-yy'
    });
    $("#txttglpekerjaansrcend").datepicker({
        //dateFormat: 'dd-mm-yy'
    });
    var full = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '') + '~/RigMaterial/EditExisting';
    var full2 = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '') + '~/RigMaterial/DeleteRigMaterial';
    var grid = $('#List').DataTable({
        ajax: {
            url: Get,
            type: "GET",
            datatype: "json"
        },
        "columns": [
            {
                "data": 'id',
                "searchable": false,
                "orderable": false,

            },
            {
                "data": "id", className: 'center', 'searchable': false, orderable: false, render: function (data, type, row, meta) {
                    return '<a href="' + full + "?param=" + row.id + '"><button type="button" class="btn btn-success"><span class="fa fa-pencil"></span></button></a>&nbsp' +
                        '<a href="' + full2 + "?param=" + row.id + '"><button type="button" class="btn btn-danger"><span class="fa fa-trash"></span></button></a>'
                }
            },
            { "data": 'area' },
            { "data": 'rig' },
            { "data": 'rute_dari' },
            { "data": 'rute_ke' },
            { "data": 'jarak' },
            { "data": 'transporter' },
            { "data": 'biaya' },
            {
                "data": 'tanggal_mulai',
                "render": function (data) {
                    var StartDateServer = data;
                    var parsedDate = new Date(parseInt(StartDateServer.substr(6)));
                    var finalDate = parsedDate.toLocaleDateString('en-GB'); //result as mm/dd/yyyy      
                    return finalDate;
                },
            },
            { "data": 'target_hari' },
            { "data": 'target_trip' },
            { "data": 'harike' },
            { "data": 'trip' },
            {
                "data": 'persentase2',
                "render": function (data) {
                    var num = Number(data) // The Number() only visualizes the type and is not needed
                    var roundedString = num.toFixed(2);
                    var rounded = Number(roundedString) + "%"; // toFixed() returns a string (often suitable for printing already)
                    return rounded;
                },
            },
            { "data": 'penilaian1' },
            { "data": 'tripMoveIn' },
            {
                "data": 'persentaseMoveIn2',
                "render": function (data) {
                    var num = Number(data) // The Number() only visualizes the type and is not needed
                    var roundedString = num.toFixed(2);
                    var rounded = Number(roundedString) + "%"; // toFixed() returns a string (often suitable for printing already)
                    return rounded;
                },
            },
            { "data": 'penilaian2' },
            //{
            //    "data": 'no_wo',
            //    "render": function (data, type, row, meta) {
            //        return '<a href="' + full + "?param=" + data + '">' + data + '</a>';
            //    }
            //},
            //{ "data": 'no_activity' },
            //{ "data": 'judul_pekerjaan' },
            //{ "data": 'lokasi_asal' },
            //{ "data": 'lokasi_tujuan' },
            //{
            //    "data": 'tanggal_pekerjaan',
            //    "render": function (data) {
            //        var StartDateServer = data;
            //        var parsedDate = new Date(parseInt(StartDateServer.substr(6)));
            //        var finalDate = parsedDate.toLocaleDateString('en-GB'); //result as mm/dd/yyyy      
            //        return finalDate;
            //    },
            //},
            //{ "data": 'status' },
            //{ "data": 'approved_by' },
            //{ "data": 'last_modified_by' },



            //}
        ],
        dom: "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "columnDefs": [
            { "searchable": true }
        ],
        "order": [[0, 'desc']]
    });
    grid.on('order.dt search.dt', function () {
        grid.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});


function tes() {
    var getData = {
        area: $('#ddlareasrc').val(),
        rig: $('#ddlrigsrc').val(),
        transporter: $('#ddltransportersrc').val(),
        tanggal_mulai_dari: $('#txttglpekerjaansrcstart').val(),
        tanggal_mulai_sampai: $('#txttglpekerjaansrcend').val()

    }
    var grid = $('#grid').DataTable({
        "destroy": true,
        ajax: {
            url: Filter,
            type: "post",
            data: getData,
            datatype: "json"
        },
        "columns": [
            {
                "data": 'id',
                "searchable": false,
                "orderable": false,

            },
            {
                "data": "id", className: 'center', 'searchable': false, orderable: false, render: function (data, type, row, meta) {
                    return '<a href="' + full + "?param=" + row.id + '"><button type="button" class="btn btn-success"><span class="fa fa-pencil"></span></button></a>&nbsp' +
                        '<a href="' + full2 + "?param=" + row.id + '"><button type="button" class="btn btn-danger"><span class="fa fa-trash"></span></button></a>'
                }
            },
            { "data": 'area' },
            { "data": 'rig' },
            { "data": 'rute_dari' },
            { "data": 'rute_ke' },
            { "data": 'jarak' },
            { "data": 'transporter' },
            { "data": 'biaya' },
            {
                "data": 'tanggal_mulai',
                "render": function (data) {
                    var StartDateServer = data;
                    var parsedDate = new Date(parseInt(StartDateServer.substr(6)));
                    var finalDate = parsedDate.toLocaleDateString('en-GB'); //result as mm/dd/yyyy      
                    return finalDate;
                },
            },
            { "data": 'target_hari' },
            { "data": 'target_trip' },
            { "data": 'harike' },
            { "data": 'trip' },
            {
                "data": 'persentase2',
                "render": function (data) {
                    var num = Number(data) // The Number() only visualizes the type and is not needed
                    var roundedString = num.toFixed(2);
                    var rounded = Number(roundedString) + "%"; // toFixed() returns a string (often suitable for printing already)
                    return rounded;
                },
            },
            { "data": 'penilaian1' },
            { "data": 'tripMoveIn' },
            {
                "data": 'persentaseMoveIn2',
                "render": function (data) {
                    var num = Number(data) // The Number() only visualizes the type and is not needed
                    var roundedString = num.toFixed(2);
                    var rounded = Number(roundedString) + "%"; // toFixed() returns a string (often suitable for printing already)
                    return rounded;
                },
            },
            { "data": 'penilaian2' },
        ],
        dom: "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "columnDefs": [
            { "searchable": true }
        ],
        "order": [[0, 'desc']]
    });
    grid.on('order.dt search.dt', function () {
        grid.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}