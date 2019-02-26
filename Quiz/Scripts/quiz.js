$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    $(".viewDialog").on("click", function (e) {
        e.preventDefault();

        $("<div></div>")
            .addClass("dialog")
            .appendTo("body")
            .dialog({
                title: $(this).attr("data-dialog-title"),
                close: function () { $(this).remove() },
                modal: true
            })
            .load(this.href);
    });
});

//to replace the groups depending on the choice of the university in the DropDownList
$(function () {
    $('#university').change(function () {
        var id = $(this).val();
        $.ajax({
            type: 'GET',
            url: '/Home/GetGroups/' + id,
            success: function (data) {
                $('#group').replaceWith(data);
            }
        });
    });
});