using InvntoryManagementSoftware.CatPackage;
using InvntoryManagementSoftware.ClientsPackage;
using InvntoryManagementSoftware.MandoobPackage;
using InvntoryManagementSoftware.MowaredeenPackage;
using InvntoryManagementSoftware.SalePackage.SaleBillPackage;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Resources.ResXFileRef;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Diagnostics;
using InvntoryManagementSoftware.PaymentsPackage.ClientsPaymentPackage;
using InvntoryManagementSoftware.BuyPackage.BuyBillPackage;

namespace InvntoryManagementSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Excel.Window
    {
        BrushConverter converter = new BrushConverter();
        SqlConnection con = App.con;
        DataTable CatsDT = new DataTable();
        public MainWindow()
        {
            InitializeComponent(); //var converter = new BrushConverter();
            //ObservableCollection<Member> members = new ObservableCollection<Member>();
            _InitialCatsData();

        }
        void _InitialCatsData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                CatsDT.Clear();
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
                        character = dr.Field<string>("CatName").Substring(0, 1),
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
        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello in new menu item");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs"; 
            //    saveFileDialog.InitialDirectory = @"E:\";
            //if (saveFileDialog.ShowDialog() == true)
            //{

            //    File.WriteAllText(saveFileDialog.FileName, "الأصناف");
            //}
        }
        private void clientsBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientsPackage.ClientsWindow clientsWindow= new ClientsPackage.ClientsWindow();
            clientsWindow.ShowDialog();
        }

        private void mowaredeenWindowMI_Click(object sender, RoutedEventArgs e)
        {
            MowaredeenWindow mowaredeenWindow = new MowaredeenWindow();
            mowaredeenWindow.ShowDialog();
        }

        private void MandoobWindowMI_Click(object sender, RoutedEventArgs e)
        {
            MandoobWindow mandoobWindow = new MandoobWindow();
            mandoobWindow.ShowDialog();
        }

        private void CatsBtn_Click(object sender, RoutedEventArgs e)
        {
            CatPackage.CatWindow catWindow = new CatPackage.CatWindow();
            catWindow.ShowDialog();
        }

        private void MainCatWindowMI_Click(object sender, RoutedEventArgs e)
        {
            addorMainCatName.AddMainCatName addMainCatName = new addorMainCatName.AddMainCatName();
            addMainCatName.ShowDialog();
        }

        private void SubCatWindowMI_Click(object sender, RoutedEventArgs e)
        {
            addSubCatName.AddSubCatName addSubCatName = new addSubCatName.AddSubCatName();
            addSubCatName.ShowDialog();
        }

        private void SaleBillBtn_Click(object sender, RoutedEventArgs e)
        {
            SaleBillWindow saleBillWindow = new SaleBillWindow();
            saleBillWindow.ShowDialog();
        }

        private void MowaredeenPaymentsMI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClientsPaymentsMI_Click(object sender, RoutedEventArgs e)
        {
            ClientsPaymentsWindow clientsPaymentsWindow = new ClientsPaymentsWindow();
            clientsPaymentsWindow.ShowDialog();
        }
        private void ReloadWindow_btn_Click(object sender, RoutedEventArgs e)
        {
            _InitialCatsData();
        }
        private void BuySaleWindow_Click(object sender, RoutedEventArgs e)
        {
            BuyBillWindow buyBillWindow = new BuyBillWindow();
            buyBillWindow.ShowDialog();
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

        public XlCreator Creator => throw new NotImplementedException();

        dynamic Excel.Window.Parent => throw new NotImplementedException();

        public Range ActiveCell => throw new NotImplementedException();

        public Chart ActiveChart => throw new NotImplementedException();

        public Pane ActivePane => throw new NotImplementedException();

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
        public XlColorIndex GridlineColorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Index => throw new NotImplementedException();

        public string OnWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Panes Panes => throw new NotImplementedException();

        public Range RangeSelection => throw new NotImplementedException();

        public int ScrollColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ScrollRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Sheets SelectedSheets => throw new NotImplementedException();

        public dynamic Selection => throw new NotImplementedException();

        public bool Split { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TabRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public XlWindowType Type => throw new NotImplementedException();

        public double UsableHeight => throw new NotImplementedException();

        public double UsableWidth => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Range VisibleRange => throw new NotImplementedException();

        public int WindowNumber => throw new NotImplementedException();

        XlWindowState Excel.Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlWindowView View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SheetViews SheetViews => throw new NotImplementedException();

        public dynamic ActiveSheetView => throw new NotImplementedException();

        public bool DisplayRuler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFilterDateGrouping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Hwnd => throw new NotImplementedException();

       
    }
   
}
