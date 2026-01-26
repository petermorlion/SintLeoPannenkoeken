window.homeCharts = {
    loadIngaveTotalen: (data) => {
        const ingaveTotalenChart = document.getElementById('ingaveTotalenChart');
        console.log('chart', ingaveTotalenChart);
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
    }
}