$(function () {
    $("Select#Day").bind("change keyup input", function () {
        var selectedClassroom = $('select#ClassroomId').find(":selected").attr('value');
        var selectedDay = $('select#Day').find(":selected").attr('value');
        if (selectedClassroom != "" && selectedDay != "") {
            $.getJSON('/Subjects/Hours', { classroom: selectedClassroom, day: selectedDay }, function (hours) {
                var list = $('Select#Hour');
                list.find('option').remove();
                $(hours).each(function (index, hour) {
                    list.append('<option value="' + hour.Value + '">' + hour.Text + '</option>');
                });
            });
        }
    });


    $("Select#ClassroomId").bind("change keyup input", function () {
        var selectedClassroom = $('select#ClassroomId').find(":selected").attr('value');
        var selectedDay = $('select#Day').find(":selected").attr('value');
        if (selectedClassroom != "" && selectedDay != "") {
            $.getJSON('/Subjects/Hours', { classroom: selectedClassroom, day: selectedDay }, function (hours) {
                var list = $('Select#Hour');
                list.find('option').remove();
                $(hours).each(function (index, hour) {
                    list.append('<option value="' + hour.Value + '">' + hour.Text + '</option>');
                });
            });
        }
    });
});