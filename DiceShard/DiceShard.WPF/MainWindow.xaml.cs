using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts;
using LiveCharts.Wpf;

namespace DiceShard
{
    public partial class MainWindow : Window
    {
        Dice Dice { get; set; } = new Dice();
        MyMenu slideMenu = new MyMenu();
        public ChartValues<double> SC { get; set; }
        public Animation RollAnimation { get; set; }
        public SeriesCollection PC { get; set; }
        public string[] BarLabels { get; set; }
        public Func<ChartPoint, string> PieFormater { get; set; }
        public Func<ChartPoint, string> Formater { get; set; }
        public ChartValues<double> TmpValue { get; set; }
        const string pathTo3dModel = @"../../Images/dice.obj";

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                Dice.MyWindow = this;
                slideMenu.Dice = Dice;
                slideMenu.MyWindow = this;
                Butt1.Click += Dice.Draw;
                
                Reset_Button.Click += (o, e) => slideMenu.ResetCharts();
                Save_Button.Click += (o, e) => slideMenu.TryToSaveFile();
                Load_Button.Click += (o, e) => slideMenu.TryToReadFile();

                TG1.Checked += slideMenu.CloseOtherActiveWindow; 
                TG2.Checked += slideMenu.CloseOtherActiveWindow;
                TG1.Checked += (o, e) => { Panel.SetZIndex(Res2, 2); slideMenu.IsActivated = true;};
                TG2.Checked += (o, e) => { Panel.SetZIndex(Res2, 2); slideMenu.IsActivated = true; };
                TG1.Unchecked += (o, e) => { Panel.SetZIndex(Res2, -1); slideMenu.IsActivated = false; };
                TG2.Unchecked += (o, e) => { Panel.SetZIndex(Res2, -1); slideMenu.IsActivated = false; };
                Res2.MouseLeftButtonDown += (o, e) => slideMenu.CloseNavsMenuOnClick(new ToggleButton[] { TG1, TG2 });


                Object3D myobject = new Object3D(pathTo3dModel, "object1", this);
                RollAnimation = new Animation("MyStoryboard", this);
                TmpValue = new ChartValues<double> { 0.0 };
                SC = new ChartValues<double> { 0, 0, 0, 0, 0, 0 };
                PC = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "1",
                    Values = new ChartValues<double>{0},
                },
                new PieSeries
                {
                    Title = "2",
                    Values = new ChartValues<double>{0},
                },
                new PieSeries
                {
                    Title = "3",
                    Values = new ChartValues<double>{0},
                },
                new PieSeries
                {
                    Title = "4",
                    Values = new ChartValues<double>{0},
                },
                new PieSeries
                {
                    Title = "5",
                    Values = new ChartValues<double>{0},
                },
                new PieSeries
                {
                    Title = "6",
                    Values = new ChartValues<double>{0},
                }
            };

                BarLabels = new[] { "1", "2", "3", "4", "5", "6" };

                RollAnimation.myStoryBoard.Completed += (o,e) => UpdateValueForChars();

                PieFormater = point => point.Participation.ToString() != "0" ? point.Participation.ToString("##%") : "";
                Formater = point => point.Y != 0 ? point.Y.ToString() : "";
                DataContext = this;
                MouseDown += (o, e) => DragMove();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }
        private void Window_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void UpdateValueForChars()
        {
            TmpValue[0] = (double)PC[Dice.Number - 1].Values[0];
            TmpValue[0] += 1;
            PC[Dice.Number - 1].Values = new ChartValues<double> { TmpValue[0] };
            SC[Dice.Number - 1] = TmpValue[0];
        }

    }
}
