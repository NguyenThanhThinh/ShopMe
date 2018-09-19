var cart = {
    init: function() {

        cart.loadData();


        cart.registerEvent();


    },

    registerEvent: function() {


        //$(".btnAddCart").off('click').on('click', function (e) {

        //    e.preventDefault();
        //    var productId = parseInt($(this).data('id'));
        //    cart.addItem(productId);
        //});
        $(".btnDeleteItem").off("click").on("click",
            function(e) {
                3
                e.preventDefault();
                var productId = parseInt($(this).data("id"));
                cart.deleteItem(productId);
            });
        $(".btnDeleteAll").off("click").on("click",
            function(e) {
                e.preventDefault();
                cart.deleteAll();
            });
        $(".txtQuantity").keydown(function(e) {

            var k = this.value;
            var number1 = 48, number2 = 96;
            //alert(k);
            if (e.keyCode === 8 && k.length === 1) {
                //alert('pressspace');
                e.preventDefault();
                return; //

            }
            if (k.length === 0) { // nud. lnength no > 1 =.="
                number1 = 49;
                number2 = 97;
            }

            //alert(k);
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress // kiểm tra thêm press số 0 thì ko cho nhập 
            if ((e.shiftKey || (e.keyCode < number1 || e.keyCode > 57)) && (e.keyCode < number2 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $(".txtQuantity").keypress(function(event) {
            var k = this.value;
            //alert(k);
            if (event.keyCode === 8 && k.length === 1) {
                //alert('pressspace===');
                return; //

            }
            // Backspace, tab, enter, end, home, left, right
            // We don't support the del key in Opera because del == . == 46.
            var controlKeys = [8, 9, 13, 35, 36, 37, 39];
            // IE doesn't support indexOf
            var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
            // Some browsers just don't raise events for control keys. Easy.
            // e.g. Safari backspace.
            if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
                (49 <= event.which && event.which <= 57) || // Always 1 through 9
                (48 === event.which && $(this).attr("value")) || // No 0 first digit
                isControlKey) { // Opera assigns values for control keys.
                return;
            } else {
                event.preventDefault();
            }
        });
        $(".txtQuantity").off("keyup").on("keyup",
            function() {

                var quantity = parseInt($(this).val());
                var productid = parseInt($(this).data("id"));
                var price = parseFloat($(this).data("price"));

                if (quantity >= 1) {

                    var amount = quantity * price;

                    $("#amount_" + productid).text(numeral(amount).format("0,0"));
                } else {
                    $("#amount_" + productid).text(0);
                }
                cart.updateAll1(productid, quantity); //ham xu ly ve controller
                $("#lblTotalOrder").text(numeral(cart.getTotalOrder()).format("0,0"));
                $("#lblTotalOrder1").text(numeral(cart.getTotalOrder()).format("0,0"));


            });
        $(".txtQuantity").off("change").on("change",
            function() {

                var quantity = parseInt($(this).val());
                var productid = parseInt($(this).data("id"));
                var price = parseFloat($(this).data("price"));

                if (quantity >= 1) {

                    var amount = quantity * price;

                    $("#amount_" + productid).text(numeral(amount).format("0,0"));
                } else {
                    $("#amount_" + productid).text(0);
                }
                cart.updateAll(productid, quantity);
                $("#lblTotalOrder").text(numeral(cart.getTotalOrder()).format("0,0"));
                $("#lblTotalOrder1").text(numeral(cart.getTotalOrder()).format("0,0"));


            });
        $("#btnContinue").off("click").on("click",
            function(e) {
                e.preventDefault();
                window.location.href = "/trang-chu.html";
            });
        //$('body').off('change.txtQuantitychange', '.txtQuantitychange [data-direction]').on('change.txtQuantitychange', '.txtQuantitychange [data-direction]', function () {

        //    var quantity = parseInt($(this).val());
        //    var productid = parseInt($(this).data('id'));
        //    var price = parseFloat($(this).data('price'));
        //    //cart.qty1();
        //    if (isNaN(quantity) == false) {

        //        var amount = quantity * price;

        //        $('#amount_' + productid).text(numeral(amount).format('0,0'));
        //    }
        //    else {
        //        $('#amount_' + productid).text(0);
        //    }

        //    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0'));


        //    cart.updateAll(productid, quantity);
        //    cart.loadData();

        //});

        $('input[name="paymentMethod"]').off("click").on("click",
            function() {
                if ($(this).val() == "NL") {
                    $(".boxContent").hide();
                    $("#nganluongContent").show();
                } else if ($(this).val() == "ATM_ONLINE") {
                    $(".boxContent").hide();
                    $("#bankContent").show();
                } else {
                    $(".boxContent").hide();
                }
            });

        $("#AddCartQuantity").off("click").on("click",
            function(e) {

                var quantity = $(".quantity").val();
                var productId = parseInt($(this).data("id"));

                cart.addItemQuantity(productId, quantity);
                $(".cart-count").css("display", "block");


            });

        $("#btnCreateOrder").off("click").on("click",
            function(e) {
                //e.preventDefault();

                var name = $(this).closest(".frmPayment").find("#txtName").val();
                var phone = $(this).closest(".frmPayment").find("#txtPhone").val();
                var email = $(this).closest(".frmPayment").find("#txtEmail").val();
                var address = $(this).closest(".frmPayment").find("#txtAddress").val();
                var province = $("#CustomerProvince").val();
                var districts = $("#CustomerDistricts").val();
                var paymethod = $('input[name="paymentMethod"]:checked').val();


                var error = 0;
                if (name === "") {
                    error++;
                    toastr.warning("Vui lòng nhập họ tên !");
                    return false;
                }
                if (name.length < 6) {
                    error++;
                    toastr.warning("Họ và tên phải lớn hơn 6 ký tự !");
                    return false;
                }
                if (phone === "") {
                    error++;
                    toastr.warning("Vui lòng nhập số điện thoại !");
                    return false;
                }
                if (phone.length < 9 && phone.length == 12) {
                    error++;
                    toastr.warning("Số điện thoại phải 9 số và không quá 13 số !");
                    return false;
                }

                var filter = /^[0-9-+]+$/;
                var ktphone = filter.test($("#txtPhone").val());

                if (!ktphone) {
                    error++;
                    toastr.warning("số điện thoại phải là số !");
                    return false;
                }
                //kiem tra mail
                dinhdang = /^[0-9A-Za-z]+[0-9A-Za-z_]@[\w\d.]+\.\w{2,4}$/;
                KTemail = dinhdang.test($("#txtEmail").val());
                if (!KTemail) {
                    toastr.warning("Email không hợp lệ");
                    error++;
                    return false;
                }
                if (email === "") {
                    error++;
                    toastr.warning("Vui lòng nhập Email");
                    return false;
                }
                if (email.length < 6) {
                    error++;
                    toastr.warning("Email phải lớn hơn 6 ký tự");
                    return false;
                }
                if (address === "") {
                    error++;
                    toastr.warning("Vui lòng nhập thông tin địa chỉ");
                    return false;
                }
                if (address.length < 6) {
                    error++;
                    toastr.warning("Địa chỉ phải lớn hơn 6 ký tự !");
                    return false;
                }
                if (paymethod === "ATM_ONLINE") {
                    var getbank = $('input[groupname="bankcode"]:checked').prop("id");


                    if (getbank === undefined) {
                        error++;
                        toastr.warning("Bạn phải chọn thẻ ngân hàng thanh toán !")
                        return false;
                    }
                }

                if (province === "") {
                    error++;
                    toastr.warning("Vui lòng chọn tỉnh !");
                    return false;
                }
                if (districts === "") {
                    error++;
                    toastr.warning("Vui lòng chọn huyện !");
                    return false;
                }

                if (error !== 0) {
                    return false;
                } else {
                    $(".loader").css("display", "block");
                    cart.createOrder();
                }

            });

        $("#btnnetxcheckout").off("click").on("click",
            function(e) {
                e.preventDefault();
                window.location.href = "/order/checkout.html";
            });

        $(".adds").off("click").on("click",
            function(e) {
                e.preventDefault();
                window.location.href = "/Order/Detail.html";
            });


        //$('.addss').off('click').on('click', function (e) {

        //    cart.loadData();

        //    $('#shoppingcart-modal').modal('show');
        //});


    },
    addItem: function(productId) {
        $.ajax({
            url: "/ShoppingCart/Add",
            data: {
                productId: productId
            },
            type: "POST",
            dataType: "Json",
            success: function(response) {
                if (response.status == true) {
                    toastr.success("them thanh cong");
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);
                }
            }
        });
    },

    addItemQuantity: function(productId, quantity) {
        $.ajax({
            url: "/ShoppingCart/AddCartQuantity",
            data: {
                productId: productId,
                Quantity: quantity
            },
            type: "POST",
            dataType: "Json",
            success: function(response) {
                if (response.status === true) {

                    toastr.success(response.message);

                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);

                } else {
                    bootbox.confirm({
                        message: "Số lượng tối ta quý khách có thể mua là " +
                            response.quantity +
                            "   bạn có muốn không ?",
                        buttons: {
                            confirm: {
                                label: "Đồng ý",

                                className: "btn-success"
                            },
                            cancel: {
                                label: "Không",
                                className: "btn-danger"
                            }
                        },
                        callback: function(result) {
                            console.log("This was logged in the callback: " + result);
                            if (result == true) {
                                cart.addItemQuantity(productId, response.quantity)
                                $("#lblQuantity").text(response.sumquantity);
                                $("#lblQuantity1").text(response.sumquantity);
                                return;
                            } else {
                                window.location.reload();
                            }
                        }
                    });
                }
            }
        });
    },
    //qty1: function () {

    //    $('body').off('click.txtQuantitychange', '.txtQuantitychange [data-direction]').on('click.txtQuantitychange', '.txtQuantitychange [data-direction]', function () {

    //        var $this = $(this),
    //            d = $this.data('direction'),
    //            input = $this.siblings('input'),
    //            c = +input.val();

    //        if (c == 1 && d == "minus") return;

    //        input.val(d == "minus" ? --c : ++c);

    //    }).on('keydown.txtQuantitychange', '.txtQuantitychange input', function (event) {

    //        var kC = event.keyCode;

    //        if (!((kC >= 48 && kC <= 57) || (kC >= 96 && kC <= 105) || kC == 8)) event.preventDefault();

    //    });

    //},

    deleteItem: function(productId) {
        $.ajax({
            url: "/ShoppingCart/DeleteItem",
            data: {
                productId: productId
            },
            type: "POST",
            dataType: "json",
            success: function(response) {
                if (response.status == true) {
                    cart.loadData();
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);

                }
            }
        });
    },
    deleteAll: function() {
        $.ajax({
            url: "/ShoppingCart/DeleteAll",
            type: "POST",
            dataType: "json",
            success: function(response) {
                if (response.status === true) {
                    cart.loadData();
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);

                }
            }
        });
    },


    getTotalOrder: function() {
        var listTextBox = $(".txtQuantity");
        var total = 0;
        $.each(listTextBox,
            function(i, item) {
                total += parseInt($(item).val()) * parseFloat($(item).data("price"));
            });
        return total;
    },

    updateAll: function(ProductId, Quantity) {


        $.ajax({
            url: "/ShoppingCart/Update",
            type: "POST",
            data: {
                productId: ProductId,
                Quantity: Quantity
            },
            dataType: "json",
            success: function(response) {
                if (response.status === true) {
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);
                    $(".cart-count").css("display", "block");
                    console.log("Update ok");
                } else {
                    bootbox.confirm({
                        message: "Số lượng tối ta quý khách có thể mua là " +
                            response.quantity +
                            "   bạn có muốn không ?",
                        buttons: {
                            confirm: {
                                label: "Đồng ý",

                                className: "btn-success"
                            },
                            cancel: {
                                label: "Không",
                                className: "btn-danger"
                            }
                        },
                        callback: function(result) {
                            console.log("This was logged in the callback: " + result);
                            if (result === true) {
                                cart.updateAll(response.productid, response.quantity);
                                $("#lblQuantity").text(response.sumquantity);
                                $("#lblQuantity1").text(response.sumquantity);
                                cart.loadData();
                                return;
                            } else {
                                window.location.reload();
                            }
                        }
                    });
                }
            }
        });
    },

    updateAll1: function(ProductId, Quantity) {


        $.ajax({
            url: "/ShoppingCart/Update",
            type: "POST",
            data: {
                productId: ProductId,
                Quantity: Quantity
            },
            dataType: "json",
            success: function(response) {
                if (response.status == true) {
                    $("#lblQuantity").text(response.sumquantity);
                    $("#lblQuantity1").text(response.sumquantity);
                    $(".cart-count").css("display", "block");
                    console.log("Update ok");
                    cart.loadData();
                }

            }
        });
    },
    loadData: function() {
        $.ajax({
            url: "/ShoppingCart/GetAll",
            type: "GET",
            dataType: "json",
            success: function(res) {
                if (res.status) {
                    var template = $("#tplCart").html();
                    var html = "";
                    var data = res.data;
                    $.each(data,
                        function(i, item) {
                            html += Mustache.render(template,
                                {
                                    ProductId: item.ProductId,
                                    ProductName: item.Product.Name,
                                    Image: item.Product.Image,
                                    Price: item.Product.Price,
                                    PriceF: numeral(item.Product.Price).format("0,0"),
                                    Quantity: item.Quantity,
                                    Amount: numeral(item.Quantity * item.Product.Price).format("0,0")

                                });
                        });

                    $("#cartBody").html(html);

                    $(".cart-count").css("display", "block");
                    $("#lblQuantity").text(res.sumquantity);
                    $("#lblQuantity1").text(res.sumquantity);

                    if (html === "") {
                        $("#cartContent")
                            .html(
                                '<div class="alert alert-info" role="alert">Không có sản phẩm nào trong giỏ hàng.</div>');
                        $(".nextcart").show();
                        $(".cart-count").css("display", "none");
                    }

                    $("#lblTotalOrder").text(numeral(cart.getTotalOrder()).format("0,0"));
                    var teset = $("#lblTotalOrder1").text(numeral(cart.getTotalOrder()).format("0,0"));

                    cart.registerEvent();


                }
            }
        });
    },
    getLoginUser: function() {
        $.ajax({
            url: "/ShoppingCart/GetUser",
            type: "POST",
            dataType: "json",
            success: function(response) {
                if (response.status) {
                    var user = response.data;
                    $("#txtName").val(user.FullName);
                    $("#txtAddress").val(user.Address);
                    $("#txtEmail").val(user.Email);
                    $("#txtPhone").val(user.PhoneNumber);
                    $("#CustomerProvince").val(user.Provinces);
                    $("#CustomerDistricts").val(user.Districts);

                }
            }
        });
    },


    createOrder: function() {
        var order = {
            CustomerName: $("#txtName").val(),
            CustomerAddress: $("#txtAddress").val(),
            CustomerEmail: $("#txtEmail").val(),
            CustomerMobile: $("#txtPhone").val(),
            CustomerMessage: $("#txtMessage").val(),
            CustomerProvince: $("#CustomerProvince").val(),
            CustomerDistricts: $("#CustomerDistricts").val(),
            PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
            BankCode: $('input[groupname="bankcode"]:checked').prop("id"),
            Status: 10,
            PaymentStatus: 11
        }
        $.ajax({
            url: "/ShoppingCart/CreateOrder",
            type: "POST",
            dataType: "json",
            data: {
                OrderViewModel: JSON.stringify(order)
            },
            success: function(res) {

                if (res.status === true) {

                    if (res.paymenthod === "CASH") {
                        $("#divCheckout").hide();
                        cart.deleteAll();
                        setTimeout(function() {
                                toastr.success(res.message);
                            },
                            200);
                        window.location.href = "/trang-chu.html"

                    }


                } else if (res.status === true && res.urlCheckout != undefined && res.urlCheckout !== "") {

                    $("#divCheckout").hide();
                    toastr.success(res.message);
                    cart.deleteAll();
                    setTimeout(function() {
                            toastr.success(res.message);
                        },
                        200);
                    window.location.href = res.urlCheckout;

                } else {

                    $("#divCheckout").hide();
                    setTimeout(function() {
                            toastr.error(res.message);
                        },
                        200);
                    cart.deleteAll();

                }


            }
        });
    },

}
cart.init();