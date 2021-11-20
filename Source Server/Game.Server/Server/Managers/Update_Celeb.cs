using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.Managers
{
    public class Update_Celeb
    {
        private bool isuptop;
        private string req = ConfigurationManager.AppSettings["request"] + "CelebList/CreateAllCeleb.ashx";
        private void uptop()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadString(req);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Update_Celeb] Error");
                Console.ResetColor();
            }
        }
        public void UpdateCeleb()
        {

            GameServer.log.Info((object)"UpdateCeleb Scaning ...");
            if (DateTime.Now.Minute > 5)
                isuptop = false;
            if (DateTime.Now.Minute <= 5 && !isuptop)
            {
                this.uptop();
                isuptop = true;
            }

        }
    }
}
