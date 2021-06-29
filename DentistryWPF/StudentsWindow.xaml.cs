using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Window
    {
        public int idUser, Gender;
        public string Last_name, First_name, Patronomyc, Date_b;

        string connectionString;
        SqlDataAdapter adapter;
        DataTable studentsTable;
        public StudentsWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        private void Fill_comboBox()
        {
            DataTable dataTable_Patients = new DataTable();
            DB db = new DB();

            string sql = "SELECT Last_name FROM Students";

            try
            {
                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);

                db.openConnection();
                adapter.Fill(dataTable_Patients);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (db.connection != null)
                    db.connection.Close();
            }
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
            Fill_comboBox();
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
                //adapter = new SqlDataAdapter(command);

                //string country, region, city, street, numberHome, numberApartment;

                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные студента удалены");
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
                    //System.Windows.Forms.MessageBox.Show("Данные студента изменены");
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
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    //System.Windows.Forms.MessageBox.Show("Данные студента изменены");
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

        private void MI_List_Delete_Patient_Click(object sender, RoutedEventArgs e)
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
                    //SQLRecordDelete(ID_Student);
                    UpdateDB();
                }
            }
            else
                System.Windows.MessageBox.Show("Выберете запись для успешного удаления");
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateDB();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddEddStudentWindow addEddStudentWindow = new AddEddStudentWindow();
            addEddStudentWindow.ID_Student = 0;
            addEddStudentWindow.Title = "Добавление записи студента";

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
                AddEddStudentWindow addEddStudentWindow = new AddEddStudentWindow();
                addEddStudentWindow.ID_Student = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                addEddStudentWindow.Title = "Редактирование записи студента";
                //addEdPatientWindow.Title = ((drv.Row[0] ?? String.Empty).ToString());
                addEddStudentWindow.ShowDialog();
                UpdateDB();
            }
        }
    }
}
