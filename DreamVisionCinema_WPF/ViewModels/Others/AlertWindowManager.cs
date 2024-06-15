using System.Windows;

namespace DreamVisionCinema_WPF.ViewModels.Others
{
    public static class AlertWindowManager
    {
        private static Dictionary<string, Window> openAlerts = new Dictionary<string, Window>();
      
        public static void AddAlert(string name, Window alert)
        {
            if (!openAlerts.ContainsKey(name))
            {
                openAlerts[name] = alert;
            }
        }

        public static Window? GetAlert(string name)
        {
            openAlerts.TryGetValue(name, out Window? alert);
            return alert;
        }

        public static void RemoveAlert(string name)
        {
            if (openAlerts.ContainsKey(name))
            {
                openAlerts.Remove(name);
            }
        }
        public static Dictionary<string, Window> OpenAlerts
        {
            get
            {
                return openAlerts;
            }
        }
    }

}
