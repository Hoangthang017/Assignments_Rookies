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

// cart
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
            window.location.href = response.redirectToUrl;
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
        }
    });
})

$('body').on('click', '.quantity-right-plus', function (e) {
    e.preventDefault();
    var currentQuantity = $('.input-order-quantity').val();
    $('.input-order-quantity').val(parseInt(currentQuantity) + 1);
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