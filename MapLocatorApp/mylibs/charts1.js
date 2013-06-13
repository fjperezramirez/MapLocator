function FillSampleChart(chartId, lstFeaturesWithCount) {

    alert(lstFeaturesWithCount.GetItemCount());

    if (lstFeaturesWithCount != null && lstFeaturesWithCount.GetItemCount() > 0) {

        var s1 = [];

        // add counts var s1 = [200, 600, 700, 1000];
        for(int i = 0; i < lstFeaturesWithCount.GetItemCount(); i++){
            var s1[i] = lstFeaturesWithCount[i].Count;
        }

        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        // fill crime Types  var ticks = ['May', 'June', 'July', 'August'];
        var ticks = [];
        for(int i = 0; i < lstFeaturesWithCount.GetItemCount(); i++){
            var ticks[i] = lstFeaturesWithCount[i].Name;
        }
        
        var plot1 = $.jqplot(chartId, [s1], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true }
            },
            // Custom labels for the series are specified with the "label"
            // option on the series option.  Here a series option object
            // is specified for each series.
            series: [
            { label: '' },
            { label: '' },
            { label: '' }
        ],
            // Show the legend and put it outside the grid, but inside the
            // plot container, shrinking the grid to accomodate the legend.
            // A value of "outside" would not shrink the grid and allow
            // the legend to overflow the container.
            legend: {
                show: true,
                placement: 'outsideGrid'
            },
            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                    pad: 1.05,
                    tickOptions: { formatString: '$%d' }
                }
            }
        });
    }
    else {
        alert("No Data Received.");
    }
}