
var login = {
    init: function () {
       
        login.registerEvent();

    },

    registerEvent: function () {
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
                            $("#thongbaodn").text(result.message);
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

        $("#register").on('click',function () {
            var username = $(this).closest(".popresign").find("#UserName").val();
            var password = $(this).closest(".popresign").find("#Password").val();
            var fullname = $(this).closest(".popresign").find("#FullName").val();
           
            var phonenumber = $(this).closest(".popresign").find("#PhoneNumber").val();
            var email = $(this).closest(".popresign").find("#Email").val();
            var replacePassword = $(this).closest(".popresign").find("#ReplacePassword").val();

            var error = 0;
            if (username == "" || password == "" || fullname == "" || email == "")
            {
               
                toastr.warning("vui lòng nhập đầy đủ thông tin");
                error++;

            }
            //kiem tra mail
            dinhdang = /^[0-9A-Za-z]+[0-9A-Za-z_]*@[\w\d.]+\.\w{2,4}$/;
            KTemail = dinhdang.test($('#Email').val());
            if (!KTemail) {
                toastr.warning("Email không hợp lệ");
                error++;                          }
            if (username.length < 6) {
                error++;
                toastr.warning("Tài khoản phải lớn hơn 6 ký tự");
              
            }
            if (password.length < 6) {
                error++;
                toastr.warning("Mật khẩu phải lớn hơn 6 ký tự");
                
            }
            if (password != replacePassword) {
                error++;
                toastr.warning("mật khẩu không trùng khớp");
               
            }

            if (error != 0) {
                return false;
            }
            else {
                $.ajax({
                    type: 'post',
                    url: 'Account/Register',
                    dataType: 'json',
                    data: {
                        FullName: fullname,

                        Password: password,
                        UserName: username,
                        Email: email,
                        PhoneNumber: phonenumber
                    },


                    success: function (result) {
                        if (result.status == true) {
                            toastr.success(result.message);
                            window.location.reload();

                        }
                        else {
                            toastr.error(result.message);
                        }
                    }
                });
            }

        });
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $("#register").trigger("click");
            }
        });

        $('#btnregister1').click(function () {
            $("#myModal6").modal("show");
            $("#myModal5").modal("hide");
        });
        $("#btnlogin").click(function () {
            $("#myModal6").modal("hide");
            $("#myModal5").modal("show");
        });
      





    },
    

}
login.init();
