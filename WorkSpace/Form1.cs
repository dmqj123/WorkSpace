using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Ionic.Zip;
using System.IO;

namespace WorkSpace
{
    public partial class Form1 : Form
    {
        public bool IsInSpace = false;
        public String SpacePath;
        public String SpacePassword;

        public Form1()
        {
            InitializeComponent();
            label1.BackColor = Color.Transparent;
        }

        private void on_window_resize(object sender, EventArgs e)
        {
            //居中控件
            label1.Location = new Point((this.Width - label1.Width) / 2, (this.Height - label1.Height) / 2 - 180);
            textBox1.Location = new Point((this.Width - label1.Width) / 2 - 80, (this.Height - label1.Height) / 2);
            button1.Location = new Point((this.Width - label1.Width) / 2 + 563, (this.Height - label1.Height) / 2);
            button2.Location = new Point((this.Width - label1.Width) / 2 + 125, (this.Height - label1.Height) / 2 + 46);
        }

        private void on_choosefilebutton_click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //成功打开
                string path = openFileDialog1.FileName;
                textBox1.Text = path;
            }
        }

        private void on_go_button_click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("文件不存在!");
                return;
            }
            string password = "";
            using (Form prompt = new Form()
            {
                MaximizeBox = false,
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Enter Password",
                StartPosition = FormStartPosition.CenterScreen
            })
            {
                TextBox textBox = new TextBox() { Left = 50, Top = 20, Width = 200, PasswordChar = '*' };
                Button confirmation = new Button() { Text = "OK", Left = 100, Width = 100, Top = 50, DialogResult = DialogResult.OK };
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textBox);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    password = textBox.Text;
                }
            }
            Gointo_Space(textBox1.Text, password);
        }
        private void Save_Space(String file_path,String save_path,String password)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = password;
                zip.AddFile(file_path+"/.", "");
                zip.Save(save_path);
            }
        }
        private void New_Space()
        {

        }
        private void Gointo_Space(String path,String password)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                if (password != "")
                {
                    zip.Password = password;
                }
                foreach (ZipEntry entry in zip)
                {
                    string tempPath = Environment.GetEnvironmentVariable("TEMP")+"\\workspaces\\"+GetTime();
                    entry.Extract(tempPath, ExtractExistingFileAction.OverwriteSilently);
                    System.Diagnostics.Process.Start(tempPath);
                }
            }
        }
        private String GetTime()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
