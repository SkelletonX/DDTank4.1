using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Actions;
using Game.Logic.Effects;
using Game.Logic.PetEffects;
using Game.Logic.Phy.Maths;
using Game.Logic.Spells;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
    public class Player : TurnedLiving
    {
        private static readonly int CARRY_TEMPLATE_ID = 10016;
        public int MaxPsychic = 999;
        public bool CanFly = true;
        private readonly List<int> AllowedItems = new List<int>()
    {
      10009,
      10010,
      10011,
      10012,
      10018,
      10021
    };
        public int BossCardCount;
        public int CanTakeOut;
        private int deputyWeaponResCount;
        public bool FinishTakeCard;
        public int GainGP;
        public int GainOffer;
      
        public bool HasPaymentTakeCard;
        private Dictionary<int, int> ItemFightBag;
        public bool LockDirection;
        private int m_AddWoundBallId;
        private int m_ballCount;
        private bool m_canGetProp;
        private BallInfo m_currentBall;
        private int m_changeSpecialball;
        private SqlDataProvider.Data.ItemInfo m_DeputyWeapon;
        private int m_energy;
        private int m_flyCoolDown;
        private SqlDataProvider.Data.ItemInfo m_Healstone;
        private bool m_isActive;
        private int m_loadingProcess;
        private int m_mainBallId;
        private int m_MultiBallId;
        private int m_oldx;
        public bool AttackInformation;
        public bool DefenceInformation;
        private int m_killedPunishmentOffer;
        private int m_powerRatio;
        private int m_oldy;
        private IGamePlayer m_player;
        private int m_prop;
        private int m_shootCount;
        private int m_spBallId;
        private ArrayList m_tempBoxes;
        private SqlDataProvider.Data.ItemInfo m_weapon;
        public bool Ready;
        public Point TargetPoint;
        public int TotalAllCure;
        public int TotalAllExperience;
        public int TotalAllHitTargetCount;
        public int TotalAllHurt;
        public int TotalAllKill;
        public int TotalAllScore;
        public int TotalAllShootCount;
      
        private UsersPetInfo userPetInfo;
        private Dictionary<int, PetSkillInfo> dictionary_0;
        private PetFightPropertyInfo petFightPropertyInfo;
        private BufferInfo VhknhwCsyV;
        public int MOVE_SPEED;
        private double speedMultiplier;

        public event PlayerEventHandle AfterPlayerShooted;

        public event PlayerEventHandle BeforeBomb;

        public event PlayerEventHandle BeforePlayerShoot;

        public event PlayerEventHandle CollidByObject;

        public event PlayerEventHandle LoadingCompleted;

        public event PlayerEventHandle PlayerBeginMoving;

        public event PlayerEventHandle PlayerBuffSkillPet;

        public event PlayerEventHandle PlayerCompleteShoot;

        public event PlayerEventHandle PlayerAnyShellThrow;

        public event PlayerEventHandle PlayerCure;

        public event PlayerEventHandle PlayerGuard;

        public event PlayerEventHandle PlayerShoot;

        public event PlayerEventHandle PlayerClearBuffSkillPet;

        public Player(IGamePlayer player, int id, BaseGame game, int team, int maxBlood)
          : base(id, game, team, "", "", maxBlood, 0, 1)
        {
            this.m_tempBoxes = new ArrayList();
            this.m_flyCoolDown = 2;
            this.speedMultiplier = 1.0;
            this.MOVE_SPEED = 2;
            this.m_rect = new Rectangle(-15, -20, 30, 30);
            this.ItemFightBag = new Dictionary<int, int>();
            this.dictionary_0 = new Dictionary<int, PetSkillInfo>();
            this.userPetInfo = player.Pet;
            if (this.userPetInfo != null && game != null && !game.IsSpecialPVE())
            {
                this.isPet = true;
                base.PetEffects.PetBaseAtt = this.GetPetBaseAtt();
                this.InitPetSkillEffect(this.userPetInfo);
                this.petFightPropertyInfo = PetMgr.FindFightProperty(player.PlayerCharacter.evolutionGrade);
            }
            this.m_player = player;
            this.m_player.GamePlayerId = id;
            this.m_player.GameId = id;
            this.m_isActive = true;
            this.m_canGetProp = true;
            this.Grade = player.PlayerCharacter.Grade;
            if (this.AutoBoot)
                this.VaneOpen = true;
            else
                this.VaneOpen = player.PlayerCharacter.Grade >= 9;
            this.InitFightBuffer(player.FightBuffs);
            this.TotalAllHurt = 0;
            this.TotalAllHitTargetCount = 0;
            this.TotalAllShootCount = 0;
            this.TotalAllKill = 0;
            this.TotalAllExperience = 0;
            this.TotalAllScore = 0;
            this.TotalAllCure = 0;
            this.m_DeputyWeapon = this.m_player.SecondWeapon;
            this.m_Healstone = this.m_player.Healstone;
            this.ChangeSpecialBall = 0;
            this.BlockTurn = false;
            this.deputyWeaponResCount = this.m_DeputyWeapon == null ? 1 : this.m_DeputyWeapon.StrengthenLevel + 1;
            this.m_weapon = this.m_player.MainWeapon;
            if (this.m_weapon != null)
            {
                BallConfigInfo ball = BallConfigMgr.FindBall(this.m_weapon.TemplateID);
                if (this.m_weapon.isGold)
                    ball = BallConfigMgr.FindBall(this.m_weapon.GoldEquip.TemplateID);
                this.m_mainBallId = ball.Common;
                this.m_spBallId = ball.Special;
                this.m_AddWoundBallId = ball.CommonAddWound;
                this.m_MultiBallId = ball.CommonMultiBall;
            }
            this.m_loadingProcess = 0;
            this.m_prop = 0;
            this.InitBuffer(this.m_player.EquipEffect);
            this.m_energy = (this.m_player.PlayerCharacter.AgiAddPlus + this.m_player.PlayerCharacter.Agility) / 30 + 240;
            this.m_maxBlood = this.m_player.PlayerCharacter.hp;
            if (this.FightBuffers.ConsortionAddMaxBlood > 0)
                this.m_maxBlood += this.m_maxBlood * this.FightBuffers.ConsortionAddMaxBlood / 100;
            this.m_maxBlood += this.m_player.PlayerCharacter.HpAddPlus + this.FightBuffers.WorldBossHP + this.FightBuffers.WorldBossHP_MoneyBuff + this.PetEffects.MaxBlood;
            this.CanFly = true;
            this.m_powerRatio = 100;
            BufferInfo fightBuffByType = this.GetFightBuffByType(BuffType.Agility);
            if (fightBuffByType == null || !this.m_player.UsePayBuff(BuffType.Agility))
                return;
            this.VhknhwCsyV = fightBuffByType;
        }

        public bool CanUseItem(ItemTemplateInfo item)
        {
            if (this.m_currentBall.IsSpecial() && !this.AllowedItems.Contains(item.TemplateID) || this.m_energy < item.Property4)
                return false;
            if (this.IsAttacking)
                return true;
            if (!this.IsLiving && this.Team == this.m_game.CurrentLiving.Team)
                return this.IsActive;
            return false;
        }

        public bool CanUseItem(ItemTemplateInfo item, int place)
        {
            if (this.m_currentBall.IsSpecial() && !this.AllowedItems.Contains(item.TemplateID))
                return false;
            if (!this.IsLiving && place == -1)
                return this.psychic >= item.Property7;
            if (!this.IsLiving && place != -1 && this.Team == this.m_game.CurrentLiving.Team)
                return true;
            if (this.m_energy < item.Property4)
                return false;
            if (this.IsAttacking)
                return true;
            if (!this.IsLiving && this.Team == this.m_game.CurrentLiving.Team)
                return this.IsActive;
            return false;
        }

        public void capnhatstate(string loai1, string loai2)
        {
            this.m_game.capnhattrangthai((Living)this, loai1, loai2);
        }

        public override void CollidedByObject(Physics phy)
        {
            base.CollidedByObject(phy);
            if (!(phy is SimpleBomb))
                return;
            this.OnCollidedByObject();
        }

        public void OnMissionEventHandle(GSPacketIn packet)
        {
            if (this.MissionEventHandle == null)
                return;
            this.MissionEventHandle(packet);
        }

        public event PlayerMissionEventHandle MissionEventHandle;

        public bool CheckCanUseItem(ItemTemplateInfo item)
        {
            switch (item.TemplateID)
            {
                case 10001:
                    if (this.ItemFightBag.ContainsKey(10003) && this.ItemFightBag.ContainsKey(10002) || this.ItemFightBag.ContainsKey(10001) && this.ItemFightBag[10001] >= 2)
                        return false;
                    break;
                case 10002:
                    if (this.ItemFightBag.ContainsKey(10003) && this.ItemFightBag.ContainsKey(10001) || this.ItemFightBag.ContainsKey(10002) && this.ItemFightBag[10002] >= 2)
                        return false;
                    break;
                case 10003:
                    if (this.ItemFightBag.ContainsKey(10024) || this.ItemFightBag.ContainsKey(10025) || this.ItemFightBag.ContainsKey(10001) && this.ItemFightBag.ContainsKey(10002))
                        return false;
                    break;
                case 10025:
                    if (this.ItemFightBag.ContainsKey(10003) || this.ItemFightBag.ContainsKey(10024))
                        return false;
                    break;
            }
            if (!this.ItemFightBag.ContainsKey(item.TemplateID))
            {
                this.ItemFightBag.Add(item.TemplateID, 1);
            }
            else
            {
                Dictionary<int, int> itemFightBag;
                int templateId;
                (itemFightBag = this.ItemFightBag)[templateId = item.TemplateID] = itemFightBag[templateId] + 1;
            }
            return true;
        }

        public bool CheckShootPoint(int x, int y)
        {
            return true;
        }

        public void DeadLink()
        {
            this.m_isActive = false;
            if (!this.IsLiving)
                return;
            this.Die();
        }

        public override void Die()
        {
            if (!this.IsLiving)
                return;
            this.m_y -= 70;
            base.Die();
        }

        private int GetInitDelay()
        {
            return 1600 - 1200 * this.PlayerDetail.PlayerCharacter.Agility / (this.PlayerDetail.PlayerCharacter.Agility + 1200);
        }

        private int GetTurnDelay()
        {
            return 1600 - 1200 * this.PlayerDetail.PlayerCharacter.Agility / (this.PlayerDetail.PlayerCharacter.Agility + 1200);
        }

        public void InitBuffer(List<int> equpedEffect)
        {
            for (int index = 0; index < equpedEffect.Count; ++index)
            {
                ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(equpedEffect[index]);
                switch (itemTemplate.Property3)
                {
                    case 1:
                        new AddAttackEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 2:
                        new AddDefenceEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 3:
                        new AddAgilityEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 4:
                        new AddLuckyEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 5:
                        new AddDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 6:
                        new ReduceDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 7:
                        new AddBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 8:
                        new FatalEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 9:
                        new IceFronzeEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 10:
                        new NoHoleEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 11:
                        new AtomBombEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 12:
                        new ArmorPiercerEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 13:
                        new AvoidDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 14:
                        new MakeCriticalEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 15:
                        new AssimilateDamageEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 16:
                        new AssimilateBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 17:
                        new SealEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 18:
                        new AddTurnEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 19:
                        new AddDanderEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 20:
                        new ReflexDamageEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 21:
                        new ReduceStrengthEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 22:
                        new ContinueReduceBloodEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 23:
                        new LockDirectionEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 24:
                        new AddBombEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 25:
                        new ContinueReduceDamageEquipEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                    case 26:
                        new RecoverBloodEffect(itemTemplate.Property4, itemTemplate.Property5).Start((Living)this);
                        break;
                }
            }
        }

        public void InitPetSkillEffect(UsersPetInfo pet)
        {
            string skillEquip = pet.SkillEquip;
            char[] chArray1 = new char[1] { '|' };
            foreach (string str1 in skillEquip.Split(chArray1))
            {
                char[] chArray2 = new char[1] { ',' };
                int num = int.Parse(str1.Split(chArray2)[0]);
                PetSkillInfo petSkill = PetMgr.FindPetSkill(num);
                if (petSkill == null)
                    break;
                string[] strArray = petSkill.ElementIDs.Split(',');
                int coldDown = petSkill.ColdDown;
                int probability = petSkill.Probability;
                int delay = petSkill.Delay;
                int gameType = petSkill.GameType;
                foreach (string elementID in strArray)
                {
                    string element = elementID;
                    if (element != null)
                    {
                        string key = element;
                        if (key != null)
                        {
                            switch (key)
                            {
                                #region PET Chicken
                                case "1017"://Di chuyển không thể
                                    new PlayerStopMovingEffect(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1021":// Miễn kháng
                                    new ImmunityEffect(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1038"://Dẫn đường
                                    new GuideShoot(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1082"://nhận hiệu quả miễn kháng
                                    new CantDropPlayer(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1138"://100% xác suất bạo kích
                                    new CriticalChance(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1328"://Sát thương tăng 100%
                                    new DamageInCreaseLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1329"://Sát thương tăng 130%
                                    new DamageInCreaseLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1330"://Sát thương tăng 155%
                                    new DamageInCreaseLv3(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1331"://phòng thủ +15%
                                    new InCreaseDefenceLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1332"://phòng thủ +20%
                                    new InCreaseDefenceLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1333"://phòng thủ +30%
                                    new InCreaseDefenceLv3(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1334"://Tấn công giảm 20%
                                    new RemoveAttackLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1335"://Tấn công giảm 15%
                                    new RemoveAttackLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1336"://Tăng hộ giáp = 250
                                    new AddBaseGuardLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1337"://Tăng hộ giáp = 365
                                    new AddBaseGuardLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1338"://Di chuyển sẽ bị mất hiệu quả
                                case "1339"://Di chuyển sẽ bị mất hiệu quả
                                    new RemoveEffectForAddBaseGuard(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1340"://Mỗi lần bắn gây sát thương bằng 2% HP hiện tại của bản thân
                                    new UseBloodAndShootLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1341"://Mỗi lần bắn gây sát thương bằng 2% HP hiện tại của bản thân
                                    new UseBloodAndShootLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1342"://Tăng 30% sát thương
                                    new AddBaseDamageEffectLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1343"://Tăng 45% sát thương
                                    new AddBaseDamageEffectLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1344"://May mắn tăng 10%
                                    new AddLuckyEffectLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1345"://May mắn tăng 15%
                                    new AddLuckyEffectLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1346"://Hộ giáp giảm 30%
                                    new RemoveBaseGuardEffectLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1347"://Hộ giáp giảm 25%
                                    new RemoveBaseGuardEffectLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1353"://Mỗi lần bị tấn công trúng chính xác, nhận 40 sát thương thêm
                                    new AdditionalDamagePointLv1(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1354"://Mỗi lần bị tấn công trúng chính xác, nhận 40 sát thương thêm
                                    new AdditionalDamagePointLv2(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1348"://Giải trừ kỹ năng pháo đài v3
                                    new RemoveEffectSkill01(coldDown, probability, gameType, num, delay, "1349").Start((Living)this);
                                    new RemoveEffectSkill02(coldDown, probability, gameType, num, delay, "1350").Start((Living)this);
                                    continue;
                                case "1351"://Nhận 4 điểm ma pháp
                                    new AddMagicPointPet(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1352"://Giảm 3.3% HP.
                                    new RemoveBloodOverTurn(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                #region PET Snake
                                case "1039"://Sát thương tăng 150%
                                    new CASE1039(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1133"://Sát thương tăng 120%
                                    new CASE1133(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1134"://Sát thương tăng 180%
                                    new CASE1134(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1201"://Sát thương-100
                                    new CASE1201(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1202"://Sát thương-200
                                    new CASE1202(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1203"://Sát thương-300
                                    new CASE1203(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1204"://Tấn công -300
                                    new CASE1204(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1205"://Tấn công -500
                                    new CASE1205(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1206"://Phòng thủ -300
                                    new CASE1206(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1207"://Phòng thủ -500
                                    new CASE1207(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1208"://-10 MP Team địch
                                    new CASE1208(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1209"://-30 MP Team địch
                                    new CASE1209(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1210"://Mỗi turn -500 HP
                                    new CASE1210(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1211"://Mỗi turn -1000 HP
                                    new CASE1211(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1212"://Tỷ lệ bạo kích +20%
                                    new CASE1212(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1213"://Tỷ lệ bạo kích +50%
                                    new CASE1213(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1226"://Mỗi turn giảm 500 HP
                                    new CASE1226(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1227"://Mỗi turn giảm 1000 HP
                                    new CASE1227(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1214"://Hộ giáp +100, HP+1500
                                    new CASE1214(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1215"://Hộ giáp +200, HP+3000
                                    new CASE1215(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1216"://Tăng 1500 HP
                                    new CASE1216(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1217"://Tăng 3000 HP
                                    new CASE1217(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1220"://Hộ giáp -100
                                    new CASE1220(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1221"://Hộ giáp -200
                                    new CASE1221(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1222":
                                    new CASE1222(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1223":
                                    new CASE1223(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                #region PET Ant
                                case "1439"://Tăng hộ giáp 100 điểm
                                    new CASE1439(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1440"://Tăng hộ giáp 300 điểm
                                    new CASE1440(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1441"://Tăng hộ giáp 500 điểm
                                    new CASE1441(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1442"://Giảm sát thương phải chịu
                                    new CASE1442(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1443"://Giảm sát thương phải chịu
                                    new CASE1443(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1444"://Giảm sát thương phải chịu
                                    new CASE1444(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1445"://
                                case "1032"://
                                case "1033"://
                                    new CASE1445(coldDown, probability, gameType, num, delay, "1445").Start((Living)this);
                                    continue;
                                case "1446"://
                                case "1034"://
                                    new CASE1446(coldDown, probability, gameType, num, delay, "1446").Start((Living)this);
                                    continue;
                                case "1067"://Phản đòn bằng 30% sát thương
                                    new CASE1067(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1068"://Phản đòn bằng 50% sát thương
                                    new CASE1068(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1136"://duy trì phản kích
                                    new CASE1136(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1449"://
                                    new CASE1449(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1450"://
                                    new CASE1450(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1451"://
                                    new CASE1451(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1452"://
                                    new CASE1452(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1459"://
                                    new CASE1459(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1460"://
                                    new CASE1460(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1137"://
                                    new CASE1137(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1455"://
                                    new CASE1455(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1456"://
                                    new CASE1456(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1117"://
                                    new CASE1117(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1457"://
                                    new CASE1457(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                #region PET Green
                                case "1358"://
                                    new CASE1358(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1359"://
                                    new CASE1359(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1360"://
                                    new CASE1360(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1361"://
                                    new CASE1361(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1362"://
                                    new CASE1362(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1363"://
                                    new CASE1363(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1364"://
                                    new CASE1364(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1365"://
                                    new CASE1365(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1366"://
                                    new CASE1366(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1367"://
                                    new CASE1367(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1058"://
                                case "1059"://
                                    new CASE1058(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1063"://
                                case "1064"://
                                    new CASE1063(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1368"://
                                    new CASE1368(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1369"://
                                    new CASE1369(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1372"://
                                    new CASE1372(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1373"://
                                    new CASE1373(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1374"://
                                    new CASE1374(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1375"://
                                    new CASE1375(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1376"://
                                    new CASE1376(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                #region PET Dragon
                                case "1150"://
                                    new CASE1150(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1151"://
                                    new CASE1151(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1152"://
                                    new CASE1152(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1153"://
                                    new CASE1153(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1154"://
                                    new CASE1154(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1155"://
                                    new CASE1155(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1156"://
                                    new CASE1156(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1172"://
                                case "1173"://
                                    new CASE1172(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1174"://
                                case "1175"://
                                    new CASE1174(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1170"://
                                    new CASE1170(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1171"://
                                    new CASE1171(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1176"://
                                    new CASE1176(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1177"://
                                    new CASE1177(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1163"://
                                    new CASE1163(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1164"://
                                    new CASE1164(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1165"://
                                    new CASE1165(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1166"://
                                    new CASE1166(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1161"://
                                    new CASE1161(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1162"://
                                    new CASE1162(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1323"://
                                    new CASE1323(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1324"://
                                    new CASE1324(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1322"://
                                    new CASE1322(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                #region PET Fight
                                case "1040"://
                                    new CASE1040(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1041"://
                                    new CASE1041(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1042"://
                                    new CASE1042(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1022"://
                                    new CASE1022(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1023"://
                                    new CASE1023(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1024"://
                                    new CASE1024(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1025"://
                                    new CASE1025(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1056"://
                                    new CASE1056(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1057"://
                                    new CASE1057(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1074"://
                                case "1075"://
                                    new CASE1074(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1078"://
                                case "1079"://
                                    new CASE1078(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1092"://
                                    new CASE1092(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1093"://
                                    new CASE1093(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1094"://
                                    new CASE1094(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1095"://
                                    new CASE1095(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1096"://
                                    new CASE1096(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1097"://
                                    new CASE1097(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1098"://
                                    new CASE1098(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1099"://
                                    new CASE1099(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1100"://
                                    new CASE1100(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                case "1101"://
                                    new CASE1101(coldDown, probability, gameType, num, delay, elementID).Start((Living)this);
                                    continue;
                                #endregion
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        public void InitFightBuffer(List<BufferInfo> buffers)
        {
            foreach (BufferInfo buffer in buffers)
            {
                switch (buffer.Type)
                {
                    case 101:
                        this.FightBuffers.ConsortionAddBloodGunCount = buffer.Value;
                        continue;
                    case 102:
                        this.FightBuffers.ConsortionAddDamage = buffer.Value;
                        continue;
                    case 103:
                        this.FightBuffers.ConsortionAddCritical = buffer.Value;
                        continue;
                    case 104:
                        this.FightBuffers.ConsortionAddMaxBlood = buffer.Value;
                        continue;
                    case 105:
                        this.FightBuffers.ConsortionAddProperty = buffer.Value;
                        continue;
                    case 106:
                        this.FightBuffers.ConsortionReduceEnergyUse = buffer.Value;
                        continue;
                    case 107:
                        this.FightBuffers.ConsortionAddEnergy = buffer.Value;
                        continue;
                    case 108:
                        this.FightBuffers.ConsortionAddEffectTurn = buffer.Value;
                        continue;
                    case 109:
                        this.FightBuffers.ConsortionAddOfferRate = buffer.Value;
                        continue;
                    case 110:
                        this.FightBuffers.ConsortionAddPercentGoldOrGP = buffer.Value;
                        continue;
                    case 111:
                        this.FightBuffers.ConsortionAddSpellCount = buffer.Value;
                        continue;
                    case 112:
                        this.FightBuffers.ConsortionReduceDander = buffer.Value;
                        continue;
                    case 400:
                        this.FightBuffers.WorldBossHP = buffer.Value;
                        continue;
                    case 401:
                        this.FightBuffers.WorldBossAttrack = buffer.Value;
                        continue;
                    case 402:
                        this.FightBuffers.WorldBossHP_MoneyBuff = buffer.Value;
                        continue;
                    case 403:
                        this.FightBuffers.WorldBossAttrack_MoneyBuff = buffer.Value;
                        continue;
                    case 404:
                        this.FightBuffers.WorldBossMetalSlug = buffer.Value;
                        continue;
                    case 405:
                        this.FightBuffers.WorldBossAncientBlessings = buffer.Value;
                        continue;
                    case 406:
                        this.FightBuffers.WorldBossAddDamage = buffer.Value;
                        continue;
                    case 73: //Dev SkelletonX
                        this.FightBuffers.WorldBossAddSH = buffer.Value;
                        continue;
                    default:
                        Console.WriteLine(string.Format("Not Found FightBuff Type {0} Value {1}", (object)buffer.Type, (object)buffer.Value));
                        continue;
                }
            }
        }

        public bool IsCure()
        {
            switch (this.Weapon.TemplateID)
            {
                case 17000:
                case 17001:
                case 17002:
                case 17005:
                case 17007:
                case 17010:
                case 17100:
                case 17102:
                    return true;
                default:
                    return false;
            }
        }

        public void CalculatePlayerOffer(Player player)
        {
            if (this.m_game.RoomType != eRoomType.Match || this.m_game.GameType != eGameType.Guild && this.m_game.GameType != eGameType.Free || player.IsLiving)
                return;
            int num1 = this.Game.GameType != eGameType.Guild ? (this.PlayerDetail.PlayerCharacter.ConsortiaID == 0 || player.PlayerDetail.PlayerCharacter.ConsortiaID == 0 ? 1 : 3) : 10;
            if (num1 > player.PlayerDetail.PlayerCharacter.Offer)
                num1 = player.PlayerDetail.PlayerCharacter.Offer;
            int num2 = num1 + this.TotalHurt / 2000;
            if (num2 <= 0)
                return;
            this.GainOffer += num2;
            player.KilledPunishmentOffer = num2;
        }

        public override void OnAfterKillingLiving(Living target, int damageAmount, int criticalAmount)
        {
            base.OnAfterKillingLiving(target, damageAmount, criticalAmount);
            if (target is Player)
            {
                this.m_player.OnKillingLiving((AbstractGame)this.m_game, 1, target.Id, target.IsLiving, damageAmount + criticalAmount);
            }
            else
            {
                int id = 0;
                if (target is SimpleBoss)
                    id = (target as SimpleBoss).NpcInfo.ID;
                if (target is SimpleNpc)
                    id = (target as SimpleNpc).NpcInfo.ID;
                this.m_player.OnKillingLiving((AbstractGame)this.m_game, 2, id, target.IsLiving, damageAmount + criticalAmount);
            }
        }

        protected void OnAfterPlayerShoot()
        {
            if (this.AfterPlayerShooted == null)
                return;
            this.AfterPlayerShooted(this);
        }

        protected void OnBeforePlayerShoot()
        {
            if (this.BeforePlayerShoot == null)
                return;
            this.BeforePlayerShoot(this);
        }

        protected void OnCollidedByObject()
        {
            if (this.CollidByObject == null)
                return;
            this.CollidByObject(this);
        }

        protected void OnLoadingCompleted()
        {
            if (this.LoadingCompleted == null)
                return;
            this.LoadingCompleted(this);
        }

        public void OnPlayerBuffSkillPet()
        {
            if (this.PlayerBuffSkillPet == null)
                return;
            this.PlayerBuffSkillPet(this);
        }

        public void OnPlayerCompleteShoot()
        {
            if (this.PlayerCompleteShoot == null)
                return;
            this.PlayerCompleteShoot(this);
        }

        public void OnPlayerAnyShellThrow()
        {
            if (this.PlayerAnyShellThrow == null)
                return;
            this.PlayerAnyShellThrow(this);
        }

        public void OnPlayerClearBuffSkillPet()
        {
            if (this.PlayerClearBuffSkillPet == null)
                return;
            this.PlayerClearBuffSkillPet(this);
        }

        public void OnPlayerCure()
        {
            if (this.PlayerCure == null)
                return;
            this.PlayerCure(this);
        }

        public void OnPlayerGuard()
        {
            if (this.PlayerGuard == null)
                return;
            this.PlayerGuard(this);
        }

        protected void OnPlayerMoving()
        {
            if (this.PlayerBeginMoving == null)
                return;
            this.PlayerBeginMoving(this);
        }

        public void OnPlayerShoot()
        {
            if (this.PlayerShoot == null)
                return;
            this.PlayerShoot(this);
        }

        public void OpenBox(int boxId)
        {
            Box box = (Box)null;
            foreach (Box tempBox in this.m_tempBoxes)
            {
                if (tempBox.Id == boxId)
                {
                    box = tempBox;
                    break;
                }
            }
            if (box == null || box.Item == null)
                return;
            SqlDataProvider.Data.ItemInfo cloneItem = box.Item;
            switch (cloneItem.TemplateID)
            {
                case -1100:
                    this.m_player.AddGiftToken(cloneItem.Count);
                    break;
                case -200:
                    this.m_player.AddMoney(cloneItem.Count);
                    this.m_player.LogAddMoney(AddMoneyType.Box, AddMoneyType.Box_Open, this.m_player.PlayerCharacter.ID, cloneItem.Count, this.m_player.PlayerCharacter.Money);
                    break;
                case -100:
                    this.m_player.AddGold(cloneItem.Count);
                    break;
                default:
                    if (cloneItem.Template.CategoryID == 10)
                    {
                        if (!this.m_player.AddTemplate(cloneItem, eBageType.FightBag, cloneItem.Count, eGameView.RouletteTypeGet))
                            break;
                        break;
                    }
                    this.m_player.AddTemplate(cloneItem, eBageType.TempBag, cloneItem.Count, eGameView.dungeonTypeGet);
                    break;
            }
            this.m_tempBoxes.Remove((object)box);
        }

        public override void PickBox(Box box)
        {
            this.m_tempBoxes.Add((object)box);
            base.PickBox(box);
        }

        public override void PrepareNewTurn()
        {
            this.ItemFightBag.Clear();
            if (this.CurrentIsHitTarget)
                ++this.TotalHitTargetCount;
            this.m_energy = (int)this.Agility / 30 + 240;
            if (this.FightBuffers.ConsortionAddEnergy > 0)
                this.m_energy += this.FightBuffers.ConsortionAddEnergy;
            this.PetEffects.CurrentUseSkill = 0;
            this.PetEffects.PetDelay = 0;
            this.SpecialSkillDelay = 0;
            this.PetEffectTrigger = false;
            this.SpecialSkillDelay = 0;
            this.m_shootCount = 1;
            this.m_ballCount = 1;
            this.AttackInformation = true;
            this.DefenceInformation = true;
            this.EffectTrigger = false;
            this.PetEffectTrigger = false;
            --this.m_flyCoolDown;
            this.SetCurrentWeapon(this.PlayerDetail.MainWeapon);
            if (this.m_currentBall.ID != this.m_mainBallId)
                this.m_currentBall = BallMgr.FindBall(this.m_mainBallId);
            if (!this.IsLiving)
            {
                this.StartGhostMoving();
                this.TargetPoint = Point.Empty;
            }
            if (!this.PetEffects.StopMoving)
                base.SpeedMultX(3);
            this.CanFly = true;
            base.PrepareNewTurn();
        }

        public override void PrepareSelfTurn()
        {
            base.PrepareSelfTurn();
            this.DefaultDelay = this.m_delay;
            --this.m_flyCoolDown;
            if (this.IsFrost || this.BlockTurn)
                this.AddDelay(this.GetTurnDelay());
            this.m_game.method_51((Living)this);
            if (this.userPetInfo == null)
                return;
            string skillEquip = this.userPetInfo.SkillEquip;
            char[] chArray1 = new char[1] { '|' };
            foreach (string str in skillEquip.Split(chArray1))
            {
                char[] chArray2 = new char[1] { ',' };
                int key = int.Parse(str.Split(chArray2)[0]);
                if (this.dictionary_0.ContainsKey(key) && this.dictionary_0[key].Turn > 0)
                    --this.dictionary_0[key].Turn;
            }
        }

        public void PrepareShoot(byte speedTime)
        {
            int turnWaitTime = this.m_game.GetTurnWaitTime();
            this.AddDelay(((int)speedTime > turnWaitTime ? turnWaitTime : (int)speedTime) * 20);
            ++this.TotalShootCount;
        }

        public bool ReduceEnergy(int value)
        {
            if (value > this.m_energy)
                return false;
            this.m_energy -= value;
            return true;
        }

        public Dictionary<int, PetSkillInfo> PetSkillCD
        {
            get
            {
                return this.dictionary_0;
            }
        }

        public void ResetSkillCd()
        {
            if (this.userPetInfo == null)
                return;
            string skillEquip = this.userPetInfo.SkillEquip;
            char[] chArray1 = new char[1] { '|' };
            foreach (string str in skillEquip.Split(chArray1))
            {
                char[] chArray2 = new char[1] { ',' };
                int key = int.Parse(str.Split(chArray2)[0]);
                if (this.dictionary_0.ContainsKey(key))
                    this.dictionary_0[key].Turn = this.dictionary_0[key].ColdDown;
            }
        }

        public int GetPetBaseAtt()
        {
            try
            {
                string[] skillArray = this.userPetInfo.SkillEquip.Split(new char[]
                {
                    '|'
                });
                for (int i = 0; i < skillArray.Length; i++)
                {
                    int skillID = Convert.ToInt32(skillArray[i].Split(new char[]
                    {
                        ','
                    })[0]);
                    PetSkillInfo newBall = PetMgr.FindPetSkill(skillID);
                    if (newBall != null && newBall.Damage > 0)
                    {
                        int result = newBall.Damage;
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("______________GetPetBaseAtt ERROR______________");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("_______________________________________________");
                int result = 0;
                return result;
            }
            return 0;
        }

        public override void Reset()
        {
            if (this.m_game.RoomType == eRoomType.Dungeon)
                this.m_game.Cards = new int[21];
            else
                this.m_game.Cards = new int[9];
            this.Dander = 0;
            this.PetMP = 10;
            this.psychic = 0;
            this.IsLiving = true;
            this.FinishTakeCard = false;
            if (this.AutoBoot)
                this.VaneOpen = true;
            else
                this.VaneOpen = this.m_player.PlayerCharacter.Grade >= 9;
            this.InitFightBuffer(this.m_player.FightBuffs);
            this.m_Healstone = this.m_player.Healstone;
            this.m_changeSpecialball = 0;
            this.m_DeputyWeapon = this.m_player.SecondWeapon;
            this.m_weapon = this.m_player.MainWeapon;
            BallConfigInfo ball = BallConfigMgr.FindBall(this.m_weapon.TemplateID);
            this.m_mainBallId = ball.Common;
            this.m_spBallId = ball.Special;
            this.m_AddWoundBallId = ball.CommonAddWound;
            this.m_MultiBallId = ball.CommonMultiBall;
            this.BaseDamage = this.m_player.GetBaseAttack();
            this.BaseGuard = this.m_player.GetBaseDefence();
            this.Attack = (double)this.m_player.PlayerCharacter.Attack;
            this.Defence = (double)this.m_player.PlayerCharacter.Defence;
            this.Agility = (double)this.m_player.PlayerCharacter.Agility;
            this.Lucky = (double)this.m_player.PlayerCharacter.Luck;
            this.m_maxBlood = this.m_player.PlayerCharacter.hp;
            this.BaseDamage += (double)this.m_player.PlayerCharacter.DameAddPlus;
            if (this.FightBuffers.ConsortionAddDamage > 0)
                this.BaseDamage += (double)this.FightBuffers.ConsortionAddDamage;
            this.BaseGuard += (double)this.m_player.PlayerCharacter.GuardAddPlus;
            this.Attack += (double)this.m_player.PlayerCharacter.AttackAddPlus;
            this.Defence += (double)this.m_player.PlayerCharacter.DefendAddPlus;
            this.Agility += (double)this.m_player.PlayerCharacter.AgiAddPlus;
            this.Lucky += (double)this.m_player.PlayerCharacter.LuckAddPlus;
            this.Attack += (double)this.m_player.PlayerCharacter.StrengthEnchance;
            this.Defence += (double)this.m_player.PlayerCharacter.StrengthEnchance;
            this.Agility += (double)this.m_player.PlayerCharacter.StrengthEnchance;
            this.Lucky += (double)this.m_player.PlayerCharacter.StrengthEnchance;
            if (this.FightBuffers.ConsortionAddMaxBlood > 0)
                this.m_maxBlood += this.m_maxBlood * this.FightBuffers.ConsortionAddMaxBlood / 100;
            this.m_maxBlood += this.m_player.PlayerCharacter.HpAddPlus + this.PetEffects.MaxBlood;
            if (this.VhknhwCsyV != null)
            {
                this.Attack += this.Attack / 100.0 * (double)this.VhknhwCsyV.Value;
                this.Defence += this.Defence / 100.0 * (double)this.VhknhwCsyV.Value;
                this.Agility += this.Agility / 100.0 * (double)this.VhknhwCsyV.Value;
                this.Lucky += this.Lucky / 100.0 * (double)this.VhknhwCsyV.Value;
            }
            if (this.FightBuffers.ConsortionAddProperty > 0)
            {
                this.Attack += (double)this.FightBuffers.ConsortionAddProperty;
                this.Defence += (double)this.FightBuffers.ConsortionAddProperty;
                this.Agility += (double)this.FightBuffers.ConsortionAddProperty;
                this.Lucky += (double)this.FightBuffers.ConsortionAddProperty;
            }
            this.m_energy = (int)this.Agility / 30 + 240;
            if (this.FightBuffers.ConsortionAddEnergy > 0)
                this.m_energy += this.FightBuffers.ConsortionAddEnergy;
            if (this.petFightPropertyInfo != null)
            {
                this.Attack += (double)this.petFightPropertyInfo.Attack;
                this.Defence += (double)this.petFightPropertyInfo.Defence;
                this.Agility += (double)this.petFightPropertyInfo.Agility;
                this.Lucky += (double)this.petFightPropertyInfo.Lucky;
                this.m_maxBlood += this.petFightPropertyInfo.Blood;
            }
            this.m_currentBall = BallMgr.FindBall(this.m_mainBallId);
            this.m_shootCount = 1;
            this.m_ballCount = 1;
            this.CurrentIsHitTarget = false;
            this.TotalCure = 0;
            this.TotalHitTargetCount = 0;
            this.TotalHurt = 0;
            this.TotalKill = 0;
            this.TotalShootCount = 0;
            this.LockDirection = false;
            this.GainGP = 0;
            this.GainOffer = 0;
            this.Ready = false;
            this.PlayerDetail.ClearTempBag();
            this.m_delay = this.GetTurnDelay();
            this.m_killedPunishmentOffer = 0;
            this.m_loadingProcess = 0;
            this.m_prop = 0;
            this.InitBuffer(this.m_player.EquipEffect);
            this.CanFly = true;
            this.BlockTurn = false;
            this.deputyWeaponResCount = this.m_DeputyWeapon == null ? 1 : this.m_DeputyWeapon.StrengthenLevel + 1;
            this.ResetSkillCd();
            this.m_powerRatio = 100;
            base.Reset();
        }

        public void SetBall(int ballId)
        {
            this.SetBall(ballId, false);
        }

        public void SetBall(int ballId, bool special)
        {
            if (ballId == this.m_currentBall.ID)
                return;
            if (BallMgr.FindBall(ballId) != null)
                this.m_currentBall = BallMgr.FindBall(ballId);
            this.m_game.SendGameUpdateBall(this, special);
        }

        public void SetCurrentWeapon(SqlDataProvider.Data.ItemInfo item)
        {
            this.m_weapon = item;
            BallConfigInfo ball = BallConfigMgr.FindBall(this.m_weapon.TemplateID);
            if (this.m_weapon.isGold)
                ball = BallConfigMgr.FindBall(this.m_weapon.GoldEquip.TemplateID);
            if (this.ChangeSpecialBall > 0)
                ball = BallConfigMgr.FindBall(70396);
            this.m_mainBallId = ball.Common;
            this.m_spBallId = ball.Special;
            this.m_AddWoundBallId = ball.CommonAddWound;
            this.m_MultiBallId = ball.CommonMultiBall;
            this.SetBall(this.m_mainBallId);
        }

        public int PowerRatio
        {
            get
            {
                return this.m_powerRatio;
            }
        }

        public override void SetXY(int x, int y)
        {
            if (this.m_x == x && this.m_y == y)
                return;
            int num = Math.Abs(this.m_x - x);
            this.m_x = x;
            this.m_y = y;
            if (this.IsLiving)
            {
                this.m_energy -= Math.Abs(this.m_x - x);
                if (num <= 0)
                    return;
                this.OnPlayerMoving();
            }
            else
            {
                Rectangle rect = this.m_rect;
                rect.Offset(this.m_x, this.m_y);
                foreach (Physics physicalObject in this.m_map.FindPhysicalObjects(rect, (Physics)this))
                {
                    if (physicalObject is Box)
                    {
                        this.PickBox(physicalObject as Box);
                        this.Game.CheckBox();
                    }
                }
            }
        }

        public bool Shoot(int x, int y, int force, int angle)
        {
            if (this.m_shootCount == 1)
                this.PetEffects.ActivePetHit = true;
            if (this.m_shootCount > 0)
            {
                this.OnPlayerShoot();
                int bombId = this.m_currentBall.ID;
                if (this.m_ballCount == 1 && !this.IsSpecialSkill)
                {
                    if (this.Prop == 20002)
                        bombId = this.m_MultiBallId;
                    if (this.Prop == 20008)
                        bombId = this.m_AddWoundBallId;
                }
                this.OnBeforePlayerShoot();
                if (this.IsSpecialSkill)
                    this.SpecialSkillDelay = 2000;
                this.OnPlayerAnyShellThrow();//this
                if (this.ShootImp(bombId, x, y, force, angle, this.m_ballCount,0))
                {
                    if (bombId == 4)
                        this.m_game.AddAction((IAction)new FightAchievementAction((Living)this, eFightAchievementType.SuperMansNuclearExplosion, this.Direction, 1000));
                    --this.m_shootCount;
                    if (this.m_shootCount <= 0 || !this.IsLiving)
                    {
                        this.StopAttacking();
                        this.AddDelay(this.m_currentBall.Delay + (this.m_weapon.isGold ? this.m_weapon.GoldEquip.Property8 : this.m_weapon.Template.Property8));
                        this.AddDander(20);
                        this.AddPetMP(10);
                        this.m_prop = 0;
                        if (this.CanGetProp)
                        {
                            int gold = 0;
                            int money = 0;
                            int giftToken = 0;
                            int medal = 0;
                            int honor = 0;
                            int hardCurrency = 0;
                            int token = 0;
                            int dragonToken = 0;
                            int magicStonePoint = 0;
                            List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>)null;
                            if (DropInventory.FireDrop(this.m_game.RoomType, ref info) && info != null)
                            {
                                foreach (SqlDataProvider.Data.ItemInfo itemInfo in info)
                                {
                                    ShopMgr.FindSpecialItemInfo(itemInfo, ref gold, ref money, ref giftToken, ref medal, ref honor, ref hardCurrency, ref token, ref dragonToken, ref magicStonePoint);
                                    if (itemInfo != null && this.VaneOpen && itemInfo.TemplateID > 0)
                                    {
                                        if (itemInfo.Template.CategoryID == 10)
                                        {
                                            if (!this.PlayerDetail.AddTemplate(itemInfo, eBageType.FightBag, itemInfo.Count, eGameView.RouletteTypeGet))
                                                ;
                                        }
                                        else
                                            this.PlayerDetail.AddTemplate(itemInfo, eBageType.TempBag, itemInfo.Count, eGameView.dungeonTypeGet);
                                    }
                                }
                                this.PlayerDetail.AddGold(gold);
                                this.PlayerDetail.AddMoney(money);
                                this.PlayerDetail.LogAddMoney(AddMoneyType.Game, AddMoneyType.Game_Shoot, this.PlayerDetail.PlayerCharacter.ID, money, this.PlayerDetail.PlayerCharacter.Money);
                                this.PlayerDetail.AddGiftToken(giftToken);
                            }
                        }
                        this.OnPlayerCompleteShoot();
                    }
                    this.SendAttackInformation();
                    this.OnAfterPlayerShoot();
                    return true;
                }
            }
            return false;
        }

        public override void Skip(int spendTime)
        {
            if (!this.IsAttacking)
                return;
            base.Skip(spendTime);
            this.AddDelay(100);
            this.AddDander(40);
            this.AddPetMP(10);
        }

        public void PetUseKill(int skillID, int type)
        {
            if (!this.CanUseSkill(skillID))
                return;
            PetSkillInfo petSkill = PetMgr.FindPetSkill(skillID);
            if (this.PetMP > 0 && this.PetMP >= petSkill.CostMP)
            {
                if (petSkill.NewBallID != -1)
                {
                    this.m_delay += petSkill.Delay;
                    this.SetBall(petSkill.NewBallID);
                }
                this.PetMP -= petSkill.CostMP;
                if (petSkill.DamageCrit > 0)
                {
                    base.PetEffects.CritActive = true;
                    this.CurrentDamagePlus += (float)(petSkill.DamageCrit / 100);
                }
                this.PetEffects.IsPetUseSkill = true;
                this.PetEffects.CurrentUseSkill = skillID;
                this.m_game.SendPetUseKill(this, type);
                this.OnPlayerBuffSkillPet();
            }
            else
                this.m_player.SendMessage("Ma Pháp không đủ.");
        }

        public bool CanUseSkill(int Id)
        {
            if (this.userPetInfo != null)
            {
                string skillEquip = this.userPetInfo.SkillEquip;
                char[] chArray1 = new char[1] { '|' };
                foreach (string str in skillEquip.Split(chArray1))
                {
                    char[] chArray2 = new char[1] { ',' };
                    if (int.Parse(str.Split(chArray2)[0]) == Id)
                        return true;
                }
            }
            return false;
        }

        public override void StartAttacking()
        {
            if (this.IsAttacking)
                return;
            if (this.m_Healstone != null && this.m_blood < this.m_maxBlood && (!this.Game.IsSpecialPVE() && this.m_player.RemoveHealstone()))
            {
                int property2 = this.m_Healstone.Template.Property2;
                BufferInfo fightBuffByType = this.GetFightBuffByType(BuffType.ReHealth);
                if (fightBuffByType != null && this.m_player.UsePayBuff(BuffType.ReHealth))
                    property2 *= fightBuffByType.Value;
                this.AddBlood(property2);
            }
            this.AddDelay(this.GetTurnDelay());
            base.StartAttacking();
        }

        public BufferInfo GetFightBuffByType(BuffType buff)
        {
            foreach (BufferInfo fightBuff in this.m_player.FightBuffs)
            {
                if ((BuffType)fightBuff.Type == buff)
                    return fightBuff;
            }
            return (BufferInfo)null;
        }

        public void SendAttackInformation()
        {
            if (!this.EffectTrigger || !this.AttackInformation)
                return;
            this.Game.SendMessage(this.PlayerDetail, LanguageMgr.GetTranslation("PlayerEquipEffect.Success"), LanguageMgr.GetTranslation("PlayerEquipEffect.Success1", (object)this.PlayerDetail.PlayerCharacter.NickName), 3);
            this.EffectTrigger = false;
            this.AttackInformation = false;
        }

        public Point StartFalling(bool direct)
        {
            return this.StartFalling(direct, 0, this.MOVE_SPEED * 10);
        }

        public virtual Point StartFalling(bool direct, int delay, int speed)
        {
            Point p = this.m_map.FindYLineNotEmptyPointDown(this.X, this.Y);
            if (p == Point.Empty)
                p = new Point(this.X, this.m_game.Map.Bound.Height + 1);
            if (p.Y == this.Y)
                return Point.Empty;
            if (direct)
            {
                this.SetXY(p);
                if (this.m_map.IsOutMap(p.X, p.Y))
                    base.Die();
                return p;
            }
            this.m_game.AddAction((IAction)new LivingFallingAction((Living)this, p.X, p.Y, speed, (string)null, delay, 0, (LivingCallBack)null));
            return p;
        }

        public void StartGhostMoving()
        {
            if (this.TargetPoint.IsEmpty)
                return;
            Point point = new Point(this.TargetPoint.X - this.X, this.TargetPoint.Y - this.Y);
            if (point.Length() > 160.0)
                point.Normalize(160);
            this.m_game.AddAction((IAction)new GhostMoveAction(this, new Point(this.X + point.X, this.Y + point.Y)));
        }

        public override void StartMoving()
        {
            if (this.m_map == null)
                return;
            Point notEmptyPointDown = this.m_map.FindYLineNotEmptyPointDown(this.m_x, this.m_y);
            if (notEmptyPointDown.IsEmpty)
            {
                if (this.m_map.Ground != null)
                    this.m_y = this.m_map.Ground.Height;
            }
            else
            {
                this.m_x = notEmptyPointDown.X;
                this.m_y = notEmptyPointDown.Y;
            }
            if (!notEmptyPointDown.IsEmpty)
                return;
            this.m_syncAtTime = false;
            this.Die();
        }

        public override void StartMoving(int delay, int speed)
        {
            if (this.m_map == null)
                return;
            Point notEmptyPointDown = this.m_map.FindYLineNotEmptyPointDown(this.m_x, this.m_y);
            if (notEmptyPointDown.IsEmpty)
            {
                this.m_y = this.m_map.Ground.Height;
            }
            else
            {
                this.m_x = notEmptyPointDown.X;
                this.m_y = notEmptyPointDown.Y;
            }
            base.StartMoving(delay, speed);
            if (!notEmptyPointDown.IsEmpty)
                return;
            this.m_syncAtTime = false;
            this.Die();
        }

        public void StartRotate(int rotation, int speed, string endPlay, int delay)
        {
            this.m_game.AddAction((IAction)new LivingRotateTurnAction(this, rotation, speed, endPlay, delay));
        }

        public void StartSpeedMoving(int x, int y, int delay)
        {
            Point point = new Point(x - this.X, y - this.Y);
            this.m_game.AddAction((IAction)new PlayerMoveAction(this, new Point(this.X + point.X, this.Y + point.Y), delay));
        }

        public void StartSpeedMult(int x, int y)
        {
            this.StartSpeedMult(x, y, 3000);
        }

        public void StartSpeedMult(int x, int y, int delay)
        {
            Point point = new Point(x - this.X, y - this.Y);
            this.m_game.AddAction((IAction)new PlayerSpeedMultAction(this, new Point(this.X + point.X, this.Y + point.Y), delay));
        }

        public override bool TakeDamage(
          Living source,
          ref int damageAmount,
          ref int criticalAmount,
          string msg,
          int delay)
        {
            if ((source == this || source.Team == this.Team) && damageAmount + criticalAmount >= this.m_blood)
            {
                damageAmount = this.m_blood - 1;
                criticalAmount = 0;
            }
            bool damage = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg, delay);
            if (this.IsLiving)
            {
                this.AddDander((damageAmount * 2 / 5 + 5) / 2);
                if (!this.Game.IsSpecialPVE() && this.Blood < this.MaxBlood / 100 * 30)
                {
                    BufferInfo fightBuffByType = this.GetFightBuffByType(BuffType.Save_Life);
                    if (fightBuffByType != null && this.m_player.UsePayBuff(BuffType.Save_Life))
                    {
                        int num = this.MaxBlood / 100 * fightBuffByType.Value;
                        this.AddBlood(num);
                        this.m_game.method_53((Living)this, LanguageMgr.GetTranslation("GameServer.PayBuff.ReLife.UseNotice", (object)this.PlayerDetail.PlayerCharacter.NickName, (object)num));
                    }
                }
            }
            return damage;
        }

        public void UseFlySkill()
        {
            if (!this.CanFly)
                return;
            this.m_game.SendPlayerUseProp(this, -2, -2, Player.CARRY_TEMPLATE_ID);
            this.SetBall(3);
        }

        public bool UseItem(ItemTemplateInfo item)
        {
            if (!this.CanUseItem(item))
                return false;
            this.m_energy -= item.Property4;
            this.m_delay += item.Property5;
            this.m_game.SendPlayerUseProp((Living)this, -2, -2, item.TemplateID, this);
            SpellMgr.ExecuteSpell(this.m_game, this.m_game.CurrentLiving as Player, item);
            return true;
        }

        public bool UseItem(ItemTemplateInfo item, int place)
        {
            if (!this.CanUseItem(item, place))
                return false;
            if (this.IsLiving)
            {
                this.ReduceEnergy(item.Property4);
                this.AddDelay(item.Property5);
            }
            else if (place == -1)
            {
                this.psychic -= item.Property7;
                this.Game.CurrentLiving.AddDelay(item.Property5);
            }
            this.m_game.method_39(this, -2, -2, item.TemplateID);
            SpellMgr.ExecuteSpell(this.m_game, this.m_game.CurrentLiving as Player, item);
            if (item.Property6 == 1 && this.IsAttacking)
            {
                this.StopAttacking();
                this.m_game.CheckState(0);
            }
            return true;
        }

        public void UseSecondWeapon()
        {
            if (!this.CanUseItem(this.m_DeputyWeapon.Template))
                return;
            if (this.m_DeputyWeapon.Template.Property3 == 31)
            {
                new AddGuardEquipEffect((int)this.getHertAddition(this.m_DeputyWeapon), 1).Start((Living)this);
                this.OnPlayerGuard();
            }
            else
            {
                this.SetCurrentWeapon(this.m_DeputyWeapon);
                this.OnPlayerCure();
            }
            this.ShootCount = 1;
            this.m_energy -= this.m_DeputyWeapon.Template.Property4;
            this.m_delay += this.m_DeputyWeapon.Template.Property5;
            this.m_game.SendPlayerUseProp(this, -2, -2, this.m_DeputyWeapon.Template.TemplateID);
            if (this.deputyWeaponResCount <= 0)
                return;
            --this.deputyWeaponResCount;
            this.m_game.SendUseDeputyWeapon(this, this.deputyWeaponResCount);
        }

        public void UseSpecialSkill()
        {
            if (this.Dander < 200)
                return;
            this.SetBall(this.m_spBallId, true);
            this.m_ballCount = this.m_currentBall.Amount;
            this.SetDander(0);
        }

        public double SpeedMult
        {
            get
            {
                return this.speedMultiplier;
            }
            set
            {
                this.speedMultiplier = value / (double)this.STEP_X;
            }
        }

        public int StepX
        {
            get
            {
                return (int)((double)this.STEP_X * this.speedMultiplier);
            }
        }

        public int StepY
        {
            get
            {
                return (int)((double)this.STEP_Y * this.speedMultiplier);
            }
        }

        public override void SpeedMultX(int value)
        {
            this.SpeedMult = (double)value;
            this.MOVE_SPEED = value - 1;
            base.SpeedMultX(value);
        }

        public bool canMoveDirection(int dir)
        {
            return !this.m_map.IsOutMap(this.X + (15 + this.MOVE_SPEED) * dir, this.Y);
        }

        public Point getNextWalkPoint(int dir)
        {
            if (this.canMoveDirection(dir))
                return this.m_map.FindNextWalkPoint(this.X, this.Y, dir, this.StepX, this.StepY);
            return Point.Empty;
        }

        public Point FindYLineNotEmptyPointDown(int tx, int ty)
        {
            Rectangle bound = this.m_map.Bound;
            return this.m_map.FindYLineNotEmptyPointDown(tx, ty, this.m_map.Bound.Height);
        }

        public void OnBeforeBomb(int delay)
        {
            if (this.BeforeBomb == null)
                return;
            this.BeforeBomb(this);
        }

        public int BallCount
        {
            get
            {
                return this.m_ballCount;
            }
            set
            {
                if (this.m_ballCount == value)
                    return;
                this.m_ballCount = value;
            }
        }

        public bool CanGetProp
        {
            get
            {
                return this.m_canGetProp;
            }
            set
            {
                if (this.m_canGetProp == value)
                    return;
                this.m_canGetProp = value;
            }
        }

        public BallInfo CurrentBall
        {
            get
            {
                return this.m_currentBall;
            }
        }

        public int ChangeSpecialBall
        {
            get
            {
                return this.m_changeSpecialball;
            }
            set
            {
                this.m_changeSpecialball = value;
            }
        }

        public SqlDataProvider.Data.ItemInfo DeputyWeapon
        {
            get
            {
                return this.m_DeputyWeapon;
            }
            set
            {
                this.m_DeputyWeapon = value;
            }
        }

        public int deputyWeaponCount
        {
            get
            {
                return this.deputyWeaponResCount;
            }
        }

        public int Energy
        {
            get
            {
                return this.m_energy;
            }
            set
            {
                this.m_energy = value;
            }
        }

        public int flyCount
        {
            get
            {
                return this.m_flyCoolDown;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.m_isActive;
            }
        }

        public bool IsSpecialSkill
        {
            get
            {
                return this.m_currentBall.ID == this.m_spBallId;
            }
        }

        public int LoadingProcess
        {
            get
            {
                return this.m_loadingProcess;
            }
            set
            {
                if (this.m_loadingProcess == value)
                    return;
                this.m_loadingProcess = value;
                if (this.m_loadingProcess < 100)
                    return;
                this.OnLoadingCompleted();
            }
        }

        public int KilledPunishmentOffer
        {
            get
            {
                return this.m_killedPunishmentOffer;
            }
            set
            {
                this.m_killedPunishmentOffer = value;
            }
        }

        public int OldX
        {
            get
            {
                return this.m_oldx;
            }
            set
            {
                this.m_oldx = value;
            }
        }

        public int OldY
        {
            get
            {
                return this.m_oldy;
            }
            set
            {
                this.m_oldy = value;
            }
        }

        public IGamePlayer PlayerDetail
        {
            get
            {
                return this.m_player;
            }
        }

        public int Prop
        {
            get
            {
                return this.m_prop;
            }
            set
            {
                this.m_prop = value;
            }
        }

        public int ShootCount
        {
            get
            {
                return this.m_shootCount;
            }
            set
            {
                if (this.m_shootCount == value)
                    return;
                this.m_shootCount = value;
                this.m_game.SendGameUpdateShootCount(this);
            }
        }

        public SqlDataProvider.Data.ItemInfo Weapon
        {
            get
            {
                return this.m_weapon;
            }
        }

        public UsersPetInfo Pet
        {
            get
            {
                return this.userPetInfo;
            }
        }
    }
}
