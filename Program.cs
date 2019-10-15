using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
namespace EcdLogin
{
    public class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        [CommandMethod("EcdLogin")]
        public void ShowForm()
        {
            Form1 lgnForm = new Form1();
            Application.ShowModelessDialog(lgnForm);//显示无模式对话框


        }
    }
}
