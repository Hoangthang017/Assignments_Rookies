// product card
$('body').on('click', '.btn-add-to-card', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    var culture = "en-us";
    $.ajax({
        type: "GET",
        url: "/" + culture + "/addcart/" + productId.toString(),
        success: function (response) {
            $('#numOfCartItems').text(response.numOfItems);

            // get element
            var alertModal = $('#alert-modal-message');
            var btnContinue = $('.btn-modal-continue');

            // reset alert
            alertModal.removeClass();
            btnContinue.removeAttr('href');
            $('.btn-modal-continue').attr("hidden", true);

            // add new infor config
            $('.modal-title').text("Add To Cart State");
            alertModal.addClass("alert");
            alertModal.addClass("alert-success");
            alertModal.text("Succes to add to cart");
            $('#modal-alert').modal('show');
        }
    });
})

$('body').on('click', '.buy-now', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    var culture = "en-us";
    $.ajax({
        type: "GET",
        url: "/" + culture + "/addcart/" + productId.toString(),
        success: function (response) {
            window.location.href = "/cart";
        }
    });
})

// show modal -> add data to modal -> call delete from btn of modal
$(".btn-confirm-remove").click(function (e) {
    e.preventDefault();

    // get data form element
    var productId = $(this).attr("data-productId");

    $.ajax({
        url: '/cart/' + productId,
        type: 'DELETE',
        success: function (response) {
            // remove item out of cart
            $('.cart-item-infor-' + productId.toString()).remove();
            $(".btn-confirm-remove").attr("hidden", true);
            $('#modal-alert').modal('toggle');

            // update number of item at navbar
            $('#numOfCartItems').text(parseInt($("#numOfCartItems").text()) - 1);
            $('.cart-items-subTotal').text((parseFloat($(".cart-items-subTotal").text().split(' ')[1]) - response.removeProductTotalPrice).toPrecision(2));
            $('.cart-items-totalPrice').text((parseFloat($(".cart-items-totalPrice").text().split(' ')[1]) - response.removeProductTotalPrice).toPrecision(2));
        }
    });
});

// product detail
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
            /*window.location.href = response.redirectToUrl;*/
            if (response != null) {
                var query = ".total-price-item-" + productId;
                $(query).text("$" + response.productTotalPrice);
                $('.cart-items-subTotal').text(response.subTotal);
                $('.cart-items-totalPrice').text(response.total);
            }
        }
    });
})

$('body').on('click', '.btn-add-to-card-productDetail', function (e) {
    e.preventDefault();

    var productId = $(this).attr("data-productId");
    var quantity = $('.input-order-quantity').val();
    var culture = "en-us";
    $.ajax({
        type: "GET",
        url: "/" + culture + "/addcart/" + productId.toString() + "/" + quantity,
        success: function (response) {
            $('#numOfCartItems').text(response.numOfItems);

            // get element
            var alertModal = $('#alert-modal-message');
            var btnContinue = $('.btn-modal-continue');

            // reset alert
            alertModal.removeClass();
            btnContinue.removeAttr('href');
            $('.btn-modal-continue').attr("hidden", true);

            // add new infor config
            $('.modal-title').text("Add To Cart State");
            alertModal.addClass("alert");
            alertModal.addClass("alert-success");
            alertModal.text("Succes to add to cart");
            $('#modal-alert').modal('show');
        }
    });
})

$('body').on('change', '.input-order-quantity', function (e) {
    e.preventDefault();
    var currentQuantity = $('.input-order-quantity').val();
    var maxQuantity = $('.product-stock-availale').text();
    if (parseInt(currentQuantity) > maxQuantity) {
        // get element
        var alertModal = $('#alert-modal-message');
        var btnContinue = $('.btn-modal-continue');

        // reset alert
        alertModal.removeClass();
        btnContinue.removeAttr('href');
        $('.btn-modal-continue').attr("hidden", true);

        // add new infor config
        $('.modal-title').text("Quantity Available");
        alertModal.addClass("alert");
        alertModal.addClass("alert-danger");
        alertModal.text("Exceed the available quantity");
        $('#modal-alert').modal('show');

        $('.input-order-quantity').val(maxQuantity);
    }
})

$('body').on('click', '.quantity-right-plus', function (e) {
    e.preventDefault();
    var currentQuantity = $('.input-order-quantity').val();
    var maxQuantity = $('.product-stock-availale').text();
    if (parseInt(currentQuantity) < maxQuantity) {
        $('.input-order-quantity').val(parseInt(currentQuantity) + 1);
    }
})

$('body').on('click', '.quantity-left-minus', function (e) {
    e.preventDefault();
    var currentQuantity = $('.input-order-quantity').val();
    if (currentQuantity > 1) {
        $('.input-order-quantity').val(parseInt(currentQuantity) - 1);
    }
})

// rating
function SubmitComment() {
    if ($("#Rating").val() == "0") {
        alert("Please rate this service provider.");
        return false;
    }
    else {
        return true;
    }
}

function CRate(r) {
    $("#Rating").val(parseInt(r));
    for (var i = 1; i <= r; i++) {
        $("#Rate" + i).attr('class', 'starGlow');
    }
    // unselect remaining
    for (var i = r + 1; i <= 5; i++) {
        $("#Rate" + i).attr('class', 'starFade');
    }
}

function CRateOver(r) {
    for (var i = 1; i <= r; i++) {
        $("#Rate" + i).attr('class', 'starGlow');
    }
}

function CRateOut(r) {
    for (var i = 1; i <= r; i++) {
        $("#Rate" + i).attr('class', 'starFade');
    }
}

function CRateSelected() {
    var setRating = $("#Rating").val();
    for (var i = 1; i <= setRating; i++) {
        $("#Rate" + i).attr('class', 'starGlow');
    }
}