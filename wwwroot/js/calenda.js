
$(function () {
    $('#dtReleaseDate').datetimepicker({
        defaultDate: new Date(),
    });
});
function validateOnSubmit() {
    if (!$('#fileUpload').val()) {
        $('span[data-valmsg-for="errFileUpload"]').text('The file upload is required.');
    }

    if (!$('#duration').val()) {
        $('span[data-valmsg-for="errDuration"]').text('The duration is required.');
    }
}
