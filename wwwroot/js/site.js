//// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.

// Side bar Toggle

var sidebaropen = false;
var sideBar = document.getElementById('sidebar');

function openSidebar() {
    if (!sidebaropen) {
        sideBar.classList.add("sidebar-responsive");
        sidebaropen = true;
    }
}
function closeSidebar() {
    if (sidebaropen) {
        sideBar.classList.remove("sidebar-responsive");
        sidebaropen = false;
    }
}


//------charts-------
//----Bar chart-------

var barChartOptions = {
    series: [{
        data: [400, 500, 300, 450]
    }],
    chart: {
        type: 'bar',
        height: 350,
        Width: 200,
        toolbar: {
            show: false
        },
    },
    colors: ["#246dec"],
    plotOptions: {
        bar: {
            borderRadius: 1,
            horizontal: false,
            columnWidth: '30%',
            barHeight: '80%',
            gap: '1%'
        }
    },
    dataLabels: {
        enabled: false
    },
    legend: {
        show: false
    },
    xaxis: {
        categories: ['2020', '2021', '2022', '2023'],
    },
    yaxis: {
        title: {
            text: "count"
        }
    }
};

var barChart = new ApexCharts(document.querySelector("#bar-chart"), barChartOptions);
barChart.render();



//-----round chart-----

var areaChartOptions = {
    series: [44, 55, 13, 33],
    chart: {
        width: 400,
        type: 'donut',
    },
    dataLabels: {
        enabled: false
    },
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200,
                toolbar: {
                    show: false
                }
            },
            legend: {
                show: false
            }
        }
    }],
    dataLabels: {
        enabled: false,
    },
    legend: {
        position: 'right',
        offsetY: 0,
        height: 230,
    }
};

var areaChart = new ApexCharts(document.querySelector("#area-chart"), areaChartOptions);
areaChart.render();


function appendData() {
    var arr = chart.w.globals.series.slice()
    arr.push(Math.floor(Math.random() * (100 - 1 + 1)) + 1)
    return arr;
}

function removeData() {
    var arr = chart.w.globals.series.slice()
    arr.pop()
    return arr;
}

function randomize() {
    return chart.w.globals.series.map(function () {
        return Math.floor(Math.random() * (100 - 1 + 1)) + 1
    })
}

function reset() {
    return options.series
}

document.querySelector("#randomize").addEventListener("click", function () {
    chart.updateSeries(randomize())
})

document.querySelector("#add").addEventListener("click", function () {
    chart.updateSeries(appendData())
})

document.querySelector("#remove").addEventListener("click", function () {
    chart.updateSeries(removeData())
})

document.querySelector("#reset").addEventListener("click", function () {
    chart.updateSeries(reset())
})

