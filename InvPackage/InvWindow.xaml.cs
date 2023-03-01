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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InvntoryManagementSoftware.InvPackage
{
    /// <summary>
    /// Interaction logic for InvWindow.xaml
    /// </summary>
    public partial class InvWindow : Window
    {
        public InvWindow()
        {
            InitializeComponent(); var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();
            //Create DataGrid items info
            members.Add(new Member { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "John Doe", Position = "Coach", Email = "John.Doe@gmail.com", Phone = "01205057427" });
            members.Add(new Member { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Raze Alvai", Position = "Administrator", Email = "Raze.Alvai@gmail.com", Phone = "0142735689" });
            members.Add(new Member { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Deninis Castillo", Position = "Coach", Email = "Deninis.Castillo@gmail.com", Phone = "01246753335" });
            members.Add(new Member { Number = "4", Character = "F", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Frank Michael", Position = "Manager", Email = "Frank.Michael@gmail.com", Phone = "0123654752" });
            members.Add(new Member { Number = "5", Character = "S", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "Sara John", Position = "CEO", Email = "Sara.John@gmail.com", Phone = "01243765998" });
            members.Add(new Member { Number = "6", Character = "H", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Honz Deninis", Position = "Coach", Email = "Honz.Deninis@gmail.com", Phone = "0134657289" });
            members.Add(new Member { Number = "7", Character = "N", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Noah Cox", Position = "Manager", Email = "Noah.Cox@gmail.com", Phone = "01437245699" });
            members.Add(new Member { Number = "8", Character = "V", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Viola Muris", Position = "Coach", Email = "Viola.Muris@gmail.com", Phone = "01134276549" });
            members.Add(new Member { Number = "9", Character = "M", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "Michael Magdy", Position = "Manager", Email = "Michael.Magdy@gmail.com", Phone = "01342265721" });
            members.Add(new Member { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "John Doe", Position = "Coach", Email = "John.Doe@gmail.com", Phone = "01205057427" });
            members.Add(new Member { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Raze Alvai", Position = "Administrator", Email = "Raze.Alvai@gmail.com", Phone = "0142735689" });
            members.Add(new Member { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Deninis Castillo", Position = "Coach", Email = "Deninis.Castillo@gmail.com", Phone = "01246753335" });
            members.Add(new Member { Number = "4", Character = "F", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Frank Michael", Position = "Manager", Email = "Frank.Michael@gmail.com", Phone = "0123654752" });
            members.Add(new Member { Number = "5", Character = "S", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "Sara John", Position = "CEO", Email = "Sara.John@gmail.com", Phone = "01243765998" });
            members.Add(new Member { Number = "6", Character = "H", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Honz Deninis", Position = "Coach", Email = "Honz.Deninis@gmail.com", Phone = "0134657289" });
            members.Add(new Member { Number = "7", Character = "N", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Noah Cox", Position = "Manager", Email = "Noah.Cox@gmail.com", Phone = "01437245699" });
            members.Add(new Member { Number = "8", Character = "V", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Viola Muris", Position = "Coach", Email = "Viola.Muris@gmail.com", Phone = "01134276549" });
            members.Add(new Member { Number = "9", Character = "M", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "Michael Magdy", Position = "Manager", Email = "Michael.Magdy@gmail.com", Phone = "01342265721" });
            membersDataGrid.ItemsSource = members;
        }
    }
    public class Member
    {
        public string Character { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Brush BgColor { get; set; }
    }
}
