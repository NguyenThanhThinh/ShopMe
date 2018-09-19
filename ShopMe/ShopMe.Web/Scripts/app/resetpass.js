$(document).ready(function () {
    $("#btnrestpass1").click(function () {

        var email = $(this).closest(".resetpassword").find("#resetpass").val();

        var error = 0;
        if (email == "") {
            toastr.warning("vui lòng nhập mail");
            error++;
            return;

        }
        else {
            alert(email);
            $.ajax({
                type: 'POST',
                url: '/Account/ResetPassword',
                dataType: 'json',

                data: { resetEmail: email },
                success: function (result) {
                    if (result.status == true) {
                        toastr.success(result.message);
                        window.location.reload();
                    }

                }
            });
        }
    })

})