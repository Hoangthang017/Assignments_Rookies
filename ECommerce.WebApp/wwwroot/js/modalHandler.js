function resetAlertModal() {
    var alertModal = $('#alert-modal-message');
    var btnContinue = $('.btn-modal-continue');

    alertModal.removeClass();
    alertModal.addClass("alert");
    btnContinue.removeAttr('href');
    btnContinue.removeAttr("hidden");
}

function addAlertModalInfor(text, type, message, href) {
    var alertModal = $('#alert-modal-message');

    $('.modal-title').text(text);
    alertModal.addClass(type);
    alertModal.text(message);
    $('.btn-modal-continue').attr("href", href);
}

// show/hide modal
$(".btn-modal-close").click(function (e) {
    e.preventDefault();
    $('#modal-alert').modal('toggle');
})

$(".btn-modal-logout").click(function (e) {
    e.preventDefault();
    $('#modal-alert').modal('show');
})

// redirect to external page
$(".redirect-external-page").click(function (e) {
    e.preventDefault();
    var externalLink = $(this).attr("href");
    resetAlertModal();
    addAlertModalInfor("Redirect To External Page Confirm", "alert-warning", "Are you sure want to leave shopping?", externalLink);
    $('#modal-alert').modal('show');
});

// logout confirm
$(".btn-modal-logout").click(function () {
    resetAlertModal();
    addAlertModalInfor("Logout Confirm", "alert-warning", "Are you sure want to log out?", "/en-us/account/logout");
    $('#modal-alert').modal('show');
})

// remove item in cart confirm
$(".btn-remove-cart-item").click(function () {
    // get data form element
    var productId = $(this).attr("data-productId");

    // reset modal
    resetAlertModal();

    // hidden / show and add data t0 remove buttoon
    $(".btn-modal-continue").attr("hidden", true);
    $(".btn-confirm-remove").removeAttr("hidden");
    $(".btn-confirm-remove").attr("data-productId", productId);

    // config infor for modal
    addAlertModalInfor("Remove Confirm", "alert-warning", "Are you sure want to remove this item out of your cart?");

    $('#modal-alert').modal('show');
})