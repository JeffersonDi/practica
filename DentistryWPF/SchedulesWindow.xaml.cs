using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Forms;
using System.ComponentModel;
using TextBox = System.Windows.Controls.TextBox;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для SchedulesWindow.xaml
    /// </summary>
    public partial class SchedulesWindow : Window
    {
        public SchedulesWindow()
        {
            InitializeComponent();

        }
        public int row;
        public int col;//столбец       
        public int id_Student;
        private int id_Schedule;
        int i;
        DateTime dateTime;
        DataTable staffsTable;
        DataTable schelTable, dt_Time = new DataTable();
        DataTable dt = new DataTable();
        SqlDataAdapter adapter;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            SQL_SelectStaffs();
            Calendar.SelectedDate = DateTime.Today;
            //SQL_SelectScheld();
            this.Height = Properties.Settings.Default.SchedulesWindow_Height;
            this.Width = Properties.Settings.Default.SchedulesWindow_Width;
            // Very quick and dirty - but it does the job
            if (Properties.Settings.Default.SchedulesWindow_Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void SQL_SelectStaffs()
        {
            string sql = "SELECT ID, Last_name + ' ' + First_name + ' ' + Patronymic FLP_D, Date_of_birth, Сheck_for_deletion " +
                "FROM Staffs " +
                "WHERE Сheck_for_deletion = 1";

            staffsTable = new DataTable();
            DB db = new DB();
            try
            {

                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);

                db.openConnection();
                adapter.Fill(staffsTable);
                Data_Grid_Status.ItemsSource = staffsTable.DefaultView;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (db.getConnection() != null)
                    db.closeConnection();
            }
        }//ТАБЛИЦА ВРАЧЕЙ

        public void SQL_SelectScheld()
        {
            DB db = new DB();
            try
            {
                DataTable dt2 = new DataTable();
                string sql =
                   "SELECT Schedules.ID, Schedules.Date_schedule, Schedules.id_Staff, Schedules.Сheck_for_deletion, " +
                   "Staffs.ID, Staffs.Last_name, Staffs.First_name, Staffs.Patronymic, Staffs.Post " +
                   "FROM Schedules " +
                   "INNER JOIN Staffs ON Schedules.id_Staff = Staffs.ID " +
                   "WHERE Schedules.Сheck_for_deletion = 1 AND Date_schedule = @td ";

                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);
                command.Parameters.Add("@td", SqlDbType.Date).Value = dateTime;

                db.openConnection();

                adapter.Fill(dt);
                adapter.Fill(dt2);

                dt = dt2;
                schelTable = new DataTable();
                schelTable.Clear();
                schelTable.Reset();
                schelTable.Columns.Add("Время", typeof(string));
                schelTable.Rows.Add(" 9:00");
                schelTable.Rows.Add(" 9:30");
                schelTable.Rows.Add("10:00");
                schelTable.Rows.Add("10:30");
                schelTable.Rows.Add("11:00");
                schelTable.Rows.Add("11:30");
                schelTable.Rows.Add("12:00");
                schelTable.Rows.Add("12:30");
                schelTable.Rows.Add("13:00");
                schelTable.Rows.Add("13:30");
                schelTable.Rows.Add("14:00");
                schelTable.Rows.Add("14:30");
                schelTable.Rows.Add("15:00");
                schelTable.Rows.Add("15:30");
                schelTable.Rows.Add("16:00");
                schelTable.Rows.Add("16:30");
                schelTable.Rows.Add("17:00");
                schelTable.Rows.Add("17:30");
                schelTable.Rows.Add("18:00");
                schelTable.Rows.Add("18:30");
                schelTable.Rows.Add("19:00");
                schelTable.Rows.Add("19:30");
                i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    schelTable.Columns.Add((string)row["Post"] + " " + row["Last_name"] + " " + row["First_name"] + " " + row["Patronymic"], typeof(string));
                    sql =
                        "SELECT * " +
                        "FROM Times " +
                        "INNER JOIN Students ON Times.id_Student = Students.ID " +
                        "WHERE id_Schedule = " + (int)row["ID"] + " AND Times.Сheck_for_deletion = 1";

                    command = new SqlCommand(sql, db.getConnection());
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt_Time);

                    foreach (DataRow row_Time in dt_Time.Rows)
                    {
                        int k = (int)row_Time["id_Time"];
                        schelTable.Rows[k][i+1] = (string)row_Time["Last_name"] + " " + row_Time["First_name"].ToString().Substring(0, 1) + ". " + row_Time["Patronymic"].ToString().Substring(0, 1) + ". (" + row_Time["Date_of_birth"].ToString() + ")";                      
                    }
                    
                    Data_Grid_Schedule.ItemsSource = dt_Time.DefaultView;
                    dt_Time = new DataTable();
                    i++;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                Data_Grid_Schedules_1.ItemsSource = schelTable.DefaultView;
                for (int index = 1; index <= i; index++)
                {
                    Data_Grid_Schedules_1.Columns[index].Width = 290;
                    Data_Grid_Schedules_1.Columns[index].CanUserSort = false;
                    //Data_Grid_Schedules_1.Columns[index].MinWidth = 190;
                }
                if (db.getConnection() != null)
                    db.closeConnection();
            }
        }

        private void SQLSchelduleInsert(int id_Staff)
        {
            if (SQL_FindSchedule(id_Staff) == 0)
            {
                string sql =
                        "INSERT INTO Schedules (Date_schedule, id_Staff, Сheck_for_deletion) " +
                        "VALUES (@date_schedule, @id_staff, @check_for_deletion)";

                DB db = new DB();

                SqlCommand command = new SqlCommand(sql, db.getConnection());

                adapter = new SqlDataAdapter(command);

                command.Parameters.Add("@date_schedule", SqlDbType.DateTime).Value = Calendar.SelectedDate;
                command.Parameters.Add("@id_staff", SqlDbType.Int).Value = id_Staff;
                command.Parameters.Add("@check_for_deletion", SqlDbType.Int).Value = 1;

                db.openConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные внесены");
                    //command.CommandText = "SELECT @@IDENTITY";
                    //ID_Scheldule = Convert.ToInt32(command.ExecuteScalar());
                    //this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                db.closeConnection();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Инструктор уже добавлен на эту дату", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable SelectPatient()
        {
            string sql =
                "SELECT ID, Last_name + ' ' + First_name + ' ' + Patronymic + ' (' + Date_of_birth + ') ' FLP, Сheck_for_deletion " +
                "FROM Students " +
                "WHERE Сheck_for_deletion = 1";
            DataTable studentsTable = new DataTable();

            DB db = new DB();
            try
            {
                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);

                db.openConnection();
                adapter.Fill(studentsTable);
                return studentsTable;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if (db.getConnection() != null)
                    db.closeConnection();
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Console.WriteLine(Calendar.SelectedDate.ToString());
            dateTime = (DateTime)Calendar.SelectedDate;
            SQL_SelectScheld();
        }

        private void Data_Grid_Status_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView drv = Data_Grid_Status.SelectedItem as DataRowView;
            if (drv != null)
            {
                SQLSchelduleInsert(Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString()));
                Data_Grid_Schedules_1.Columns.RemoveAt(0);
                SQL_SelectScheld();
            }
        }

        private void Data_Grid_Schedules_1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            row = Data_Grid_Schedules_1.SelectedIndex;//строка
           
            //Console.WriteLine(row.ToString());
            //Console.WriteLine(col.ToString());
            if (Data_Grid_Schedules_1.SelectedIndex >= 0)
            {
                col = Data_Grid_Schedules_1.CurrentCell.Column.DisplayIndex;//столбецъ
                if (SQL_FindTime(row, dt, col) == 0)
                {
                    AddStudentScheduleWindow addStudentScheduleWindow = new AddStudentScheduleWindow();
                    addStudentScheduleWindow.Staff = Data_Grid_Schedules_1.Columns[col].Header.ToString();
                    addStudentScheduleWindow.col = col;
                    addStudentScheduleWindow.row = row;
                    addStudentScheduleWindow.dt_ = dt;
                    addStudentScheduleWindow.Date = Calendar.SelectedDate.ToString().Substring(0, 10);

                    addStudentScheduleWindow.ShowDialog();
                    SQL_SelectScheld();
                }
                else if(SQL_FindTime(row, dt, col) > 0)
                {
                    if (System.Windows.MessageBox.Show(this, "На это время уже записан студент, хотите перейти на форму студента?",
                        "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        AddEddStudentWindow addEddStudentWindow = new AddEddStudentWindow();
                        addEddStudentWindow.ID_Student = SQL_FindTimeID_STUDENT(row, dt, col);
                        addEddStudentWindow.ShowDialog();
                    }
                }else
                    System.Windows.MessageBox.Show("На это время запись не возможна");
            }
        }

        public void SQLTimeInsert(int id_Student_, int col_, int row_, DataTable dt_, string date_)
        {

            id_Schedule = SQL_FindSchedule(col_, dt_, date_);

            Console.WriteLine(id_Schedule.ToString());
            string sql =
                    "INSERT INTO Times (id_Time, id_Schedule, id_Student, Сheck_for_deletion) " +
                    "VALUES (@id_time, @id_schedule, @id_student, @check_for_deletion)";

            DB db = new DB();

            SqlCommand command = new SqlCommand(sql, db.getConnection());

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@id_time", SqlDbType.Int).Value = row_;
            command.Parameters.Add("@id_schedule", SqlDbType.Int).Value = id_Schedule;
            command.Parameters.Add("@id_student", SqlDbType.Int).Value = id_Student_;
            command.Parameters.Add("@check_for_deletion", SqlDbType.Int).Value = 1;

            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {

                System.Windows.Forms.MessageBox.Show("Данные внесены");
                //command.CommandText = "SELECT @@IDENTITY";
                //ID_Scheldule = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            db.closeConnection();
        }

        private void Data_Grid_Schedules_1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow dataGridRow = e.Row;
            dataGridRow.Height = 55;
        }

        private void Data_Grid_Schedules_1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            /*DataGridTextColumn col = e.Column as DataGridTextColumn;

            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            style.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));

            col.ElementStyle = style;*/

            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
                if (descriptor.DisplayName == "Description")
                {
                    var textWrappingSetter = new Setter
                    {
                        Property = TextBox.TextWrappingProperty,
                        Value = TextWrapping.Wrap
                    };

                    // Style for non-edit mode (TextBlock)
                    var elementStyle = new Style(typeof(TextBlock));
                    elementStyle.Setters.Add(textWrappingSetter);

                    // Style for edit mode (TextBox)
                    var editingElementStyle = new Style(typeof(TextBox));
                    editingElementStyle.Setters.Add(textWrappingSetter);

                    var dataGridTextColumn = (DataGridTextColumn)e.Column;
                    dataGridTextColumn.ElementStyle = elementStyle;
                    dataGridTextColumn.EditingElementStyle = editingElementStyle;

                    e.Column.Width = 300;
                }
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void DeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DB db = new DB();
                try
                {
                    string sql =
                        "UPDATE Schedules " +
                        "SET Сheck_for_deletion = 0 " +
                        "WHERE Schedules.ID = @ID_S";
                    SqlCommand command = new SqlCommand(sql, db.getConnection());
                    row = Data_Grid_Schedules_1.SelectedIndex;//строка
                                                              //col = Data_Grid_Schedules_1.CurrentCell.Column.Header;//столбецъ
                    if (Data_Grid_Schedules_1.SelectedIndex >= 0) ///ТРАБЛЫ
                    {
                        col = Data_Grid_Schedules_1.CurrentCell.Column.DisplayIndex;
                        command.Parameters.Add("@ID_S", SqlDbType.Int).Value = SQL_FindSchedule(col, dt, Calendar.SelectedDate.ToString().Substring(0, 10));

                        db.openConnection();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            System.Windows.Forms.MessageBox.Show("Запись удалена");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        db.closeConnection();
                        SQL_SelectScheld();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (db.getConnection() != null)
                        db.closeConnection();
                }
            }        
            //else
                //System.Windows.MessageBox.Show("Выберете запись для успешного удаления"); 
           
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Data_Grid_Schedules_1.SelectedIndex.ToString());
            row = Convert.ToInt32(Data_Grid_Schedules_1.SelectedIndex);//строка
            Console.WriteLine(row);
            if (System.Windows.MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?",
                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string sql =
                    "UPDATE Times " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Times.ID = @ID";

                DB db = new DB();
                SqlCommand command = new SqlCommand(sql, db.getConnection());

                try
                {                   
                    if (row >= 0)
                    {
                        col = Data_Grid_Schedules_1.CurrentCell.Column.DisplayIndex;//столбецъ

                        command.Parameters.Add("@ID", SqlDbType.Int).Value = SQL_FindTime(row, dt, col);

                        db.openConnection();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            System.Windows.Forms.MessageBox.Show("Запись пациента удалена");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        db.closeConnection();
                        SQL_SelectScheld();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (db.getConnection() != null)
                        db.closeConnection();
                }
            }
            else
                System.Windows.MessageBox.Show("Выберете запись для успешного удаления");
        }

        public int SQL_FindSchedule(int col_, DataTable dt_, string date_)
        {
            string sql = 
                "SELECT ID " +
                "FROM Schedules " +
                "WHERE Сheck_for_deletion = 1 AND Date_schedule = @ds AND id_Staff = @iS";

            DataTable dt_S = new DataTable();
            DB db = new DB();
            try
            {
                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);
                command.Parameters.Add("@ds", SqlDbType.Date).Value = date_;
                command.Parameters.Add("@iS", SqlDbType.Int).Value = dt_.Rows[col_-1][2];

                db.openConnection();
                adapter.Fill(dt_S);
                //Console.WriteLine(this.col.ToString());
                return (int)dt_S.Rows[0][0];
            }

            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("На это время запись не возможна");
                Console.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (db.getConnection() != null)
                    db.closeConnection();
            }
        }//ТАБЛИЦА ВРАЧЕЙ
        public int SQL_FindSchedule(int idS)
        {
            string sql =
                "SELECT ID " +
                "FROM Schedules " +
                "WHERE Сheck_for_deletion = 1 AND Date_schedule = @ds AND id_Student = @iS";

            DataTable dt_S = new DataTable();
            DB db = new DB();
            try
            {
                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);
                command.Parameters.Add("@ds", SqlDbType.Date).Value = Calendar.SelectedDate.ToString().Substring(0, 10);
                command.Parameters.Add("@iS", SqlDbType.Int).Value = idS;

                db.openConnection();
                adapter.Fill(dt_S);
                //Console.WriteLine(this.col.ToString());
                return (int)dt_S.Rows[0][0];
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (db.getConnection() != null)
                    db.closeConnection();
            }
        }//ТАБЛИЦА ВРАЧЕЙ

        public int SQL_FindTime(int row_, DataTable dt_, int col_)
        {
            if(SQL_FindSchedule(col_, dt_, Calendar.SelectedDate.ToString().Substring(0, 10)) == 0)
            {
                return -1;
            }
            else
            {
                int id_s = SQL_FindSchedule(col_, dt_, Calendar.SelectedDate.ToString().Substring(0, 10));

                DB db_T = new DB();
                try
                {
                    string sql =
                    "SELECT Times.ID " +
                    "FROM Times " +
                    //"WHERE Сheck_for_deletion = 1 AND id_Schedule = @id_s AND id_Time = @id_t AND id_Patient = @id_p";
                    "WHERE Сheck_for_deletion = 1 AND id_Schedule = @id_s AND id_Time = @id_t";
                    DataTable dt_T = new DataTable();

                    SqlCommand command = new SqlCommand(sql, db_T.getConnection());
                    adapter = new SqlDataAdapter(command);

                    command.Parameters.Add("@id_t", SqlDbType.Int).Value = row_;
                    command.Parameters.Add("@id_s", SqlDbType.Int).Value = id_s;
                    //command.Parameters.Add("@id_p", SqlDbType.Int).Value = ;

                    db_T.openConnection();
                    adapter.Fill(dt_T);

                    Data_Grid_Schedule.ItemsSource = dt_T.DefaultView;

                    return (int)dt_T.Rows[0][0];
                }
                catch (Exception ex)
                {
                    //System.Windows.MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
                finally
                {
                    if (db_T.getConnection() != null)
                        db_T.closeConnection();
                }
            }
            
            
        }//ТАБЛИЦА В

        private void Window_Activated(object sender, EventArgs e)
        {
            //SQL_SelectScheld();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.SchedulesWindow_Height = RestoreBounds.Height;
                Properties.Settings.Default.SchedulesWindow_Width = RestoreBounds.Width;
                Properties.Settings.Default.SchedulesWindow_Maximized = true;
            }
            else
            {
                Properties.Settings.Default.SchedulesWindow_Height = this.Height;
                Properties.Settings.Default.SchedulesWindow_Width = this.Width;
                Properties.Settings.Default.SchedulesWindow_Maximized = false;
            }

            Properties.Settings.Default.Save();
        }

        public int SQL_FindTimeID_STUDENT(int row_, DataTable dt_, int col_)
        {
            int id_s = SQL_FindSchedule(col_, dt_, Calendar.SelectedDate.ToString().Substring(0, 10));

            DB db_T = new DB();
            try
            {
                string sql =
                "SELECT Times.id_Student " +
                "FROM Times " +
                //"WHERE Сheck_for_deletion = 1 AND id_Schedule = @id_s AND id_Time = @id_t AND id_Patient = @id_p";
                "WHERE Сheck_for_deletion = 1 AND id_Schedule = @id_s AND id_Time = @id_t";
                DataTable dt_T = new DataTable();

                SqlCommand command = new SqlCommand(sql, db_T.getConnection());
                adapter = new SqlDataAdapter(command);

                command.Parameters.Add("@id_t", SqlDbType.Int).Value = row_;
                command.Parameters.Add("@id_s", SqlDbType.Int).Value = id_s;
                //command.Parameters.Add("@id_p", SqlDbType.Int).Value = ;

                db_T.openConnection();
                adapter.Fill(dt_T);

                Data_Grid_Schedule.ItemsSource = dt_T.DefaultView;

                return (int)dt_T.Rows[0][0];
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (db_T.getConnection() != null)
                    db_T.closeConnection();
            }
        }//ТАБЛИЦА В
    }
}