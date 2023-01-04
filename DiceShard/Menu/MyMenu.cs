using System;
using System.Windows.Controls.Primitives;
using LiveCharts;
using System.IO;
using System.Windows;

namespace DiceShard
{
    class MyMenu
    {
        public bool IsActivated { get; set; } = false;
        public MainWindow MyWindow { get; set; } = null;
        public Dice Dice { get; set; }
        public const string savePath = "./CharData.txt";

        public void CloseNavsMenuOnClick(object[] toggleButtonObject)
        {
            foreach (ToggleButton tg in toggleButtonObject)
            {
                if ((bool)tg.IsChecked)
                {
                    tg.IsChecked = false;
                }
            }
        }
        public void CloseOtherActiveWindow(object sender, EventArgs ea)
        {
            if (IsActivated)
            {
                ((ToggleButton)sender).IsChecked = false;
                if ((bool)MyWindow.TG1.IsChecked)
                {
                    MyWindow.TG1.IsChecked = false;
                }
                if ((bool)MyWindow.TG2.IsChecked)
                {
                    MyWindow.TG2.IsChecked = false;
                }
                ((ToggleButton)sender).IsChecked = true;
            }
        }
        public void ResetCharts()
        {
            var msres = MessageBox.Show("Do you want to reset your own data?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question); //Message result
            if (msres == MessageBoxResult.Yes)
            { 
                foreach (var Serie in MyWindow.PC)
                    Serie.Values = new ChartValues<double> { 0 };
                for (int i = 0; i < MyWindow.SC.Count; i++)
                    MyWindow.SC[i] = 0.0;
                Dice.RollCount = 0;
                MyWindow.NumOfRols.Content = "Roll Counter: 0";
            }
        }
        private void SaveToFile(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(Dice.RollCount);
                foreach(var serie in MyWindow.PC)
                {
                    if (serie != MyWindow.PC[MyWindow.PC.Count-1])
                        writer.Write(serie.Values[0]+" ");
                    else
                        writer.Write(serie.Values[0]+"\n");
                }
                writer.Close();
            }
        }
        private void ReadFile(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                ushort.TryParse(reader.ReadLine(), out ushort som);
                Dice.RollCount = som;
                string[] inputt = reader.ReadLine().Split(' ');
                for(int i = 0; i<inputt.Length; i++)
                {
                    double.TryParse(inputt[i], out double outt);
                    MyWindow.PC[i].Values[0] = outt;
                }
                for(int i = 0; i<MyWindow.SC.Count;i++)
                {
                    MyWindow.SC[i] = (double)MyWindow.PC[i].Values[0];
                }
                MyWindow.NumOfRols.Content = "Roll Counter: " + Dice.RollCount.ToString();
                reader.Close();
            }
        }
        public void TryToSaveFile()
        {
            try
            {
                using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate))
                { 
                    var ms = MessageBox.Show("Do you want to save your data?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ms == MessageBoxResult.Yes)
                    { 
                        SaveToFile(fs);
                        fs.Close();
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"File error {0}", e.ToString());
                throw;
            }
        }
        public void TryToReadFile()
        {
            try
            {
                using (FileStream fs = new FileStream(savePath, FileMode.Open))
                { 
                    var ms = MessageBox.Show("Do you want to load your data?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ms == MessageBoxResult.Yes)
                    { 
                        ReadFile(fs);
                        fs.Close();
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("You doesn't have file to open, you need to save you data first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    

}
