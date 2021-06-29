using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Media;
//using System.Drawing;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCalendar_Click(object sender, RoutedEventArgs e)
        {
            SchedulesWindow schedulesWindow = new SchedulesWindow();
            schedulesWindow.ShowDialog();
        }

        private void buttonStaff_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow staffWindow = new StaffWindow();
            staffWindow.ShowDialog();
        }

        private void buttonStudents_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            studentsWindow.ShowDialog();
        }
    }
}