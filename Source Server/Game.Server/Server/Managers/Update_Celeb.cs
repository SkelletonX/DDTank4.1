using System;
using System.Collections.Generic;
using System.Linq;
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
                Console.WriteLine("[Update_Celeb] Error");
            }
        }
        public void UpdateCeleb()
        {
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
