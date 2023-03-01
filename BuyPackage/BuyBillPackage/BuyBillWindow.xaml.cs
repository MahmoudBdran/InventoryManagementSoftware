using InvntoryManagementSoftware.CatPackage;
using InvntoryManagementSoftware.ClientsPackage;
using InvntoryManagementSoftware.MowaredeenPackage;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
using static System.Resources.ResXFileRef;

namespace InvntoryManagementSoftware.BuyPackage.BuyBillPackage
{
    /// <summary>
    /// Interaction logic for SaleBillWindow.xaml
    /// </summary>
    public partial class BuyBillWindow : Window
    {
        DataTable categorydt = new DataTable();
        int billnumber = 00;
        bool itemfound = false;
        SqlConnection con = App.con;
        private DataGridCellInfo activeCellAtEdit { get; set; }
        int currentselectedindex = 0;
        public BuyBillWindow()
        {
            InitializeComponent();
            initializingDatatable();
        }
        void generateBillNumber()
        {

            while (true)
            {
                billnumber = generateRandomNumber();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand Checkcmd = new SqlCommand("SELECT BillNumber from BuyBillInfo where BillNumber = @billnumber ", con);
                Checkcmd.Parameters.AddWithValue("@billnumber", billnumber.ToString());
                SqlDataReader dr = Checkcmd.ExecuteReader();
                if (!dr.Read())
                {
                   billNumber_tb.CustomText = billnumber.ToString();
                    dr.Close();


                    break;

                }
            }
        }
        public  int generateRandomNumber()
        {
            Random rand = new Random();
            int RandomNumber = 10000000 * rand.Next();
            if (RandomNumber <= 0)
            {
                RandomNumber *= -1;
            }

            return RandomNumber;
        }
        void initializingDatatable() {
            categorydt.Columns.Add("CBarcode_col");
            categorydt.Columns.Add("CName_col");
            categorydt.Columns.Add("CUnit_col");
            categorydt.Columns.Add("CPrice_col");
            categorydt.Columns.Add("CQuantity_col");
            categorydt.Columns.Add("CFullPrice_col");
        
        }
        void addingNewItemInDG() {
            DataRow dr = categorydt.NewRow();
            dr["CBarcode_col"] = CBarCode_tb.CustomText;
            dr["CName_col"] = CName_tb.CustomText;
            dr["CUnit_col"] = CUnit_tb.CustomText;
            dr["CPrice_col"] = CPrice_tb.CustomText;
            dr["CQuantity_col"] = CQuantity_tb.CustomText;
            dr["CFullPrice_col"] = CResult_tb.CustomText;
            categorydt.Rows.Add(dr);
            membersDataGrid.ItemsSource = categorydt.DefaultView;

        }
        private void clientsearch_tb_Click(object sender, RoutedEventArgs e)
        {
            MowaredeenSearchWindow mowaredeenSearchWindow = new MowaredeenSearchWindow("buy");
            mowaredeenSearchWindow.ShowDialog();
        }

        private void catSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CatSearch catSearch = new CatSearch("buy");
            catSearch.ShowDialog();
        }
        void addingCategory(string SerialNumber)
        {
            if (SerialNumber == null) return;
            if (con.State == ConnectionState.Closed)
                con.Open();
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                if (SerialNumber == categorydt.Rows[i]["CBarcode_col"].ToString())
                {
                    itemfound = true;


                    categorydt.Rows[i]["CQuantity_col"] = (double.Parse(categorydt.Rows[i]["CQuantity_col"].ToString()) + double.Parse(CQuantity_tb.CustomText)).ToString();
                    categorydt.Rows[i]["CFullPrice_col"] = (double.Parse(categorydt.Rows[i]["CFullPrice_col"].ToString()) + double.Parse(CResult_tb.CustomText)).ToString();


                    calcReceipt();
                    membersDataGrid.ItemsSource = categorydt.DefaultView;
                    break;
                }
                else itemfound = false;
            }

            if (itemfound == false)
            {
                addingNewItemInDG(); calcReceipt();
                
            }

        }

        void calcReceipt()
        {////calc final price
            float fPriceBill = 0;
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                fPriceBill += float.Parse(categorydt.Rows[i]["CFullPrice_col"].ToString());
            }
            BillFinalPrice.CustomText = fPriceBill.ToString();
        }
        private void CQuantity_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                if (CQuantity_tb.CustomText.Length > 0&& double.TryParse(CQuantity_tb.CustomText,NumberStyles.Any,CultureInfo.InvariantCulture,out _))
                {
                    CResult_tb.CustomText = (double.Parse(CPrice_tb.CustomText) * double.Parse(CQuantity_tb.CustomText)).ToString();
                }
                else if ( CName_tb.CustomText.Length == 0)

                {
                    CQuantity_tb.CustomText = "";
                    CResult_tb.CustomText = "";

                }else
                {
                    CQuantity_tb.CustomText = "";
                    CResult_tb.CustomText = "";
                    MessageBox.Show("رجاء ملء خانة الكمية");

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addCatToBill_btn_Click(object sender, RoutedEventArgs e)
        {
            if ( CName_tb.CustomText.Length>0 &&CPrice_tb.CustomText.Length>0 && CQuantity_tb.CustomText.Length>0 &&CResult_tb.CustomText.Length>0 && CUnit_tb.CustomText.Length>0 )
            {
                addingCategory(CBarCode_tb.CustomText);
               }
            else
            {
                MessageBox.Show("رجاء كتابة كمية المنتج التي ستضاف إلي الفاتورة");
            }
            CBarCode_tb.CustomText = ""; CName_tb.CustomText = ""; CPrice_tb.CustomText = ""; CQuantity_tb.CustomText = ""; CResult_tb.CustomText = ""; CUnit_tb.CustomText = "";

        }

        private void updateCatInBill_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetBill_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteCurrentCat_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BillFinalPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcBill();
        }
       

        private void CustomerPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcBill();
        } 
        void calcBill() {
            try
            {

                if (BillFinalPrice.CustomText.Length > 0 && CustomerPay_tb.CustomText.Length > 0 && double.TryParse(CustomerPay_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    CustomerPay_tb.ErrorCaption = "";
                    rest_tb.CustomText = (double.Parse(CustomerPay_tb.CustomText) - double.Parse(BillFinalPrice.CustomText)).ToString();
                }
                else
                {
                    CustomerPay_tb.CustomText = "";
                    rest_tb.CustomText = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            generateBillNumber();
        }
        public DataTable preparingSaveProcess()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select CatBarCode,CatName from Categories", con);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            DataTable finalDT = new DataTable();
            finalDT.Columns.Add("CatSerial");
            finalDT.Columns.Add("CatName");
            finalDT.Columns.Add("CatQuantity");
            finalDT.Columns.Add("BillNumber");
            finalDT.Columns.Add("FinalPrice");
            finalDT.Columns.Add("Date");
            finalDT.Columns.Add("Time");
            finalDT.Columns.Add("Merchant");
            finalDT.Columns.Add("BasePrice");
            finalDT.Columns.Add("totaldate2");
            finalDT.Columns.Add("realtime");
            finalDT.Columns.Add("realdate");
            finalDT.Columns.Add("MowaredBillNumber"); 
            DataRow dr = null;
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                dr = finalDT.NewRow();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (categorydt.Rows[i]["CName_col"].ToString() == dt.Rows[j]["CatName"].ToString())
                    {
                        dr["CatSerial"] = dt.Rows[j]["CatBarCode"].ToString();

                    }
                }
                dr["CatName"] = categorydt.Rows[i]["CName_col"].ToString();
                dr["CatQuantity"] = categorydt.Rows[i]["CQuantity_col"].ToString();
                dr["BillNumber"] = billNumber_tb.CustomText;
                dr["FinalPrice"] =categorydt.Rows[i]["CFullPrice_col"].ToString();
                dr["Date"] = DateTime.Now.ToString("dd/MM/yyyy");
                dr["Time"] = DateTime.Now.ToString("hh:mm tt");
                /*insert into TasksCopy (Start_Date) 
                    values (CONVERT(date, datetime.date.ToString("mm/dd/yyyy")));*/
                // حاول بقا شيل حوار ال bulk واشتغل عادي 
                var date = DateTime.ParseExact(DateTime.Today.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var time = DateTime.ParseExact(DateTime.Now.ToString("HH:mm:ss.000"), "HH:mm:ss.000", CultureInfo.InvariantCulture);
                SqlParameter convertingtoDateTimeParameter = new SqlParameter(DateTime.Today.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss.000"), SqlDbType.DateTime);
                dr["totaldate2"] = convertingtoDateTimeParameter;
                SqlParameter convertingtoTimeParameter = new SqlParameter(DateTime.Now.ToString("HH:mm:ss.000"), SqlDbType.Time);
                dr["realtime"] = convertingtoTimeParameter;
                dr["realdate"] = date;
                

                dr["Merchant"] = "المدير";

                dr["BasePrice"] = categorydt.Rows[i]["CPrice_col"].ToString();
                dr["MowaredBillNumber"] = MowaredbillNumber_tb.CustomText.Length>0? MowaredbillNumber_tb.CustomText:"";
                finalDT.Rows.Add(dr);

            }

            return finalDT;


        }
        private void SaveAndPrint_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PaymentMethod_cmb.SelectedIndex == 0)
                {
                    if (ClientCode_tb.CustomText.Length > 0 && ClientName_tb.CustomText.Length > 0 && ClientPhone_tb.CustomText.Length > 0 &&
                    categorydt.Rows.Count > 0 && BillFinalPrice.CustomText.Length > 0 &&
                    CustomerPay_tb.CustomText.Length > 0 && rest_tb.CustomText.Length > 0 && billNumber_tb.CustomText.Length > 0 && double.Parse(rest_tb.CustomText) >= 0 && double.Parse(CustomerPay_tb.CustomText) > 0)
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        DataTable db_datatable = preparingSaveProcess();
                        SqlBulkCopy objBulk = new SqlBulkCopy(con);
                        objBulk.DestinationTableName = "BuyHistory";
                        objBulk.ColumnMappings.Add(0, 1);
                        objBulk.ColumnMappings.Add(1, 2);
                        objBulk.ColumnMappings.Add(2, 3);
                        objBulk.ColumnMappings.Add(3, 4);
                        objBulk.ColumnMappings.Add(4, 5);
                        objBulk.ColumnMappings.Add(5, 6);
                        objBulk.ColumnMappings.Add(6, 7);
                        objBulk.ColumnMappings.Add(7, 8);
                        objBulk.ColumnMappings.Add(8, 9);
                        objBulk.ColumnMappings.Add(9, 10);
                        objBulk.ColumnMappings.Add(10, 11);
                        objBulk.ColumnMappings.Add(11, 12);
                        objBulk.ColumnMappings.Add(12, 14);
                        objBulk.WriteToServer(db_datatable);
                        SqlCommand cmd = new SqlCommand("insert into BuyBillInfo" +
                            "(BillNumber,MowaredCode,MowaredName,MowaredPhone,BillPrice,WePay,Rest,PaymentMethod,BillFullDate,BillDate,merchant,realdate,realtime,MowaredBillNumber)" +
                          "values" +
                            "(@BillNumber,@MowaredCode,@MowaredName,@MowaredPhone,@BillPrice,@WePay,@Rest,@PaymentMethod,@BillFullDate,@BillDate,@merchant,@realdate,@realtime,@MowaredBillNumber)", con);
                        cmd.Parameters.AddWithValue("@BillNumber", billnumber);
                        cmd.Parameters.AddWithValue("@MowaredCode", ClientCode_tb.CustomText);
                        cmd.Parameters.AddWithValue("@MowaredName", ClientName_tb.CustomText);
                        cmd.Parameters.AddWithValue("@MowaredPhone", ClientPhone_tb.CustomText);
                        cmd.Parameters.AddWithValue("@BillPrice", BillFinalPrice.CustomText);
                        cmd.Parameters.AddWithValue("@WePay", CustomerPay_tb.CustomText);
                        cmd.Parameters.AddWithValue("@Rest", rest_tb.CustomText);
                        cmd.Parameters.AddWithValue("@PaymentMethod", ((ComboBoxItem)PaymentMethod_cmb.SelectedItem).Content.ToString());////////////////////////
                        cmd.Parameters.AddWithValue("@merchant", "المدير");
                        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm"));
                        cmd.Parameters.AddWithValue("@BillDate", DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.Add("@BillFullDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000");
                        cmd.Parameters.Add("@realdate", SqlDbType.Date).Value = DateTime.Today.ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("@realtime", SqlDbType.Time).Value = DateTime.Now.ToString("HH:mm:ss.000");
                        cmd.Parameters.AddWithValue("@MowaredBillNumber", MowaredbillNumber_tb.CustomText.Length > 0 ? MowaredbillNumber_tb.CustomText : "");
                        cmd.ExecuteNonQuery();
                        con.Close();


                        // LogsHelper.Log_Sell(merchant, "كاشير", FinalPrice_tb.Text);
                        DataTable tempDt = categorydt.Copy();
                        MessageBox.Show("تم حفظ الفاتورة بنجاح");
                        BuyBillWindow saleBillWindow = new BuyBillWindow();
                        this.Close();
                        saleBillWindow.Show();
                    }
                    else
                    {
                        if (ClientCode_tb.CustomText.Length == 0 || ClientName_tb.CustomText.Length == 0 || ClientPhone_tb.CustomText.Length == 0) clientSectionErrorText.Visibility = Visibility.Visible;
                        else if (CustomerPay_tb.CustomText.Length == 0 || double.Parse(rest_tb.CustomText) < 0 || double.TryParse(CustomerPay_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _)) CustomerPay_tb.ErrorCaption = "اكتب مادفعته للمورد بطريقة صحيحة";
                        else MessageBox.Show("اكمل ملء خانات الفاتورة");
                    }
                }
                else
                {
                    if (ClientCode_tb.CustomText.Length > 0 && ClientName_tb.CustomText.Length > 0 && ClientPhone_tb.CustomText.Length > 0 &&
                    categorydt.Rows.Count > 0 && BillFinalPrice.CustomText.Length > 0 &&
                     billNumber_tb.CustomText.Length > 0)
                    {
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        DataTable db_datatable = preparingSaveProcess();
                        SqlBulkCopy objBulk = new SqlBulkCopy(con);
                        objBulk.DestinationTableName = "BuyHistory";
                        objBulk.ColumnMappings.Add(0, 1);
                        objBulk.ColumnMappings.Add(1, 2);
                        objBulk.ColumnMappings.Add(2, 3);
                        objBulk.ColumnMappings.Add(3, 4);
                        objBulk.ColumnMappings.Add(4, 5);
                        objBulk.ColumnMappings.Add(5, 6);
                        objBulk.ColumnMappings.Add(6, 7);
                        objBulk.ColumnMappings.Add(7, 8);
                        objBulk.ColumnMappings.Add(8, 9);
                        objBulk.ColumnMappings.Add(9, 10);
                        objBulk.ColumnMappings.Add(10, 11);
                        objBulk.ColumnMappings.Add(11, 12);
                        objBulk.ColumnMappings.Add(12, 14);
                        objBulk.WriteToServer(db_datatable);
                        SqlCommand cmd = new SqlCommand("insert into BuyBillInfo" +
                            "(BillNumber,MowaredCode,MowaredName,MowaredPhone,BillPrice,WePay,Rest,PaymentMethod,BillFullDate,BillDate,merchant,realdate,realtime,MowaredBillNumber)" +
                          "values" +
                            "(@BillNumber,@MowaredCode,@MowaredName,@MowaredPhone,@BillPrice,@WePay,@Rest,@PaymentMethod,@BillFullDate,@BillDate,@merchant,@realdate,@realtime,@MowaredBillNumber)", con);
                        cmd.Parameters.AddWithValue("@BillNumber", billnumber);
                        cmd.Parameters.AddWithValue("@MowaredCode", ClientCode_tb.CustomText);
                        cmd.Parameters.AddWithValue("@MowaredName", ClientName_tb.CustomText);
                        cmd.Parameters.AddWithValue("@MowaredPhone", ClientPhone_tb.CustomText);
                        cmd.Parameters.AddWithValue("@BillPrice", BillFinalPrice.CustomText);
                        cmd.Parameters.AddWithValue("@WePay","0");
                        cmd.Parameters.AddWithValue("@Rest", "0");
                        cmd.Parameters.AddWithValue("@PaymentMethod", ((ComboBoxItem)PaymentMethod_cmb.SelectedItem).Content.ToString());////////////////////////
                        cmd.Parameters.AddWithValue("@merchant", "المدير");
                        cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm"));
                        cmd.Parameters.AddWithValue("@BillDate", DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.Add("@BillFullDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000");
                        cmd.Parameters.Add("@realdate", SqlDbType.Date).Value = DateTime.Today.ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("@realtime", SqlDbType.Time).Value = DateTime.Now.ToString("HH:mm:ss.000");
                        cmd.Parameters.AddWithValue("@MowaredBillNumber", MowaredbillNumber_tb.CustomText.Length>0? MowaredbillNumber_tb.CustomText:"");
                        cmd.ExecuteNonQuery();
                        con.Close();


                        // LogsHelper.Log_Sell(merchant, "كاشير", FinalPrice_tb.Text);
                        DataTable tempDt = categorydt.Copy();
                        MessageBox.Show("تم حفظ الفاتورة بنجاح");
                        BuyBillWindow saleBillWindow = new BuyBillWindow();
                        this.Close();
                        saleBillWindow.Show();
                    }
                    else
                    {
                        if (ClientCode_tb.CustomText.Length == 0 || ClientName_tb.CustomText.Length == 0 || ClientPhone_tb.CustomText.Length == 0) clientSectionErrorText.Visibility = Visibility.Visible;
                        else MessageBox.Show("اكمل ملء خانات الفاتورة");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("تأكد من ان كل خانة تحتوي علي ارقامها الصحيحه : " + ex.Message);
            }
        }


        private void clientDGRemove_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذا المنتج من قائمة الشراء؟", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {


            }
            else
            {
                categorydt.Rows[membersDataGrid.SelectedIndex].Delete();

                categorydt.AcceptChanges();

                calcReceipt();
                if (categorydt.Rows.Count == 0)
                {
                    BuyBillWindow buyBillWindow = new BuyBillWindow();
                    this.Close();
                    buyBillWindow.Show();
                }
            }
        }

        private void membersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            double n;
            bool s = double.TryParse(((TextBox)e.EditingElement).Text.ToString(), out n);

            if (((TextBox)e.EditingElement).Text == "" || !s)
            {
                e.Cancel = true;
                MessageBox.Show("       اكتب الكمية بالأرقام وليس الأحرف    " + s.ToString());
                ((TextBox)e.EditingElement).Text = categorydt.Rows[currentselectedindex]["CQuantity_col"].ToString();
                membersDataGrid.ItemsSource = categorydt.DefaultView;

            }
            else
            {//
                categorydt.Rows[e.Row.GetIndex()]["CQuantity_col"] = ((TextBox)e.EditingElement).Text;
                categorydt.Rows[e.Row.GetIndex()]["CFullPrice_col"] = (double.Parse(categorydt.Rows[e.Row.GetIndex()]["CQuantity_col"].ToString())*double.Parse(categorydt.Rows[e.Row.GetIndex()]["CPrice_col"].ToString())).ToString();
                calcReceipt();
            }

        }
        private void membersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.activeCellAtEdit = membersDataGrid.CurrentCell;
            this.currentselectedindex = membersDataGrid.SelectedIndex;
           // Trace.WriteLine("current index is : "+currentselectedindex.ToString());
            if (MessageBox.Show("متأكد من تعديل كمية هذا المنتج", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {

                e.Cancel = true;
                //categoryAdd_tb.Focus();
                FocusNavigationDirection focusDirection = FocusNavigationDirection.Last;

                // MoveFocus takes a TraveralReqest as its argument.
                TraversalRequest request = new TraversalRequest(focusDirection);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    membersDataGrid.MoveFocus(request);
                }
            }
            else
            {

            }



        }

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل تريد بالفعل إغلاق الصفحة وفتح فاتورة جديدة؟", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {


            }
            else
            {
                BuyBillWindow saleBillWindow = new BuyBillWindow();
                this.Close();
                saleBillWindow.Show();
            }
        }

      
        private void PaymentMethod_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { if (PaymentMethod_cmb.SelectedIndex == 0)
            {
                CustomerPay_tb.Visibility = Visibility.Visible;
                rest_tb.Visibility = Visibility.Visible;
            }
            else
            {

                CustomerPay_tb.Visibility = Visibility.Collapsed;
                rest_tb.Visibility = Visibility.Collapsed;
            }

        }
    }
    
}
