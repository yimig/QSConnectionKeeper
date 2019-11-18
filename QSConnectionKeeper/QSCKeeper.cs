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
using System.Timers;

namespace QSConnectionKeeper
{
    partial class QSCKeeper : ServiceBase
    {
        public QSCKeeper()
        {
            this.ServiceName = "qsckeeper";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(TimedEvent);
            aTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalHour"]) * 3600 +
                              Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMin"]) * 60 +
                              Convert.ToInt32(ConfigurationManager.AppSettings["IntervalSec"]);
            aTimer.Enabled = true;
            InitializeComponent();
        }

        private void TimedEvent(object source, ElapsedEventArgs e)         //运行期间执行  
        {
            if (!TestConnect()) ConnectionStart();
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
            string url = ConfigurationManager.AppSettings["PingTarget"];
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
