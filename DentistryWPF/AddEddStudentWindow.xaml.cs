using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;

namespace DentistryWPF
{
    /// <summary>
    /// Логика взаимодействия для AddEddStudentWindow.xaml
    /// </summary>
    public partial class AddEddStudentWindow : Window
    {
        SqlDataAdapter adapter;
        public int ID_Student, IdProc;
        DataTable dataTable_Student = new DataTable();
        public AddEddStudentWindow()
        {
            InitializeComponent();
        }

        private void VisibilityTBAdress()
        {
            if (TB_Number_Apartment.Visibility != Visibility)
            {
                Border_ContactDetail.Height = 152;
                //this.Height = 100;
                this.Height = 370;
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
                TB_PlaceWork.IsEnabled = false;
                TB_Comment.IsEnabled = false;

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
                SaveButton.IsEnabled = false;
                cancelButton.IsEnabled = false;
            }
            else
            {
                Border_ContactDetail.Height = 76;
                this.Height = 305;
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
                TB_PlaceWork.IsEnabled = true;
                TB_Comment.IsEnabled = true;

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
                SaveButton.IsEnabled = true;
                cancelButton.IsEnabled = true;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)/////////////////////////////////////////////////////
        {
            //new GeneratedCode.GeneratedClass().CreatePackage(@"C:\Temp\Output.docx");

            Fill_comboBox();
            VisibilityTBAdress();
            if (ID_Student != 0)
            {
                string sql =
                    "SELECT * " +
                    "FROM Students " +
                    "INNER JOIN Contact_details ON Students.ID = Contact_details.id_Student " +
                    "INNER JOIN Passport_datas ON Students.ID = Passport_datas.id_Student " +
                    "WHERE Students.ID = @ID_S;";
                DB db = new DB();
                try
                {
                    SqlCommand command = new SqlCommand(sql, db.getConnection());
                    adapter = new SqlDataAdapter(command);

                    command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                    db.connection.Open();
                    adapter.Fill(dataTable_Student);

                    DataGrid1.ItemsSource = dataTable_Student.DefaultView;
                    //-------------------------------
                    TB_LastName.Text = dataTable_Student.DefaultView[0][1].ToString();
                    TB_FirstName.Text = dataTable_Student.DefaultView[0][2].ToString();
                    TB_Patronymic.Text = dataTable_Student.DefaultView[0][3].ToString();
                    ComboBox_Sex.SelectedIndex = Convert.ToInt32(dataTable_Student.DefaultView[0][4].ToString());
                    DP_Date_births.Text = dataTable_Student.DefaultView[0][5].ToString();
                    TB_PlaceWork.Text = dataTable_Student.DefaultView[0][6].ToString();
                    TB_Comment.Text = dataTable_Student.DefaultView[0][7].ToString();

                    TB_Phone.Text = dataTable_Student.DefaultView[0][11].ToString();
                    TB_HomePhone.Text = dataTable_Student.DefaultView[0][12].ToString();
                    TB_Email.Text = dataTable_Student.DefaultView[0][13].ToString();
                    TB_Country.Text = dataTable_Student.DefaultView[0][14].ToString();
                    TB_Region.Text = dataTable_Student.DefaultView[0][15].ToString();
                    TB_City.Text = dataTable_Student.DefaultView[0][16].ToString();
                    TB_Street.Text = dataTable_Student.DefaultView[0][17].ToString();
                    TB_Number_Home.Text = dataTable_Student.DefaultView[0][18].ToString();
                    TB_Number_Apartment.Text = dataTable_Student.DefaultView[0][19].ToString();
                    TB_Adress.Text = TB_Country.Text + ", " + TB_Region.Text + ", г." + TB_City.Text + ", ул." + TB_Street.Text + ", " + TB_Number_Home.Text + "/" + TB_Number_Apartment.Text;

                    TB_Series.Text = dataTable_Student.DefaultView[0][23].ToString();
                    TB_Number.Text = dataTable_Student.DefaultView[0][24].ToString();
                    DP_DateIssue.Text = dataTable_Student.DefaultView[0][25].ToString();
                    TB_Code.Text = dataTable_Student.DefaultView[0][26].ToString();
                    TB_IssuedBy.Text = dataTable_Student.DefaultView[0][27].ToString();
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
            else
                PrintButton.Visibility = Visibility.Collapsed;
        }

        private void SQLStudentUpdate()
        {
            string sql =
                    "UPDATE Students " +
                    "SET Students.Last_name = @last_name, Students.First_name = @first_name, Students.Patronymic = @patronymic, Students.id_Gender = @id_gender, " +
                    "Students.Date_of_birth = @dob, Students.Job = @job, Students.Patient_comment = @patient_comm " +
                    "WHERE Students.ID = @ID_S";
           
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;
                command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = TB_LastName.Text;
                command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = TB_FirstName.Text;
                command.Parameters.Add("@patronymic", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
                command.Parameters.Add("@id_gender", SqlDbType.Int).Value = Convert.ToInt32(ComboBox_Sex.SelectedIndex);
                command.Parameters.Add("@dob", SqlDbType.NChar).Value = DP_Date_births.Text;
                command.Parameters.Add("@job", SqlDbType.NChar).Value = TB_PlaceWork.Text;
                command.Parameters.Add("@patient_comm", SqlDbType.NVarChar).Value = TB_Comment.Text;

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

        private void SQLContactDetailsUpdate()
        {
            string sql =
                    "UPDATE Contact_details " +
                    "SET Contact_details.Phone = @phone, Contact_details.Phone_Home = @phone_home, Contact_details.Email = @email, Contact_details.Country = @country, Contact_details.Region = @region, " +
                    "Contact_details.City = @city, Contact_details.Street = @street, Contact_details.Number_Home = @number_home, Contact_details.Number_Apartment = @number_apartment " +
                    "WHERE Contact_details.id_Student = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

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

        private void SQLPassportDataUpdate()
        {
            string sql =
                    "UPDATE Passport_datas " +
                    "SET Passport_datas.Series = @series, Passport_datas.Number = @number, " +
                    "Passport_datas.Date_of_issue = @date_of_issue, Passport_datas.Code = @code, Passport_datas.Issued_by = @issued_by " +
                    "WHERE Passport_datas.id_Student = @ID_S";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            try
            {
                command.Parameters.Add("@ID_S", SqlDbType.Int).Value = ID_Student;

                command.Parameters.Add("@series", SqlDbType.NVarChar).Value = TB_Series.Text;
                command.Parameters.Add("@number", SqlDbType.NVarChar).Value = TB_Number.Text;
                command.Parameters.Add("@date_of_issue", SqlDbType.NVarChar).Value = DP_DateIssue.Text;
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

        private void SQLStudentInsert()
        {
            string sql =
                    "INSERT INTO Students (Last_name, First_name, Patronymic, id_Gender, Date_of_birth, Job, Patient_comment, Сheck_for_deletion) " +
                    "VALUES (@last_name, @first_name, @patronymic, @id_gender, @dob, @job, @patient_comm, @cfd)";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = TB_LastName.Text;
            command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = TB_FirstName.Text;
            command.Parameters.Add("@patronymic", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
            command.Parameters.Add("@id_gender", SqlDbType.Int).Value = Convert.ToInt32(ComboBox_Sex.SelectedIndex);
            command.Parameters.Add("@dob", SqlDbType.NChar).Value = DP_Date_births.Text;
            command.Parameters.Add("@job", SqlDbType.NChar).Value = TB_PlaceWork.Text;
            command.Parameters.Add("@patient_comm", SqlDbType.NVarChar).Value = TB_Comment.Text;
            command.Parameters.Add("@cfd", SqlDbType.Int).Value = 1;

            sqlConnection.Open();

            if (command.ExecuteNonQuery() == 1)
            {
                //System.Windows.Forms.MessageBox.Show("Данные изменены");
                command.CommandText = "SELECT @@IDENTITY";
                ID_Student = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlConnection.Close();
        }

        private void SQLContactDetailsInsert()
        {
            string sql =
                    "INSERT INTO Contact_details (id_Student, Phone, Phone_Home, Email, Country, Region, City, Street, Number_Home, Number_Apartment, Сheck_for_deletion) " +
                    "VALUES (@id_student ,@phone, @phone_home, @email, @country, @region, @city, @street, @number_home, @number_apartment, @cfd)";

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

            command.Parameters.Add("@id_student", SqlDbType.NVarChar).Value = ID_Student;
            command.Parameters.Add("@cfd", SqlDbType.Int).Value = 1;

            sqlConnection.Open();

            if (command.ExecuteNonQuery() == 1)
            {
                //System.Windows.Forms.MessageBox.Show("Данные изменены");
                //command.CommandText = "SELECT @@IDENTITY";
                //ID_Patient = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SQLPassportDataInsert()
        {
            string sql =
                    "INSERT INTO Passport_datas (id_Student, Series, Number, Date_of_issue, Code, Issued_by, Сheck_for_deletion) " +
                    "VALUES (@id_student ,@series, @number, @dateofissue, @code, @issued_by, @cfd)";

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand command = new SqlCommand(sql, sqlConnection);

            adapter = new SqlDataAdapter(command);

            command.Parameters.Add("@series", SqlDbType.NVarChar).Value = TB_Series.Text;
            command.Parameters.Add("@number", SqlDbType.NVarChar).Value = TB_Number.Text;
            command.Parameters.Add("@dateofissue", SqlDbType.NVarChar).Value = DP_DateIssue.Text;
            command.Parameters.Add("@code", SqlDbType.NVarChar).Value = TB_Code.Text;
            command.Parameters.Add("@issued_by", SqlDbType.NVarChar).Value = TB_IssuedBy.Text;

            command.Parameters.Add("@id_student", SqlDbType.NVarChar).Value = ID_Student;
            command.Parameters.Add("@cfd", SqlDbType.NVarChar).Value = 1;

            sqlConnection.Open();
            if (command.ExecuteNonQuery() == 1)
            {
                //System.Windows.Forms.MessageBox.Show("Данные изменены");
                //command.CommandText = "SELECT @@IDENTITY";
                //ID_Patient = Convert.ToInt32(command.ExecuteScalar());
                //this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ошибка", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ID_Student == 0 && TB_LastName.Text != "" && TB_FirstName.Text != "" && TB_Patronymic.Text != "" && DP_Date_births.Text != "")
            {
                SQLStudentInsert();
                SQLContactDetailsInsert();
                SQLPassportDataInsert();
            }
            if (ID_Student > 0 && TB_LastName.Text != "" && TB_FirstName.Text != "" && TB_Patronymic.Text != "" && DP_Date_births.Text != "")
            {
                SQLStudentUpdate();
                SQLContactDetailsUpdate();
                SQLPassportDataUpdate();
            }
            else
                System.Windows.MessageBox.Show("Проверьте: заполнены ли все поля");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            {
                string sql = "INSERT INTO * Students, Contact_details";
                DB db = new DB();
                try
                {
                    SqlCommand command = new SqlCommand(sql, db.connection);
                    adapter = new SqlDataAdapter(command);
                    command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = TB_LastName.Text;
                    command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = TB_FirstName.Text;
                    command.Parameters.Add("@patronymic", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
                    command.Parameters.Add("@id_gender", SqlDbType.Int).Value = Convert.ToInt32(ComboBox_Sex.Text);
                    command.Parameters.Add("@dob", SqlDbType.NChar).Value = DP_Date_births.Text;
                    command.Parameters.Add("@job", SqlDbType.NChar).Value = TB_PlaceWork.Text;
                    command.Parameters.Add("@patient_comm", SqlDbType.NVarChar).Value = TB_Comment.Text;

                    command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = TB_Phone.Text;
                    command.Parameters.Add("@phone_home", SqlDbType.NVarChar).Value = TB_HomePhone.Text;
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = TB_Email.Text;

                    command.Parameters.Add("@country", SqlDbType.NVarChar).Value = TB_Patronymic.Text;
                    command.Parameters.Add("@region", SqlDbType.NVarChar).Value = Convert.ToInt32(ComboBox_Sex.Text);
                    command.Parameters.Add("@city", SqlDbType.NVarChar).Value = DP_Date_births.Text;
                    command.Parameters.Add("@street", SqlDbType.NVarChar).Value = TB_PlaceWork.Text;
                    command.Parameters.Add("@number_home", SqlDbType.NVarChar).Value = TB_Comment.Text;
                    command.Parameters.Add("@number_apartment", SqlDbType.NVarChar).Value = TB_Comment.Text;

                    db.openConnection();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (db.connection != null)
                        db.closeConnection();
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
