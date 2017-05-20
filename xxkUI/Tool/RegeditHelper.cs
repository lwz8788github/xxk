using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace xxkUI.Tool
{
    /// <summary>
    /// 32位程序读取64位系统的注册表
    /// </summary>
    public class RegeditHelper
    {
        /// <summary>
        /// //获取32位或64系统注册表的值。
        /// </summary>
        /// <param name="rh">根级别的名称，如Registry.LocalMachine</param>
        /// <param name="keyName">不包括根级别的名称，如：@"SOFTWARE\Microsoft\Office\12.0\Word\InstallRoot\"</param>
        /// <param name="valueName">不包括根级别的名称，如：Path</param>
        /// <returns>若没有找到，返回null</returns>
        public static string GetRegistryValue(RegistryHive rh, string keyName, string valueName)
        {
            try
            {
                object value = null;

                if (SystemHelper.Is32System())
                {
                    value = RegeditHelper.GetValueWithRegView(rh, keyName, valueName, RegistryView.Registry32);
                }
                else if (SystemHelper.Is64System())
                {
                    value = RegeditHelper.GetValueWithRegView(rh, keyName, valueName, RegistryView.Registry64);
                }

                if (value != null)
                {
                    return value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取注册表失败，" + ex.Message);
            }

            return null;
        }


        //获得根节点的句柄，常数是固定的。
        public static IntPtr GetHiveHandle(RegistryHive hive)
        {
            IntPtr preexistingHandle = IntPtr.Zero;

            IntPtr HKEY_CLASSES_ROOT = new IntPtr(-2147483648);
            IntPtr HKEY_CURRENT_USER = new IntPtr(-2147483647);
            IntPtr HKEY_LOCAL_MACHINE = new IntPtr(-2147483646);
            IntPtr HKEY_USERS = new IntPtr(-2147483645);
            IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(-2147483644);
            IntPtr HKEY_CURRENT_CONFIG = new IntPtr(-2147483643);
            IntPtr HKEY_DYN_DATA = new IntPtr(-2147483642);
            switch (hive)
            {
                case RegistryHive.ClassesRoot: preexistingHandle = HKEY_CLASSES_ROOT; break;
                case RegistryHive.CurrentUser: preexistingHandle = HKEY_CURRENT_USER; break;
                case RegistryHive.LocalMachine: preexistingHandle = HKEY_LOCAL_MACHINE; break;
                case RegistryHive.Users: preexistingHandle = HKEY_USERS; break;
                case RegistryHive.PerformanceData: preexistingHandle = HKEY_PERFORMANCE_DATA; break;
                case RegistryHive.CurrentConfig: preexistingHandle = HKEY_CURRENT_CONFIG; break;
                case RegistryHive.DynData: preexistingHandle = HKEY_DYN_DATA; break;
            }
            return preexistingHandle;
        }

        /// <summary>
        /// 用于32位程序访问64位注册表
        /// </summary>
        /// <param name="hive">根级别的名称</param>
        /// <param name="keyName">不包括根级别的名称</param>
        /// <param name="valueName">项名称</param>
        /// <param name="view">注册表视图</param>
        /// <returns>值</returns>
        public static object GetValueWithRegView(RegistryHive hive, string keyName, string valueName, RegistryView view)
        {
            try
            {
                SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);//获得根节点的安全句柄

                RegistryKey subkey = RegistryKey.FromHandle(handle, view).OpenSubKey(keyName);//获得要访问的键

                if (subkey == null)
                {
                    return null;
                }

                RegistryKey key = RegistryKey.FromHandle(subkey.Handle, view);//根据键的句柄和视图获得要访问的键

                if (key == null)
                {
                    return null;
                }

                return key.GetValue(valueName);//获得键下指定项的值
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 用于32位的程序设置64位的注册表
        /// </summary>
        /// <param name="hive">根级别的名称</param>
        /// <param name="keyName">不包括根级别的名称</param>
        /// <param name="valueName">项名称</param>
        /// <param name="value">值</param>
        /// <param name="kind">值类型</param>
        /// <param name="view">注册表视图</param>
        public static void SetValueWithRegView(RegistryHive hive, string keyName, string valueName, object value, RegistryValueKind kind, RegistryView view)
        {
            try
            {
                SafeRegistryHandle handle = new SafeRegistryHandle(GetHiveHandle(hive), true);

                RegistryKey subkey = RegistryKey.FromHandle(handle, view).OpenSubKey(keyName, true);//需要写的权限,这里的true是关键。0227更新

                RegistryKey key = RegistryKey.FromHandle(subkey.Handle, view);

                key.SetValue(valueName, value, kind);
            }
            catch (Exception ex)
            {
                throw new Exception("写注册表失败," + ex.Message);
            }
        }


    }
}
