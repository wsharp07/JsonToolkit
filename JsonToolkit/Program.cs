using JsonToolkit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonToolkit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }

    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;

        public MyCustomApplicationContext()
        {
            
            // Initialize Tray Icon
            _trayIcon = new NotifyIcon()
            {
                Icon = Resources.json_icon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };

            _trayIcon.Text = "This is the tooltip";
            _trayIcon.ContextMenu = new ContextMenu();

            var menuParseJson = new MenuItem("Parse Json Clipboard", new EventHandler(OnParseJson_Click));
            var menuExit = new MenuItem("Exit", new EventHandler(Exit));

            _trayIcon.ContextMenu.MenuItems.Add(menuParseJson);
            _trayIcon.ContextMenu.MenuItems.Add(menuExit);
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;

            Application.Exit();
        }

        private void OnParseJson_Click(object sender, EventArgs e)
        {
            var mainForm = new MainForm();
            mainForm.LoadJsonClipboard();
        }

        
    }
}
