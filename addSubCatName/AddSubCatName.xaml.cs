
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
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
using System.Windows.Shell;
using Microsoft.Data.SqlClient;

namespace InvntoryManagementSoftware.addSubCatName
{
    /// <summary>
    /// Interaction logic for AddSubCatNameWindow.xaml
    /// </summary>
    public partial class AddSubCatName : Window
    {
        SqlConnection con = App.con;
        DataTable dt1 = new DataTable();

        //DataTable dt = new DataTable(); 
        DataTable MainCategoriesDataTable = new DataTable(); 
        List<string> CategoriesList = new List<string>();


        public AddSubCatName()
        {
            InitializeComponent(); 
            SaveMainCatNewNameBtn.IsEnabled = false;
            LoadEssentialData();
            // يتم فك هذا الكومنت عند الرجوع للتعديل
            /*NewMainCattb.Visibility = Visibility.Hidden;
            EditMainCatNameBtn.Visibility = Visibility.Hidden;
            id_tb.Visibility = Visibility.Hidden;
             */
        }

        public void LoadEssentialData()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select MainCatName from MainCategory", con); 
            da.Fill(MainCategoriesDataTable);
            for(int i =0; i < MainCategoriesDataTable.Rows.Count; i++)
            {
                CategoriesList.Add(MainCategoriesDataTable.Rows[i]["MainCatName"].ToString());
            }

            MainCategory_cmb.ItemsSource = MainCategoriesDataTable.DefaultView; 
            MainCategory_cmb.SelectedIndex = 0;
           // MessageBox.Show(MainCategory_cmb.SelectedIndex.ToString()); 
            realoadDataGrid();
        }

        private void btn_grid_edit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        

        private void deleteRow_btn_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذه الفئة؟\n سيتم حذف اسم الفئة وكل البضائع التي تتبع هذه الفئة\n هل تريد الإستمرار", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {


            }
            else
            {
                deleteMainCatFromSql(int.Parse(dt1.Rows[MainCatDG.SelectedIndex]["Id"].ToString()) );
                dt1.Rows[MainCatDG.SelectedIndex].Delete();

                dt1.AcceptChanges();
                realoadDataGrid();

            }

        }

        private void deleteMainCatFromSql(int selectedCatId )
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("delete from SubCategories where Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", selectedCatId);
                cmd.ExecuteNonQuery(); 
                
                MessageBox.Show("Deleted Successfully");
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditMainCattb_LostFocus(object sender, RoutedEventArgs e)
        {
           
            
                
              
            
        }

        private void EditMainCatNameBtn_Click(object sender, RoutedEventArgs e)
        {
             if (MessageBox.Show("هل تريد تعديل اسم هذه الفئة؟ \n سيتعدل اسمها في صفحة البضائع وصفحة الفئات وستتحول البضائع ضمن الاسم القديم إلي الإسم الجديد", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
             {
                 
                 EditMainCattb.CustomText = "";
                 id_tb.CustomText = "";

                EditMainCatNameBtn.IsEnabled = false;

            }
             else
             {
                 try
                 {
                     if (con.State == ConnectionState.Closed)
                         con.Open();
                   
                    SqlCommand cmd2 = new SqlCommand("update category set catMainName =@catMainName where catMainName=(select MainCatName from MainCategory where Id =@id)", con);
                    cmd2.Parameters.AddWithValue("@catMainName", EditMainCattb.CustomText);
                    cmd2.Parameters.AddWithValue("@id", int.Parse(id_tb.CustomText));
                    cmd2.ExecuteNonQuery(); 
                    SqlCommand cmd = new SqlCommand("update MainCategory set MainCatName= @MainCatName where Id = @id", con);
                    cmd.Parameters.AddWithValue("@MainCatName", EditMainCattb.CustomText);
                    cmd.Parameters.AddWithValue("@id", int.Parse(id_tb.CustomText));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم تحديث اسم الفئة بنجاح");

                    EditMainCattb.CustomText = "";
                    id_tb.CustomText = "";
                    AddSubCatName addOrEditMainCategory = new AddSubCatName();
                    this.Close();
                    addOrEditMainCategory.ShowDialog();

                }
                 catch(Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }

             }
             
        }

        private void SaveMainCatNewNameBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd1 = new SqlCommand("select SubCategoryName from SubCategories where SubCategoryName =@SubCategoryName and MainCategoryName=@MainCategoryName", con);
                cmd1.Parameters.AddWithValue("@SubCategoryName", NewMainCattb.CustomText);
                cmd1.Parameters.AddWithValue("@MainCategoryName", CategoriesList[MainCategory_cmb.SelectedIndex]);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    Trace.WriteLine(dr.GetString("MainCategoryName"));
                    dr.Close();
                    MessageBox.Show("هذه الفئة الفرعية موجوده بالفعل لديك");
                    this.Close();
                    AddSubCatName addOrEditMainCategory = new AddSubCatName();
                    addOrEditMainCategory.Show();
                    
                }
                else
                {
                    dr.Close(); 
                    SqlCommand cmd = new SqlCommand("insert into SubCategories(SubCategoryName,MainCategoryName) values (@SubCategoryName,@MainCategoryName)", con);
                    cmd.Parameters.AddWithValue("@SubCategoryName", NewMainCattb.CustomText);
                    cmd.Parameters.AddWithValue("@MainCategoryName", CategoriesList[MainCategory_cmb.SelectedIndex]);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم حفظ اسم الفئة بنجاح");
                    NewMainCattb.CustomText = ""; 
                    realoadDataGrid();
                }
                /*
                
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void realoadDataGrid()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            dt1.Clear();
            SqlDataAdapter cmd = new SqlDataAdapter("select * from SubCategories where MainCategoryName=@MainCategoryName", con);
            cmd.SelectCommand.Parameters.AddWithValue("@MainCategoryName", CategoriesList[MainCategory_cmb.SelectedIndex]);
            
            cmd.Fill(dt1);
            
            MainCatDG.ItemsSource = dt1.DefaultView;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //realoadDataGrid();
            EditMainCatNameBtn.IsEnabled = false;

        }

        private void NewMainCattb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewMainCattb.CustomText.Length > 0)
            {
                SaveMainCatNewNameBtn.IsEnabled = true;
            }
            else
            {
                SaveMainCatNewNameBtn.IsEnabled = false;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void EditMainCattb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EditMainCattb.CustomText.Length > 0)
            {
                EditMainCatNameBtn.IsEnabled = true;
            }
            else
            {
                EditMainCatNameBtn.IsEnabled = false;
            }
        }

        private void MainCategory_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            realoadDataGrid();
        }
    }
}
