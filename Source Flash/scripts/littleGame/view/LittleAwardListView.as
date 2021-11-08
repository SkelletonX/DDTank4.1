package littleGame.view
{
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.BaseButton;
   import com.pickgliss.ui.controls.ISelectable;
   import com.pickgliss.ui.core.Disposeable;
   import com.pickgliss.ui.image.MutipleImage;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.utils.DisplayUtils;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.ShopType;
   import ddt.data.goods.ShopItemInfo;
   import ddt.events.ItemEvent;
   import ddt.manager.ShopManager;
   import ddt.manager.SoundManager;
   import ddt.utils.PositionUtils;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import shop.view.ShopGoodItem;
   
   public class LittleAwardListView extends Sprite implements Disposeable
   {
      
      public static const AWARD_ITEM_NUM:uint = 8;
       
      
      private var _bg:MutipleImage;
      
      private var _goodItemContainerAll:Sprite;
      
      private var _goodItems:Vector.<AwardGoodItem>;
      
      private var _firstPage:BaseButton;
      
      private var _prePageBtn:BaseButton;
      
      private var _nextPageBtn:BaseButton;
      
      private var _endPageBtn:BaseButton;
      
      private var _currentPage:int;
      
      private var _currentPageTxt:FilterFrameText;
      
      private var _paginationBg:Bitmap;
      
      private var _noteDesc:Bitmap;
      
      private var _list:Vector.<ShopItemInfo>;
      
      public function LittleAwardListView()
      {
         super();
         this.initView();
         this.addEvent();
      }
      
      private function initView() : void
      {
         this._bg = ComponentFactory.Instance.creatComponentByStylename("littleGame.AwardListBg");
         addChild(this._bg);
         this._noteDesc = ComponentFactory.Instance.creat("asset.littleGame.ScoreNote");
         addChild(this._noteDesc);
         this._paginationBg = ComponentFactory.Instance.creat("asset.shop.RightViewBg1");
         PositionUtils.setPos(this._paginationBg,"littleGame.paginationBg.pos");
         addChild(this._paginationBg);
         this._firstPage = ComponentFactory.Instance.creat("littleGame.BtnFirstPage");
         this._prePageBtn = ComponentFactory.Instance.creat("littleGame.BtnPrePage");
         this._nextPageBtn = ComponentFactory.Instance.creat("littleGame.BtnNextPage");
         this._endPageBtn = ComponentFactory.Instance.creat("littleGame.BtnEndPage");
         this._currentPageTxt = ComponentFactory.Instance.creatComponentByStylename("littleGame.CurrentPage");
         this._goodItems = new Vector.<AwardGoodItem>();
         this._goodItemContainerAll = new Sprite();
         PositionUtils.setPos(this._goodItemContainerAll,"littleGame.goodItemContainer.pos");
         var _loc1_:int = 0;
         while(_loc1_ < AWARD_ITEM_NUM)
         {
            this._goodItems[_loc1_] = ComponentFactory.Instance.creatCustomObject("littleGame.GoodItem");
            this._goodItemContainerAll.addChild(this._goodItems[_loc1_]);
            this._goodItems[_loc1_].addEventListener(ItemEvent.ITEM_CLICK,this.__itemClick);
            this._goodItems[_loc1_].addEventListener(ItemEvent.ITEM_SELECT,this.__itemSelect);
            _loc1_++;
         }
         DisplayUtils.horizontalArrange(this._goodItemContainerAll,2,14);
         addChild(this._firstPage);
         addChild(this._prePageBtn);
         addChild(this._nextPageBtn);
         addChild(this._endPageBtn);
         addChild(this._currentPageTxt);
         addChild(this._goodItemContainerAll);
         this._currentPage = 1;
         this.loadList();
      }
      
      private function addEvent() : void
      {
         this._firstPage.addEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._prePageBtn.addEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._nextPageBtn.addEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._endPageBtn.addEventListener(MouseEvent.CLICK,this.__pageBtnClick);
      }
      
      private function removeEvent() : void
      {
         this._firstPage.removeEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._prePageBtn.removeEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._nextPageBtn.removeEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         this._endPageBtn.removeEventListener(MouseEvent.CLICK,this.__pageBtnClick);
         var _loc1_:uint = 0;
         while(_loc1_ < AWARD_ITEM_NUM)
         {
            this._goodItems[_loc1_].removeEventListener(ItemEvent.ITEM_CLICK,this.__itemClick);
            _loc1_++;
         }
      }
      
      public function loadList() : void
      {
         this.setList(ShopManager.Instance.getValidSortedGoodsByType(ShopType.LITTLEGAME_AWARD_TYPE,this._currentPage));
      }
      
      public function setList(param1:Vector.<ShopItemInfo>) : void
      {
         this._list = param1;
         this.clearitems();
         var _loc2_:int = 0;
         while(_loc2_ < AWARD_ITEM_NUM)
         {
            this._goodItems[_loc2_].selected = false;
            if(_loc2_ < param1.length && param1[_loc2_])
            {
               this._goodItems[_loc2_].shopItemInfo = param1[_loc2_];
            }
            _loc2_++;
         }
         this._currentPageTxt.text = this._currentPage + "/" + ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE);
      }
      
      private function clearitems() : void
      {
         var _loc1_:int = 0;
         while(_loc1_ < AWARD_ITEM_NUM)
         {
            this._goodItems[_loc1_].shopItemInfo = null;
            _loc1_++;
         }
      }
      
      private function __pageBtnClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
         if(ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE) == 0)
         {
            return;
         }
         switch(param1.currentTarget)
         {
            case this._firstPage:
               if(this._currentPage != 1)
               {
                  this._currentPage = 1;
               }
               break;
            case this._prePageBtn:
               if(this._currentPage == 1)
               {
                  this._currentPage = ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE) + 1;
               }
               this._currentPage--;
               break;
            case this._nextPageBtn:
               if(this._currentPage == ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE))
               {
                  this._currentPage = 0;
               }
               this._currentPage++;
               break;
            case this._endPageBtn:
               if(this._currentPage != ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE))
               {
                  this._currentPage = ShopManager.Instance.getResultPages(ShopType.LITTLEGAME_AWARD_TYPE);
               }
         }
         this.loadList();
      }
      
      private function __itemClick(param1:ItemEvent) : void
      {
      }
      
      private function __itemSelect(param1:ItemEvent) : void
      {
         var _loc3_:ISelectable = null;
         param1.stopImmediatePropagation();
         var _loc2_:ShopGoodItem = param1.currentTarget as ShopGoodItem;
         for each(_loc3_ in this._goodItems)
         {
            _loc3_.selected = false;
         }
         _loc2_.selected = true;
      }
      
      public function dispose() : void
      {
         this.removeEvent();
         ObjectUtils.disposeAllChildren(this);
         ObjectUtils.disposeObject(this._goodItemContainerAll);
         this._goodItemContainerAll = null;
         this._currentPageTxt.dispose();
         this._currentPageTxt = null;
         ObjectUtils.disposeObject(this._paginationBg);
         this._paginationBg = null;
         ObjectUtils.disposeObject(this._nextPageBtn);
         this._nextPageBtn = null;
         ObjectUtils.disposeObject(this._prePageBtn);
         this._prePageBtn = null;
         ObjectUtils.disposeObject(this._firstPage);
         this._firstPage = null;
         ObjectUtils.disposeObject(this._endPageBtn);
         this._endPageBtn = null;
         var _loc1_:int = 0;
         while(_loc1_ < AWARD_ITEM_NUM)
         {
            ObjectUtils.disposeObject(this._goodItems[_loc1_]);
            this._goodItems[_loc1_] = null;
            _loc1_++;
         }
         if(parent)
         {
            parent.removeChild(this);
         }
      }
   }
}
