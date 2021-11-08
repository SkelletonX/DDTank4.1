package littleGame.character
{
   import ddt.data.EquipType;
   import ddt.data.player.PlayerInfo;
   import ddt.manager.ItemManager;
   import ddt.view.character.ILayer;
   import flash.display.BitmapData;
   import flash.display.BlendMode;
   import flash.utils.getTimer;
   
   public class LittleGameCharaterLoader
   {
      
      private static const HAIR_LAYER:int = 2;
      
      private static const EAR_LAYER:int = 3;
      
      private static var boyCloth:BitmapData;
      
      private static var girlCloth:BitmapData;
      
      private static var effect:BitmapData;
      
      private static var specialHeads:Vector.<BitmapData>;
       
      
      private var _playerInfo:PlayerInfo;
      
      private var _loaders:Vector.<LittleGameCharacterLayer>;
      
      private var _recordStyle:Array;
      
      private var _recordColor:Array;
      
      private var _head:BitmapData;
      
      private var _body:BitmapData;
      
      private var hasClothColor:Boolean = false;
      
      private var hasFaceColor:Boolean = false;
      
      private var _callBack:Function;
      
      public function LittleGameCharaterLoader(param1:PlayerInfo, param2:int = 1)
      {
         super();
         this._playerInfo = param1;
         this._loaders = new Vector.<LittleGameCharacterLayer>();
         this._recordStyle = this._playerInfo.Style.split(",");
         this._recordColor = this._playerInfo.Colors.split(",");
         this.hasFaceColor = Boolean(this._recordColor[5]);
         this.hasClothColor = Boolean(this._recordColor[4]);
         this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[4].split("|")[0])),this._recordColor[4],this._playerInfo.Sex,param2));
         this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[5].split("|")[0])),this._recordColor[5],this._playerInfo.Sex,param2));
         this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[2].split("|")[0])),this._recordColor[2],this._playerInfo.Sex,param2));
         this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[3].split("|")[0])),this._recordColor[3],this._playerInfo.Sex,param2));
         if(effect == null)
         {
            this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[5].split("|")[0])),this._recordColor[5],this._playerInfo.Sex,param2,EquipType.EFFECT,1));
         }
         if(specialHeads == null || this.hasFaceColor)
         {
            specialHeads = new Vector.<BitmapData>();
            this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[5].split("|")[0])),this._recordColor[5],this._playerInfo.Sex,param2,EquipType.FACE,1));
            this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[5].split("|")[0])),this._recordColor[5],this._playerInfo.Sex,param2,EquipType.FACE,2));
            this._loaders.push(new LittleGameCharacterLayer(ItemManager.Instance.getTemplateById(int(this._recordStyle[5].split("|")[0])),this._recordColor[5],this._playerInfo.Sex,param2,EquipType.FACE,3));
         }
      }
      
      public function load(param1:Function) : void
      {
         this._callBack = param1;
         var _loc2_:int = 0;
         while(_loc2_ < this._loaders.length)
         {
            this._loaders[_loc2_].load(this.onComplete);
            _loc2_++;
         }
      }
      
      private function onComplete(param1:ILayer) : void
      {
         var _loc4_:Number = NaN;
         var _loc2_:Boolean = true;
         var _loc3_:int = 0;
         while(_loc3_ < this._loaders.length)
         {
            if(!this._loaders[_loc3_].isComplete)
            {
               _loc2_ = false;
            }
            _loc3_++;
         }
         if(_loc2_)
         {
            _loc4_ = getTimer();
            this.drawCharacter();
            this.loadComplete();
         }
      }
      
      private function drawCharacter() : void
      {
         this._head = this.drawHeadByFace(1);
         if(this._playerInfo.Sex)
         {
            if(boyCloth)
            {
               if(!this.hasClothColor)
               {
                  this._body = boyCloth;
               }
               else
               {
                  this._body = new BitmapData(this._loaders[0].width,this._loaders[0].height,true,0);
                  this._body.draw(this._loaders[0],null,null,BlendMode.NORMAL);
               }
            }
            else
            {
               this._body = new BitmapData(this._loaders[0].width,this._loaders[0].height,true,0);
               this._body.draw(this._loaders[0],null,null,BlendMode.NORMAL);
               if(!this.hasClothColor)
               {
                  boyCloth = this._body;
               }
            }
         }
         else if(girlCloth)
         {
            if(!this.hasClothColor)
            {
               this._body = girlCloth;
            }
            else
            {
               this._body = new BitmapData(this._loaders[0].width,this._loaders[0].height,true,0);
               this._body.draw(this._loaders[0],null,null,BlendMode.NORMAL);
            }
         }
         else
         {
            this._body = new BitmapData(this._loaders[0].width,this._loaders[0].height,true,0);
            this._body.draw(this._loaders[0],null,null,BlendMode.NORMAL);
            if(!this.hasClothColor)
            {
               girlCloth = this._body;
            }
         }
         if(effect == null)
         {
            effect = new BitmapData(this._loaders[4].width,this._loaders[4].height,true,0);
            effect.draw(this._loaders[4],null,null,BlendMode.NORMAL);
         }
         if(specialHeads.length == 0)
         {
            if(!this.hasFaceColor)
            {
               specialHeads.push(this.drawHeadByFace(this._loaders.length - 3));
               specialHeads.push(this.drawHeadByFace(this._loaders.length - 2));
               specialHeads.push(this.drawHeadByFace(this._loaders.length - 1));
            }
         }
      }
      
      private function drawHeadByFace(param1:int) : BitmapData
      {
         var _loc2_:BitmapData = new BitmapData(this._loaders[param1].width,this._loaders[param1].height,true,0);
         var _loc3_:LittleGameCharacterLayer = this._loaders[param1];
         _loc2_.draw(_loc3_.getContent(),null,null,BlendMode.NORMAL);
         _loc3_ = this._loaders[LittleGameCharaterLoader.HAIR_LAYER];
         _loc2_.draw(_loc3_.getContent(),null,null,BlendMode.NORMAL);
         _loc3_ = this._loaders[LittleGameCharaterLoader.EAR_LAYER];
         _loc2_.draw(_loc3_.getContent(),null,null,BlendMode.NORMAL);
         return _loc2_;
      }
      
      public function getContent() : Vector.<BitmapData>
      {
         var _loc1_:Vector.<BitmapData> = new Vector.<BitmapData>();
         _loc1_.push(this._head);
         _loc1_.push(this._body);
         _loc1_.push(effect);
         if(this.hasFaceColor)
         {
            _loc1_.push(this.drawHeadByFace(this._loaders.length - 3));
            _loc1_.push(this.drawHeadByFace(this._loaders.length - 2));
            _loc1_.push(this.drawHeadByFace(this._loaders.length - 1));
         }
         else
         {
            _loc1_.push(specialHeads[0]);
            _loc1_.push(specialHeads[1]);
            _loc1_.push(specialHeads[2]);
         }
         return _loc1_;
      }
      
      private function loadComplete() : void
      {
         if(this._callBack != null)
         {
            this._callBack();
         }
      }
      
      public function dispose() : void
      {
         var _loc1_:LittleGameCharacterLayer = null;
         for each(_loc1_ in this._loaders)
         {
            _loc1_.dispose();
         }
         this._loaders == null;
         this._head = null;
         this._body = null;
         this._playerInfo = null;
      }
   }
}
