using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropToClass
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Stick(tbClassname, tbNamespace, tbInput);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (tbInput.Text.NotEmpty() && tbClassname.Text.NotEmpty())
            {
                tbOutput.Text = ClassGenerator.Generate(tbInput.Text, tbNamespace.Text, tbClassname.Text);

                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\output");

                File.WriteAllText($@"{Directory.GetCurrentDirectory()}\output\{tbClassname.Text}.cs", tbOutput.Text);

                Process.Start(@"explorer.exe", $@"/select,""{Directory.GetCurrentDirectory()}\output\{tbClassname.Text}.cs""");
            }
        }
    }
}
