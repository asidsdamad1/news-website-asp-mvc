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
var delmodal = $('#deleteModal');
var idx;
var deleteConfirm = function (id, title) {
    delmodal.find('.modal-body').text(title);
    delmodal.modal('show');
    idx = id;
}

$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/User/DeleteConfirmed',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idx }),
        dataType: "json",
        success: function (recData) {
            var notify = $.notify('<strong>Thành công</strong><br/>' + recData.Message + '<br />', {
                type: 'pastel-info',
                allow_dismiss: false,
                timer: 1000,
            });
            setTimeout(function () {
                window.location.reload();
            }, 1000);

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không xóa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
});