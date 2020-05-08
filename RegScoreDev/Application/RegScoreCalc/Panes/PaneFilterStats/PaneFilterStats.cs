using System;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace RegScoreCalc
{
    public partial class PaneFilterStats : Pane
    {
        #region Data members

        private ViewBILLING_1 _viewBilling;

        #endregion

        #region Ctors

        public PaneFilterStats(ViewBILLING_1 viewBilling)
        {
            InitializeComponent();

            _viewBilling = viewBilling;
        }

        #endregion

        #region Events

        private void chbShowRed_CheckedChanged(object sender, EventArgs e)
        {
            if (chbShowRed.Checked)
            {
                //Show red data in the grid
                _viewBilling.FilterDocuments(_viewBilling._filterId, "RED");
            }
            else
            {
                //Remove red data from the grid
                _viewBilling.RemoveFilterFromGrid("RED");
            }
        }

        private void chbShowGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (chbShowGreen.Checked)
            {
                //Show green data in the grid
                _viewBilling.FilterDocuments(_viewBilling._filterId, "GREEN");
            }
            else
            {
                //Remove green data from the grid
                _viewBilling.RemoveFilterFromGrid("GREEN");
            }
        }

        #endregion

        #region Operations

        public void ResetData()
        {
            chbShowGreen.Checked = false;
            chbShowRed.Checked = true;

            chartFilterStats.Hide();
            chartFilterStats.Series.Clear();
            chartFilterStats.Titles.Clear();
        }


        #endregion

        #region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
        {
            base.InitPane(views, ownerView, panel, tab);
        }

        protected override void InitPaneCommands(RibbonTab tab)
        {
        }

        public override void UpdatePane()
        {

        }

        #endregion


        #region Implementation

        public void InitPieChart(int RedNoOfDocuments, int GreenNoOfDocuments, int DocumentsCount)
        {
            lblTotalDocuments.Text = "Total: " + DocumentsCount.ToString();
            double redPercentage = Math.Round(((double)RedNoOfDocuments / DocumentsCount) * 100, 2);
            lblRedPercent.Text = "Discordant: " + redPercentage.ToString() + "%";

            double greenPercentage = Math.Round(((double)GreenNoOfDocuments / DocumentsCount) * 100, 2);
            lblGreenPercent.Text = "Concordant: " + greenPercentage.ToString() + "%";


            Color[] pallete = {
                                  Color.Red,
                                  Color.Green
                              };

            //Init pie chart data
            chartFilterStats.Series.Clear();
            chartFilterStats.Titles.Clear();
            chartFilterStats.PaletteCustomColors = pallete;

            chartFilterStats.Titles.Add("Filter Statistics");
            chartFilterStats.Titles[0].Font = lblGreenPercent.Font;

            var seriesFont = new Font( lblGreenPercent.Font.Name, 12);
            Series series = new Series
            {
                Name = "seriesFilterStats",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Pie,
                Font = seriesFont
            };


            chartFilterStats.Series.Add(series);
            series.Points.Add(RedNoOfDocuments);
            series.Points.Add(GreenNoOfDocuments);


            double redPiePercentage = Math.Round(((double)RedNoOfDocuments / (RedNoOfDocuments + GreenNoOfDocuments)) * 100);
            double greenPiePercentage = Math.Round(((double)GreenNoOfDocuments / (RedNoOfDocuments + GreenNoOfDocuments)) * 100);

            var p1 = series.Points[0];
            p1.AxisLabel = RedNoOfDocuments.ToString() + " (" + redPiePercentage.ToString() + "%)";
            //p1.Font = lblGreenPercent.Font;
            p1.LegendText = "Discordant\ndocuments";

            var p2 = series.Points[1];
            p2.AxisLabel = GreenNoOfDocuments.ToString() + " (" + greenPiePercentage.ToString() + "%)";
            //p2.Font = lblGreenPercent.Font;
            p2.LegendText = "Concordant\ndocuments";


            chartFilterStats.Show();
            chartFilterStats.Invalidate();
        }

        #endregion
    }
}
