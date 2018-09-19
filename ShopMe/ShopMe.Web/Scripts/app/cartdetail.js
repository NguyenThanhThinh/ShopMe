var cart = {
    init: function () {
        cart.loadData1();
      
        cart.registerEvent();
    },
    registerEvent: function () {
        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem1(productId);
        });
    },
  
    deleteItem1: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData1();
                }
            }
        });
    },
    getTotalOrder1: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    loadData1: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template1 = $('#tplCart1').html();
                    var html1 = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html1 += Mustache.render(template1, {
                            ProductId1: item.ProductId,
                            ProductName1: item.Product.Name,
                            Image1: item.Product.Image,
                            Price1: item.Product.Price,
                            PriceF1: numeral(item.Product.Price).format('0,0'),
                            Quantity1: item.Quantity,
                            Amount1: numeral(item.Quantity * item.Product.Price).format('0,0')
                        });
                    });

                    $('#cartBody1').html(html1);

                    if (html1 == '') {
                        $('#cartContent1').html('Không có sản phẩm nào trong giỏ hàng.');
                    }
                    $('#lblTotalOrder1').text(numeral(cart.getTotalOrder1()).format('0,0'));
                    cart.registerEvent();
                }
            }
        });
    },
   

}
cart.init();