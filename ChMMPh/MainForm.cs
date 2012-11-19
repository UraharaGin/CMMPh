using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Core.Functions;
using Core.Logic;

namespace ChMMPh
{
    public partial class MainForm : Form
    {
        private readonly Calculations _calculations;

        public MainForm()
        {
            _calculations = new Calculations
                                {
                                    Fxt = new FuncFxt(),
                                    U0 = new FuncU0X(),
                                    U1 = new FuncU1X(),
                                    U2 = new FuncU2X(),
                                    Lenth = 1
                                };

            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            _calculations.K = (int)numericUpDown3.Value;
            _calculations.N = (int)numericUpDown2.Value;
            _calculations.Time = (double)numericUpDown1.Value;

            if (!_calculations.CheckInputsForImplicit())
            {
                MessageBox.Show(null, "Не выполнены условия сходимости",string.Empty);
                return;
            }
            var implRes = _calculations.Imlicite();
            var notImpRes = _calculations.NotImplicite();

            mainChart.Series.Clear();

            var series = mainChart.Series.Add("Явная");
            foreach (var graphPoint in implRes)
            {
                series.Points.Add(new DataPoint(graphPoint.X, graphPoint.Y));
            }

            var seriesNot = mainChart.Series.Add("Неявная");
            foreach (var graphPoint in notImpRes)
            {
                seriesNot.Points.Add(new DataPoint(graphPoint.X, graphPoint.Y));
            }

            // display delta
            deltaChart.Series.Clear();
            var deltaSeries = deltaChart.Series.Add("Дельта");
            var deltaArray = implRes.Zip(notImpRes, (point, graphPoint) => new GraphPoint
                                                                               {
                                                                                   X = point.X,
                                                                                   Y = Math.Abs(point.Y - graphPoint.Y)
                                                                               }).ToList();
            foreach (var graphPoint in deltaArray)
            {
                deltaSeries.Points.Add(new DataPoint(graphPoint.X, graphPoint.Y));
            }
        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
