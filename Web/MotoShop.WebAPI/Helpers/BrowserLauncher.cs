using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MotoShop.WebAPI.Helpers
{
    public static class BrowserLauncher
    {
        public static void Launch(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception ex)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    link = link.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {link}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", link);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                { 
                    Process.Start("open", link);
                }
                else
                {
                    throw;
                }
            }

        }
    }
}
