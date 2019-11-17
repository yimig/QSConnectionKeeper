using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QSConnectionKeeper
{
    partial class QSCKeeper : ServiceBase
    {
        public QSCKeeper()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            ThreadStart start=new ThreadStart(() =>
            {
                while (true)
                {
                    try
                    {
                        if(!TestConnect())ConnectionStart();
                        Thread.Sleep(new TimeSpan(0,2,0));
                    }
                    catch { }
                }
            });
            Thread th=new Thread(start);
            th.IsBackground = true;
            th.Start();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }

        private void ConnectionStart()
        {
            Process p = new Process();
            p.StartInfo.FileName = ConfigurationManager.AppSettings["QSNetBootLocation"];
            p.StartInfo.Arguments = "-c";
            p.Start();
        }

        private bool TestConnect()
        {
            string url = "www.baidu.com";
            bool isConnect =false;
            Ping ping=new Ping();
            try
            {
                PingReply reply = ping.Send(url);
                isConnect=reply.Status == IPStatus.Success ? true : false;
            }
            catch (Exception e)
            {
                isConnect = false;
            }

            return isConnect;
        }
    }
}
