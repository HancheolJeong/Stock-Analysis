function createLineChart(chartId, chartLabels, chartDataSets, chartTitle) {
    var ctx = document.getElementById(chartId).getContext('2d');
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: chartLabels,
            datasets: chartDataSets
        },
        options: {
            responsive: false,
            title: {
                display: true,
                text: chartTitle
            },
            scales: {
                xAxes: [{
                    type: 'linear',
                    position: 'bottom'
                }]
            }
        }
    });
}