using System;
using System.Windows.Forms;

namespace deceed.DsLight.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Left = (int)Math.Round(Screen.GetWorkingArea(this).Width * 0.1);
            Width = (int)Math.Round(Screen.GetWorkingArea(this).Width * 0.8);
            Top = (int)Math.Round(Screen.GetWorkingArea(this).Height * 0.1);
            Height = (int)Math.Round(Screen.GetWorkingArea(this).Height * 0.8);

            DsLightEditor.SetConnectionString("Data Source=WS2\\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True");
            DsLightEditor.LoadData("e:\\data.dslt");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DsLightEditor.SaveData("e:\\data.dslt");
        }
    }
}
