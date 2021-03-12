using System;
using System.Configuration;
using static System.Configuration.ConfigurationManager;

namespace PMPlatform.JT808TestTool
{
    public class LocalConfigs
    {
        public static bool EquipmentPaging
        {
            get => (bool) "T"?.Equals(AppSettings["EquipmentPaging"]);
            //set => AppSettings["EquipmentPaging"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["EquipmentPaging"].Value = value ? "T" : "F";
                coc.Save();
            }
        }
        public static bool DataDisplayShowLocationResponse
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplay1"]);
            // set => AppSettings["DataDisplay1"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplay1"].Value = value ? "T" : "F";
                coc.Save();
            }
        }
        public static bool DataDisplayShowTerminalGeneralResponse
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplay2"]);
            // set => AppSettings["DataDisplay2"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplay2"].Value = value ? "T" : "F";
                coc.Save();
            }
        }
        public static bool DataDisplayShowTerminalParaSelectResponse
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplay3"]);
            // set => AppSettings["DataDisplay3"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplay3"].Value = value ? "T" : "F";
                coc.Save();
            }
        }
        public static bool DataDisplayShowTerminalLocationSelectResponse
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplay4"]);
            // set => AppSettings["DataDisplay4"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplay4"].Value = value ? "T" : "F";
                coc.Save();
            }
        }
        public static bool DataDisplayShowTerminalOthersResponse
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplay5"]);
            // set => AppSettings["DataDisplay5"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplay5"].Value = value ? "T" : "F";
                coc.Save();
            }
        }

        public static int EquipmentPageSize
        {
            get => Convert.ToInt32(AppSettings["EquipmentPageSize"]);
            // set => AppSettings["EquipmentPageSize"] = value.ToString();
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["EquipmentPageSize"].Value = value.ToString();
                coc.Save();
            }
        }

        public static bool DataDisplayIsSerialization
        {
            get => (bool)"T"?.Equals(AppSettings["DataDisplayIsSerialization"]);
            // set => AppSettings["DataDisplayIsSerialization"] = value ? "T" : "F";
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplayIsSerialization"].Value = value ? "T" : "F";
                coc.Save();
            }
        }

        public static string ParaSet0X0001
        {
            get => AppSettings["ParaSet0X0001"];
            // set => AppSettings["ParaSet0X0001"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0001"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0010
        {
            get => AppSettings["ParaSet0X0010"];
            // set => AppSettings["ParaSet0X0010"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0010"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0013
        {
            get => AppSettings["ParaSet0X0013"];
            // set => AppSettings["ParaSet0X0013"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0013"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0017
        {
            get => AppSettings["ParaSet0X0017"];
            // set => AppSettings["ParaSet0X0017"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0017"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0018
        {
            get => AppSettings["ParaSet0X0018"];
            // set => AppSettings["ParaSet0X0018"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0018"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0020
        {
            get => AppSettings["ParaSet0X0020"];
            // set => AppSettings["ParaSet0X0020"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0020"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0027
        {
            get => AppSettings["ParaSet0X0027"];
            // set => AppSettings["ParaSet0X0027"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0027"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0029
        {
            get => AppSettings["ParaSet0X0029"];
            // set => AppSettings["ParaSet0X0029"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0029"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0030
        {
            get => AppSettings["ParaSet0X0030"];
            // set => AppSettings["ParaSet0X0030"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0030"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0055
        {
            get => AppSettings["ParaSet0X0055"];
            // set => AppSettings["ParaSet0X0055"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0055"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0056
        {
            get => AppSettings["ParaSet0X0056"];
            // set => AppSettings["ParaSet0X0056"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0056"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0080
        {
            get => AppSettings["ParaSet0X0080"];
            // set => AppSettings["ParaSet0X0080"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0080"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0081
        {
            get => AppSettings["ParaSet0X0081"];
            // set => AppSettings["ParaSet0X0081"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0081"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0082
        {
            get => AppSettings["ParaSet0X0082"];
            // set => AppSettings["ParaSet0X0082"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0082"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0083
        {
            get => AppSettings["ParaSet0X0083"];
            // set => AppSettings["ParaSet0X0083"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0083"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X0084
        {
            get => AppSettings["ParaSet0X0084"];
            // set => AppSettings["ParaSet0X0084"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X0084"].Value = value;
                coc.Save();
            }
        }
        public static string ParaSet0X1018
        {
            get => AppSettings["ParaSet0X1018"];
            // set => AppSettings["ParaSet0X1018"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["ParaSet0X1018"].Value = value;
                coc.Save();
            }
        }
        public static string KafkaServer
        {
            get => AppSettings["KafkaServer"];
            // set => AppSettings["KafkaServer"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["KafkaServer"].Value = value;
                coc.Save();
            }
        }

        public static string DataDisplayMaxMessageCount
        {
            get => AppSettings["DataDisplayMaxMessageCount"];
            // set => AppSettings["DataDisplayMaxMessageCount"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplayMaxMessageCount"].Value = value;
                coc.Save();
            }
        }

        public static string DataDisplayShowMessageCount
        {
            get => AppSettings["DataDisplayShowMessageCount"];
            // set => AppSettings["DataDisplayShowMessageCount"] = value;
            set
            {
                var coc = OpenExeConfiguration(ConfigurationUserLevel.None);
                coc.AppSettings.Settings["DataDisplayShowMessageCount"].Value = value;
                coc.Save();
            }
        }
    }
}