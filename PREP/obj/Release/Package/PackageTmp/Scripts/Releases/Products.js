$(document).ready(function () {
    $("#FamilyCB").change(selectFamilyProduct);


});

var selectFamilyProduct = function () {
    if (this.checked)
        $('.family-item:first > .product-item ').show()
    else
    {
        $('.family-item:first > .product-item ').hide();
        $('.family-item:first > .product-item > input[type=checkbox]').attr('checked', false);
    }

}