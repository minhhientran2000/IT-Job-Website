function GenerateChart(countryInfos) {
    am4core.useTheme(am4themes_animated);

    var chart = am4core.create("chartdiv", am4charts.PieChart3D);
    chart.hiddenState.properties.opacity = 0;
    chart.legend = new am4charts.Legend();
    chart.data = countryInfos;
    var series = chart.series.push(new am4charts.PieSeries3D());
    series.dataFields.value = "percent";
    series.dataFields.category = "name";
}


function GeneratePieChart(companyData) {
    am4core.useTheme(am4themes_animated);

    var chart = am4core.create("piechartdiv", am4charts.PieChart3D);
    chart.hiddenState.properties.opacity = 0;
    chart.legend = new am4charts.Legend();

    // Prepare data
    let chartData = [
        { name: "Approved", percent: companyData.approvedPercent },
        { name: "Rejected", percent: companyData.rejectedPercent }
    ];

    chart.data = chartData;

    var series = chart.series.push(new am4charts.PieSeries3D());
    series.dataFields.value = "percent";
    series.dataFields.category = "name";
}

function GenerateHistogram(list) {
    am4core.useTheme(am4themes_animated);

    // Source data
    var data = list;

    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    function getMonthlyData(source) {
        var monthlyData = [];
        for (var i = 0; i < months.length; i++) {
            monthlyData.push({
                month: months[i],
                total: 0,
                color: am4core.color("#104547") // Mặc định màu cho cột
            });
        }

        for (var i = 0; i < source.length; i++) {
            var monthIndex = source[i].month - 1; // Convert month to zero-based index
            monthlyData[monthIndex].total += source[i].userCount;
        }

        return monthlyData;
    }

    // Mảng chứa các màu
    var colors = ["#104547", "#20794c", "#3c6e71", "#5d5d5a", "#7a5335", "#a73e46", "#d43f51", "#f57447", "#ff7b9b", "#b77a9e", "#725ac1", "#4e2c70"];

    // Create chart instance
    var chart = am4core.create("graphdiv", am4charts.XYChart);

    // Add data
    chart.data = getMonthlyData(data);

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "month";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "total";
    series.dataFields.categoryX = "month";
    series.columns.template.tooltipText = "{month}\n[bold]Total: {total}[/]";

    // Áp dụng màu cho từng cột
    series.columns.template.adapter.add("fill", function (fill, target) {
        return colors[target.dataItem.index];
    });

}


function GenerateStaticUserHistogram(data) {
    am4core.useTheme(am4themes_animated);

    // Source data
    var sourceData = [
        { month: "Tháng này", total: data.thisMonth },
        { month: "Tháng trước", total: data.lastMonth }
    ];

    // Mảng chứa các màu
    var colors = ["#104547", "#20794c"]; // Thay đổi màu tại đây

    // Create chart instance
    var chart = am4core.create("staticgraphdiv", am4charts.XYChart);

    // Add data
    chart.data = sourceData;

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "month";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "total";
    series.dataFields.categoryX = "month";
    series.columns.template.tooltipText = "{month}\n[bold]Total: {total}[/]";

    // Áp dụng màu cho từng cột
    series.columns.template.adapter.add("fill", function (fill, target) {
        return colors[target.dataItem.index];
    });

    series.columns.template.width = am4core.percent(20);
}


/*function GenerateStaticUserHistogram(data) {
    am4core.useTheme(am4themes_animated);

    // Source data
    var sourceData = [
        { month: "Tháng này", total: data.thisMonth },
        { month: "Tháng trước", total: data.lastMonth }
    ];

    // Create chart instance
    var chart = am4core.create("staticgraphdiv", am4charts.XYChart);

    // Add data
    chart.data = sourceData;

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "month";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "total";
    series.dataFields.categoryX = "month";
    series.columns.template.tooltipText = "{month}\n[bold]Total: {total}[/]";
    series.columns.template.fill = am4core.color("#104547"); // fill color

    series.columns.template.width = am4core.percent(20);
}*/
