using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Window = System.Windows.Window;
using Application = System.Windows.Application;
using DataTable = System.Data.DataTable;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Resources.ResXFileRef;
using Excel = Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using InvntoryManagementSoftware.ClientsPackage;
using InvntoryManagementSoftware.SalePackage.SaleBillPackage;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Diagnostics;
using InvntoryManagementSoftware.BuyPackage.BuyBillPackage;

namespace InvntoryManagementSoftware.MowaredeenPackage
{
    /// <summary>
    /// Interaction logic for ClientsSearchWindow.xaml
    /// </summary>
    public partial class MowaredeenSearchWindow : Excel.Window
    {
        BrushConverter converter = new BrushConverter();
        SqlConnection con = App.con;
        DataTable MowaredeenDT = new DataTable();
        string processName;
        public MowaredeenSearchWindow()
        {
            InitializeComponent();
           
        }
        public MowaredeenSearchWindow(string processName)
        {
            InitializeComponent();
            _InitialMowaredeenData();
            this.processName = processName;


        }
        void _InitialMowaredeenData()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter RetreiveClients = new SqlDataAdapter("select * from Mowaredeen", con);
                RetreiveClients.Fill(MowaredeenDT);

                ObservableCollection<Mowaredeen> mowaredeen = new ObservableCollection<Mowaredeen>();
                foreach (DataRow dr in MowaredeenDT.Rows)
                {

                    mowaredeen.Add(new Mowaredeen
                    {
                        Id = dr.Field<Int32>("Id"),
                        MName = dr.Field<string>("MName"),
                        BgColor = (Brush)converter.ConvertFromString("#1e88e5"),
                        character = dr.Field<string>("MName").Substring(0, 1),
                        MPhone = dr.Field<string>("MPhone"),
                        MCompanyName = dr.Field<string>("MCompanyName"),
                        MGov = dr.Field<string>("MGov"),
                        MArea = dr.Field<string>("MArea"),
                        MEmail = dr.Field<string>("MEmail"),
                        MNotes = dr.Field<string>("MNotes"),
                        MState = dr.Field<string>("MState"),
                        MMoney = dr.Field<string>("MMoney")
                    });
                }

                membersDataGrid.ItemsSource = mowaredeen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClientDGEdit_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (processName == "buy")
                {
                    foreach (Window window in System.Windows.Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(BuyBillWindow))
                        {
                            (window as BuyBillWindow).ClientCode_tb.CustomText = MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["Id"].ToString();
                            (window as BuyBillWindow).ClientName_tb.CustomText = MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["MName"].ToString();
                            (window as BuyBillWindow).ClientPhone_tb.CustomText = MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["MPhone"].ToString();
                            (window as BuyBillWindow).ClientState_tblock.Text = MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["MState"].ToString();
                            (window as BuyBillWindow).ClientMoney_tb.CustomText = MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["MMoney"].ToString();
                            (window as BuyBillWindow).clientSectionErrorText.Visibility = Visibility.Collapsed;
                            this.Close();
                        }
                    }
                }
                else if (processName == "search")
                {
                    MowaredeenWindow mowaredeenWindow = new MowaredeenWindow(MowaredeenDT.Rows[membersDataGrid.SelectedIndex]["Id"].ToString());
                    this.Close();
                    mowaredeenWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clientDGRemove_btn_Click(object sender, RoutedEventArgs e)
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


                for (int j = 0; j < membersDataGrid.Columns.Count; j++) //Başlıklar için
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true; //Başlığın Kalın olması için
                    sheet1.Columns[j + 1].ColumnWidth = 15; //Sütun genişliği ayarı
                    myRange.Value2 = membersDataGrid.Columns[j].Header;
                }
                Trace.WriteLine("col: " + membersDataGrid.Columns.Count + "\n items : " + membersDataGrid.Items.Count);
                for (int i = 0; i < membersDataGrid.Columns.Count; i++)
                { //www.yazilimkodlama.com
                    for (int j = 0; j < membersDataGrid.Items.Count; j++)
                    {
                        TextBlock? b = membersDataGrid?.Columns[i].GetCellContent(membersDataGrid.Items[j]) as TextBlock;

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
        private void ClientnameSearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {

            LoadMowaredeenFromDB(MowaredNameSearchTB.CustomText.Length > 0 ? MowaredNameSearchTB.CustomText : "",
                MowaredPhoneSearchTB.CustomText.Length > 0 ? MowaredPhoneSearchTB.CustomText : "",
               MowaredCompanyNameSearchTB.CustomText.Length > 0 ? MowaredCompanyNameSearchTB.CustomText : "");

        }
        void LoadMowaredeenFromDB(string Mname, string MPhone, string MCompanyName)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                MowaredeenDT.Clear();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from Mowaredeen where MName like @MName and MPhone like @MPhone and MCompanyName like @MCompanyName", con);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@MName", "%" + Mname + "%");
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@MPhone", "%" + MPhone + "%");
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@MCompanyName", "%" + MCompanyName + "%");

                sqlDataAdapter.Fill(MowaredeenDT);
                membersDataGrid.ItemsSource = MowaredeenDT.DefaultView;




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

        public Range ActiveCell => throw new NotImplementedException();

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

        public Range RangeSelection => throw new NotImplementedException();

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

        public Range VisibleRange => throw new NotImplementedException();

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