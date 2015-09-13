/*
 *  EQEmuItemEditor - Application for customizing EQEmu server items
 *  
 *  By Shendare (Jon D. Jackson)
 * 
 *  Portions of this code not covered by another author's or entity's copyright are released under
 *  the Creative Commons Zero (CC0) public domain license.
 *  
 *  To the extent possible under law, Shendare (Jon D. Jackson) has waived all copyright and
 *  related or neighboring rights to this EQEmuItemEditor application.
 *  This work is published from: The United States. 
 *  
 *  You may copy, modify, and distribute the work, even for commercial purposes, without asking permission.
 * 
 *  For more information, read the CC0 summary and full legal text here:
 *  
 *  https://creativecommons.org/publicdomain/zero/1.0/
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EQEmuItemEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Settings.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formMain());
        }
    }

    class Settings
    {
        public static string IconFolder = @"%UserProfile%\Pictures\EQIcons";
        public static string SearchName = "";
		public static bool   AutoClasses = true;
		public static bool   Auto3DModel = true;
        
        public class Database
        {
            public static string Conn = "DRIVER={MySQL ODBC 5.3 ANSI Driver};Database={Database};Server={Server};Port={Port};UID={Username};PWD={Password};";
            public static string Host = "127.0.0.1";
            public static string Name = "peqdb";
            public static string User = "root";
            public static string Pass = "";
            public static string Port = "3306";
        }

        public static void Load()
        {
            IconFolder = Properties.Settings.Default.IconFolder;
            SearchName = Properties.Settings.Default.SearchName;
			AutoClasses = Properties.Settings.Default.AutoClasses;
			Auto3DModel = Properties.Settings.Default.Auto3DModel;

            Database.Conn = Properties.Settings.Default.DBConn;
            Database.Host = Properties.Settings.Default.DBHost;
            Database.Name = Properties.Settings.Default.DBName;
            Database.User = Properties.Settings.Default.DBUser;
            Database.Pass = Properties.Settings.Default.DBPass;
            Database.Port = Properties.Settings.Default.DBPort;
        }

        public static void Save()
        {
            Properties.Settings.Default.IconFolder = IconFolder;
            Properties.Settings.Default.SearchName = SearchName;
			Properties.Settings.Default.AutoClasses = AutoClasses;
			Properties.Settings.Default.Auto3DModel = Auto3DModel;

            Properties.Settings.Default.DBConn = Database.Conn;
            Properties.Settings.Default.DBHost = Database.Host;
            Properties.Settings.Default.DBName = Database.Name;
            Properties.Settings.Default.DBUser = Database.User;
            Properties.Settings.Default.DBPass = Database.Pass;
            Properties.Settings.Default.DBPort = Database.Port;

            Properties.Settings.Default.Save();
        }
    }
}
