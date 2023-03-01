using InvntoryManagementSoftware.ClientsPackage;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace InvntoryManagementSoftware.PaymentsPackage.ClientsPaymentPackage
{
    /// <summary>
    /// Interaction logic for ClientsPaymentsWindow.xaml
    /// </summary>
    public partial class ClientsPaymentsWindow : Window
    {
        SqlConnection con = App.con;
        public ClientsPaymentsWindow()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClientSearch_btn_Click(object sender, RoutedEventArgs e)
        {
            ClientsSearchWindow clientsSearchWindow = new ClientsSearchWindow("payment");
            clientsSearchWindow.ShowDialog();
        }

        private void CPay_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            CGet_tb.CustomText = ""; 

                if (double.TryParse(CPay_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _) && CPay_tb.CustomText!="")
                {

                    if (ClientState_tblock.Text=="عليه")
                    {
                        double restActualValue = double.Parse(CPay_tb.CustomText) - double.Parse(clientMoney_tb.CustomText);                        
                        RestOnClient_tb.Text = double.Parse(CPay_tb.CustomText) - double.Parse(clientMoney_tb.CustomText) >= 0 ? (double.Parse(CPay_tb.CustomText) - double.Parse(clientMoney_tb.CustomText)).ToString() : (-1 * (double.Parse(CPay_tb.CustomText) - double.Parse(clientMoney_tb.CustomText))).ToString();

                        if (restActualValue >= 0) StateOnClient_tb.Text = "له";
                        else StateOnClient_tb.Text = "عليه";
                        
                    }else
                {
                        double restActualValue = double.Parse(CPay_tb.CustomText) + double.Parse(clientMoney_tb.CustomText);
                        RestOnClient_tb.Text = double.Parse(CPay_tb.CustomText) + double.Parse(clientMoney_tb.CustomText) >= 0 ? (double.Parse(CPay_tb.CustomText) + double.Parse(clientMoney_tb.CustomText)).ToString() : (-1 * (double.Parse(CPay_tb.CustomText) + double.Parse(clientMoney_tb.CustomText))).ToString();
                        if (restActualValue >= 0) StateOnClient_tb.Text = "له";
                }

                }
                else if (!double.TryParse(CPay_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {

                    CPay_tb.CustomText = "";
                    CGet_tb.CustomText = "";

                    RestOnClient_tb.Text = clientMoney_tb.CustomText;
                    StateOnClient_tb.Text = ClientState_tblock.Text;
                }
                else
                {
                    RestOnClient_tb.Text = clientMoney_tb.CustomText;
                    StateOnClient_tb.Text = ClientState_tblock.Text;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void CGet_tb_TextChanged(object sender, TextChangedEventArgs e)
        {

           
            try
            {
                CPay_tb.CustomText = "";
                if (double.TryParse(CGet_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _) && CGet_tb.CustomText != "")
                {

                    if (ClientState_tblock.Text == "عليه")
                    {
                        double restActualValue = double.Parse(CGet_tb.CustomText) + double.Parse(clientMoney_tb.CustomText);

                        RestOnClient_tb.Text = double.Parse(CGet_tb.CustomText) + double.Parse(clientMoney_tb.CustomText) >= 0 ? (double.Parse(CGet_tb.CustomText) + double.Parse(clientMoney_tb.CustomText)).ToString() : (-1 * (double.Parse(CGet_tb.CustomText) + double.Parse(clientMoney_tb.CustomText))).ToString();

                        if (restActualValue >= 0) StateOnClient_tb.Text = "عليه";
                        else StateOnClient_tb.Text = "له";
                    }
                    else
                    {
                        double restActualValue = double.Parse(CGet_tb.CustomText) - double.Parse(clientMoney_tb.CustomText);

                        RestOnClient_tb.Text = double.Parse(CGet_tb.CustomText) - double.Parse(clientMoney_tb.CustomText) >= 0 ? (double.Parse(CGet_tb.CustomText) - double.Parse(clientMoney_tb.CustomText)).ToString() : (-1 * (double.Parse(CGet_tb.CustomText) - double.Parse(clientMoney_tb.CustomText))).ToString();
                        if (restActualValue >= 0) StateOnClient_tb.Text = "عليه";
                        else StateOnClient_tb.Text = "له";
                    }

                }
                else if (!double.TryParse(CGet_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {

                    CPay_tb.CustomText = "";
                    CGet_tb.CustomText = "";

                    RestOnClient_tb.Text = clientMoney_tb.CustomText;
                    StateOnClient_tb.Text = ClientState_tblock.Text;
                }
                else
                {
                    RestOnClient_tb.Text = clientMoney_tb.CustomText;
                    StateOnClient_tb.Text = ClientState_tblock.Text;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void clientMoney_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            

        }
    }
}
