
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Cmd
{
  [GameCommand(143, "BOT Turn")]
  public class BotCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!(game is PVPGame))
        return;
      Player[] allPlayers = (game as PVPGame).GetAllPlayers();
      List<Player> source = new List<Player>();
      foreach (Player player1 in allPlayers)
      {
        if (player1.Team != player.Team)
          source.Add(player1);
      }
      int num1 = 0;
      int num2 = 0;
      if (new Random().Next(0, 10) < 7)
        num1 = new Random().Next(80, 280);
      int index1 = new Random().Next(0, source.Count);
      Player player2 = source.ElementAt<Player>(index1);
      if (player2.X > player.X)
        player.ChangeDirection(1, 500);
      else
        player.ChangeDirection(-1, 500);
      int num3 = new Random().Next(0, 3);
      float time = 1f;
      int bombCount;
      int num4;
      int num5;
      int num6;
      if (Math.Abs(player.X - player2.X) > 60)
      {
        int templateId1;
        int templateId2;
        int templateId3;
        if (num3 == 0)
        {
          bombCount = 1;
          templateId1 = 10002;
          templateId2 = 10004;
          templateId3 = 10004;
          num4 = player2.X;
          num5 = player2.Y;
          num6 = 2;
        }
        else if (player2.X < player.X && player.X - player2.X > 200 && player.X - player2.X < 800)
        {
          bombCount = 3;
          templateId1 = 10001;
          templateId2 = 10003;
          templateId3 = 10004;
          num4 = player2.X + 20;
          num5 = player2.Y;
          num6 = 3;
        }
        else if (Math.Abs(player.X - player2.X) > 1200)
        {
          num4 = player.X <= player2.X ? player2.X - 300 : player2.X + 300;
          bombCount = 1;
          templateId1 = 0;
          templateId2 = 10016;
          templateId3 = 10010;
          num5 = player2.Y - 100;
          num6 = 1;
        }
        else
        {
          bombCount = 1;
          templateId1 = 10001;
          templateId2 = 10004;
          templateId3 = 10004;
          num4 = player2.X;
          num5 = player2.Y;
          num6 = 3;
        }
        if ((uint) templateId1 > 0U)
        {
          ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId1);
          player.UseItem(itemTemplate);
        }
        if ((uint) templateId2 > 0U)
        {
          ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId2);
          player.UseItem(itemTemplate);
        }
        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(templateId3);
        player.UseItem(itemTemplate1);
        time = Math.Abs(player.X - player2.X) >= 200 ? (Math.Abs(player.X - player2.X) >= 400 ? (Math.Abs(player.X - player2.X) >= 700 ? (Math.Abs(player.X - player2.X) >= 1000 ? (Math.Abs(player.X - player2.X) >= 1100 ? 3.5f : 3f) : 2.5f) : 2f) : 1.5f) : 1f;
      }
      else if ((uint) num3 > 0U)
      {
        bombCount = 1;
        int templateId1 = 10010;
        int templateId2 = 10016;
        num5 = player.Y;
        num6 = 1;
        time = 4f;
        num4 = player.X <= 700 ? player.X + 600 : player.X - 600;
        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(templateId1);
        player.UseItem(itemTemplate1);
        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(templateId2);
        player.UseItem(itemTemplate2);
      }
      else
      {
        bombCount = 1;
        int templateId1 = 10001;
        int templateId2 = 10004;
        int templateId3 = 10004;
        num4 = player2.X;
        num5 = player2.Y;
        num6 = 3;
        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(templateId1);
        player.UseItem(itemTemplate1);
        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(templateId2);
        player.UseItem(itemTemplate2);
        ItemTemplateInfo itemTemplate3 = ItemMgr.FindItemTemplate(templateId3);
        player.UseItem(itemTemplate3);
      }
      for (int index2 = 0; index2 < num6; ++index2)
        player.ShootPoint(num4 + num1, num5 + num2, player.CurrentBall.ID, 1001, 10001, bombCount, time, 2000);
      if (player.IsAttacking)
        player.StopAttacking();
      GSPacketIn pkg = new GSPacketIn((short) 91, player.Id);
      pkg.WriteByte((byte) 143);
      game.SendToAll(pkg);
    }
  }
}
