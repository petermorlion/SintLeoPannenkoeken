window.homeCharts = {
    loadIngaveTotalen: (data) => {
        const ingaveTotalenChart = document.getElementById('ingaveTotalenChart');
        new Chart(
            ingaveTotalenChart,
            {
                type: 'line',
                data: {
                    labels: data.ingaveTotalen.map((row) => ''),
                    datasets: [
                        {
                            label: 'Verkoop',
                            data: data.ingaveTotalen.map(row => row.oplopendTotaal),
                            backgroundColor: '#9dc3ca',
                            fill: true,
                            tension: 0.4,
                            cubicInterpolationMode: 'monotone',
                        },
                    ]
                },
                options: {
                    scales: {
                        y: {
                            min: 0,
                            max: data.ingaveTotalen.at(-1)?.oplopendTotaal + 20 ?? 100
                        }
                    },
                    elements: {
                        point: {
                            pointStyle: false
                        }
                    }
                }
            }
        );
    },
    loadVerkoopPerPostcode: (data) => {
        const verkoopPerPostcodeChart = document.getElementById('verkoopPerPostcodeChart');
        new Chart(
            verkoopPerPostcodeChart,
            {
                type: 'doughnut',
                data: {
                    labels: Object.keys(data),
                    datasets: [
                        {
                            label: 'Aantal pakken',
                            data: Object.values(data),
                            backgroundColor: Object.keys(data).map((value, index) => index % 2 === 0 ? '#9dc3ca' : '#004474'),
                        },
                    ]
                },
            }
        );
    }
}