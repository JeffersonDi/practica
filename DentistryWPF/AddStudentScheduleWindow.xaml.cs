using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using Application = System.Windows.Application;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для AddStudentScheduleWindow.xaml
    /// </summary>
    public partial class AddStudentScheduleWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable studentsTable;
        public DataTable dt_ = new DataTable();
        public string Staff, Date, Time;
        public int id_Sche, id_Staff, col, row;

        public AddStudentScheduleWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void UpdateDB()
        {
            string sql = "SELECT Students.ID, Students.Last_name + ' ' + Students.First_name + ' ' + Students.Patronymic FLP, Students.Date_of_birth, Students.Сheck_for_deletion, Contact_details.Phone Phones " +
                "FROM Students " +
                "INNER JOIN Contact_details ON Students.ID = Contact_details.id_Student " +
                "WHERE Students.Сheck_for_deletion = 1";

            studentsTable = new DataTable();
            SqlConnection connection = null;
            //Fill_comboBox();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(studentsTable);
                patientsGrid.ItemsSource = studentsTable.DefaultView;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void SQLStudentDelete(int ID_Student)
        {
            string sql =
                    "UPDATE Students " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Students.ID = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные студента изменены");
                    //this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    //this.Close();
                }
            }
        }

        private void SQLContact_detailsDelete(int ID_Student)
        {
            string sql =
                    "UPDATE Contact_details " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Contact_details.id_Student = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные студента изменены");
                    //this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    //this.Close();
                }
            }
        }

        private void SQLPassport_datasDelete(int ID_Student)
        {
            string sql =
                    "UPDATE Passport_datas " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Passport_datas.id_Student = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                //adapter = new SqlDataAdapter(command);

                //string country, region, city, street, numberHome, numberApartment;

                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные студента изменены");
                    //this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    //this.Close();
                }
            }
        }
                
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TB_Search.Text == "")
            {
                studentsTable.DefaultView.RowFilter = "";
                return;
            }
            studentsTable.DefaultView.RowFilter = string.Format("[FLP] Like '%{0}%'", TB_Search.Text);
            patientsGrid.ItemsSource = studentsTable.DefaultView;
        }

        private void MI_List_EdPatient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = patientsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                AddEddStudentWindow addEddStudentWindow = new AddEddStudentWindow();
                addEddStudentWindow.ID_Student = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                addEddStudentWindow.Title = "Редактирование записи студента";
                //addEdPatientWindow.Title = ((drv.Row[0] ?? String.Empty).ToString());
                addEddStudentWindow.ShowDialog();
                UpdateDB();
            }
        }

        private void MI_List_delPatient_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = patientsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                if (System.Windows.MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?",
                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int ID_Patient = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                    SQLStudentDelete(ID_Patient);
                    SQLContact_detailsDelete(ID_Patient);
                    SQLPassport_datasDelete(ID_Patient);
                    //SQLRecordDelete(ID_Patient);
                    UpdateDB();
                }
            }
            else
                System.Windows.MessageBox.Show("Выберете запись для успешного удаления");
        }

        private void MI_List_AddEd_Record_Click(object sender, RoutedEventArgs e)
        {
            //DataRowView drv = patientsGrid.SelectedItem as DataRowView;
            //if (drv != null)
            //{   AddEdPatientRecordsWindow addEdPatientRecordsWindow = new AddEdPatientRecordsWindow();
            //    addEdPatientRecordsWindow.ID_Patient = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
            //    addEdPatientRecordsWindow.FLP = (drv.Row[1] ?? String.Empty).ToString();

            //    addEdPatientRecordsWindow.Title = "Карта пациента '" + (drv.Row[1] ?? String.Empty).ToString() + "'";
            //    addEdPatientRecordsWindow.ShowDialog();
            //}
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddEddStudentWindow addEddStudentWindow = new AddEddStudentWindow();
            addEddStudentWindow.ID_Student = 0;
            addEddStudentWindow.Title = "Добавление записи пациента" + addEddStudentWindow.ID_Student.ToString();

            addEddStudentWindow.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = patientsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                if (System.Windows.MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?",
                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int ID_Student = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                    SQLStudentDelete(ID_Student);
                    SQLContact_detailsDelete(ID_Student);
                    SQLPassport_datasDelete(ID_Student);
                    UpdateDB();
                }
            }
            else
                System.Windows.MessageBox.Show("Выберете запись для успешного удаления");
        }

        private void patientsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataRowView drv = patientsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                if (System.Windows.MessageBox.Show(this, "Записать пациента,'" + (drv.Row[1] ?? String.Empty).ToString() + "', к '" + Staff + " " + Date + "', на: " + Time + "?",
                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SchedulesWindow schedulesWindow = new SchedulesWindow();

                    schedulesWindow.SQLTimeInsert(Convert.ToInt32(drv.Row[0] ?? String.Empty), col, row, dt_, Date);
                    //schedulesWindow.SQL_SelectScheld();
                    schedulesWindow.Close();
                    this.Close();
                }
            }
        }
    }
}
