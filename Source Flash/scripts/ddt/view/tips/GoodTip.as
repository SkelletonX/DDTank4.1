package ddt.view.tips
{
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.ui.image.Image;
   import com.pickgliss.ui.image.MovieImage;
   import com.pickgliss.ui.image.ScaleFrameImage;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.ui.tip.BaseTip;
   import com.pickgliss.ui.tip.ITip;
   import ddt.data.EquipType;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.data.goods.ItemTemplateInfo;
   import ddt.data.goods.QualityType;
   import ddt.manager.ItemManager;
   import ddt.manager.LanguageMgr;
   import ddt.manager.PathManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.TimeManager;
   import ddt.utils.PositionUtils;
   import ddt.utils.StaticFormula;
   import ddt.view.SimpleItem;
   import flash.display.Bitmap;
   import flash.display.DisplayObject;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import road7th.utils.DateUtils;
   import road7th.utils.StringHelper;
   
   public class GoodTip extends BaseTip implements Disposeable, ITip
   {
      
      public static const BOUND:uint = 1;
      
      public static const UNBOUND:uint = 2;
      
      public static const ITEM_NORMAL_COLOR:uint = 16777215;
      
      public static const ITEM_NECKLACE_COLOR:uint = 16750899;
      
      public static const ITEM_PROPERTIES_COLOR:uint = 16750899;
      
      public static const ITEM_HOLES_COLOR:uint = 16777215;
      
      public static const ITEM_HOLE_RESERVE_COLOR:uint = 16776960;
      
      public static const ITEM_HOLE_GREY_COLOR:uint = 6710886;
      
      public static const ITEM_FIGHT_PROP_CONSUME_COLOR:uint = 14520832;
      
      public static const ITEM_NEED_LEVEL_COLOR:uint = 13421772;
      
      public static const ITEM_NEED_LEVEL_FAILED_COLOR:uint = 16711680;
      
      public static const ITEM_UPGRADE_TYPE_COLOR:uint = 10092339;
      
      public static const ITEM_NEED_SEX_COLOR:uint = 10092339;
      
      public static const ITEM_NEED_SEX_FAILED_COLOR:uint = 16711680;
      
      public static const ITEM_ETERNAL_COLOR:uint = 16776960;
      
      public static const ITEM_PAST_DUE_COLOR:uint = 16711680;
      
      private static const PET_SPECIAL_FOOD:int = 334100;
       
      
      private var _strengthenLevelImage:MovieImage;
      
      private var _fusionLevelImage:MovieImage;
      
      private var _boundImage:ScaleFrameImage;
      
      private var _nameTxt:FilterFrameText;
      
      private var _qualityItem:SimpleItem;
      
      private var _typeItem:SimpleItem;
      
      private var _mainPropertyItem:SimpleItem;
      
      private var _armAngleItem:SimpleItem;
      
      private var _otherHp:SimpleItem;
      
      private var _necklaceItem:FilterFrameText;
      
      private var _attackTxt:FilterFrameText;
      
      private var _defenseTxt:FilterFrameText;
      
      private var _agilityTxt:FilterFrameText;
      
      private var _luckTxt:FilterFrameText;
      
      private var _needLevelTxt:FilterFrameText;
      
      private var _needSexTxt:FilterFrameText;
      
      private var _holes:Vector.<FilterFrameText>;
      
      private var _upgradeType:FilterFrameText;
      
      private var _descriptionTxt:FilterFrameText;
      
      private var _bindTypeTxt:FilterFrameText;
      
      private var _remainTimeTxt:FilterFrameText;
      
      private var _goldRemainTimeTxt:FilterFrameText;
      
      private var _fightPropConsumeTxt:FilterFrameText;
      
      private var _boxTimeTxt:FilterFrameText;
      
      private var _info:ItemTemplateInfo;
      
      private var _bindImageOriginalPos:Point;
      
      private var _maxWidth:int;
      
      private var _minWidth:int = 196;
      
      private var _isArmed:Boolean;
      
      private var _displayList:Vector.<DisplayObject>;
      
      private var _displayIdx:int;
      
      private var _lines:Vector.<Image>;
      
      private var _lineIdx:int;
      
      private var _isReAdd:Boolean;
      
      private var _remainTimeBg:Bitmap;
      
      private var _gp:FilterFrameText;
      
      private var _maxGP:FilterFrameText;
      
      private var _levelTxt:FilterFrameText;
      
      public function GoodTip()
      {
         this._holes = new Vector.<FilterFrameText>();
         super();
      }
      
      override protected function init() : void
      {
         this._lines = new Vector.<Image>();
         this._displayList = new Vector.<DisplayObject>();
         _tipbackgound = ComponentFactory.Instance.creat("core.GoodsTipBg");
         this._strengthenLevelImage = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemNameMc");
         this._fusionLevelImage = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTrinketLevelMc");
         this._boundImage = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.BoundImage");
         this._bindImageOriginalPos = new Point(this._boundImage.x,this._boundImage.y);
         this._nameTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemNameTxt");
         this._qualityItem = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.QualityItem");
         this._typeItem = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.TypeItem");
         this._mainPropertyItem = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.MainPropertyItem");
         this._armAngleItem = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.armAngleItem");
         this._otherHp = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.otherHp");
         this._necklaceItem = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._attackTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._defenseTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._agilityTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._luckTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._levelTxt = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.LimitGradeTxt");
         this._gp = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._maxGP = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._holes = new Vector.<FilterFrameText>();
         var _loc1_:int = 0;
         while(_loc1_ < 6)
         {
            this._holes.push(ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt"));
            _loc1_++;
         }
         this._needLevelTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._needSexTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._upgradeType = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._descriptionTxt = ComponentFactory.Instance.creatComponentByStylename("core.goodTip.DescriptionTxt");
         this._bindTypeTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._remainTimeTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemDateTxt");
         this._goldRemainTimeTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipGoldItemDateTxt");
         this._remainTimeBg = ComponentFactory.Instance.creatBitmap("asset.core.tip.restTime");
         this._fightPropConsumeTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
         this._boxTimeTxt = ComponentFactory.Instance.creatComponentByStylename("core.GoodsTipItemTxt");
      }
      
      override public function get tipData() : Object
      {
         return _tipData;
      }
      
      override public function set tipData(param1:Object) : void
      {
         if(param1)
         {
            _tipData = param1 as GoodTipInfo;
            this.showTip(_tipData.itemInfo,_tipData.typeIsSecond);
            visible = true;
         }
         else
         {
            _tipData = null;
            visible = false;
         }
      }
      
      public function showTip(param1:ItemTemplateInfo, param2:Boolean = false) : void
      {
         this._displayIdx = 0;
         this._displayList = new Vector.<DisplayObject>();
         this._lineIdx = 0;
         this._isReAdd = false;
         this._maxWidth = 0;
         this._info = param1;
         this.updateView();
      }
      
      private function updateView() : void
      {
         if(this._info == null)
         {
            return;
         }
         this.clear();
         this._isArmed = false;
         this.createItemName();
         this.createQualityItem();
         this.createCategoryItem();
         this.createMainProperty();
         this.seperateLine();
         this.createNecklaceItem();
         this.createProperties();
         this.creatLevel();
         this.seperateLine();
         this.createHoleItem();
         this.createOperationItem();
         this.seperateLine();
         this.createDescript();
         this.createBindType();
         this.createRemainTime();
         this.createGoldRemainTime();
         this.createFightPropConsume();
         this.createBoxTimeItem();
         this.addChildren();
         this.createStrenthLevel();
      }
      
      private function clear() : void
      {
         var _loc1_:DisplayObject = null;
         while(numChildren > 0)
         {
            _loc1_ = getChildAt(0) as DisplayObject;
            if(_loc1_.parent)
            {
               _loc1_.parent.removeChild(_loc1_);
            }
         }
      }
      
      override protected function addChildren() : void
      {
         var _loc4_:DisplayObject = null;
         var _loc1_:int = this._displayList.length;
         var _loc2_:Point = new Point(4,4);
         var _loc3_:int = 6;
         var _loc5_:int = this._maxWidth;
         var _loc6_:int = 0;
         while(_loc6_ < _loc1_)
         {
            _loc4_ = this._displayList[_loc6_] as DisplayObject;
            if(this._lines.indexOf(_loc4_) < 0 && _loc4_ != this._descriptionTxt)
            {
               _loc5_ = Math.max(_loc4_.width,_loc5_);
            }
            PositionUtils.setPos(_loc4_,_loc2_);
            addChild(_loc4_);
            _loc2_.y = _loc4_.y + _loc4_.height + _loc3_;
            _loc6_++;
         }
         this._maxWidth = Math.max(this._minWidth,_loc5_);
         if(this._descriptionTxt.width != this._maxWidth)
         {
            this._descriptionTxt.width = this._maxWidth;
            this._descriptionTxt.height = this._descriptionTxt.textHeight + 10;
            this.addChildren();
            return;
         }
         if(!this._isReAdd)
         {
            _loc6_ = 0;
            while(_loc6_ < this._lines.length)
            {
               this._lines[_loc6_].width = this._maxWidth;
               if(_loc6_ + 1 < this._lines.length && this._lines[_loc6_ + 1].parent != null && Math.abs(this._lines[_loc6_ + 1].y - this._lines[_loc6_].y) <= 10)
               {
                  this._displayList.splice(this._displayList.indexOf(this._lines[_loc6_ + 1]),1);
                  this._lines[_loc6_ + 1].parent.removeChild(this._lines[_loc6_ + 1]);
                  this._isReAdd = true;
               }
               _loc6_++;
            }
            if(this._isReAdd)
            {
               this.addChildren();
               return;
            }
         }
         if(_loc1_ > 0)
         {
            _width = _tipbackgound.width = this._maxWidth + 8;
            _height = _tipbackgound.height = _loc4_.y + _loc4_.height + 8;
         }
         if(_tipbackgound)
         {
            addChildAt(_tipbackgound,0);
         }
         if(this._remainTimeBg.parent)
         {
            this._remainTimeBg.x = this._remainTimeTxt.x + 2;
            this._remainTimeBg.y = this._remainTimeTxt.y + 2;
            this._remainTimeBg.parent.addChildAt(this._remainTimeBg,1);
         }
         if(this._remainTimeBg.parent)
         {
            this._goldRemainTimeTxt.x = this._remainTimeTxt.x + 2;
            this._goldRemainTimeTxt.y = this._remainTimeTxt.y + 22;
            this._remainTimeBg.parent.addChildAt(this._goldRemainTimeTxt,1);
         }
      }
      
      private function createItemName() : void
      {
         var _loc3_:TextFormat = null;
         this._nameTxt.text = String(this._info.Name);
         var _loc1_:InventoryItemInfo = this._info as InventoryItemInfo;
         if(_loc1_ && _loc1_.StrengthenLevel > 0)
         {
            if(_loc1_.isGold)
            {
               if(_loc1_.StrengthenLevel > PathManager.solveStrengthMax())
               {
                  this._nameTxt.text = this._nameTxt.text + LanguageMgr.GetTranslation("store.view.exalt.goodTips",_loc1_.StrengthenLevel - 12);
               }
               else
               {
                  this._nameTxt.text = this._nameTxt.text + LanguageMgr.GetTranslation("wishBead.StrengthenLevel");
               }
            }
            else if(_loc1_.StrengthenLevel <= PathManager.solveStrengthMax())
            {
               this._nameTxt.text = this._nameTxt.text + ("(+" + (this._info as InventoryItemInfo).StrengthenLevel + ")");
            }
            else if(_loc1_.StrengthenLevel > PathManager.solveStrengthMax())
            {
               this._nameTxt.text = this._nameTxt.text + LanguageMgr.GetTranslation("store.view.exalt.goodTips",_loc1_.StrengthenLevel - 12);
            }
         }
         var _loc2_:int = this._nameTxt.text.indexOf("+");
         if(_loc2_ > 0)
         {
            _loc3_ = ComponentFactory.Instance.model.getSet("core.goodTip.ItemNameNumTxtFormat");
            this._nameTxt.setTextFormat(_loc3_,_loc2_,_loc2_ + 1);
         }
         this._nameTxt.textColor = QualityType.QUALITY_COLOR[this._info.Quality];
         var _loc4_:* = this._displayIdx++;
         this._displayList[_loc4_] = this._nameTxt;
      }
      
      private function createQualityItem() : void
      {
         var _loc1_:FilterFrameText = this._qualityItem.foreItems[0] as FilterFrameText;
         _loc1_.text = QualityType.QUALITY_STRING[this._info.Quality];
         _loc1_.textColor = QualityType.QUALITY_COLOR[this._info.Quality];
         var _loc2_:* = this._displayIdx++;
         this._displayList[_loc2_] = this._qualityItem;
      }
      
      private function createCategoryItem() : void
      {
         var _loc1_:FilterFrameText = this._typeItem.foreItems[0] as FilterFrameText;
         var _loc2_:Array = EquipType.PARTNAME;
         _loc1_.text = EquipType.PARTNAME[this._info.CategoryID];
         var _loc3_:* = this._displayIdx++;
         this._displayList[_loc3_] = this._typeItem;
      }
      
      private function createMainProperty() : void
      {
         var _loc1_:String = "";
         var _loc2_:int = 0;
         var _loc3_:FilterFrameText = this._mainPropertyItem.foreItems[0] as FilterFrameText;
         var _loc4_:ScaleFrameImage = this._mainPropertyItem.backItem as ScaleFrameImage;
         var _loc5_:InventoryItemInfo = this._info as InventoryItemInfo;
         if(EquipType.isArm(this._info))
         {
            if(_loc5_ && _loc5_.StrengthenLevel > 0)
            {
               _loc2_ = !!_loc5_.isGold?int(_loc5_.StrengthenLevel + 1):int(_loc5_.StrengthenLevel);
               _loc1_ = "(+" + StaticFormula.getHertAddition(int(_loc5_.Property7),_loc2_) + ")";
            }
            _loc4_.setFrame(1);
            _loc3_.text = " " + this._info.Property7.toString() + _loc1_;
            FilterFrameText(this._armAngleItem.foreItems[0]).text = " " + this._info.Property5 + "°~" + this._info.Property6 + "°";
            this._displayList[this._displayIdx++] = this._mainPropertyItem;
            this._displayList[this._displayIdx++] = this._armAngleItem;
         }
         else if(EquipType.isHead(this._info) || EquipType.isCloth(this._info))
         {
            if(_loc5_ && _loc5_.StrengthenLevel > 0)
            {
               _loc2_ = !!_loc5_.isGold?int(_loc5_.StrengthenLevel + 1):int(_loc5_.StrengthenLevel);
               _loc1_ = "(+" + StaticFormula.getDefenseAddition(int(_loc5_.Property7),_loc2_) + ")";
            }
            _loc4_.setFrame(2);
            _loc3_.text = " " + this._info.Property7.toString() + _loc1_;
            this._displayList[_loc6_] = this._mainPropertyItem;
            if(_loc5_ && _loc5_.isGold)
            {
               FilterFrameText(this._otherHp.foreItems[0]).text = _loc5_.Boold.toString();
               this._displayList[this._displayIdx++] = this._otherHp;
            }
         }
         else if(StaticFormula.isDeputyWeapon(this._info))
         {
            if(this._info.Property3 == "32")
            {
               if(_loc5_ && _loc5_.StrengthenLevel > 0)
               {
                  _loc2_ = !!_loc5_.isGold?int(_loc5_.StrengthenLevel + 1):int(_loc5_.StrengthenLevel);
                  _loc1_ = "(+" + StaticFormula.getRecoverHPAddition(int(_loc5_.Property7),_loc2_) + ")";
               }
               _loc4_.setFrame(3);
            }
            else
            {
               if(_loc5_ && _loc5_.StrengthenLevel > 0)
               {
                  _loc2_ = !!_loc5_.isGold?int(_loc5_.StrengthenLevel + 1):int(_loc5_.StrengthenLevel);
                  _loc1_ = "(+" + StaticFormula.getDefenseAddition(int(_loc5_.Property7),_loc2_) + ")";
               }
               _loc4_.setFrame(4);
            }
            _loc3_.text = " " + this._info.Property7.toString() + _loc1_;
            this._displayList[this._displayIdx++] = this._mainPropertyItem;
         }
      }
      
      private function createNecklaceItem() : void
      {
         if(this._info.CategoryID == 14)
         {
            this._necklaceItem.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.life") + ":" + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.advance") + this._info.Property1 + "%";
            this._necklaceItem.textColor = ITEM_NECKLACE_COLOR;
            this._displayList[this._displayIdx++] = this._necklaceItem;
         }
         else if(this._info.CategoryID == EquipType.HEALSTONE)
         {
            this._necklaceItem.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.life") + ":" + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.reply") + this._info.Property2;
            this._necklaceItem.textColor = ITEM_NECKLACE_COLOR;
            this._displayList[this._displayIdx++] = this._necklaceItem;
         }
      }
      
      private function createProperties() : void
      {
         var _loc5_:InventoryItemInfo = null;
         var _loc1_:String = "";
         var _loc2_:String = "";
         var _loc3_:String = "";
         var _loc4_:String = "";
         if(this._info is InventoryItemInfo)
         {
            _loc5_ = this._info as InventoryItemInfo;
            if(_loc5_.AttackCompose > 0)
            {
               _loc1_ = "(+" + String(_loc5_.AttackCompose) + ")";
            }
            if(_loc5_.DefendCompose > 0)
            {
               _loc2_ = "(+" + String(_loc5_.DefendCompose) + ")";
            }
            if(_loc5_.AgilityCompose > 0)
            {
               _loc3_ = "(+" + String(_loc5_.AgilityCompose) + ")";
            }
            if(_loc5_.LuckCompose > 0)
            {
               _loc4_ = "(+" + String(_loc5_.LuckCompose) + ")";
            }
         }
         if(this._info.Attack != 0)
         {
            this._attackTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.fire") + ":" + String(this._info.Attack) + _loc1_;
            this._attackTxt.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._attackTxt;
         }
         if(this._info.Defence != 0)
         {
            this._defenseTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.recovery") + ":" + String(this._info.Defence) + _loc2_;
            this._defenseTxt.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._defenseTxt;
         }
         if(this._info.Agility != 0)
         {
            this._agilityTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.agility") + ":" + String(this._info.Agility) + _loc3_;
            this._agilityTxt.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._agilityTxt;
         }
         if(this._info.Luck != 0)
         {
            this._luckTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.lucky") + ":" + String(this._info.Luck) + _loc4_;
            this._luckTxt.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._luckTxt;
         }
         if(this._info.TemplateID == PET_SPECIAL_FOOD)
         {
            this._gp.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.gp") + ":" + InventoryItemInfo(this._info).DefendCompose;
            this._gp.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._gp;
            this._maxGP.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.maxGP") + ":" + InventoryItemInfo(this._info).AgilityCompose;
            this._maxGP.textColor = ITEM_PROPERTIES_COLOR;
            this._displayList[this._displayIdx++] = this._maxGP;
         }
      }
      
      private function createHoleItem() : void
      {
         var _loc1_:Array = null;
         var _loc2_:Array = null;
         var _loc3_:InventoryItemInfo = null;
         var _loc4_:int = 0;
         var _loc5_:String = null;
         var _loc6_:Array = null;
         var _loc7_:FilterFrameText = null;
         var _loc8_:int = 0;
         if(!StringHelper.isNullOrEmpty(this._info.Hole))
         {
            _loc1_ = [];
            _loc2_ = this._info.Hole.split("|");
            _loc3_ = this._info as InventoryItemInfo;
            if(_loc2_.length > 0 && String(_loc2_[0]) != "" && _loc3_ != null)
            {
               _loc4_ = 0;
               while(_loc4_ < _loc2_.length)
               {
                  _loc5_ = String(_loc2_[_loc4_]);
                  _loc6_ = _loc5_.split(",");
                  if(_loc4_ < 4)
                  {
                     if(int(_loc6_[0]) > 0 && int(_loc6_[0]) - _loc3_.StrengthenLevel <= 3 || this.getHole(_loc3_,_loc4_ + 1) >= 0)
                     {
                        _loc8_ = int(_loc6_[0]);
                        _loc7_ = this.createSingleHole(_loc3_,_loc4_,_loc8_,_loc6_[1]);
                        this._displayList[this._displayIdx++] = _loc7_;
                     }
                  }
                  else if(_loc3_["Hole" + (_loc4_ + 1) + "Level"] >= 1 || _loc3_["Hole" + (_loc4_ + 1)] > 0)
                  {
                     _loc7_ = this.createSingleHole(_loc3_,_loc4_,int.MAX_VALUE,_loc6_[1]);
                     this._displayList[this._displayIdx++] = _loc7_;
                  }
                  _loc4_++;
               }
            }
         }
      }
      
      private function creatLevel() : void
      {
         if(this._info.CategoryID == 50 || this._info.CategoryID == 51 || this._info.CategoryID == 52)
         {
            this._levelTxt.text = LanguageMgr.GetTranslation("ddt.petEquipLevel",this._info.Property2);
            this._displayList[this._displayIdx++] = this._levelTxt;
         }
      }
      
      private function createSingleHole(param1:InventoryItemInfo, param2:int, param3:int, param4:int) : FilterFrameText
      {
         var _loc6_:ItemTemplateInfo = null;
         var _loc8_:int = 0;
         var _loc5_:FilterFrameText = this._holes[param2];
         var _loc7_:int = this.getHole(param1,param2 + 1);
         if(param1.StrengthenLevel >= param3)
         {
            if(_loc7_ <= 0)
            {
               _loc5_.text = this.getHoleType(param4) + ":" + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.holeenable");
               _loc5_.textColor = ITEM_HOLES_COLOR;
            }
            else
            {
               _loc6_ = ItemManager.Instance.getTemplateById(_loc7_);
               if(_loc6_)
               {
                  _loc5_.text = _loc6_.Data;
                  _loc5_.textColor = ITEM_HOLE_RESERVE_COLOR;
               }
            }
         }
         else if(param2 >= 4)
         {
            _loc8_ = param1["Hole" + (param2 + 1) + "Level"];
            if(_loc7_ > 0)
            {
               _loc6_ = ItemManager.Instance.getTemplateById(_loc7_);
               _loc5_.text = _loc6_.Data + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.holeLv",param1["Hole" + (param2 + 1) + "Level"]);
               if(Math.floor(_loc6_.Level + 1 >> 1) <= _loc8_)
               {
                  _loc5_.textColor = ITEM_HOLE_RESERVE_COLOR;
               }
               else
               {
                  _loc5_.textColor = ITEM_HOLE_GREY_COLOR;
               }
            }
            else
            {
               _loc5_.text = this.getHoleType(param4) + StringHelper.format(LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.holeLv",param1["Hole" + (param2 + 1) + "Level"]));
               _loc5_.textColor = ITEM_HOLES_COLOR;
            }
         }
         else if(_loc7_ <= 0)
         {
            _loc5_.text = this.getHoleType(param4) + StringHelper.format(LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.holerequire"),param3.toString());
            _loc5_.textColor = ITEM_HOLE_GREY_COLOR;
         }
         else
         {
            _loc6_ = ItemManager.Instance.getTemplateById(_loc7_);
            if(_loc6_)
            {
               _loc5_.text = _loc6_.Data + StringHelper.format(LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.holerequire"),param3.toString());
               _loc5_.textColor = ITEM_HOLE_GREY_COLOR;
            }
         }
         return _loc5_;
      }
      
      public function getHole(param1:InventoryItemInfo, param2:int) : int
      {
         return int(param1["Hole" + param2.toString()]);
      }
      
      private function getHoleType(param1:int) : String
      {
         switch(param1)
         {
            case 1:
               return LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.trianglehole");
            case 2:
               return LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.recthole");
            case 3:
               return LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.ciclehole");
            default:
               return LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.unknowhole");
         }
      }
      
      private function createOperationItem() : void
      {
         var _loc2_:uint = 0;
         if(this._info.NeedLevel > 1)
         {
            if(PlayerManager.Instance.Self.Grade >= this._info.NeedLevel)
            {
               _loc2_ = ITEM_NEED_LEVEL_COLOR;
            }
            else
            {
               _loc2_ = ITEM_NEED_LEVEL_FAILED_COLOR;
            }
            this._needLevelTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.need") + ":" + String(this._info.NeedLevel);
            this._needLevelTxt.textColor = _loc2_;
            this._displayList[this._displayIdx++] = this._needLevelTxt;
         }
         if(this._info.NeedSex == 1)
         {
            this._needSexTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.man");
            this._needSexTxt.textColor = !!PlayerManager.Instance.Self.Sex?uint(ITEM_NEED_SEX_COLOR):uint(ITEM_NEED_SEX_FAILED_COLOR);
            this._displayList[this._displayIdx++] = this._needSexTxt;
         }
         else if(this._info.NeedSex == 2)
         {
            this._needSexTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.woman");
            this._needSexTxt.textColor = !!PlayerManager.Instance.Self.Sex?uint(ITEM_NEED_SEX_FAILED_COLOR):uint(ITEM_NEED_SEX_COLOR);
            this._displayList[this._displayIdx++] = this._needSexTxt;
         }
         var _loc1_:String = "";
         if(this._info.CanStrengthen && this._info.CanCompose)
         {
            _loc1_ = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.may");
            if(EquipType.isRongLing(this._info))
            {
               _loc1_ = _loc1_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.melting");
            }
            this._upgradeType.text = _loc1_;
            this._upgradeType.textColor = ITEM_UPGRADE_TYPE_COLOR;
            this._displayList[this._displayIdx++] = this._upgradeType;
         }
         else if(this._info.CanCompose)
         {
            _loc1_ = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.compose");
            if(EquipType.isRongLing(this._info))
            {
               _loc1_ = _loc1_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.melting");
            }
            this._upgradeType.text = _loc1_;
            this._upgradeType.textColor = ITEM_UPGRADE_TYPE_COLOR;
            this._displayList[this._displayIdx++] = this._upgradeType;
         }
         else if(this._info.CanStrengthen)
         {
            _loc1_ = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.strong");
            if(EquipType.isRongLing(this._info))
            {
               _loc1_ = _loc1_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.melting");
            }
            this._upgradeType.text = _loc1_;
            this._upgradeType.textColor = ITEM_UPGRADE_TYPE_COLOR;
            this._displayList[this._displayIdx++] = this._upgradeType;
         }
         else if(EquipType.isRongLing(this._info))
         {
            _loc1_ = _loc1_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.canmelting");
            this._upgradeType.text = _loc1_;
            this._upgradeType.textColor = ITEM_UPGRADE_TYPE_COLOR;
            this._displayList[this._displayIdx++] = this._upgradeType;
         }
      }
      
      private function createDescript() : void
      {
         if(this._info.Description == "")
         {
            return;
         }
         this._descriptionTxt.text = this._info.Description;
         this._descriptionTxt.height = this._descriptionTxt.textHeight + 10;
         var _loc1_:* = this._displayIdx++;
         this._displayList[_loc1_] = this._descriptionTxt;
      }
      
      private function createBindType() : void
      {
         var _loc1_:InventoryItemInfo = this._info as InventoryItemInfo;
         if(_loc1_)
         {
            if(_loc1_.IsVisleBound == true)
            {
               this._boundImage.setFrame(!!_loc1_.IsBinds?int(BOUND):int(UNBOUND));
               PositionUtils.setPos(this._boundImage,this._bindImageOriginalPos);
               addChild(this._boundImage);
            }
            if(!_loc1_.IsBinds)
            {
               if(_loc1_.BindType == 3)
               {
                  this._bindTypeTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.bangding");
                  this._bindTypeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._bindTypeTxt;
               }
               else if(this._info.BindType == 2)
               {
                  this._bindTypeTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.zhuangbei");
                  this._bindTypeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._bindTypeTxt;
               }
               else if(this._info.BindType == 4)
               {
                  if(this._boundImage.parent)
                  {
                     this._boundImage.parent.removeChild(this._boundImage);
                  }
               }
            }
         }
         else if(this._boundImage.parent)
         {
            this._boundImage.parent.removeChild(this._boundImage);
         }
      }
      
      private function createRemainTime() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:InventoryItemInfo = null;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:String = null;
         var _loc6_:Number = NaN;
         if(this._remainTimeBg.parent)
         {
            this._remainTimeBg.parent.removeChild(this._remainTimeBg);
         }
         if(this._info is InventoryItemInfo)
         {
            _loc2_ = this._info as InventoryItemInfo;
            _loc3_ = _loc2_.getRemainDate();
            _loc4_ = _loc2_.getColorValidDate();
            _loc5_ = _loc2_.CategoryID == EquipType.ARM?LanguageMgr.GetTranslation("bag.changeColor.tips.armName"):"";
            if(_loc4_ > 0 && _loc4_ != int.MAX_VALUE)
            {
               if(_loc4_ >= 1)
               {
                  this._remainTimeTxt.text = (!!_loc2_.IsUsed?LanguageMgr.GetTranslation("bag.changeColor.tips.name") + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.less"):LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.time")) + Math.ceil(_loc4_) + LanguageMgr.GetTranslation("shop.ShopIIShoppingCarItem.day");
                  this._remainTimeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._remainTimeTxt;
               }
               else
               {
                  _loc6_ = Math.floor(_loc4_ * 24);
                  if(_loc6_ < 1)
                  {
                     _loc6_ = 1;
                  }
                  this._remainTimeTxt.text = (!!_loc2_.IsUsed?LanguageMgr.GetTranslation("bag.changeColor.tips.name") + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.less"):LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.time")) + _loc6_ + LanguageMgr.GetTranslation("hours");
                  this._remainTimeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._remainTimeTxt;
               }
            }
            if(_loc3_ == int.MAX_VALUE)
            {
               this._remainTimeTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.use");
               this._remainTimeTxt.textColor = ITEM_ETERNAL_COLOR;
               this._displayList[this._displayIdx++] = this._remainTimeTxt;
            }
            else if(_loc3_ > 0)
            {
               if(_loc3_ > 1)
               {
                  _loc1_ = Math.ceil(_loc3_);
                  this._remainTimeTxt.text = (!!_loc2_.IsUsed?_loc5_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.less"):LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.time")) + "  " + _loc1_ + "  " + LanguageMgr.GetTranslation("shop.ShopIIShoppingCarItem.day");
                  this._remainTimeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._remainTimeTxt;
               }
               else
               {
                  _loc1_ = Math.floor(_loc3_ * 24);
                  _loc1_ = _loc1_ < 1?Number(1):Number(_loc1_);
                  this._remainTimeTxt.text = (!!_loc2_.IsUsed?_loc5_ + LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.less"):LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.time")) + "  " + _loc1_ + "  " + LanguageMgr.GetTranslation("hours");
                  this._remainTimeTxt.textColor = ITEM_NORMAL_COLOR;
                  this._displayList[this._displayIdx++] = this._remainTimeTxt;
               }
               addChild(this._remainTimeBg);
            }
            else if(!isNaN(_loc3_))
            {
               this._remainTimeTxt.text = LanguageMgr.GetTranslation("tank.view.bagII.GoodsTipPanel.over");
               this._remainTimeTxt.textColor = ITEM_PAST_DUE_COLOR;
               this._displayList[this._displayIdx++] = this._remainTimeTxt;
            }
         }
      }
      
      private function createGoldRemainTime() : void
      {
         var _loc1_:Number = NaN;
         var _loc2_:InventoryItemInfo = null;
         var _loc3_:Number = NaN;
         var _loc4_:Number = NaN;
         var _loc5_:String = null;
         if(this._remainTimeBg.parent)
         {
            this._remainTimeBg.parent.removeChild(this._remainTimeBg);
         }
         if(this._info is InventoryItemInfo)
         {
            _loc2_ = this._info as InventoryItemInfo;
            _loc3_ = _loc2_.getGoldRemainDate();
            _loc4_ = _loc2_.goldValidDate;
            _loc5_ = _loc2_.goldBeginTime;
            if((this._info as InventoryItemInfo).isGold)
            {
               if(_loc3_ >= 1)
               {
                  _loc1_ = Math.ceil(_loc3_);
                  this._goldRemainTimeTxt.text = LanguageMgr.GetTranslation("wishBead.GoodsTipPanel.txt1") + _loc1_ + LanguageMgr.GetTranslation("wishBead.GoodsTipPanel.txt2");
               }
               else
               {
                  _loc1_ = Math.floor(_loc3_ * 24);
                  _loc1_ = _loc1_ < 1?Number(1):Number(_loc1_);
                  this._goldRemainTimeTxt.text = LanguageMgr.GetTranslation("wishBead.GoodsTipPanel.txt1") + _loc1_ + LanguageMgr.GetTranslation("wishBead.GoodsTipPanel.txt3");
               }
               addChild(this._remainTimeBg);
               this._goldRemainTimeTxt.textColor = ITEM_NORMAL_COLOR;
               this._displayList[this._displayIdx++] = this._goldRemainTimeTxt;
            }
         }
      }
      
      private function createFightPropConsume() : void
      {
         if(this._info.CategoryID == EquipType.FRIGHTPROP)
         {
            this._fightPropConsumeTxt.text = " " + LanguageMgr.GetTranslation("tank.view.common.RoomIIPropTip.consume") + this._info.Property4;
            this._fightPropConsumeTxt.textColor = ITEM_FIGHT_PROP_CONSUME_COLOR;
            this._displayList[this._displayIdx++] = this._fightPropConsumeTxt;
         }
      }
      
      private function createBoxTimeItem() : void
      {
         var _loc1_:Date = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         if(EquipType.isTimeBox(this._info))
         {
            _loc1_ = DateUtils.getDateByStr((this._info as InventoryItemInfo).BeginDate);
            _loc2_ = int(this._info.Property3) * 60 - (TimeManager.Instance.Now().getTime() - _loc1_.getTime()) / 1000;
            if(_loc2_ > 0)
            {
               _loc3_ = _loc2_ / 3600;
               _loc4_ = _loc2_ % 3600 / 60;
               _loc4_ = _loc4_ > 0?int(_loc4_):int(1);
               this._boxTimeTxt.text = LanguageMgr.GetTranslation("ddt.userGuild.boxTip",_loc3_,_loc4_);
               this._boxTimeTxt.textColor = ITEM_NORMAL_COLOR;
               this._displayList[this._displayIdx++] = this._boxTimeTxt;
            }
         }
      }
      
      private function createStrenthLevel() : void
      {
         var _loc1_:InventoryItemInfo = this._info as InventoryItemInfo;
         if(_loc1_ && _loc1_.StrengthenLevel > 0)
         {
            if(_loc1_.isGold)
            {
               this._strengthenLevelImage.setFrame(16);
            }
            else
            {
               this._strengthenLevelImage.setFrame(_loc1_.StrengthenLevel);
            }
            addChild(this._strengthenLevelImage);
            if(this._boundImage.parent)
            {
               this._boundImage.x = this._strengthenLevelImage.x + this._strengthenLevelImage.displayWidth / 2 - this._boundImage.width / 2;
               this._boundImage.y = this._lines[0].y + 4;
            }
            this._maxWidth = Math.max(this._strengthenLevelImage.x + this._strengthenLevelImage.displayWidth,this._maxWidth);
            _width = _tipbackgound.width = this._maxWidth + 8;
         }
      }
      
      private function seperateLine() : void
      {
         var _loc1_:Image = null;
         this._lineIdx++;
         if(this._lines.length < this._lineIdx)
         {
            _loc1_ = ComponentFactory.Instance.creatComponentByStylename("HRuleAsset");
            this._lines.push(_loc1_);
         }
         this._displayList[_loc2_] = this._lines[this._lineIdx - 1];
      }
   }
}
