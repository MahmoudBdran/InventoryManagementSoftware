using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace InvntoryManagementSoftware.CatPackage
{
    /// <summary>
    /// Interaction logic for CatWindow.xaml
    /// </summary>
    public partial class CatWindow : Window
    {
        string id;
        SqlConnection con = App.con;
        DataTable MainCategoriesDataTable = new DataTable();
        List<string> MainCatList = new List<string>();
        DataTable SubCategoriesDataTable = new DataTable();
        List<string> SubCatList = new List<string>();
        public CatWindow()
        {
            InitializeComponent(); 
            loadMainCatData();
            saveBtn.IsEnabled = true;
            UpdateBtn.IsEnabled = false;
        }
        public CatWindow(string Id)
        {
            InitializeComponent();
            this.id = Id;
            saveBtn.IsEnabled = false;
            UpdateBtn.IsEnabled = true;
            loadMainCatData();
            loadSelectedData(Id);
        }
        void loadMainCatData() {
            try
            {
                if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT MainCatName from MainCategory", con);

            cmd.Fill(MainCategoriesDataTable);
            for (int i = 0; i < MainCategoriesDataTable.Rows.Count; i++)
            {
                MainCatList.Add(MainCategoriesDataTable.Rows[i]["MainCatName"].ToString());
            }
            MainCat_cmb.ItemsSource = MainCategoriesDataTable.DefaultView;
            MainCat_cmb.SelectedIndex = MainCat_cmb.Items.Count-1;
            loadSubCatData();
        }catch(Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
}
        void loadSubCatData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SubCategoriesDataTable.Clear();
                SubCatList.Clear();
                SqlDataAdapter cmd = new SqlDataAdapter("SELECT SubCategoryName from SubCategories where MainCategoryName = @MainCategoryName", con);
                cmd.SelectCommand.Parameters.AddWithValue("@MainCategoryName", MainCatList[MainCat_cmb.SelectedIndex]);

                cmd.Fill(SubCategoriesDataTable);
                for (int i = 0; i < SubCategoriesDataTable.Rows.Count; i++)
                {
                    SubCatList.Add(SubCategoriesDataTable.Rows[i]["SubCategoryName"].ToString());
                }
                SubCat_cmb.ItemsSource = SubCategoriesDataTable.DefaultView;
                SubCat_cmb.SelectedIndex = 0;
            }catch(Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        void InsertToDB() {
            try
            {
                if (CatName_tb.CustomText.Length>0&& SalePrice_tb.CustomText.Length > 0&&BuyPrice_tb.CustomText.Length > 0
                    &&quantity_tb.CustomText.Length > 0 && unit_tb.CustomText.Length > 0 &&
                    double.TryParse(SalePrice_tb.CustomText,NumberStyles.Any,CultureInfo.InvariantCulture,out _) &&
                    double.TryParse(BuyPrice_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _) &&
                    double.TryParse(quantity_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand insertCMD = new SqlCommand("insert into Categories(CatName,CatBarCode,MainCatName,SubCatName,SalePrice,BuyPrice,Quantity,UnitName,Description)" +
                        "values (@CatName,@CatBarCode,@MainCatName,@SubCatName,@SalePrice,@BuyPrice,@Quantity,@UnitName,@Description)", con);
                    insertCMD.Parameters.AddWithValue("@CatName", CatName_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@CatBarCode", CatBarCode_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@MainCatName", MainCatList[MainCat_cmb.SelectedIndex]);
                    insertCMD.Parameters.AddWithValue("@SubCatName", SubCatList[SubCat_cmb.SelectedIndex]);
                    insertCMD.Parameters.AddWithValue("@SalePrice", SalePrice_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@BuyPrice", BuyPrice_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@Quantity", quantity_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@UnitName", unit_tb.CustomText);
                    insertCMD.Parameters.AddWithValue("@Description", Desc_tb.CustomText);
                    insertCMD.ExecuteNonQuery();
                    MessageBox.Show("تم حفظ الصنف بنجاح");
                    CatWindow catWindow = new CatWindow();
                    this.Close();
                    catWindow.Show();
                }
                else
                {
                    if (CatName_tb.CustomText.Length == 0) CatName_tb.ErrorCaption = "رجاء إدخال إسم الصنف";
                    else CatName_tb.ErrorCaption = "";
                    if (CatBarCode_tb.CustomText.Length == 0) CatBarCode_tb.ErrorCaption = "رجاء إدخال باركود الصنف";
                    else CatBarCode_tb.ErrorCaption = "";
                    if (SalePrice_tb.CustomText.Length == 0 || !double.TryParse(SalePrice_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        SalePrice_tb.ErrorCaption = "رجاء إدخال سعر البيع بالأرقام";
                    else SalePrice_tb.ErrorCaption = "";
                    if (BuyPrice_tb.CustomText.Length == 0 || !double.TryParse(BuyPrice_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        BuyPrice_tb.ErrorCaption = "رجاء إدخال سعر الشراء بالأرقام";
                    else BuyPrice_tb.ErrorCaption = "";
                    if (quantity_tb.CustomText.Length == 0 || !double.TryParse(quantity_tb.CustomText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        quantity_tb.ErrorCaption = "رجاء إدخال كمية الصنف بالأرقام";
                    else quantity_tb.ErrorCaption = "";
                    if (unit_tb.CustomText.Length == 0)
                        unit_tb.ErrorCaption = "رجاء إدخال إسم الوحدة";
                    else unit_tb.ErrorCaption = "";

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        void loadSelectedData(string id)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter getselectedData = new SqlDataAdapter("select * from Categories where id = @id",con);
                getselectedData.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                getselectedData.Fill(dt);
                //CatName,CatBarCode,MainCatName,SubCatName,SalePrice,BuyPrice,Quantity,UnitName,Description
                CatName_tb.CustomText = dt.Rows[0]["CatName"].ToString();
                CatBarCode_tb.CustomText = dt.Rows[0]["CatBarCode"].ToString();
                MainCat_cmb.SelectedIndex = MainCatList.IndexOf(dt.Rows[0]["MainCatName"].ToString());
                SubCat_cmb.SelectedIndex = SubCatList.IndexOf(dt.Rows[0]["SubCatName"].ToString());
                SalePrice_tb.CustomText = dt.Rows[0]["SalePrice"].ToString();
                BuyPrice_tb.CustomText = dt.Rows[0]["BuyPrice"].ToString();
                quantity_tb.CustomText = dt.Rows[0]["Quantity"].ToString();
                unit_tb.CustomText = dt.Rows[0]["UnitName"].ToString();
                Desc_tb.CustomText = dt.Rows[0]["Description"].ToString();


            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }
        void updateSelectedCat(string id) {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand updateCMD = new SqlCommand("update Categories set CatName=@CatName,CatBarCode=@CatBarCode,MainCatName=@MainCatName,SubCatName=@SubCatName,SalePrice=@SalePrice,BuyPrice=@BuyPrice,Quantity=@Quantity,UnitName=@UnitName,Description=@Description where Id =@id", con);
                updateCMD.Parameters.AddWithValue("@id", id);
                updateCMD.Parameters.AddWithValue("@CatName", CatName_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@CatBarCode", CatBarCode_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@MainCatName", MainCatList[MainCat_cmb.SelectedIndex]);
                updateCMD.Parameters.AddWithValue("@SubCatName", SubCatList[SubCat_cmb.SelectedIndex]);
                updateCMD.Parameters.AddWithValue("@SalePrice", SalePrice_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@BuyPrice", BuyPrice_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@Quantity", quantity_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@UnitName", unit_tb.CustomText);
                updateCMD.Parameters.AddWithValue("@Description", Desc_tb.CustomText);
                updateCMD.ExecuteNonQuery();
                MessageBox.Show("تم تحديث معلومات الصنف بنجاح");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            CatSearch catSearch = new CatSearch("search");
            this.Close();
            catSearch.ShowDialog();
           
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertToDB();
        }

        private void MainCat_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadSubCatData();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            updateSelectedCat(id);
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            CatWindow catWindow = new CatWindow();
            this.Close();
            catWindow.Show();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
