using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensekeyGenerator
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string client = txtClient.Text.Trim();
                string machine = txtMachine.Text.Trim();
                string expiry = dtExpiry.Value.ToString("yyyy-MM-dd");

                string data = $"CLIENT={client}|MACHINE={machine}|EXPIRES={expiry}";
                string privateKey = File.ReadAllText("privatekey.xml");
                string signature = SignData(data, privateKey);

                string licenseContent = $"{data}|SIGN={signature}";
                string fileName = $"{client}_license.dat";
                File.WriteAllText(fileName, licenseContent);

                lblStatus.Text = $"✅ License generated: {fileName}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }
        private static string SignData(string data, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                byte[] sig = rsa.SignData(bytes, new SHA256CryptoServiceProvider());
                return Convert.ToBase64String(sig);
            }
        }
    }
}

