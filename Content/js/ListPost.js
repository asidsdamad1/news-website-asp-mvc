$(function () {
    $.noConflict();
    $("#lstPost").DataTable();
});

 $('#lstPost').DataTable({
        responsive: true
    });
var delmodal = $('#deleteModal');
var idx;

//$('.slider').click(function () {
//    var prev = $(this).prev();
//    alert(prev.prop('checked'));
//});
var changeStt = function (xthis) {
    var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
        url: '/User/changeStatus',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: xid, state: st }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('Thành công' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
        },
        error: function () { alert('An error occured'); }
    });
}
