
function check(table) {
    $('tbody td', table).removeClass('highLight');
    var def = $('input[name=Keparahan]:checked', table).val();
    var abc = $('input[name=Kemungkinan]:checked', table).val();
    $('tbody td.risk-ser-' + def + '.risk-prob-' + abc + '', table).addClass('highLight');
    if (abc == 'A' && def == '0') {
        $('#txtresult').val('0-A');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'B' && def == '0') {
        $('#txtresult').val('0-B');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'C' && def == '0') {
        $('#txtresult').val('0-C');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'D' && def == '0') {
        $('#txtresult').val('0-D');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'E' && def == '0') {
        $('#txtresult').val('0-E');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'A' && def == '1') {
        $('#txtresult').val('1-A');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'B' && def == '1') {
        $('#txtresult').val('1-B');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'C' && def == '1') {
        $('#txtresult').val('1-C');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'D' && def == '1') {
        $('#txtresult').val('1-D');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'E' && def == '1') {
        $('#txtresult').val('1-E');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'A' && def == '2') {
        $('#txtresult').val('2-A');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'B' && def == '2') {
        $('#txtresult').val('2-B');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'C' && def == '2') {
        $('#txtresult').val('2-C');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'D' && def == '2') {
        $('#txtresult').val('2-D');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'E' && def == '2') {
        $('#txtresult').val('2-E');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'A' && def == '3') {
        $('#txtresult').val('3-A');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'B' && def == '3') {
        $('#txtresult').val('3-B');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'C' && def == '3') {
        $('#txtresult').val('3-C');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'D' && def == '3') {
        $('#txtresult').val('3-D');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'E' && def == '3') {
        $('#txtresult').val('3-E');
        $('#txtresiko').val('Tinggi');
    } else if (abc == 'A' && def == '4') {
        $('#txtresult').val('4-A');
        $('#txtresiko').val('Rendah');
    } else if (abc == 'B' && def == '4') {
        $('#txtresult').val('4-B');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'C' && def == '4') {
        $('#txtresult').val('4-C');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'D' && def == '4') {
        $('#txtresult').val('4-D');
        $('#txtresiko').val('Tinggi');
    } else if (abc == 'E' && def == '4') {
        $('#txtresult').val('4-E');
        $('#txtresiko').val('Tinggi');
    } else if (abc == 'A' && def == '5') {
        $('#txtresult').val('5-A');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'B' && def == '5') {
        $('#txtresult').val('5-B');
        $('#txtresiko').val('Sedang');
    } else if (abc == 'C' && def == '5') {
        $('#txtresult').val('5-C');
        $('#txtresiko').val('Tinggi');
    } else if (abc == 'D' && def == '5') {
        $('#txtresult').val('5-D');
        $('#txtresiko').val('Tinggi');
    } else if (abc == 'E' && def == '5') {
        $('#txtresult').val('5-E');
        $('#txtresiko').val('Tinggi');
    }
}
function ShowRisk(Prob,Ser) {
    $.ajax({
        method: "GET",
        url: routePath + "odata/ResikoOdata?$filter=SEVERITY eq '" + Ser + "'and PROBABILITY eq '" + Prob + "'"
    })
.done(function (msg) {
    debugger;
    $('#txtresult').val(msg.RESIKO)
    })
}