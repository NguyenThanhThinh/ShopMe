var states =
    {
        init: function () {
            states.getall();
        },
        

        
        getall: (function () {
            $.ajax({
                type: "GET",
                url: "/ShoppingCart/getcountries",
                datatype: "Json",
                success: function (data) {
                    $.each(data, function (index, value) {
                        $('#CustomerProvince').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                    });
                }
            });

            $('#CustomerProvince').change(function () {

                $('#CustomerDistricts').empty();

                $.ajax({
                    type: "POST",
                    url: "/ShoppingCart/GetStatesByCountryId",
                    datatype: "Json",
                    data: { countryId: $('#CustomerProvince').val() },
                    success: function (data) {
                        $.each(data, function (index, value) {
                            $('#CustomerDistricts').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                        });
                    }
                });
            });
        })
    }
states.init();