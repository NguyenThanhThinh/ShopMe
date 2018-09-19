(function (app) {
    app.controller('productStatisticController', productStatisticController)

    productStatisticController.$inject = ['apiService', '$scope', 'notificationService', '$filter'];
    function productStatisticController(apiService, $scope, notificationService, $filter) {
        $scope.tabledata = [];

        $scope.getStatistic = getStatistic;
        $scope.datePicker = {          
            dateStart: new Date().setMonth(new Date().getMonth()-1),
            dateEnd: new Date()
        };    
        $scope.sortType = 1;
        $scope.numTop = 10;

        $scope.exportData = function () {
            var blob = new Blob([document.getElementById('tblDanhSach').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
            //var dateNow = new Date();
            //saveAs(blob, "Thong_Ke_So_Luong_Ban" + dateNow.getFullYear() + dateNow.getMonth() + dateNow.getDate() + dateNow.getHours() + dateNow.getMinutes() + dateNow.getSeconds() + ".xls");
        };

        function getStatistic() {
                      
            var config = {
                param: {
                    fromDate: moment($scope.datePicker.dateStart).format("YYYY-MM-DD"),
                    toDate: moment($scope.datePicker.dateEnd).format("YYYY-MM-DD")  ,
                    sort: $scope.sortType,
                    top: $scope.numTop
                }
            }
            if (config.param.fromDate > config.param.toDate) {
                notificationService.displayError('Khoảng thời gian không hợp lệ!');
            }
            else {
                apiService.get('api/statistic/getproductstatistic?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate + "&sort=" + config.param.sort + "&top=" + config.param.top
                    , null, function (response) {
                        if (response.data.length === 0) {
                            notificationService.displayWarning('Không có dữ liệu trong khoảng thời gian đã chọn!');
                        }                                                                                          
                        $scope.tabledata = response.data;
                    }, function (response) {
                        notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
                    });
            }               
        }

        getStatistic();

    }


})(angular.module('shopme.statistics'));