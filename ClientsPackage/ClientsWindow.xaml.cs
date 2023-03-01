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

namespace InvntoryManagementSoftware.ClientsPackage
{
    /// <summary>
    /// Interaction logic for ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        SqlConnection con = App.con;
        string id;
        public ClientsWindow()
        {
            InitializeComponent();
            saveBtn.IsEnabled = true;
            UpdateBtn.IsEnabled = false;
        }
        public ClientsWindow(string id)
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
                SqlDataAdapter getselectedData = new SqlDataAdapter("select * from T_Clients where Id = @id", con);
                getselectedData.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                getselectedData.Fill(dt);
                //CatName,CatBarCode,MainCatName,SubCatName,SalePrice,BuyPrice,Quantity,UnitName,Description
                CName_tb.CustomText = dt.Rows[0]["CName"].ToString();
                CPhone_tb.CustomText = dt.Rows[0]["CPhone"].ToString();
                CGov_tb.CustomText = dt.Rows[0]["CGov"].ToString();
                clientGender_cmb.SelectedIndex = dt.Rows[0]["CGender"].ToString() == "ذكر" ? clientGender_cmb.SelectedIndex = 0 : clientGender_cmb.SelectedIndex = 1;
                CArea_tb.CustomText = dt.Rows[0]["CArea"].ToString();
                CEmail_tb.CustomText = dt.Rows[0]["CEmail"].ToString();
                CNotes_tb.CustomText = dt.Rows[0]["CNotes"].ToString();
                CBareed_tb.CustomText = dt.Rows[0]["CBareed"].ToString();
                clientState_cmb.SelectedIndex = dt.Rows[0]["CState"].ToString()=="له"?clientState_cmb.SelectedIndex=0: clientState_cmb.SelectedIndex = 1;
                CMoney_tb.CustomText = dt.Rows[0]["CMoney"].ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientsSearchWindow clientsSearch = new ClientsSearchWindow("search");
            this.Close();
            clientsSearch.ShowDialog();
        }
        void CreateClient() {
            try
            {
                if (CName_tb.CustomText.Length>0 && CPhone_tb.CustomText.Length > 0 && CMoney_tb.CustomText.Length>0)
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand CreateClientCmd = new SqlCommand(
                        "insert into T_Clients (CName,CPhone,CGender,CGov,CArea,CEmail,CNotes,CBareed,CCreatedFullDate,CState,CMoney,CDate)" +
                        "values" +
                        "(@CName,@CPhone,@CGender,@CGov,@CArea,@CEmail,@CNotes,@CBareed,@CCreatedFullDate,@CState,@CMoney,@CDate)"
                        , con);
                    CreateClientCmd.Parameters.AddWithValue("@CName", CName_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CPhone", CPhone_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CGender", ((ComboBoxItem)clientGender_cmb.SelectedItem).Content.ToString());
                    CreateClientCmd.Parameters.AddWithValue("@CGov", CGov_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CArea", CArea_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CEmail", CEmail_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CNotes", CNotes_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CBareed", CBareed_tb.CustomText);
                    CreateClientCmd.Parameters.Add("@CCreatedFullDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
                    CreateClientCmd.Parameters.AddWithValue("@CState", ((ComboBoxItem)clientState_cmb.SelectedItem).Content.ToString());
                    CreateClientCmd.Parameters.AddWithValue("@CMoney", CMoney_tb.CustomText);
                    CreateClientCmd.Parameters.AddWithValue("@CDate", DateTime.Today.ToString("dd/MM/yyyy"));
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
                MessageBox.Show("حدث خطأ أثناء حفظ العميل \n "+ ex.Message,"ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
            }




        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateClient();
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientsWindow clientsWindow = new ClientsWindow();
            this.Close();
            clientsWindow.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
