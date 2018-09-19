(function () {
    $('.btnAddCart').off('click').on('click', function (e) {
        e.preventDefault();
        var productId = parseInt($(this).data('id'));
        $.ajax({
            url: '/ShoppingCart/Add',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true)
                {
                    toastr.options = {
                        "closeButton": true,
                        "debug": false,
                        "positionClass": "toast-top-right",
                        "onClick": null,
                        "showDuration": "300",
                        "hideDuration": "1000",
                        "timeOut": "500",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    };
                    toastr.success('Thêm sản phẩm thành công.');
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);
                    $(".cart-count").css("display", "block");
                    
                }
                else
                {
                    toastr.warning(response.message);
                    //alert(response.message);
                    //$(".cart-count").css("display", "none");
                }
            }
        });
    });
    $('.AddCartQuantityMuaNgay').off('click').on('click', function (e) {
        e.preventDefault();
        var productId = parseInt($(this).data('id'));
        $.ajax({
            url: '/ShoppingCart/Add',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    //toastr.success('Thêm sản phẩm thành công.');
                    //$("#lblQuantity").text(response.sumquantity);
                    //$("#lblQuantity1").text(response.sumquantity);
                    //$(".cart-count").css("display", "block");
                    window.location.href = '/order/checkout.html'
                    $(".cart-count").css("display", "block");
                }
                else {
                    //alert(response.message);
                    //$(".cart-count").css("display", "none");
                    $(".cart-count").css("display", "block");
                }
            }
        });
    });
})();