using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для AddEddStaffWindow.xaml
    /// </summary>
    public partial class AddEddStaffWindow : Window
    {
        public AddEddStaffWindow()
        {
            InitializeComponent();
        }
        public int ID_Staff;
        SqlDataAdapter adapter;

        private void VisibilityTBAdress()
        {
            if (TB_Number_Apartment.Visibility != Visibility)
            {
                Border_ContactDetail.Height = 168;
                this.Height = 463;
                TB_Country.Visibility = Visibility.Visible;
                TB_Region.Visibility = Visibility.Visible;
                TB_City.Visibility = Visibility.Visible;
                TB_Street.Visibility = Visibility.Visible;
                TB_Number_Home.Visibility = Visibility.Visible;
                TB_Number_Apartment.Visibility = Visibility.Visible;
                SaveAdressButton.Visibility = Visibility.Visible;

                TB_LastName.IsEnabled = false;
                TB_FirstName.IsEnabled = false;
                TB_Patronymic.IsEnabled = false;
                ComboBox_Sex.IsEnabled = false;
                DP_Date_births.IsEnabled = false;
                TB_Specialty.IsEnabled = false;
                TB_Post.IsEnabled = false;
                TB_N_Cabinet.IsEnabled = false;

                TB_Phone.IsEnabled = false;
                TB_HomePhone.IsEnabled = false;
                TB_Email.IsEnabled = false;

                //TB_Adress.IsEnabled = false;

                TB_Series.IsEnabled = false;
                TB_Number.IsEnabled = false;
                DP_DateIssue.IsEnabled = false;
                TB_Code.IsEnabled = false;
                TB_IssuedBy.IsEnabled = false;

                AddEdAdressButton.IsEnabled = false;

                SaveButton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;
            }
            else
            {
                Border_ContactDetail.Height = 91;
                this.Height = 500;
                TB_Country.Visibility = Visibility.Hidden;
                TB_Region.Visibility = Visibility.Hidden;
                TB_City.Visibility = Visibility.Hidden;
                TB_Street.Visibility = Visibility.Hidden;
                TB_Number_Home.Visibility = Visibility.Hidden;
                TB_Number_Apartment.Visibility = Visibility.Hidden;
                SaveAdressButton.Visibility = Visibility.Hidden;

                TB_LastName.IsEnabled = true;
                TB_FirstName.IsEnabled = true;
                TB_Patronymic.IsEnabled = true;
                ComboBox_Sex.IsEnabled = true;
                DP_Date_births.IsEnabled = true;
                TB_Specialty.IsEnabled = true;
                TB_Post.IsEnabled = true;
                TB_N_Cabinet.IsEnabled = true;

                TB_Phone.IsEnabled = true;
                TB_HomePhone.IsEnabled = true;
                TB_Email.IsEnabled = true;

                //TB_Adress.IsEnabled = true;

                TB_Series.IsEnabled = true;
                TB_Number.IsEnabled = true;
                DP_DateIssue.IsEnabled = true;
                TB_Code.IsEnabled = true;
                TB_IssuedBy.IsEnabled = true;

                AddEdAdressButton.IsEnabled = true;

                //ButtonStackPanel.Margin = new Thickness(-174, -34, -178, 0);

                SaveButton.Visibility = Visibility.Visible;
                cancelButton.Visibility = Visibility.Visible;
            }
        }

        private void Fill_comboBox()
        {
            DataTable dataTable_Sex = new DataTable();
            DB db = new DB();

            string sql = "SELECT Sex FROM DIR_Sex";
            try
            {
                SqlCommand command = new SqlCommand(sql, db.getConnection());
                adapter = new SqlDataAdapter(command);

                db.openConnection();
                adapter.Fill(dataTable_Sex);
                ComboBox_Sex.ItemsSource = dataTable_Sex.Rows
                    .Cast<DataRow>()
                    .Select(x => x.Field<string>("Sex"));
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

        private void SQLStaffUpdate()
        {
            string sql =
                    "UPDATE Staffs " +
                    "SET Staffs.Last_name = @last_name, Staffs.First_name = @first_name, Staffs.Patronymic = @patronymic, Staffs.id_Gender = @id_gender, " +
                    "Staffs.Date_of_birth = @dob, Staffs.Post = @post, Staffs.Specialty = @specialty, Staffs.N_Cabinet = @n_cabinet " +
                    "WHERE Staffs.ID = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;
                command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = TB_LastName.Text;
                command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = TB_FirstName.Text;
                command.Parameters.Add("@patronymic", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
                command.Parameters.Add("@id_gender", SqlDbType.Int).Value = Convert.ToInt32(ComboBox_Sex.SelectedIndex);
                command.Parameters.Add("@dob", SqlDbType.NChar).Value = DP_Date_births.Text;
                command.Parameters.Add("@post", SqlDbType.NVarChar).Value = TB_Post.Text;
                command.Parameters.Add("@specialty", SqlDbType.NVarChar).Value = TB_Specialty.Text;
                command.Parameters.Add("@n_cabinet", SqlDbType.NVarChar).Value = TB_N_Cabinet.Text;

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

        private void SQLContactDetailsDUpdate()
        {
            string sql =
                    "UPDATE Contact_details_D " +
                    "SET Contact_details_D.Phone = @phone, Contact_details_D.Phone_Home = @phone_home, Contact_details_D.Email = @email, Contact_details_D.Country = @country, Contact_details_D.Region = @region, " +
                    "Contact_details_D.City = @city, Contact_details_D.Street = @street, Contact_details_D.Number_Home = @number_home, Contact_details_D.Number_Apartment = @number_apartment " +
                    "WHERE Contact_details_D.id_Staff = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = TB_Phone.Text;
                command.Parameters.Add("@phone_home", SqlDbType.NVarChar).Value = TB_HomePhone.Text;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = TB_Email.Text;

                command.Parameters.Add("@country", SqlDbType.NVarChar).Value = TB_Country.Text;
                command.Parameters.Add("@region", SqlDbType.NVarChar).Value = TB_Region.Text;
                command.Parameters.Add("@city", SqlDbType.NVarChar).Value = TB_City.Text;
                command.Parameters.Add("@street", SqlDbType.NVarChar).Value = TB_Street.Text;
                command.Parameters.Add("@number_home", SqlDbType.NVarChar).Value = TB_Number_Home.Text;
                command.Parameters.Add("@number_apartment", SqlDbType.NVarChar).Value = TB_Number_Apartment.Text;


                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Контактные данные изменены");
                    //this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlConnection.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }

        private void SQLPassportDataDUpdate()
        {
            string sql =
                    "UPDATE Passport_datas_D " +
                    "SET Passport_datas_D.Series = @series, Passport_datas_D.Number = @number, " +
                    "Passport_datas_D.Date_of_issue = @date_of_issue, Passport_datas_D.Code = @code, Passport_datas_D.Issued_by = @issued_by " +
                    "WHERE Passport_datas_D.id_Staff = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                command.Parameters.Add("@series", SqlDbType.NVarChar).Value = TB_Series.Text;
                command.Parameters.Add("@number", SqlDbType.NVarChar).Value = TB_Number.Text;
                command.Parameters.Add("@date_of_issue", SqlDbType.NChar).Value = DP_DateIssue.Text;
                command.Parameters.Add("@code", SqlDbType.NVarChar).Value = TB_Code.Text;
                command.Parameters.Add("@issued_by", SqlDbType.NVarChar).Value = TB_IssuedBy.Text;


                sqlConnection.Open();
                if (command.ExecuteNonQuery() == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Паспортные данные изменены");
                    this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlConnection.Close();
                //this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }

        private void SQLStaffInsert()
        {
            string sql =
                    "INSERT INTO Staffs (Last_name, First_name, Patronymic, id_Gender, Date_of_birth, Post, Specialty, N_Cabinet, Сheck_for_deletion) " +
                    "VALUES (@last_name, @first_name, @patronymic, @id_gender, @dob, @post, @specialty, @n_cabinet, @cfd)";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = TB_LastName.Text;
            command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = TB_FirstName.Text;
            command.Parameters.Add("@patronymic", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
            command.Parameters.Add("@id_gender", SqlDbType.Int).Value = Convert.ToInt32(ComboBox_Sex.SelectedIndex);
            command.Parameters.Add("@dob", SqlDbType.NChar).Value = DP_Date_births.Text;
            command.Parameters.Add("@post", SqlDbType.NVarChar).Value = TB_Post.Text;
            command.Parameters.Add("@specialty", SqlDbType.NVarChar).Value = TB_Specialty.Text;
            command.Parameters.Add("@n_cabinet", SqlDbType.NVarChar).Value = TB_N_Cabinet.Text;
            command.Parameters.Add("@cfd", SqlDbType.Int).Value = 1;

            sqlConnection.Open();
            //command.ExecuteNonQuery();

            //Console.WriteLine(Convert.ToString(rase));
            if (command.ExecuteNonQuery() == 1)
            {
                //System.Windows.Forms.MessageBox.Show("Данные изменены");
                command.CommandText = "SELECT @@IDENTITY";
                ID_Staff = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlConnection.Close();
        }

        private void SQLContactDetailsDInsert()
        {
            string sql =
                    "INSERT INTO Contact_details_D (id_Staff, Phone, Phone_Home, Email, Country, Region, City, Street, Number_Home, Number_Apartment, Сheck_for_deletion) " +
                    "VALUES (@id_staff ,@phone, @phone_home, @email, @country, @region, @city, @street, @number_home, @number_apartment, @cfd)";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = TB_Phone.Text;
            command.Parameters.Add("@phone_home", SqlDbType.NVarChar).Value = TB_HomePhone.Text;
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = TB_Email.Text;

            command.Parameters.Add("@country", SqlDbType.NVarChar).Value = TB_Country.Text;
            command.Parameters.Add("@region", SqlDbType.NVarChar).Value = TB_Region.Text;
            command.Parameters.Add("@city", SqlDbType.NVarChar).Value = TB_City.Text;
            command.Parameters.Add("@street", SqlDbType.NVarChar).Value = TB_Street.Text;
            command.Parameters.Add("@number_home", SqlDbType.NVarChar).Value = TB_Number_Home.Text;
            command.Parameters.Add("@number_apartment", SqlDbType.NVarChar).Value = TB_Number_Apartment.Text;

            command.Parameters.Add("@id_staff", SqlDbType.Int).Value = ID_Staff;
            command.Parameters.Add("@cfd", SqlDbType.Int).Value = 1;

            sqlConnection.Open();
            if (command.ExecuteNonQuery() == 1)
            {
                System.Windows.Forms.MessageBox.Show("Данные изменены");
                //command.CommandText = "SELECT @@IDENTITY";
                //ID_Patient = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SQLPassportDataDInsert()
        {
            string sql =
                    "INSERT INTO Passport_datas_D (id_Staff, Series, Number, Date_of_issue, Code, Issued_by, Сheck_for_deletion) " +
                    "VALUES (@id_staff ,@series, @number, @dateofissue, @code, @issued_by, @cfd)";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@series", SqlDbType.NVarChar).Value = TB_Series.Text;
            command.Parameters.Add("@number", SqlDbType.NVarChar).Value = TB_Number.Text;
            command.Parameters.Add("@dateofissue", SqlDbType.NVarChar).Value = DP_DateIssue.Text;
            command.Parameters.Add("@code", SqlDbType.NVarChar).Value = TB_Code.Text;
            command.Parameters.Add("@issued_by", SqlDbType.NVarChar).Value = TB_IssuedBy.Text;

            command.Parameters.Add("@id_staff", SqlDbType.Int).Value = ID_Staff;
            command.Parameters.Add("@cfd", SqlDbType.Int).Value = 1;

            sqlConnection.Open();
            if (command.ExecuteNonQuery() == 1)
            {
                System.Windows.Forms.MessageBox.Show("Данные изменены");
                //command.CommandText = "SELECT @@IDENTITY";
                //ID_Patient = Convert.ToInt32(command.ExecuteScalar());
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddEdAdressButton_Click(object sender, RoutedEventArgs e)
        {
            VisibilityTBAdress();
        }

        private void SaveAdressButton_Click(object sender, RoutedEventArgs e)
        {
            VisibilityTBAdress();
            TB_Adress.Text = TB_Country.Text + ", " + TB_Region.Text + ", г." + TB_City.Text + ", ул." + TB_Street.Text + ", " + TB_Number_Home.Text + "/" + TB_Number_Apartment.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VisibilityTBAdress();
            Fill_comboBox();
            if (ID_Staff != 0)
            {
                string sql =
                    "SELECT * " +
                    "FROM Staffs " +
                    "INNER JOIN Contact_details_D ON Staffs.ID = Contact_details_D.id_Staff " +
                    "INNER JOIN Passport_datas_D ON Staffs.ID = Passport_datas_D.id_Staff " +
                    "WHERE Staffs.ID = @ID_S;";

                DB db = new DB();
                try
                {
                    SqlCommand command = new SqlCommand(sql, db.getConnection());
                    adapter = new SqlDataAdapter(command);

                    command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Staff;

                    adapter.SelectCommand = command;
                    DataTable dataTable_Staff = new DataTable();
                    adapter.Fill(dataTable_Staff);

                    DoctorDataGrid.ItemsSource = dataTable_Staff.DefaultView;

                    db.connection.Open();
                    //-------------------------------
                    TB_LastName.Text = dataTable_Staff.DefaultView[0][1].ToString();
                    TB_FirstName.Text = dataTable_Staff.DefaultView[0][2].ToString();
                    TB_Patronymic.Text = dataTable_Staff.DefaultView[0][3].ToString();
                    ComboBox_Sex.SelectedIndex = Convert.ToInt32(dataTable_Staff.DefaultView[0][4].ToString());
                    DP_Date_births.Text = dataTable_Staff.DefaultView[0][5].ToString();
                    TB_Post.Text = dataTable_Staff.DefaultView[0][6].ToString();
                    TB_Specialty.Text = dataTable_Staff.DefaultView[0][7].ToString();
                    TB_N_Cabinet.Text = dataTable_Staff.DefaultView[0][8].ToString();

                    TB_Phone.Text = dataTable_Staff.DefaultView[0][12].ToString();
                    TB_HomePhone.Text = dataTable_Staff.DefaultView[0][13].ToString();
                    TB_Email.Text = dataTable_Staff.DefaultView[0][14].ToString();
                    TB_Country.Text = dataTable_Staff.DefaultView[0][15].ToString();
                    TB_Region.Text = dataTable_Staff.DefaultView[0][16].ToString();
                    TB_City.Text = dataTable_Staff.DefaultView[0][17].ToString();
                    TB_Street.Text = dataTable_Staff.DefaultView[0][18].ToString();
                    TB_Number_Home.Text = dataTable_Staff.DefaultView[0][19].ToString();
                    TB_Number_Apartment.Text = dataTable_Staff.DefaultView[0][20].ToString();
                    TB_Adress.Text = TB_Country.Text + ", " + TB_Region.Text + ", г." + TB_City.Text + ", ул." + TB_Street.Text + ", " + TB_Number_Home.Text + "/" + TB_Number_Apartment.Text;

                    TB_Series.Text = dataTable_Staff.DefaultView[0][24].ToString();
                    TB_Number.Text = dataTable_Staff.DefaultView[0][25].ToString();
                    DP_DateIssue.Text = dataTable_Staff.DefaultView[0][26].ToString();
                    TB_Code.Text = dataTable_Staff.DefaultView[0][27].ToString();
                    TB_IssuedBy.Text = dataTable_Staff.DefaultView[0][28].ToString();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (db.connection != null)
                        db.connection.Close();
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ID_Staff == 0)
            {
                SQLStaffInsert();
                SQLContactDetailsDInsert();
                SQLPassportDataDInsert();
            }
            else
            {
                SQLStaffUpdate();
                SQLContactDetailsDUpdate();
                SQLPassportDataDUpdate();
            }
        }
    }
}
