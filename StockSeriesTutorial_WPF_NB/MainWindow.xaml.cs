﻿// ------------------------------------------------------------------------------------------------------
// LightningChart® example code: StockSeries Chart Demo.
//
// If you need any assistance, or notice error in this example code, please contact support@arction.com. 
//
// Permission to use this code in your application comes with LightningChart® license. 
//
// http://arction.com/ | support@arction.com | sales@arction.com
//
// © Arction Ltd 2009-2019. All rights reserved.  
// ------------------------------------------------------------------------------------------------------
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

// Arction namespaces.
using Arction.Wpf.Charting;          // LightningChartUltimate and general types.
using Arction.Wpf.Charting.SeriesXY; // Series for 2D chart.

namespace StockSeriesTutorial_WPF_NB
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create chart.
            var chart = new LightningChartUltimate();
            chart.Title.Text = "Stock Series";

            // Disable rendering before updating chart properties to improve performance
            // and to prevent unnecessary chart redrawing while changing multiple properties.
            chart.BeginUpdate();

            // Set chart control into the parent container.
            (Content as Grid).Children.Add(chart);

            // 1. Configure X- and Y-axes.

            // X-axis configuration.
            var xAxis = chart.ViewXY.XAxes[0];
            xAxis.Title.Text = "Date";
            xAxis.ValueType = AxisValueType.DateTime;
            xAxis.LabelsAngle = 90;
            xAxis.MajorDiv = 24 * 60 * 60; // Major division is one day in seconds.

            // Y-axis configuration.
            var yAxis = chart.ViewXY.YAxes[0];
            yAxis.Title.Text = "Price";

            // 2. Create a new StockSeries.
            var stockSeries = new StockSeries(chart.ViewXY, xAxis, yAxis);
            chart.ViewXY.StockSeries.Add(stockSeries);

            // 3. Configure the stock plot.
            stockSeries.Style = StockStyle.OptimizedCandleStick;
            stockSeries.FillBorder.Width = 1;
            stockSeries.Title.Text = "Example Inc.";

            // 4. Load data from a CSV file into series data points with LoadFromCSV.
            /*
             * The data in CSV file has to be organized in columns in the following order:
             * Column 0: Date (DateTime)
             * Column 1: Open (double)
             * Column 2: Close (double)
             * Column 3: High (double)
             * Column 4: Low (double)
             * Column 5: Volume (int)
             * Column 6: Transaction (double)
             */
            stockSeries.LoadFromCSV("../../../data/data.csv", SeparatorCSV.Semicolon);

            // 5. Create a reference to the loaded data points.
            var stockData = stockSeries.DataPoints;

            // 6. Generate data for series, which matches closed values.
            var closeData = new SeriesPoint[stockData.Length];
            for (var i = 0; i < stockData.Length; i++)
            {
                closeData[i] = new SeriesPoint()
                {
                    X = xAxis.DateTimeToAxisValue(stockData[i].Date),
                    Y = stockData[i].Close
                };
            }

            // 7. Create a new PointLineSeries to show the dynamic in closed values on Stock Exchange.
            var lineSeries = new PointLineSeries();
            lineSeries.Title.Text = "Example Inc.";
            lineSeries.Points = closeData;
            chart.ViewXY.PointLineSeries.Add(lineSeries);

            // 8. Auto-scale X- and Y-axes.
            chart.ViewXY.ZoomToFit();

            #region Hidden polishing
            CustomizeChart(chart);
            #endregion

            // Call EndUpdate to enable rendering again.
            chart.EndUpdate();
        }

        #region Hidden polishing
        private void CustomizeChart(LightningChartUltimate chart)
        {
            chart.ChartBackground.Color = Color.FromArgb(255, 30, 30, 30);
            chart.ChartBackground.GradientFill = GradientFill.Solid;
            chart.ViewXY.GraphBackground.Color = Color.FromArgb(255, 20, 20, 20);
            chart.ViewXY.GraphBackground.GradientFill = GradientFill.Solid;
            chart.Title.Color = Color.FromArgb(255, 249, 202, 3);
            chart.Title.MouseHighlight = MouseOverHighlight.None;

            foreach (var yAxis in chart.ViewXY.YAxes)
            {
                yAxis.Title.Color = Color.FromArgb(255, 249, 202, 3);
                yAxis.Title.MouseHighlight = MouseOverHighlight.None;
                yAxis.MajorGrid.Color = Color.FromArgb(35, 255, 255, 255);
                yAxis.MajorGrid.Pattern = LinePattern.Solid;
                yAxis.MinorDivTickStyle.Visible = false;
            }

            foreach (var xAxis in chart.ViewXY.XAxes)
            {
                xAxis.Title.Color = Color.FromArgb(255, 249, 202, 3);
                xAxis.Title.MouseHighlight = MouseOverHighlight.None;
                xAxis.MajorGrid.Color = Color.FromArgb(35, 255, 255, 255);
                xAxis.MajorGrid.Pattern = LinePattern.Solid;
                xAxis.MinorDivTickStyle.Visible = false;
            }
        }
        #endregion
    }
}