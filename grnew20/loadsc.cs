using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Management;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace grnew20
{
    public partial class loadsc : Form
    {
        private Form2 frm = new Form2();

        public void shutdown()
        {
            Environment.Exit(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //dual screen
            if (Screen.AllScreens.Length > 1)
            {
                frm.StartPosition = FormStartPosition.Manual;

                Screen screen = GetSecondaryScreen();

                frm.Location = screen.WorkingArea.Location;

                frm.Size = new Size(screen.WorkingArea.Width, screen.WorkingArea.Height);

                frm.Show();
            }
        }

        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }

        public static string GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }

        public static string GetProcessorId()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String Id = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                Id = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return Id;
        }

        public static string GetBoardMaker()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch { }
            }

            return "Board Maker: Unknown";
        }

        public static string GetCdRomDrive()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Drive").ToString();
                }
                catch { }
            }

            return "CD ROM Drive Letter: Unknown";
        }

        public static string GetBoardProductId()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Product").ToString();
                }
                catch { }
            }

            return "Product: Unknown";
        }

        public static string GetBIOSmaker()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch { }
            }

            return "BIOS Maker: Unknown";
        }

        public static string GetBIOSserNo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("SerialNumber").ToString();
                }
                catch { }
            }

            return "BIOS Serial Number: Unknown";
        }

        public static string GetBIOScaption()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Caption").ToString();
                }
                catch { }
            }
            return "BIOS Caption: Unknown";
        }

        public static string GetAccountName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("Name").ToString();
                }
                catch { }
            }
            return "User Account Name: Unknown";
        }

        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;
            long mCap = 0;

            foreach (ManagementObject obj in oCollection)
            {
                mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            MemSize = (MemSize / 1024) / 1024;
            return MemSize.ToString() + "MB";
        }

        public static string GetNoRamSlots()
        {
            int MemSlots = 0;
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
            ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2);
            ManagementObjectCollection oCollection2 = oSearcher2.Get();
            foreach (ManagementObject obj in oCollection2)
            {
                MemSlots = Convert.ToInt32(obj["MemoryDevices"]);
            }
            return MemSlots.ToString();
        }

        public static string GetCPUManufacturer()
        {
            string cpuMan = String.Empty;

            ManagementClass mgmt = new ManagementClass("Win32_Processor");

            ManagementObjectCollection objCol = mgmt.GetInstances();

            foreach (ManagementObject obj in objCol)
            {
                if (cpuMan == String.Empty)
                {

                    cpuMan = obj.Properties["Manufacturer"].Value.ToString();
                }
            }
            return cpuMan;
        }

        public static int GetCPUCurrentClockSpeed()
        {
            int cpuClockSpeed = 0;

            ManagementClass mgmt = new ManagementClass("Win32_Processor");

            ManagementObjectCollection objCol = mgmt.GetInstances();

            foreach (ManagementObject obj in objCol)
            {
                if (cpuClockSpeed == 0)
                {

                    cpuClockSpeed = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
                }
            }

            return cpuClockSpeed;
        }

        public static string GetDefaultIPGateway()
        {

            ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection objCol = mgmt.GetInstances();
            string gateway = String.Empty;

            foreach (ManagementObject obj in objCol)
            {
                if (gateway == String.Empty)  
                {

                    if ((bool)obj["IPEnabled"] == true)
                    {
                        gateway = obj["DefaultIPGateway"].ToString();
                    }
                }

                obj.Dispose();
            }

            gateway = gateway.Replace(":", "");

            return gateway;
        }

        public static double? GetCpuSpeedInGHz()
        {
            double? GHz = null;
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    GHz = 0.001 * (UInt32)mo.Properties["CurrentClockSpeed"].Value;
                    break;
                }
            }
            return GHz;
        }

        public static string GetCurrentLanguage()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return wmi.GetPropertyValue("CurrentLanguage").ToString();
                }
                catch { }
            }

            return "BIOS Maker: Unknown";
        }

        public static string GetOSInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
                }
                catch { }
            }
            return "BIOS Maker: Unknown";
        }

        public static string GetProcessorInformation()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                string name = (string)mo["Name"];
                name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

                info = name + ", " + (string)mo["Caption"] + ", " + (string)mo["SocketDesignation"];

            }
            return info;
        }

        public static string GetComputerName()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            String info = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                info = (string)mo["Name"];

            }
            return info;
        }

        public Screen GetSecondaryScreen()
        {
            if (Screen.AllScreens.Length == 1)
            {
                return null;
            }

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    return screen;
                }
            }

            return null;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetLocalIPV6Address()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }//EVENTS

        /*       C   O  M  M  A  N  D  S         */
        private string value = "";

        private string help_x = "JAEG SOFTWARE Terminal   V.01\n" + "Date : [ " + DateTime.Now.ToString() + " ]" + "\n\n" +
            "[ COMMANDS ]\n" +
            "[ help -x ]        : Shows all commands and help.\n" +
            "[ goto path ]      : Goes to location of your given path. Ex : goto C:\\MyFiles\\JaegOs \n" +
            "[ read -x ]        : This command shows the contents of the stated readable file.\n" +
            "[ read -n ]        : This command shows the contents of the stated readable file with line numbers.\n" +
            "[ crdir dirname ]  : Creates a directory as given name at your current location. Ex : crdir MyDirectory\n" +
            "[ rmdir dirname ]  : Deletes a directory as given name at your current location or at given location. Ex : rmdir MyDirectory   or   Ex : rmdir c:\\MyDirectory\n" +
            "[ crfile -x ]      : Creates a text or any file at your given location and with your given file name. Ex: crfile -x C:\\jaegos.txt \n" +
            "[ rmfile -x ]      : Deleting file as your stated name at your current location or from given location.\n" +
            "[ edit -n ]        : Edits the stated line of the given file from given location.  Ex : edit -n c:\\MyText.txt -3 \n" +
            "[ editor ]         : Opens JAEGOS's text editor for your file of be edited from given location.  Ex : editor c:\\MyText.txt\n" +
            "[ list -x ]        : Lists all files in your location.\n" +
            "[ whoami ]         : Shows pc name.\n" +
            "[ whereami ]       : Shows your current directory location.\n" +
            "[ ip -v4 ]         : Shows your Local IpV4 address.\n" +
            "[ ip -v6 ]         : Shows your Local IpV6 address.\n" +
            "[ history ]        : Lists the commands you have previously issued on the command line.\n" +
            "[ shutdown -x ]    : Shutdown Jaeg OS.\n";

        private string whoami = "You Are " + GetAccountName().ToString();
        private string ipv4 = "[ IpV4 ADDRESS ]\n" + GetLocalIPAddress().ToString();
        private string ipv6 = "[ IpV6 ADDRESS ]\n" + GetLocalIPV6Address().ToString();

        static public SoundPlayer startup = new SoundPlayer();
        private string way = "startup.wav";

        static public SoundPlayer background = new SoundPlayer();
        private string waybg = "bgsound.wav";

        //specs
        private double fps;

        private int a = 0;
        private int b = 0;

        //
        private int status = 0;

        private string efile = "";
        private int index = 0;
        private string indexs = "";

        private string startuplbl = "      > A JAEG Software.";
        private string startuplbl2 = "      > Copyright (C) 1990-2020, JAEG Software Inc.";
        private string startuplbl3 = "_______________________________________________________";
        private string lbl1 = "[ OS VERSION ]\n" + Environment.OSVersion.ToString();
        private string lbl2 = "[ SYSTEM DIRECTORY ]\n" + Environment.SystemDirectory.ToString();
        private string lbl3 = "[ MACHINE NAME ]\n" + Environment.MachineName.ToString();
        private string lbl4 = "[ PROCESSOR COUNT ] : " + Environment.ProcessorCount.ToString();
        private string lbl5 = "[ USERNAME ] : " + Environment.UserName.ToString() + "\n\n[!] Starting [JCS Specs Protocol] ....  > SUCCESS !";
        private string lbl6 = "[ TICK RATE : 00000000 ]";

        //------------------ JCS SP //
        private string lbl10 = ">> [ JCS SProtocol ]  Receiving personal computer specs....";

        private string lbl11 = ">> [ JCS SProtocol ]  SUCCES !";
        private string lbl13 = ">> [ JCS SProtocol ]  All done!\n>> [ JCS SProtocol ]  JCS SP Deactivating now...";

        //------------------ Terminal //
        private string username = "root-" + GetAccountName().ToString().ToLower();

        private string lbl18 = "JAEG OS System Terminal\n\n>> Welcome. Type 'help -x' without quotes for all commands and help.\n";

        //specs_lbl

        private string spc1 = "[ COMPUTER NAME ] : " + GetComputerName().ToString();
        private string spc2 = "[ ACCOUNT NAME ] : " + GetAccountName().ToString();
        private string spc3 = "[ MOTHERBOARD MAKER ] : " + GetBoardMaker().ToString();
        private string spc4 = "[ MAC ADRESS ] : " + GetMACAddress().ToString();
        private string spc5 = "[ CPU MANUFACTURER ] : " + GetCPUManufacturer().ToString();
        private string spc6 = "[ BIOS MAKER ] : " + GetBIOSmaker().ToString();
        private string spc7 = "[ BIOS CAPTION ] : " + GetBIOScaption().ToString();
        private string spc8 = "[ BIOS SERIAL ] : " + GetBIOSserNo().ToString();
        private string spc9 = "[ CPU CURRENT CLOCK SPEED ] : " + GetCPUCurrentClockSpeed().ToString();
        private string spc10 = "[ CPU GHz SPEED ] : " + GetCpuSpeedInGHz().ToString();
        private string spc11 = "[ RAM SLOTS ] : " + GetNoRamSlots().ToString();
        private string spc12 = "[ OS INFORMATION ] : " + GetOSInformation().ToString();
        private string spc13 = "[ PROCESSOR INFORMATION ]\n[ " + GetProcessorInformation().ToString() + " ]";
        private string spc14 = "[ PROCESSOR ID ] : " + GetProcessorId().ToString();
        private string spc15 = "[ PYHSICAL MEMORY ] : " + GetPhysicalMemory().ToString();
        private string spc16 = "[ CURRENT LANGUAGE ] : " + GetCurrentLanguage().ToString();

        public loadsc()
        {
            InitializeComponent();
        }

        public int xax;
        public string root;
        public string path;
        private string fpath = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            path = @"C:\";
            root = @"C:\";

            //coords_and_sizes
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel1.Location = new Point(3, 3);
            panel2.Location = new Point(3, 3);
            panel3.Location = new Point(3, 3);
            panel4.Location = new Point(3, 3);
            pictureBox1.Visible = false;
            panel2.Enabled = false;
            panel3.Enabled = false;
            panel4.Enabled = false;
            int x = this.Width;
            int y = this.Height;

            xax = pictureBox1.Location.X;

            panel3.Width = xax;

            if (x < 1600)
            {
                panel2.Size = new Size((x / 100) * 80, y - 3);
            }
            else
            {
                panel2.Size = new Size((x / 100) * 60, y - 3);
            }
            int ca = panel2.Width;
            panel3.Size = new Size(pictureBox1.Location.X, y - 1);
            panel4.Size = new Size((x * 55 / 100) + 25, y - 1);
            textBox1.Size = new Size(ca - 200, 50);

            pictureBox1.Location = new Point(x - 320, 10);
            label12.Location = new Point(panel2.Location.X + 50, panel2.Location.Y + 3);

            label19.Height = this.Height - 50;

            //spec_locations
            spec1.Location = new Point(45, (panel2.Height / 4) + 30);
            spec2.Location = new Point(45, spec1.Location.Y + 30);
            spec3.Location = new Point(45, spec2.Location.Y + 30);
            spec4.Location = new Point(45, spec3.Location.Y + 30);
            spec5.Location = new Point(45, spec4.Location.Y + 30);
            spec6.Location = new Point(45, spec5.Location.Y + 30);
            spec7.Location = new Point(45, spec6.Location.Y + 30);
            spec8.Location = new Point(45, spec7.Location.Y + 30);
            spec9.Location = new Point(45, spec8.Location.Y + 30);
            spec10.Location = new Point(45, spec9.Location.Y + 30);
            spec11.Location = new Point(45, spec10.Location.Y + 30);
            spec12.Location = new Point(45, spec11.Location.Y + 30);
            spec13.Location = new Point(45, spec12.Location.Y + 30);
            spec14.Location = new Point(45, spec13.Location.Y + 55);
            spec15.Location = new Point(45, spec14.Location.Y + 30);
            spec16.Location = new Point(45, spec15.Location.Y + 30);
            //---//
            label13.Location = new Point(42, label11.Location.Y + 50);

            this.Cursor = new Cursor("asd.ico");

            timer1.Enabled = false;

            //label_colors
            label1.ForeColor = Color.Lime;
            label2.ForeColor = Color.Lime;
            label3.ForeColor = Color.Lime;
            label4.ForeColor = Color.Lime;
            label5.ForeColor = Color.Lime;
            label6.ForeColor = Color.Lime;

            //bg_sound
            startup.SoundLocation = way;
            startup.Play();

            //labels
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label13.Text = "";
            label18.Text = "";
            label19.Text = "";
            label20.Text = "";
            usrnm.Text = username;

            //specs
            spec1.Text = "";
            spec2.Text = "";
            spec3.Text = "";
            spec4.Text = "";
            spec5.Text = "";
            spec6.Text = "";
            spec7.Text = "";
            spec8.Text = "";
            spec9.Text = "";
            spec10.Text = "";
            spec11.Text = "";
            spec12.Text = "";
            spec13.Text = "";
            spec14.Text = "";
            spec15.Text = "";
            spec16.Text = "";

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            usrnm.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fps = System.Environment.TickCount;
            label6.Text = "[ TICK RATE : " + fps + " ]";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            {
                a = a + 1; ;
                if (a == 1)
                {
                    label7.Visible = true;
                    this.backgroundWorker7.RunWorkerAsync();
                }
                else if (a == 2)
                {
                    label8.Visible = true;
                    this.backgroundWorker8.RunWorkerAsync();
                }
                else if (a == 3)
                {
                    label9.Visible = true;
                    this.backgroundWorker9.RunWorkerAsync();
                }
                else if (a == 4)
                {
                    label1.Visible = true;
                    this.backgroundWorker1.RunWorkerAsync();
                }
                else if (a == 5)
                {
                    label2.Visible = true;
                    this.backgroundWorker2.RunWorkerAsync();
                }
                else if (a == 6)
                {
                    label3.Visible = true;
                    this.backgroundWorker3.RunWorkerAsync();
                }
                else if (a == 7)
                {
                    label6.Visible = true;
                    this.backgroundWorker6.RunWorkerAsync();
                }
                else if (a == 8)
                {
                    timer1.Enabled = true;
                    label4.Visible = true;
                    this.backgroundWorker4.RunWorkerAsync();
                }
                else if (a == 9)
                {
                    label5.Visible = true;
                    this.backgroundWorker5.RunWorkerAsync();
                    timer2.Enabled = false;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < lbl1.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label1.Text += (lbl1[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker2_DoWork_1(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < lbl2.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label2.Text += (lbl2[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.CancelAsync();
        }

        private void backgroundWorker3_DoWork_1(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < lbl3.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label3.Text += (lbl3[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker3.WorkerSupportsCancellation = true;
            this.backgroundWorker3.CancelAsync();
        }

        private void backgroundWorker4_DoWork_1(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            for (int i = 0; i < lbl4.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label4.Text += (lbl4[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker4.WorkerSupportsCancellation = true;
            this.backgroundWorker4.CancelAsync();
        }

        private void backgroundWorker5_DoWork_1(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            for (int i = 0; i < lbl5.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label5.Text += (lbl5[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            System.Threading.Thread.Sleep(2500);
            panel1.Visible = false;
            panel1.Enabled = false;
            System.Threading.Thread.Sleep(750);
            panel2.Enabled = true;
            panel2.Visible = true;

            this.backgroundWorker10.RunWorkerAsync();

            backgroundWorker5.WorkerSupportsCancellation = true;
            this.backgroundWorker5.CancelAsync();
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < lbl6.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label6.Text += (lbl6[i].ToString()); }));

                System.Threading.Thread.Sleep(25);

                backgroundWorker6.WorkerSupportsCancellation = true;
                this.backgroundWorker6.CancelAsync();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            b = b + 1;
            if (b == 12)
            {
                background.SoundLocation = waybg;
                background.PlayLooping();
            }
        }

        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            pictureBox1.Visible = true;
            frm.a = 1;
            System.Threading.Thread.Sleep(1000);
            for (int i = 0; i < startuplbl.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label7.Text += (startuplbl[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker7.WorkerSupportsCancellation = true;
            this.backgroundWorker7.CancelAsync();
        }

        private void backgroundWorker8_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < startuplbl2.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label8.Text += (startuplbl2[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker8.WorkerSupportsCancellation = true;
            this.backgroundWorker8.CancelAsync();
        }

        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            for (int i = 0; i < startuplbl3.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label9.Text += (startuplbl3[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            backgroundWorker9.WorkerSupportsCancellation = true;
            this.backgroundWorker9.CancelAsync();
            if (panel2.Focused == true)
            {
            }
        }

        private void loadsc_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            label10.Visible = true;
            label11.Visible = true;

            timer4.Enabled = false;
        }

        private void backgroundWorker10_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            for (int i = 0; i < lbl10.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label10.Text += (lbl10[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            this.backgroundWorker11.RunWorkerAsync();
            backgroundWorker10.WorkerSupportsCancellation = true;
            this.backgroundWorker10.CancelAsync();
        }

        private void backgroundWorker11_DoWork(object sender, DoWorkEventArgs e)
        {
            //JSC SPECS PROTOCOL SYSTEM CONTROL By JAEG SOFTWARE
            System.Threading.Thread.Sleep(1000);
            label11.Visible = true;
            if (GetHDDSerialNo().ToString() == null || GetProcessorId().ToString() == null || GetBoardMaker().ToString() == null || GetCdRomDrive().ToString() == null || GetBoardProductId().ToString() == null ||
                GetBIOSmaker().ToString() == null || GetBIOSserNo().ToString() == null || GetBIOScaption().ToString() == null || GetAccountName().ToString() == null || GetPhysicalMemory().ToString() == null ||
                GetNoRamSlots().ToString() == null || GetCPUCurrentClockSpeed().ToString() == null || GetDefaultIPGateway().ToString() == null || GetCpuSpeedInGHz().ToString() == null ||
                GetCurrentLanguage().ToString() == null || GetOSInformation().ToString() == null || GetComputerName().ToString() == null || GetCPUManufacturer().ToString() == null)
            {
                lbl11 = ">> [ JCS SProtocol ]  FAILED !";
            }
            else
            {
                lbl11 = ">> [ JCS SProtocol ]  SUCCESS !\n>> [ JCS SProtocol ]  Protocol In Progress..";
            }
            System.Threading.Thread.Sleep(500);

            for (int i = 0; i < lbl11.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label11.Text += (lbl11[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //specs 1
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc1.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec1.Text += (spc1[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //2
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc2.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec2.Text += (spc2[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //3
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc3.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec3.Text += (spc3[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            //4
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc4.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec4.Text += (spc4[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //5
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc5.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec5.Text += (spc5[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //6
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc6.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec6.Text += (spc6[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //7
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc7.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec7.Text += (spc7[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            //8
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc8.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec8.Text += (spc8[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //9
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc9.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec9.Text += (spc9[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //10
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc10.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec10.Text += (spc10[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //11
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc11.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec11.Text += (spc11[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //12
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc12.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec12.Text += (spc12[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }
            //13
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc13.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec13.Text += (spc13[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //14
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc14.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec14.Text += (spc14[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //15
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc15.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec15.Text += (spc15[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            //16
            System.Threading.Thread.Sleep(450);
            for (int i = 0; i < spc16.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { spec16.Text += (spc16[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            System.Threading.Thread.Sleep(500);
            //17
            System.Threading.Thread.Sleep(1000);
            for (int i = 0; i < lbl13.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label13.Text += (lbl13[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            System.Threading.Thread.Sleep(1500);
            panel2.Enabled = false;
            panel2.Visible = false;
            panel3.Enabled = true;
            panel3.Visible = true;
            this.backgroundWorker12.RunWorkerAsync();

            backgroundWorker11.WorkerSupportsCancellation = true;
            this.backgroundWorker11.CancelAsync();
        }

        private void backgroundWorker12_DoWork(object sender, DoWorkEventArgs e)
        {
            int x = this.Width;
            int y = this.Height;
            panel3.Size = new Size(pictureBox1.Location.X, y - 1);
            richTextBox1.Size = new Size(panel4.Width - 50, panel4.Height - 200);
            textBox1.Location = new Point(36, label18.Location.Y + 90);

            label18.Visible = true;
            usrnm.Visible = true;
            usrnm.Text = username;

            int cc = usrnm.Width;
            usrnm.Location = new Point((x / 2) - (cc / 2), 5);

            for (int i = 0; i < lbl18.Length; i++)
            {
                Invoke(new MethodInvoker(delegate { label18.Text += (lbl18[i].ToString()); }));

                System.Threading.Thread.Sleep(25);
            }

            backgroundWorker11.WorkerSupportsCancellation = true;
            this.backgroundWorker11.CancelAsync();
        }

        private void backgroundWorker13_DoWork(object sender, DoWorkEventArgs e)
        {
            label19.Visible = true;
            label19.Location = new Point(36, textBox1.Location.Y + 50);
            panel3.Location = new Point(0, 0);
            backgroundWorker11.WorkerSupportsCancellation = true;
            this.backgroundWorker11.CancelAsync();
        }

        //background workers

        private void timer5_Tick(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            richTextBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private string[] histor = new string[0];
        public List<string> history = new List<string>();
        public List<string> pathhistory = new List<string>();

        public int hsar = 0;

        private int qwe = 0;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
                label19.Font = new Font("Consolas", 10, FontStyle.Regular);
                e.SuppressKeyPress = true;
                value = textBox1.Text.ToString();
                history.Add(value);
                hsar = history.Count;
                textBox1.Clear();
                this.backgroundWorker13.RunWorkerAsync();

                if (status != 0)
                {
                    editline();
                }
                else
                {
                    if (value == "help -x")
                    {
                        string hellp_x= "JAEG SOFTWARE Terminal   V.01\n" + "Date : [ " + DateTime.Now.ToString() + " ]" + "\n\n" +
            "[ COMMANDS ]\n" +
            "[ help -x ]        : Shows all commands and help.\n" +
            "[ goto path ]      : Goes to location of your given path. Ex : goto C:\\MyFiles\\JaegOs \n" +
            "[ read -x ]        : This command shows the contents of the stated readable file.\n" +
            "[ read -n ]        : This command shows the contents of the stated readable file with line numbers.\n" +
            "[ crdir dirname ]  : Creates a directory as given name at your current location. Ex : crdir MyDirectory\n" +
            "[ rmdir dirname ]  : Deletes a directory as given name at your current location or at given location. Ex : rmdir MyDirectory   or   Ex : rmdir c:\\MyDirectory\n" +
            "[ crfile -x ]      : Creates a text or any file at your given location and with your given file name. Ex: crfile -x C:\\jaegos.txt \n" +
            "[ rmfile -x ]      : Deleting file as your stated name at your current location or from given location.\n" +
            "[ edit -n ]        : Edits the stated line of the given file from given location.  Ex : edit -n c:\\MyText.txt -3 \n" +
            "[ editor ]         : Opens JAEGOS's text editor for your file of be edited from given location.  Ex : editor c:\\MyText.txt\n" +
            "[ list -x ]        : Lists all files in your location.\n" +
            "[ whoami ]         : Shows pc name.\n" +
            "[ whereami ]       : Shows your current directory location.\n" +
            "[ ip -v4 ]         : Shows your Local IpV4 address.\n" +
            "[ ip -v6 ]         : Shows your Local IpV6 address.\n" +
            "[ history ]        : Lists the commands you have previously issued on the command line.\n" +
            "[ shutdown -x ]    : Shutdown Jaeg OS.\n";
                        label19.Text = ">" + value + "\n" + hellp_x;
                        label19.Visible = true;
                    }
                    /*else if (value == null || value == "" || value == " ")
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands. ";
                        label19.Visible = true;
                    }*/
                    else if (value == "whoami")
                    {
                        label19.Text = ">" + value + "\n" + whoami;
                        label19.Visible = true;
                    }
                    else if (value == "ip -v4")
                    {
                        label19.Text = ">" + value + "\n" + ipv4;
                        label19.Visible = true;
                    }
                    else if (value == "ip -v6")
                    {
                        label19.Text = ">" + value + "\n" + ipv6;
                        label19.Visible = true;
                    }
                    else if (value == "shutdown -x")
                    {
                        label19.Text = ">" + value + "\nShutting down now..";
                        label19.Visible = true;
                        timer6.Enabled = true;
                    }
                    /* WHEREAMI SHOW PATH BY JAEG */
                    else if (value == "whereami")
                    {
                        label19.Text = ">" + value + "\n" + path;
                        label19.Visible = true;
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FOLDER CREATE BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value == "crdir")
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'crdir dirname' ?";
                        label19.Visible = true;
                    }
                    else if (value.Length > 6 && value[0] == 'c' && value[1] == 'r' && value[2] == 'd' && value[3] == 'i' && value[4] == 'r' && value[5] == ' ')

                    {
                        //create file by jaeg os
                        label19.Text = ">" + value + "\n";
                        label19.Visible = true;

                        int lngth = value.Length;
                        string filename = "";

                        for (int i = 6; i < lngth; i++)
                        {
                            filename = filename + value[i];
                        }

                        Directory.CreateDirectory(root + "\\" + filename);
                        label19.Text = ">" + value + "\n[!] Directory was created as " + filename + " at " + path + ".";
                        label19.Visible = true;
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FOLDER DELETE BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value == "rmdir")
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'rmdir dirname' ?";
                        label19.Visible = true;
                    }
                    else if (value.Length > 6 && value[0] == 'r' && value[1] == 'm' && value[2] == 'd' && value[3] == 'i' && value[4] == 'r' && value[5] == ' ')
                    {
                        //delete folder by jaeg os
                        label19.Text = ">" + value + "\n";
                        label19.Visible = true;

                        int lngth = value.Length;
                        string filename = "";

                        for (int i = 6; i < lngth; i++)
                        {
                            filename = filename + value[i];
                        }

                        if (Directory.Exists(root + filename) == true)
                        {
                            Directory.Delete(root + "\\" + filename, true);
                            label19.Text = ">" + value + "\n[!] Directory '" + filename + "' is deleted at > " + path + ".";
                        }
                        else if (Directory.Exists(filename) == true)
                        {
                            int q = 0;
                            for (int j = filename.Length - 1; j > 0; j--)
                            {
                                if (filename[j] == '\\')
                                {
                                    q = j;
                                    break;
                                }
                            }

                            string file = "";

                            for (int f = q; f < filename.Length; f++)
                            {
                                file += filename[f];
                            }

                            Directory.Delete(filename, true);
                            label19.Text = ">" + value + "\n[!] Directory '" + file + "' is deleted at > " + path + ".";
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n[!] Directory '" + filename + "' is not exists at > " + path + " .";
                        }

                        label19.Visible = true;
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* CREATE FILE BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value == "cfile -x" || value == "crfile" || value == "crfile " || value == "crfile -")
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'crfile -x path/filename' ?";
                        label19.Visible = true;
                    }
                    else if (value.Length > 10 && value[0] == 'c' && value[1] == 'r' && value[2] == 'f' && value[3] == 'i' && value[4] == 'l' && value[5] == 'e' && value[6] == ' ' && value[7] == '-' && value[8] == 'x' && value[9] == ' ')
                    {
                        int fl = 0;
                        string file = "";
                        string filename = "";
                        for (int i = 10; i <= value.Length - 1; i++)
                        {
                            filename += value[i];
                        }

                        for (int i = filename.Length - 1; i > 0; i--)
                        {
                            if (filename[i] == '\\')
                            {
                                fl = i;
                                break;
                            }
                        }

                        for (int i = 0; i < fl; i++)
                        {
                            file += filename[i];
                        }

                        if (File.Exists(filename))
                        {
                            label19.Text = ">" + value + "\n" + "[!] File is not created. Text file is already exists.";
                        }
                        else if (Directory.Exists(file) == true)
                        {
                            if (filename[filename.Length - 1] == 't' && filename[filename.Length - 2] == 'x' && filename[filename.Length - 3] == 't' && filename[filename.Length - 4] == '.')
                            {
                                using (StreamWriter sw = File.CreateText(filename))
                                {
                                    sw.WriteLine("This file created by " + GetAccountName().ToString() + ". [" + DateTime.Now.ToString() + "]");
                                }
                            }
                            else
                            {
                                File.CreateText(filename);
                            }

                            label19.Text = ">" + value + "\n" + "[!] File is created at >" + filename;
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n" + "[!] File is not created. Unknown Path.";
                        }
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FILE DELETE BY JAEGER */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value == "rmfile -x")
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'rmfile -x' ?";
                        label19.Visible = true;
                    }
                    else if (value.Length > 10 && value[0] == 'r' && value[1] == 'm' && value[2] == 'f' && value[3] == 'i' && value[4] == 'l' && value[5] == 'e' && value[6] == ' ' && value[7] == '-' && value[8] == 'x' && value[9] == ' ')
                    {
                        label19.Text = ">" + value + "\n";
                        label19.Visible = true;

                        int lngth = value.Length;
                        string filename = "";

                        for (int i = 10; i < lngth; i++)
                        {
                            filename = filename + value[i];
                        }

                        if (File.Exists(root + filename) == true)
                        {
                            File.Delete(root + "\\" + filename);
                            label19.Text = ">" + value + "\n[!] File '" + filename + "' is deleted at > " + path + ".";
                        }
                        else if (File.Exists(filename) == true)
                        {
                            File.Delete(filename);
                            label19.Text = ">" + value + "\n[!] File '" + filename + "' is deleted at given path.";
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n[!] File '" + filename + "' is not exists at > " + path + " .";
                        }

                        label19.Visible = true;
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* GOTO PATH BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value.Length > 4 && value[0] == 'g' && value[1] == 'o' && value[2] == 't' && value[3] == 'o')
                    {
                        if (value[0] == 'g' && value[1] == 'o' && value[2] == 't' && value[3] == 'o' && value[4] == ' ')
                        {
                            string pathfortry = "";

                            for (int i = 5; i < value.Length; i++)
                            {
                                pathfortry = pathfortry + value[i];
                            }

                            int pfl = pathfortry.Length;
                            pfl = pfl - 1;
                            if (Directory.Exists(pathfortry) == true)
                            {
                                if (pathfortry[pfl] == '\'')
                                {
                                    root = pathfortry;
                                    path = pathfortry;
                                }
                                else
                                {
                                    root = pathfortry + '\\';
                                    path = pathfortry + '\\';
                                    pathfortry = pathfortry + '\\';
                                }
                                pathhistory.Add(root);

                                label19.Text = ">" + value + "\n" + "[!] You are in now at " + pathfortry;
                                label19.Visible = true;
                            }
                            else
                            {
                                label19.Text = ">" + value + "\n" + "[!] Unknown path or path is not exist. ";
                                label19.Visible = true;
                            }
                        }
                        else
                        {
                            label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'goto c:\\' ?";
                        }
                    }

                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FILE READ BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value.Length > 4 && value[0] == 'r' && value[1] == 'e' && value[2] == 'a' && value[3] == 'd')
                    {
                        if (value.Length > 6 && value[0] == 'r' && value[1] == 'e' && value[2] == 'a' && value[3] == 'd' && value[4] == ' ' && value[5] == '-' && value[6] == 'x')
                        {
                            string rdfile = "";
                            for (int i = 8; i < value.Length; i++)
                            {
                                rdfile += value[i];
                            }

                            if (File.Exists(path + rdfile))
                            {
                                label19.Text = ">" + value + "\n[!] Reading " + rdfile + " file..";
                                string[] lines = System.IO.File.ReadAllLines(path + "\\" + rdfile);
                                label19.Text += "\n\n";
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    label19.Text += lines[i] + "\n";
                                }
                            }
                            else if (File.Exists(rdfile))
                            {
                                label19.Text = ">" + value + "\n[!] Reading " + rdfile + " file..";
                                string[] lines = System.IO.File.ReadAllLines(rdfile);
                                label19.Text += "\n\n";

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    label19.Text += lines[i] + "\n";
                                }
                            }
                            else
                            {
                                label19.Text = ">" + value + "\n" + "[!] Unknown file or file is not exist.";
                            }
                        }
                        else if (value.Length > 6 && value[0] == 'r' && value[1] == 'e' && value[2] == 'a' && value[3] == 'd' && value[4] == ' ' && value[5] == '-' && value[6] == 'n')
                        {
                            string rdfile = "";
                            for (int i = 8; i < value.Length; i++)
                            {
                                rdfile += value[i];
                            }

                            if (File.Exists(path + rdfile))
                            {
                                label19.Text = ">" + value + "\n[!] Reading " + rdfile + " file..";
                                string[] lines = System.IO.File.ReadAllLines(path + "\\" + rdfile);
                                label19.Text += "\n\n";
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    label19.Text += "[ " + i + " ]   " + lines[i] + "\n";
                                }
                            }
                            else if (File.Exists(rdfile))
                            {
                                label19.Text = ">" + value + "\n[!] Reading " + rdfile + " file..";
                                string[] lines = System.IO.File.ReadAllLines(rdfile);
                                label19.Text += "\n\n";

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    label19.Text += "[ " + i + " ]  " + lines[i] + "\n";
                                }
                            }
                            else
                            {
                                label19.Text = ">" + value + "\n" + "[!] Unknown file or file is not exist.";
                            }
                        }
                        else
                        {
                            label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands.\n[!] Did you mean 'read -x example.txt' ?";
                        }
                    }
                    else if (value == "history")
                    {
                        int a = 0; ;

                        if (history.Count > 0)
                        {
                            label19.Text = ">" + value + "\n[!] Listing now history. \n\n";
                            foreach (string x in history)
                            {
                                a = a + 1;

                                label19.Text += "[ " + a + " ] " + x + "\n";
                            }
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n[!] Cant list history. History is empty. \n\n";
                        }
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FILE LIST BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value == "list -x")
                    {
                        label19.Font = new Font("Consolas", 10, FontStyle.Regular);
                        DirectoryInfo d = new DirectoryInfo(root);

                        if (Directory.Exists(path) != true)
                        {
                            label19.Text = ">" + value + "\n[!] Your current location doesn't exist. Files can't be listed. Unknown path : " + path + " ...\n\n";
                        }
                        else
                        {



                            FileInfo[] Files = d.GetFiles(); //Getting All Folders
                            string[] filednm = Directory.GetDirectories(path);

                            label19.Text = ">" + value + "\n[!] Listing now all files from in " + path + " ...\n\n";

                            int qq = 0;

                            foreach (string file in filednm)
                            {
                                qq = qq + 1;
                                int qs = path.Length;
                                string qsq = "";

                                for (int i = (path.Length); i < file.Length; i++)
                                {
                                    qsq += file[i].ToString();
                                }
                                label19.Text += qsq + " , ";

                                if (qq == 3)
                                {
                                    label19.Text = label19.Text + "\n";
                                    qq = 0;
                                }
                            }

                            foreach (FileInfo x in Files)
                            {
                                qq = qq + 1;

                                label19.Text += x + " , ";

                                if (qq == 3)
                                {
                                    label19.Text = label19.Text + "\n";
                                    qq = 0;
                                }
                            }
                        }
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FILE EDITOR BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value.Length > 7 && value[0] == 'e' && value[1] == 'd' && value[2] == 'i' && value[3] == 't' && value[4] == 'o' && value[5] == 'r' && value[6] == ' ')
                    {
                        fpath = "";

                        for (int i = 7; i <= value.Length - 1; i++)
                        {
                            fpath += value[i];
                        }

                        int q = 0;
                        for (int j = value.Length - 1; j > 7; j--)
                        {
                            // kapat öyle girek belki olmaz
                            if (value[j] == '\\')
                            {
                                q = j;
                            }
                        }

                        string qfile = "";
                        for (int f = q; f < value.Length; f++)
                        {
                            qfile += value[f];
                        }

                        if (File.Exists(fpath) == true)
                        {
                            label19.Text = ">" + value + "\n[!] Editor is now activating for > " + fpath + "  ...";
                            panel4active();
                            label20.Font = new Font("Consolas", 10, FontStyle.Regular);
                            string[] lines = System.IO.File.ReadAllLines(fpath);

                            foreach (string x in lines)
                            {
                                richTextBox1.Text += x + "\n";
                            }
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n" + "[!] Unknown file or file is not exist.";
                        }
                    }
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    /* FILE EDIT BY JAEG */
                    /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
                    else if (value.Length > 7 && value[0] == 'e' && value[1] == 'd' && value[2] == 'i' && value[3] == 't' && value[4] == ' ' && value[5] == '-' && value[6] == 'n')
                    {
                        //edit c:\users\hp\desktop\test.txt -3

                        string file = "";
                        indexs = "";
                        int q = 0;
                        for (int i = value.Length - 1; i > 7; i--)
                        {
                            if (value[i] == '-')
                            {
                                q = i + 1;
                                break;
                            }
                        }

                        int w = value.Length - q;

                        for (int j = q; j < value.Length;)
                        {
                            if (value[j] == '0' || value[j] == '1' || value[j] == '2' || value[j] == '3' || value[j] == '4' || value[j] == '5' || value[j] == '6' || value[j] == '7' || value[j] == '8' || value[j] == '9')
                            {
                                indexs += value[j];
                                j++;
                            }
                            else
                            {
                            }
                        }
                        index = Int32.Parse(indexs);

                        for (int a = 7; a < q - 1; a++)
                        {
                            file += value[a];
                        }

                        efile = file;

                        if (File.Exists(efile) == true)
                        {
                            string[] lines = System.IO.File.ReadAllLines(efile);

                            if (lines.Length > Int32.Parse(indexs))
                            {
                                label19.Text = ">" + value + "\n\n" + "[!] The line is :\n" + "[ " + index + " ] " + lines[index] + "\n\n[!] Enter text of your line to replacing.";

                                status = 1;
                            }
                            else
                            {
                                label19.Text = ">" + value + "\n" + "[!] File is not edited. Stated line number is not found.";
                            }
                        }
                        else
                        {
                            label19.Text = ">" + value + "\n" + "[!] Unknown file or file is not exist.";
                        }
                    }
                    //2 editline()
                    else
                    {
                        label19.Text = ">" + value + "\nUnknown command. Please type 'help -x' without quotes for help and commands. ";
                        label19.Visible = true;
                    }
                }
            }

            /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
            /* TEXTBOX HISTORY BY JAEG */
            /*||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||*/
            else if (e.KeyCode == Keys.Up)
            {
                if (hsar == 0)
                {
                }
                else
                {
                    hsar = hsar - 1;
                    textBox1.Text = history[hsar];
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (hsar < (history.Count) - 1)
                {
                    hsar = hsar + 1;
                    textBox1.Text = history[hsar];
                }
                else
                {
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (qwe == 0 || panel3.Location.Y == 0)
                {
                }
                else
                {
                    qwe = qwe + 50;
                    panel3.Location = new Point(panel3.Location.X, qwe);
                }
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                qwe = qwe - 50;
                panel3.Location = new Point(panel3.Location.X, qwe);
            }
            else
            {
                qwe = 0;
                panel3.Location = new Point(panel3.Location.X, 0);
            }
        }

        private void editline()
        {
            string[] lines = System.IO.File.ReadAllLines(efile);
            lines[index] = value;
            File.WriteAllLines(efile, lines);

            label19.Text = ">" + value + "\n[!] The " + efile + " is successfully edited. \n";
            status = 0;
        }

        private int tmr = 0;

        private void timer6_Tick(object sender, EventArgs e) //SHUTDOWNS
        {
            if (tmr == 2)
            {
                shutdown();
            }
            else
            {
                tmr = tmr + 1;
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                label20.Font = new Font("Consolas", 10, FontStyle.Regular);
                e.SuppressKeyPress = true;
                label20.Text = "[!] JAEGOS Text Editor > Text editor shutting down..";
                System.Threading.Thread.Sleep(1000);
                panel4.Enabled = false;
                panel4.Visible = false;
                timer7.Enabled = false;
                timer5.Enabled = true;
                panel3.Visible = true;
                panel3.Enabled = true;
                richTextBox1.Text = "";
            }
            else if (e.KeyCode == Keys.F1)
            {
                label20.Font = new Font("Consolas", 10, FontStyle.Regular);
                e.SuppressKeyPress = true;

                List<string> richs = new List<string>();

                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    richs.Add(richTextBox1.Lines[i]);
                }
                File.WriteAllLines(fpath, richs);

                label20.Text = "[!] JAEG OS Text Editor > [!] File is successfully edited.  > \t" + fpath;
            }
        }

        private void panel4active()
        {
            panel1.Visible = false;
            panel3.Visible = false;
            panel2.Visible = false;
            panel1.Enabled = false;
            panel2.Enabled = false;
            panel3.Enabled = false;
            panel4.Enabled = true;
            panel4.Visible = true;
            richTextBox1.Location = new Point(20, (panel4.Height / 100) * 9);
            timer5.Enabled = false;
            timer7.Enabled = true;
            label20.Text = "[!] JAEG OS Text Editor > \t" + fpath;
        }
    }
}