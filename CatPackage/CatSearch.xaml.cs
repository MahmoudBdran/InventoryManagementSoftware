using InvntoryManagementSoftware.ClientsPackage;
using InvntoryManagementSoftware.SalePackage.SaleBillPackage;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Excel = Microsoft.Office.Interop.Excel;
using static System.Resources.ResXFileRef;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using DataTable = System.Data.DataTable;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Diagnostics;
using InvntoryManagementSoftware.BuyPackage.BuyBillPackage;

namespace InvntoryManagementSoftware.CatPackage
{
    /// <summary>
    /// Interaction logic for CatSearch.xaml
    /// </summary>
    public partial class CatSearch : Excel.Window
    {
        BrushConverter converter = new BrushConverter();
        SqlConnection con = App.con;
        DataTable CatsDT = new DataTable();
        string processName;
        public CatSearch(string processName)
        {
            InitializeComponent();
            _InitialCatsData();
            this.processName = processName;
        }

        void _InitialCatsData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter RetreiveClients = new SqlDataAdapter("select * from Categories", con);
                RetreiveClients.Fill(CatsDT);

                ObservableCollection<CatModel> categories = new ObservableCollection<CatModel>();
                foreach (DataRow dr in CatsDT.Rows)
                {

                    categories.Add(new CatModel
                    {//CatName,CatBarCode,MainCatName,SubCatName,SalePrice,BuyPrice,Quantity,UnitName,Description
                        Id = dr.Field<Int32>("Id"),
                        CatName = dr.Field<string>("CatName"),
                        BgColor = (Brush)converter.ConvertFromString("#1e88e5"),
                        character= dr.Field<string>("CatName").Substring(0,1),
                        CatBarCode = dr.Field<string>("CatBarCode"),
                        MainCatName = dr.Field<string>("MainCatName"),
                        SubCatName = dr.Field<string>("SubCatName"),
                        SalePrice = dr.Field<string>("SalePrice"),
                        BuyPrice = dr.Field<string>("BuyPrice"),
                        Quantity = dr.Field<string>("Quantity"),
                        UnitName = dr.Field<string>("UnitName"),
                        Description = dr.Field<string>("Description"), 
                    });
                }

                CatsDataGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void gridEditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (processName == "sale")
                {
                    //for accessing another window's control 
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(SaleBillWindow))
                        {
                            (window as SaleBillWindow).CBarCode_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["CatBarCode"].ToString();
                            (window as SaleBillWindow).CId_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["Id"].ToString();
                            (window as SaleBillWindow).CName_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["CatName"].ToString();
                            (window as SaleBillWindow).CPrice_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["SalePrice"].ToString();
                            (window as SaleBillWindow).CUnit_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["UnitName"].ToString();
                            (window as SaleBillWindow).CatQuantityInInv_tblock.Text = CatsDT.Rows[CatsDataGrid.SelectedIndex]["Quantity"].ToString();
                            this.Close();
                        }
                    }
                }
                else if (processName == "buy")
                {
                    //for accessing another window's control 
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(BuyBillWindow))
                        {
                            (window as BuyBillWindow).CBarCode_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["CatBarCode"].ToString();
                            (window as BuyBillWindow).CName_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["CatName"].ToString();
                            (window as BuyBillWindow).CPrice_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["SalePrice"].ToString();
                            (window as BuyBillWindow).CUnit_tb.CustomText = CatsDT.Rows[CatsDataGrid.SelectedIndex]["UnitName"].ToString();
                            this.Close();
                        }
                    }
                }
                else if(processName=="search")
                {
                CatWindow catWindow = new CatWindow(CatsDT.Rows[CatsDataGrid.SelectedIndex]["id"].ToString());
                this.Close();
                catWindow.Show();
                }
            }
            catch(Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
            //for accessing another window's control 
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(CatWindow))
            //    {
            //        (window as CatWindow).CatName_tb.CustomText = "I changed it from another window";
            //    }
            //}



        }

        private void gridRemoveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void exportToExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel.Application excel = new Excel.Application();
                excel.Visible = true; //www.yazilimkodlama.com
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet1 = (Worksheet)workbook.Sheets[1];


                for (int j = 0; j < CatsDataGrid.Columns.Count; j++) //Başlıklar için
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true; //Başlığın Kalın olması için
                    sheet1.Columns[j + 1].ColumnWidth = 15; //Sütun genişliği ayarı
                    myRange.Value2 = CatsDataGrid.Columns[j].Header;
                }
                Trace.WriteLine("col: " + CatsDataGrid.Columns.Count + "\n items : " + CatsDataGrid.Items.Count);
                for (int i = 0; i < CatsDataGrid.Columns.Count; i++)
                { //www.yazilimkodlama.com
                    for (int j = 0; j < CatsDataGrid.Items.Count; j++)
                    {
                        TextBlock? b = CatsDataGrid?.Columns[i].GetCellContent(CatsDataGrid.Items[j]) as TextBlock;

                        Trace.WriteLine(b?.Text);
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];

                        myRange.Value2 = b?.Text;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CatNameSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadCatsFromDB(CatNameSearchTB.CustomText.Length > 0 ? CatNameSearchTB.CustomText : "", CatBarcodeSearchTB.CustomText.Length > 0 ? CatBarcodeSearchTB.CustomText : "",
                CatPrimaryTypeSearchTB.CustomText.Length > 0 ? CatPrimaryTypeSearchTB.CustomText : "", CatSecondaryTypeSearchTB.CustomText.Length > 0 ? CatSecondaryTypeSearchTB.CustomText : "");
        }
        void LoadCatsFromDB(string CatName,string CatBarcode ,string PrimType,string SecType)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                    CatsDT.Clear();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from Categories where CatName like @CatName and CatBarCode like @CatBarCode and MainCatName like @MainCatName and SubCatName like @SubCatName", con);

                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@CatName", "%" + CatName + "%");
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@CatBarCode", "%" + CatBarcode + "%");
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@MainCatName", "%" + PrimType + "%");
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SubCatName", "%" + SecType + "%");
                      
                    sqlDataAdapter.Fill(CatsDT);
                    CatsDataGrid.ItemsSource = CatsDT.DefaultView;
                   



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        dynamic Excel.Window.Activate()
        {
            throw new NotImplementedException();
        }

        public dynamic ActivateNext()
        {
            throw new NotImplementedException();
        }

        public dynamic ActivatePrevious()
        {
            throw new NotImplementedException();
        }

        public bool Close(object SaveChanges, object Filename, object RouteWorkbook)
        {
            throw new NotImplementedException();
        }

        public dynamic LargeScroll(object Down, object Up, object ToRight, object ToLeft)
        {
            throw new NotImplementedException();
        }

        public Excel.Window NewWindow()
        {
            throw new NotImplementedException();
        }

        public dynamic _PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName)
        {
            throw new NotImplementedException();
        }

        public dynamic PrintPreview(object EnableChanges)
        {
            throw new NotImplementedException();
        }

        public dynamic ScrollWorkbookTabs(object Sheets, object Position)
        {
            throw new NotImplementedException();
        }

        public dynamic SmallScroll(object Down, object Up, object ToRight, object ToLeft)
        {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsX(int Points)
        {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsY(int Points)
        {
            throw new NotImplementedException();
        }

        public dynamic RangeFromPoint(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ScrollIntoView(int Left, int Top, int Width, int Height, object Start)
        {
            throw new NotImplementedException();
        }

        public dynamic PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName)
        {
            throw new NotImplementedException();
        }

        public Excel.Application Application => throw new NotImplementedException();

        public Excel.XlCreator Creator => throw new NotImplementedException();

        dynamic Excel.Window.Parent => throw new NotImplementedException();

        public Excel.Range ActiveCell => throw new NotImplementedException();

        public Excel.Chart ActiveChart => throw new NotImplementedException();

        public Excel.Pane ActivePane => throw new NotImplementedException();

        public dynamic ActiveSheet => throw new NotImplementedException();

        public dynamic Caption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayFormulas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayGridlines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHeadings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHorizontalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayOutline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool _DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayVerticalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWorkbookTabs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayZeros { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezePanes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int GridlineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Excel.XlColorIndex GridlineColorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Index => throw new NotImplementedException();

        public string OnWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Excel.Panes Panes => throw new NotImplementedException();

        public Excel.Range RangeSelection => throw new NotImplementedException();

        public int ScrollColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ScrollRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Excel.Sheets SelectedSheets => throw new NotImplementedException();

        public dynamic Selection => throw new NotImplementedException();

        public bool Split { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TabRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Excel.XlWindowType Type => throw new NotImplementedException();

        public double UsableHeight => throw new NotImplementedException();

        public double UsableWidth => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Excel.Range VisibleRange => throw new NotImplementedException();

        public int WindowNumber => throw new NotImplementedException();

        Excel.XlWindowState Excel.Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Excel.XlWindowView View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Excel.SheetViews SheetViews => throw new NotImplementedException();

        public dynamic ActiveSheetView => throw new NotImplementedException();

        public bool DisplayRuler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFilterDateGrouping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Hwnd => throw new NotImplementedException();

        
    }


   
}
