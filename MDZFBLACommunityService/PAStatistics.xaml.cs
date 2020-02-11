﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MDZFBLACommunityService
{
    /// <summary>
    /// Interaction logic for PAStatistics.xaml
    /// </summary>
    public partial class PAStatistics : Page
    {
        public PlotModel plt;

        public PAStatistics()
        {
            
            InitializeComponent();

            var peple = Database.People();
            var b = peple.OrderByDescending(c => c.SumHours);
            var o = peple.OrderBy(c => c.SumHours);

            try
            {
                TopFiveStudentsListBox.ItemsSource = b.ToList().GetRange(0, 5);
                LastFiveStudentsListBox.ItemsSource = o.ToList().GetRange(0, 5);

            }
            catch
            {
                MessageBox.Show("Must have at least 5 students to properly generate top 5 lists");
            }
            try
            {
                MakeModel();
            }
            catch
            {
                MessageBox.Show("Model cannot be properly made without a student in all grades");

            }

            try
            {
                LabelAwards();
            }
           
            catch
            {

            }

            
        }



        public void MakeModel()
        {
            //generates model
            var peple = Database.People();
            plt = new PlotModel();
            plt.Title = "Average Hours Per Grade";

            double nine = peple.Where(fc => fc.Grade == 9).Select(gh => gh.SumHours).Average();
            double ten = peple.Where(fc => fc.Grade == 10).Select(gh => gh.SumHours).Average();
            double eleven = peple.Where(fc => fc.Grade == 11).Select(gh => gh.SumHours).Average();
            double twelve = peple.Where(fc => fc.Grade == 12).Select(gh => gh.SumHours).Average();


            plotview.Model = plt;

            string[] str = { "9th", "10th", "11th", "12th" };



            var barser = new ColumnSeries();
            plt.Series.Add(barser);
            barser.Items.Add(new ColumnItem { Value = nine, Color = OxyColor.Parse("#f1c40f") });
            barser.Items.Add(new ColumnItem { Value = ten, Color = OxyColor.Parse("#7EBDC3") });
            barser.Items.Add(new ColumnItem { Value = eleven, Color = OxyColor.Parse("#D8E4FF") });
            barser.Items.Add(new ColumnItem { Value = twelve, Color = OxyColor.Parse("#31E981") });

            plt.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                ItemsSource = str,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Selectable = false,
            });

        }

       



        private void TopFiveStudentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LastFiveStudentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void LabelAwards()
        {
            //labels awards and creates averages
            var peple = Database.People();
            var unranked = peple.Where(fc => fc.SumHours < 50).Select(gh => gh.SumHours);
            var community = peple.Where(fc => fc.SumHours >= 50 && fc.SumHours < 200).Select(gh => gh.SumHours);
            var service = peple.Where(fc => fc.SumHours >= 200 && fc.SumHours < 500).Select(gh => gh.SumHours);
            var achievement = peple.Where(fc => fc.SumHours >= 500).Select(gh => gh.SumHours);

            AmountUnrankedLabel.Content = unranked.Count();
            AverageUnrankedLabel.Content = (int)unranked.Average();

            AmountCommunityLabel.Content = community.Count();
            AverageCommunityLabel.Content = (int)community.Average();

            AmountServiceLabel.Content = service.Count();
            AverageServiceLabel.Content = (int)service.Average();

            AmountAchievementLabel.Content = achievement.Count();
            AverageAchievementLabel.Content = (int)achievement.Average();

            var ad = peple.Select(g => g.SumHours);

            TotalStudents.Content = ad.Count();
            TotalAverage.Content = (int)ad.Average();

        }



    }
}
