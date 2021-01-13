using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Text = "Add Password Bank";
            textBox1.Text = "Username";
            textBox2.Text = "Password";
            textBox3.Text = "Url/Place";
            try
            {
                string encryptdata = File.ReadAllText("output.Ecrypt");
                richTextBox1.AppendText(Decrypt(encryptdata));
            }
            catch
            {

            }
        }
        public static string hash = "";
        private static byte[] key = Encoding.ASCII.GetBytes(hash.ToLower() + "gs1dfgd2g1ertqwe");
        private static byte[] Vector = Encoding.ASCII.GetBytes("gs1dfgd2g1ertqwe");
        private void button1_Click(object sender, EventArgs e)
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }
            hash = id;
            richTextBox1.AppendText("Url/Place: " + textBox3.Text + "\n" + "Username: " + textBox1.Text + "\n" + "Password: " + textBox2.Text);
            File.AppendAllText("output.Ecrypt", Crypt("Url/Place: " + textBox3.Text + "Username: " + textBox1.Text + "\n" + "Password: " + textBox2.Text));
        }
        public static string Crypt(string textToCrypt)
        {
           using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = Vector, Mode = CipherMode.CBC })
           using (var memoryStream = new MemoryStream())
           using (var cryptoStream = new CryptoStream(memoryStream,rijndaelManaged.CreateEncryptor(key, Vector),CryptoStreamMode.Write))
           {
            using (var ws = new StreamWriter(cryptoStream))
            {
              ws.Write(textToCrypt);
            }
            return Convert.ToBase64String(memoryStream.ToArray());
           }
        }
        public static string Decrypt(string cipherData)
        {
                using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = Vector, Mode = CipherMode.CBC })
                using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherData)))
                using (var cryptoStream = new CryptoStream(memoryStream,rijndaelManaged.CreateDecryptor(key, Vector),CryptoStreamMode.Read))
                {
                    return new StreamReader(cryptoStream).ReadToEnd();
                }
        }
    }
}
