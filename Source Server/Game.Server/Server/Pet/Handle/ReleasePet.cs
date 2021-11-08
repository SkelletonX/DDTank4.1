// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.ReleasePet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
  [global::Pet(8)]
  public class ReleasePet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int slot = packet.ReadInt();
      UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
      if (Player.PetBag.RemovePet(petAt))
      {
        int capalility = Player.PetBag.Capalility;
        UsersPetInfo[] pets = Player.PetBag.GetPets();
        Player.PetBag.BeginChanges();
        try
        {
          if (Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot) != -1)
          {
            for (int index = 1; Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot) < pets[pets.Length - index].Place; ++index)
              Player.PetBag.MovePet(pets[pets.Length - index].Place, Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot));
          }
        }
        catch
        {
        }
        finally
        {
          Player.PetBag.CommitChanges();
        }
      }
      Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg19"));
      Player.PetBag.SaveToDatabase(false);
      return false;
    }
  }
}
