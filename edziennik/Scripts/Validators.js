$.validator.unobtrusive.adapters.addBool("pesel");
$.validator.addMethod("pesel", function (pesel, field, params) {
    var weights = [1, 3, 7, 9, 1, 3, 7, 9, 1, 3];
    var ctr = 0;
    for (var i = 0; i < 10; i++)
        ctr += weights[i] * pesel[i];
    ctr = (10 - ctr % 10) % 10;

    return ctr == pesel[10];
}
);

$.validator.unobtrusive.adapters.addBool("uniquepesel");
$.validator.addMethod("uniquepesel", function (pesel, field, params) {

    $.ajax({
        type: 'GET',
        url: '/Account/IsUniquePesel/',
        data: {
            pesel: pesel
        },
        success: function (result) {
            return result;
        }
    });
}
);