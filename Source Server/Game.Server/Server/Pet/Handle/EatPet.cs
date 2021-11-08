using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Pet.Handle
{
	[global::Pet(33)]
	public class EatPet : IPetCommandHadler
	{
		public bool CommandHandler(GamePlayer player, GSPacketIn packet)
		{
			int amor = packet.ReadInt();
			int num = packet.ReadInt();
			int num2 = 0;
			int templateId = 201567;
			if (num == 1)
			{
				int num3 = packet.ReadInt();
				for (int i = 0; i < num3; i++)
				{
					int slot = packet.ReadInt();
					int templateID = packet.ReadInt();
					UsersPetInfo petAt = player.PetBag.GetPetAt(slot);
					if (petAt != null)
					{
						PetTemplateInfo petTemplateInfo = PetMgr.FindPetTemplate(templateID);
						if (petTemplateInfo != null)
						{
							num2 += (int)(Math.Pow(10.0, petTemplateInfo.StarLevel - 2) + 5.0 * Math.Max(petAt.Level - 8, (double)petAt.Level * 0.2));
						}
					}
					UpGrade(player, amor, num, num2, null);
					player.PetBag.RemovePet(petAt);
				}
			}
			else
			{
				int num4 = packet.ReadInt();
				ItemInfo itemByTemplateID = player.GetItemByTemplateID(templateId);
				if (itemByTemplateID != null)
				{
					int itemCount = player.GetItemCount(templateId);
					if (itemCount < num4)
					{
						num4 = ((itemCount < num4) ? itemCount : num4);
					}
					int totalPoint = num2 + itemByTemplateID.Template.Property2 * num4;
					int count = UpGrade(player, amor, num, totalPoint, itemByTemplateID);
					if (num4 == 1)
					{
						player.RemoveTemplate(templateId, num4);
					}
					else
					{
						player.RemoveTemplate(templateId, count);
					}
				}
				else
				{
					player.SendMessage(LanguageMgr.GetTranslation("PetHandler.EatPetNotEnoughtCount"));
				}
			}
			player.Out.SendEatPetsInfo(player.PetBag.EatPets);
			player.EquipBag.UpdatePlayerProperties();
			return false;
		}

		private int UpGrade(GamePlayer player, int amor, int type, int totalPoint, ItemInfo eatItem)
		{
			int num = PetMoePropertyMgr.FindMaxLevel();
			switch (amor)
			{
				case 0:
					{
						int num3 = player.PetBag.EatPets.weaponExp + totalPoint;
						for (int j = player.PetBag.EatPets.weaponLevel; j <= num; j++)
						{
							PetMoePropertyInfo petMoePropertyInfo2 = PetMoePropertyMgr.FindPetMoeProperty(j + 1);
							if (petMoePropertyInfo2 != null && petMoePropertyInfo2.Exp <= num3)
							{
								player.PetBag.EatPets.weaponLevel = j + 1;
								num3 -= petMoePropertyInfo2.Exp;
							}
						}
						if (player.PetBag.EatPets.weaponLevel == num)
						{
							totalPoint = ((num3 > 0) ? num3 : totalPoint);
							player.PetBag.EatPets.weaponExp = 0;
						}
						else
						{
							player.PetBag.EatPets.weaponExp = num3;
						}
						break;
					}
				case 1:
					{
						int num4 = player.PetBag.EatPets.clothesExp + totalPoint;
						for (int k = player.PetBag.EatPets.clothesLevel; k <= num; k++)
						{
							PetMoePropertyInfo petMoePropertyInfo3 = PetMoePropertyMgr.FindPetMoeProperty(k + 1);
							if (petMoePropertyInfo3 != null && petMoePropertyInfo3.Exp <= num4)
							{
								player.PetBag.EatPets.clothesLevel = k + 1;
								num4 -= petMoePropertyInfo3.Exp;
							}
						}
						if (player.PetBag.EatPets.clothesLevel == num)
						{
							totalPoint = ((num4 > 0) ? num4 : totalPoint);
							player.PetBag.EatPets.clothesExp = 0;
						}
						else
						{
							player.PetBag.EatPets.clothesExp = num4;
						}
						break;
					}
				case 2:
					{
						int num2 = player.PetBag.EatPets.hatExp + totalPoint;
						for (int i = player.PetBag.EatPets.hatLevel; i <= num; i++)
						{
							PetMoePropertyInfo petMoePropertyInfo = PetMoePropertyMgr.FindPetMoeProperty(i + 1);
							if (petMoePropertyInfo != null && petMoePropertyInfo.Exp <= num2)
							{
								player.PetBag.EatPets.hatLevel = i + 1;
								num2 -= petMoePropertyInfo.Exp;
							}
						}
						if (player.PetBag.EatPets.hatLevel == num)
						{
							totalPoint = ((num2 > 0) ? num2 : totalPoint);
							player.PetBag.EatPets.hatExp = 0;
						}
						else
						{
							player.PetBag.EatPets.hatExp = num2;
						}
						break;
					}
			}
			if (type == 2 && eatItem != null)
			{
				return totalPoint / eatItem.Template.Property2;
			}
			return 0;
		}
	}
}
