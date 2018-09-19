$(document).ready(function () {
    $('#btnupdate').click(function () {

        var tk1 = $(this).closest(".popupdate").find("#password1").val();
        var mk1 = $(this).closest(".popupdate").find("#passwordnew").val();
        var passnew = $(this).closest(".popupdate").find("#passwordreplace").val();

        var loi1 = 0;
        if (tk1 == "" || mk1 == "") {
            loi1++;
            toastr.warning("Hãy nhập đầy đủ thông tin");

        }
        if (mk1 != passnew) {
            loi1++;
            toastr.warning("Mật khẩu không trùng khớp")
        }

        if (loi1 != 0) {
            return false;
        }
        else {
            $.ajax({
                url: '/Account/ChangePassword',
                type: 'POST',
                dataType: 'json',
                data: {
                    NewPassword: mk1
                },
                success: function (response) {
                    if (response.status == true)
                    {
                        toastr.success("cập nhật thành công");
                        window.location.reload();

                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": false,
                            "progressBar": false,
                            "positionClass": "toast-top-right",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "4000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        }
                    }
                }
            });
        }
    });
})