$(function () {
    $.noConflict();
    $("#lstUser").DataTable();
});

$('#lstUser').DataTable({
    responsive: true
});
var delmodal = $('#deleteModal');
var idx;
var deleteConfirm = function (id, title) {
    idx = id;
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');

}
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
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 500,
            });
        },
        error: function () {
            var enotify = $.notify('<strong>Lỗi</strong><br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 500,
            });
        }
    });
}
//$('.slider').click(function () {
//    var prev = $(this).prev();
//    alert(prev.prop('checked'));
//});
