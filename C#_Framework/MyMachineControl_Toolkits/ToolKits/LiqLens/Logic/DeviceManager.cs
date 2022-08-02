using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;


namespace LensDriverController.Logic
{
    public static class DeviceManager
    {
        public static List<string> DetectComDevice(string friendlyName)
        {
            List<string> USBComportList = new List<string>();

            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            using (managementObjectSearcher)
            {
                string caption = null;
                foreach (ManagementObject obj in managementObjectSearcher.Get())
                {
                    if (obj != null)
                    {
                        object captionObj = obj["Caption"];
                        if (captionObj != null)
                        {
                            caption = captionObj.ToString();
                            if (caption.Contains("(COM"))
                            {
                                if (caption.Contains(friendlyName))
                                {
                                    USBComportList.Add(caption.Substring(caption.LastIndexOf("(COM")).Replace("(", string.Empty).Replace(")", string.Empty));
                                }  
                            }
                        }
                    }
                }
            } 

            return USBComportList;
        }


        public static bool DetectDevice(string friendlyName)
        {
            List<string> USBComportList = new List<string>();

            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity");
            ManagementObjectSearcher managmentObjectSearcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            using (managmentObjectSearcher)
            {
                string caption = null;
                foreach (ManagementObject obj in managmentObjectSearcher.Get())
                {
                    if (obj != null)
                    {
                        object captionObj = obj["Caption"];
                        if (captionObj != null)
                        {
                            caption = captionObj.ToString();
                            if (caption.Contains(friendlyName))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static void Show()
        {
            Process.Start("devmgmt.msc");
        }
    }
}
