$('body').on('click', '.btn-add-to-card', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    var culture = "en-us";
    var currentItems = $('#numOfCartItems').text()
    $.ajax({
        type: "GET",
        url: "/" + culture + "/addcart/" + productId.toString(),
        success: function (response) {
            $('#numOfCartItems').text(response.numOfItems);
        }
    });
})

$('body').on('click', '.btn-remove-cart-item', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    $.ajax({
        url: '/cart/' + productId,
        type: 'DELETE',
        success: function (response) {
            window.location.href = response.redirectToUrl;
        }
    });
})

$('body').on('change', '.input-quantity-product-cart', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    var quantity = $(this).val();
    $.ajax({
        type: "PATCH",
        url: "/" + productId + "/updateCart/" + quantity,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location.href = response.redirectToUrl;
        }
    });
})