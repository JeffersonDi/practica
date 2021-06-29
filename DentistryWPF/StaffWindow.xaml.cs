using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        public StaffWindow()
        {
            InitializeComponent();
        }

        SqlDataAdapter adapter;

        DataTable staffsTable;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SQL_Staffs_Update();
        }

        private void SQL_Staffs_Update()
        {
            string sql = "SELECT ID ID_Staff, Last_name + ' ' + First_name + ' ' + Patronymic FLP, Specialty, N_Cabinet, Сheck_for_deletion, Post " +
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
                staffsGrid.ItemsSource = staffsTable.DefaultView;
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

        private void SQL_Staffs_Delete(int ID_Staff)
        {
            string sql =
                    "UPDATE Staffs " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Staffs.ID = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                //adapter = new SqlDataAdapter(command);

                //string country, region, city, street, numberHome, numberApartment;

                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные сотрудника изменены");
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

        private void SQL_Contact_details_D_Delete(int ID_Staff)
        {
            string sql =
                    "UPDATE Contact_details_D " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Contact_details_D.id_Staff = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                //adapter = new SqlDataAdapter(command);

                //string country, region, city, street, numberHome, numberApartment;

                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные сотрудника изменены");
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

        private void SQL_Passport_datas_D_Delete(int ID_Staff)
        {
            string sql =
                    "UPDATE Passport_datas_D " +
                    "SET Сheck_for_deletion = 0 " +
                    "WHERE Passport_datas_D.id_Staff = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                //adapter = new SqlDataAdapter(command);

                //string country, region, city, street, numberHome, numberApartment;

                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Данные врача изменены");
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddEddStaffWindow addEddStaffWindow = new AddEddStaffWindow();
            addEddStaffWindow.ID_Staff = 0;
            addEddStaffWindow.Title = "Добавление записи сотрудника";

            addEddStaffWindow.ShowDialog();
        }

        private void MI_List_Delete_Staff_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = staffsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                if (System.Windows.MessageBox.Show(this, "Вы уверены, что хотите удалить выбранный элемент?",
                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int ID_Doctor = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                    SQL_Staffs_Delete(ID_Doctor);
                    SQL_Contact_details_D_Delete(ID_Doctor);
                    SQL_Passport_datas_D_Delete(ID_Doctor);
                    SQL_Staffs_Update();
                }
            }
            else
                System.Windows.MessageBox.Show("Выберете запись для успешного удаления");

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            SQL_Staffs_Update();
        }

        private void staffsGrid_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            AddEddStaffWindow addEddStaffWindow = new AddEddStaffWindow();

            DataRowView drv = staffsGrid.SelectedItem as DataRowView;
            if (drv != null)
            {
                addEddStaffWindow.ID_Staff = Convert.ToInt32((drv.Row[0] ?? String.Empty).ToString());
                addEddStaffWindow.Title = "Редактирование записи сотрудника";
                //addEdPatientWindow.Title = ((drv.Row[0] ?? String.Empty).ToString());

                addEddStaffWindow.ShowDialog();
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TB_Search.Text == "")
            {
                staffsTable.DefaultView.RowFilter = "";
                return;
            }
            staffsTable.DefaultView.RowFilter = string.Format("[FLP] Like '%{0}%'", TB_Search.Text);
            staffsGrid.ItemsSource = staffsTable.DefaultView;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            SQL_Staffs_Update();
        }
    }
}
