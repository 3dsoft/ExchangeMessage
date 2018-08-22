using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Receiver
{
    public partial class Form1 : Form
    {
        public const int WM_COPYDATA = 0x4A;

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, ref COPYDATASTRUCT lParam);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMessage(textBox1.Text.Trim());
        }

    public void sendMessage(string msg)
    {
        int hWnd = FindWindow(null, "Sender");
        if (hWnd != 0)
        {
            byte[] bytes = Encoding.Default.GetBytes(msg);
            int num = bytes.Length;

            COPYDATASTRUCT copydatastruct;
            copydatastruct.dwData = (IntPtr)0;
            copydatastruct.lpData = msg;
            copydatastruct.cbData = num + 1;

            SendMessage(hWnd, WM_COPYDATA, 0, ref copydatastruct);
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_COPYDATA)
        {
            COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

            listBox1.Items.Add(cds.lpData);
        }
        else base.WndProc(ref m);
    }
    }
}
