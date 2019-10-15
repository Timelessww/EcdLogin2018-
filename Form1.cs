using Autodesk.AutoCAD.Interop;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcdLogin
{
    public partial class Form1 : Form
    {
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        const int LOGON32_LOGON_INTERACTIVE = 2; //通过网络验证账户合法性
        const int LOGON32_PROVIDER_DEFAULT = 0; //使用默认的Windows 2000/NT NTLM验证方

        //AcadApplication app = (AcadApplication)Marshal.GetActiveObject("AutoCAD.Application"); //这里最好用try–catch
        public Form1()
        {
            InitializeComponent();
        }


        private string HttpGet(string api)
        {
            string serviceAddress = api;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceAddress);
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            //返回json字符串
            return myStreamReader.ReadToEnd();
        }
        private void btn_Lgn_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text.Trim();
            string PassWord = txtPwd.Text.Trim();


            if (UserName == "")
            {
                MessageBox.Show("请输入用户名！");
                return;
            }

            if (PassWord == "")
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            if (ChkPassword(UserName, PassWord) == true)
            {

                //using (var httpClient = new HttpClient())
                //{
                //    //get
                //    var url = new Uri("http://127.0.0.1:8777//");
                //    // response
                //    var response = httpClient.GetAsync(url).Result;
                //    MessageBox.Show(response.ToString());
                //    var data = response.Content.ReadAsStringAsync().Result;

                //  Task <byte[]>  bt1=  response.Content.ReadAsByteArrayAsync();

                //    var targetAssembly = Assembly.Load(bt1);
                //    MessageBox.Show(bt1.ToString());
                //    MessageBox.Show(data);

                //}

                //10.96.254.24
                string response = HttpGet("http://10.96.254.24:8777//");//获取服务器上json地址

 

                JObject json = (JObject)JsonConvert.DeserializeObject(response);
                JArray jArray = (JArray)json["data"];

                IList<object> allDrives = json["data"].Select(t => (object)t).ToList();






                
                for (int i = 0; i < jArray.Count(); i++)
                {
                    

                    JArray array = (JArray)jArray[i]["data"];
                    byte[] values = new byte[jArray[i]["data"].Count()*4];
                    for (int j = 0; j < array.Count(); j++)
                    {

                        values[j] = (byte)array[j];
                    }
                    var targetAssembly = Assembly.Load(values);

                    values =null;

                   
                  
                }

        

                MessageBox.Show("加载成功！");
                this.Hide();
            }
            else
            {
                MessageBox.Show("用户登录失败");
            }
        }

        private bool ChkPassword(string account, string password)//验证LADP是否有该用户
        {
            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;
            string domainName = Environment.UserDomainName;
            if (LogonUser(account, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle))
                return true;
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int height = SystemInformation.WorkingArea.Height;
            int width = SystemInformation.WorkingArea.Width;
            int formheight = this.Size.Height;
            int formwidth = this.Size.Width;
            int newformx = width / 2 - formwidth / 2;
            int newformy = height / 2 - formheight / 2;
            this.SetDesktopLocation(newformx, newformy);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //isValidFileContent();
        }

        public static bool isValidFileContent(string filePath1, string filePath2)
        {
            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file1 = new FileStream(filePath1, FileMode.Open), file2 = new FileStream(filePath2, FileMode.Open))
                {
                    byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                    byte[] hashByte2 = hash.ComputeHash(file2);
                    string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                    string str2 = BitConverter.ToString(hashByte2);
                    return (str1 == str2);//比较哈希码 
                }
            }
        }
    }
}
