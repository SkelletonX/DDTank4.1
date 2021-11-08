// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.UpdatePet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(1)]
    public class UpdatePet : IPetCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            GamePlayer playerById = WorldMgr.GetPlayerById(num);
            UsersPetInfo[] pets;
            EatPetsInfo allEatPetsByID;
            if (playerById != null)
            {
                pets = playerById.PetBag.GetPets();
                allEatPetsByID = playerById.PetBag.EatPets;
            }
            else
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    pets = playerBussiness.GetUserPetSingles(num);
                    allEatPetsByID = playerBussiness.GetAllEatPetsByID(num);
                    for (int index = 0; index < pets.Length; ++index)
                        pets[index].PetEquips = Player.PetBag.DeserializePetEquip(pets[index].eQPets);
                }
            }
            if (pets != null && allEatPetsByID != null)
                Player.Out.SendPetInfo(num, Player.ZoneId, pets, allEatPetsByID);
            return false;
        }
    }
}
