
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
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace InvntoryManagementSoftware.addorMainCatName
{
    /// <summary>
    /// Interaction logic for AddSubCatNameWindow.xaml
    /// </summary>
    public partial class AddMainCatName : Window
    {
        SqlConnection con = App.con;
        DataTable dt = new DataTable();
        public AddMainCatName()
        {
            InitializeComponent(); 
            SaveMainCatNewNameBtn.IsEnabled = false;

            // يتم فك هذا الكومنت عند الرجوع للتعديل
            /*NewMainCattb.Visibility = Visibility.Hidden;
            EditMainCatNameBtn.Visibility = Visibility.Hidden;
            id_tb.Visibility = Visibility.Hidden;
             */
        }



        private void btn_grid_edit_Click(object sender, RoutedEventArgs e)
        {
            EditMainCatNameBtn.IsEnabled = true;

            if (con.State == ConnectionState.Closed)
                con.Open();
            NewMainCattb.Visibility = Visibility.Visible;
            EditMainCatNameBtn.Visibility = Visibility.Visible;
            id_tb.Visibility = Visibility.Visible;
            SqlCommand cmd = new SqlCommand("select Id,MainCatName from MainCategory where Id =@id",con);
            cmd.Parameters.AddWithValue("@id", dt.Rows[MainCatDG.SelectedIndex]["id"]);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                id_tb.CustomText = dr.GetInt32("Id").ToString();
                EditMainCattb.CustomText = dr.GetString("MainCatName");
                dr.Close();
            }

        }

        

        private void deleteRow_btn_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذه الفئة؟\n سيتم حذف اسم الفئة وكل البضائع التي تتبع هذه الفئة\n هل تريد الإستمرار", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {


            }
            else
            {
                deleteMainCatFromSql(int.Parse(dt.Rows[MainCatDG.SelectedIndex]["id"].ToString()), dt.Rows[MainCatDG.SelectedIndex]["MainCatName"].ToString());
                dt.Rows[MainCatDG.SelectedIndex].Delete();

                dt.AcceptChanges();

            }
             
        }

        private void deleteMainCatFromSql(int selectedCatId,string selectedCat)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("delete from MainCategory where Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", selectedCatId);
                cmd.ExecuteNonQuery(); 
                SqlCommand cmd2 = new SqlCommand("delete from SubCategories where MainCategoryName=@MainCategoryName", con);
                cmd2.Parameters.AddWithValue("@MainCategoryName", selectedCat);
                cmd2.ExecuteNonQuery();
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
                   
                    SqlCommand cmd2 = new SqlCommand("update SubCategories set MainCategoryName =@MainCategoryName where MainCategoryName=(select MainCatName from MainCategory where Id =@id)", con);
                    cmd2.Parameters.AddWithValue("@MainCategoryName", EditMainCattb.CustomText);
                    cmd2.Parameters.AddWithValue("@id", int.Parse(id_tb.CustomText));
                    cmd2.ExecuteNonQuery(); 
                    SqlCommand cmd = new SqlCommand("update MainCategory set MainCatName= @MainCatName where Id = @id", con);
                    cmd.Parameters.AddWithValue("@MainCatName", EditMainCattb.CustomText);
                    cmd.Parameters.AddWithValue("@id", int.Parse(id_tb.CustomText));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم تحديث اسم الفئة بنجاح");

                    EditMainCattb.CustomText = "";
                    id_tb.CustomText = "";
                    AddMainCatName addOrEditMainCategory = new AddMainCatName();
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
                SqlCommand cmd1 = new SqlCommand("select * from MainCategory where MainCatName =@maincatname", con);
                cmd1.Parameters.AddWithValue("@maincatname", NewMainCattb.CustomText);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    Trace.WriteLine(dr.GetString("MainCatName"));
                    dr.Close();
                    MessageBox.Show("هذه الفئة موجوده بالفعل لديك");
                    this.Close();
                    AddMainCatName addOrEditMainCategory = new AddMainCatName();
                    addOrEditMainCategory.Show();
                    
                }
                else
                {
                    dr.Close(); 
                    SqlCommand cmd = new SqlCommand("insert into MainCategory(MainCatName) values (@MainCatName)", con);
                    cmd.Parameters.AddWithValue("@MainCatName", NewMainCattb.CustomText);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم حفظ اسم الفئة بنجاح");
                    NewMainCattb.CustomText = "";
                    this.Close();
                    AddMainCatName addOrEditMainCategory = new AddMainCatName();
                    addOrEditMainCategory.Show();
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
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * from MainCategory", con);

            cmd.Fill(dt);
            MainCatDG.ItemsSource = dt.DefaultView;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            realoadDataGrid();
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
    }
}
