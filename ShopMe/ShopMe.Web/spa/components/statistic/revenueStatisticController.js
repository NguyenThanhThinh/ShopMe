(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController)

    revenueStatisticController.$inject = ['apiService', '$scope', 'notificationService', '$filter'];
    function revenueStatisticController(apiService, $scope, notificationService, $filter) {
        $scope.tabledata = [];

        $scope.labels = [];
        $scope.series = ['Doanh thu', 'lợi nhuận'];
        $scope.chartdata = [];
        $scope.getStatistic = getStatistic;
        $scope.datePicker = {
            dateStart: new Date().setMonth(new Date().getMonth() - 1),
            dateEnd: new Date()
        };

        function getStatistic() {

            var config = {
                param: {
                    fromDate: moment($scope.datePicker.dateStart).format("YYYY-MM-DD"),
                    toDate: moment($scope.datePicker.dateEnd).format("YYYY-MM-DD")
                }
            }
            if (config.param.fromDate > config.param.toDate) {
                notificationService.displayError('Khoảng thời gian không hợp lệ!');
            }
            else {
                apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                    if (response.data.length === 0)
                        notificationService.displayWarning('Không có dữ liệu trong khoảng thời gian đã chọn!');

                    $scope.tabledata = response.data;
                    var labels = [];
                    var chartData = [];
                    var revenues = [];
                    var benefits = [];
                    $.each(response.data, function (i, item) {
                        labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                        revenues.push(item.Revenues);
                        benefits.push(item.Benefit);
                    });
                    chartData.push(revenues);
                    chartData.push(benefits);

                    $scope.chartdata = chartData;
                    $scope.labels = labels;
                }, function (response) {
                    notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
                });
            }

        }

        getStatistic();

    }


})(angular.module('shopme.statistics'));









