// See https://aka.ms/new-console-template for more information

using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
//using System.Collections.Generic;


namespace monitorSerialPort
{
    static  class Program
    {
        static private List<string> _ports=new List<string>();
        static NotifyIcon notifyIcon;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            SerialPort _serialPort;
            using (_serialPort = new SerialPort())
            {
                GetPortName(true);
                CheckLoop();
            }
        }
        static bool AddIfNotExist(string s )
        {
            foreach (string p in _ports)
            {
                if (p == s) { return false; }
            }
            _ports.Add(s);
            return true;
        }
        static bool RemoveIfNotExist(string s)
        {
            foreach (string p in SerialPort.GetPortNames())
            {
                if (p == s) { return false; };
            }
            _ports.Remove(s);
            return true;
        }
        static void GetPortName(bool first)
        {

            foreach (string s in SerialPort.GetPortNames())
            {
                if ( AddIfNotExist(s))
                { 
                    if (!first)
                    {
                        ShowToastMessage(s,true);
                    }
                }
            }
            foreach (string existingPort in _ports)
            {
                if (RemoveIfNotExist(existingPort))
                {
                    if (!first)
                    {
                        ShowToastMessage(existingPort,false);
                    }
                    break;
                }
            }

        }
        static void ShowToastMessage(string usbPort, bool addflag=true)
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Information; // 通知に表示するアイコン
            notifyIcon.Visible = true;


            if (addflag)
            {
                notifyIcon.BalloonTipTitle = " COMP Port の追加";
                notifyIcon.BalloonTipText = $"{usbPort} が追加されました。";
                notifyIcon.ShowBalloonTip(6000); // 3秒表示
            } else
            {
                notifyIcon.BalloonTipTitle = " COMP Port の削除";
                notifyIcon.BalloonTipText = $"{usbPort} が削除されました。";
                notifyIcon.ShowBalloonTip(6000); // 3秒表示


            }
            System.Threading.Thread.Sleep(4000);
            notifyIcon.Dispose();


            // 表示
        }


        /*async*/ static void CheckLoop()
        {
            while (true)
            {
                GetPortName(false);
                System.Threading.Thread.Sleep(500);
            }
        }




    }
}

