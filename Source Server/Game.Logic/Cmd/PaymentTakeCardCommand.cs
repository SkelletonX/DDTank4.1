//fix By HeroDev
using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.Cmd
{
    [GameCommand(114, "付费翻牌")]
    public class PaymentTakeCardCommand : ICommandHandler
    {
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (player.HasPaymentTakeCard)
                return;
            bool flag;
            if (player.GetFightBuffByType(BuffType.Card_Get) != null && !game.IsSpecialPVE() && player.PlayerDetail.UsePayBuff(BuffType.Card_Get))
            {
                flag = true;
            }
            else
            {
                //int num = player.PlayerDetail.PlayerCharacter.typeVIP > 0 ? 437 : 486;//Valor se for VIp ou Não
                //flag = ((player.PlayerDetail.RemoveMoney(num) > 0) ? true : false);//Recebe o resultado do Valor Acima  
            }
            int index = (int)packet.ReadByte();
            ++player.CanTakeOut;
            player.FinishTakeCard = false;
            player.HasPaymentTakeCard = true;
           // player.PlayerDetail.SendHideMessage(LanguageMgr.GetTranslation("Você Obteve Desconto na Abertura das cartas por Ser Vip !"));
            if (index >= 0 && index < game.Cards.Length) { 
                game.TakeCard(player, index);
                Console.WriteLine("1");
            }
            else { 
                game.TakeCard(player);
                Console.WriteLine("2");
            }
        }
    }
}
