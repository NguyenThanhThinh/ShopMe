$(document).ready(function () {
    $('.loginshopme').off('click').on('click', function (e) {
        var tk = $(this).closest(".poplogin").find("#Username_1").val();
        var mk = $(this).closest(".poplogin").find("#dn_matkhau").val();
        var loi1 = 0;
        if (tk == "" || mk == "") {
            loi1++;
            toastr.warning("Hãy nhập đầy đủ thông tin");

        }
        if (tk.length < 6) {
            loi1++;
            toastr.warning("Tài khoản phải lớn hơn 6 ký tự");
        }
        if (loi1 != 0) {
            return false;
        }
        else {
            $.ajax({
                type: 'post',
                url: '/Account/Login',
                dataType: 'json',
                data: { username: tk, password: mk },
                success: function (result) {
                    if (result.status == true) {
                        toastr.success(result.message);
                        window.location.reload();
                    }
                    else {
                        toastr.warning(result.message);
                       
                    }
                }
            });
        }

    });
    $(document).keypress(function (e) {
        if (e.which == 13) {
            $(".loginshopme").trigger("click");
        }
    });
})