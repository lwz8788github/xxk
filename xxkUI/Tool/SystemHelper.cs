using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;
using System.Windows.Forms;

namespace xxkUI.Tool
{
    public class SystemHelper
    {
        /// <summary>
        /// 可获得NT系列的系统版本，如:xp,2000,2003，2008,vista,win7。
        /// </summary>
        /// <returns></returns>
        public static string GetWinNTSystemVersion()
        {
            return RegeditHelper.GetRegistryValue(RegistryHive.LocalMachine, @"Software\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
        }

        public static bool Is32System()
        {
            if (GetSystem64or32() == "32")
            {
                return true;
            }

            return false;
        }

        public static bool Is64System()
        {
            if (GetSystem64or32() == "64")
            {
                return true;
            }

            return false;
        }

        public static string GetSystem64or32()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }

                //MessageBox.Show(addressWidth);

                return addressWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return String.Empty;
            }
        }
    }
}