using InvntoryManagementSoftware.ClientsPackage;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace InvntoryManagementSoftware.MowaredeenPackage
{
    /// <summary>
    /// Interaction logic for ClientsWindow.xaml
    /// </summary>
    public partial class MowaredeenWindow : Window
    {
        string id;
        SqlConnection con = App.con;
        public MowaredeenWindow()
        {
            InitializeComponent();
            saveBtn.IsEnabled = true;
            UpdateBtn.IsEnabled = false;
        }
        public MowaredeenWindow(string id)
        {
            InitializeComponent();
            this.id = id;
            saveBtn.IsEnabled = false;
            UpdateBtn.IsEnabled = true;
            loadSelectedData(id);
        }

        void loadSelectedData(string id)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter getselectedData = new SqlDataAdapter("select * from Mowaredeen where Id = @id", con);
                getselectedData.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                getselectedData.Fill(dt);
                //MOWAREDname,phone,companyname,state,money,gov,area,email,bareed,notes
                MowaredName_tb.CustomText = dt.Rows[0]["MName"].ToString();
                MowaredPhone_tb.CustomText = dt.Rows[0]["MPhone"].ToString();
                MGov_tb.CustomText = dt.Rows[0]["MGov"].ToString();
                MArea_tb.CustomText = dt.Rows[0]["MArea"].ToString();
                MEmail_tb.CustomText = dt.Rows[0]["MEmail"].ToString();
                MNotes_tb.CustomText = dt.Rows[0]["MNotes"].ToString();
                MowaredCompanyName_tb.CustomText = dt.Rows[0]["MCompanyName"].ToString();
               MowaredState_cmb.SelectedIndex = dt.Rows[0]["MState"].ToString() == "له" ? MowaredState_cmb.SelectedIndex = 0 : MowaredState_cmb.SelectedIndex = 1;
                MMoney_tb.CustomText = dt.Rows[0]["MMoney"].ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void CreateMowared()
        {
            try
            {
                if (MowaredName_tb.CustomText.Length > 0 && MowaredPhone_tb.CustomText.Length > 0 && MMoney_tb.CustomText.Length > 0)
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand CreateClientCmd = new SqlCommand(
                        "insert into Mowaredeen (MName,MPhone,MCompanyName,MState,MMoney,MGov,MArea,MEmail,MNotes,MCreatedFullDate,MDate)" +
                        "values" +
                        "(@MName,@MPhone,@MCompanyName,@MState,@MMoney,@MGov,@MArea,@MEmail,@MNotes,@MCreatedFullDate,@MDate)"
                        , con);
                    CreateClientCmd.Parameters.AddWithValue("@MName", MowaredName_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MPhone", MowaredPhone_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MCompanyName", MowaredCompanyName_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MState", ((ComboBoxItem)MowaredState_cmb.SelectedItem).Content.ToString());
                    CreateClientCmd.Parameters.AddWithValue("@MMoney", MMoney_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MGov", MGov_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MArea", MArea_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MEmail", MEmail_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@MNotes", MNotes_tb.CustomText);
                    CreateClientCmd.Parameters.Add("@MCreatedFullDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
                    CreateClientCmd.Parameters.AddWithValue("@MDate", DateTime.Today.ToString("dd/MM/yyyy"));
                    CreateClientCmd.ExecuteNonQuery();
                    MessageBox.Show("تم حفظ معلومات العميل بنجاح");

                }
                else
                {
                    MessageBox.Show("يرجي ملء خانة الإسم والهاتف علي الأقل");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ العميل \n " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }




        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MowaredeenSearchWindow mowaredeenSearch = new MowaredeenSearchWindow("search");
            mowaredeenSearch.ShowDialog();
        }


        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateMowared();
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
           MowaredeenWindow mowaredeenWindow = new MowaredeenWindow();
            this.Close();
            mowaredeenWindow.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
