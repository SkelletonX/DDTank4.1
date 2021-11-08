package littleGame.view
{
   import bagAndInfo.cell.CellFactory;
   import baglocked.BaglockedManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.SimpleBitmapButton;
   import com.pickgliss.ui.text.FilterFrameText;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.goods.ShopItemInfo;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import ddt.utils.PositionUtils;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.geom.Rectangle;
   import shop.view.ShopGoodItem;
   import shop.view.ShopItemCell;
   
   [Event(name="giving",type="ddt.events.ShopItemEvent")]
   [Event(name="collect",type="ddt.events.ShopItemEvent")]
   [Event(name="purchase",type="ddt.events.ShopItemEvent")]
   public class AwardGoodItem extends ShopGoodItem
   {
      
      private static const AwardItemCell_Size:int = 61;
       
      
      private var _scoreTitleField:FilterFrameText;
      
      private var _scoreField:FilterFrameText;
      
      private var _exchangeBtn:SimpleBitmapButton;
      
      public function AwardGoodItem()
      {
         super();
      }
      
      override public function dispose() : void
      {
         super.dispose();
         ObjectUtils.disposeObject(this._scoreTitleField);
         this._scoreTitleField = null;
         ObjectUtils.disposeObject(this._scoreField);
         this._scoreField = null;
         ObjectUtils.disposeObject(this._exchangeBtn);
         this._exchangeBtn = null;
      }
      
      override protected function initContent() : void
      {
         var _loc1_:Rectangle = null;
         super.initContent();
         this._exchangeBtn = ComponentFactory.Instance.creatComponentByStylename("core.shop.exchangeButton");
         addChild(this._exchangeBtn);
         _loc1_ = ComponentFactory.Instance.creatCustomObject("littleGame.GoodItemBG.size");
         _itemBg.width = _loc1_.width;
         _itemBg.height = _loc1_.height;
         _loc1_ = ComponentFactory.Instance.creatCustomObject("littleGame.GoodItemName.size");
         _itemNameTxt.x = _loc1_.x;
         _itemNameTxt.width = _loc1_.width;
         PositionUtils.setPos(_itemPriceTxt,"littleGame.GoodItemPrice.pos");
         PositionUtils.setPos(_payType,"littleGame.GoodPayTypeLabel.pos");
         PositionUtils.setPos(_payPaneBuyBtn,"littleGame.PayPaneBuyBtn.pos");
         PositionUtils.setPos(_payPaneGetBtn,"littleGame.PayPaneBuyBtn.pos");
         PositionUtils.setPos(_shopItemCellTypeBg,"littleGame.GoodItemCellTypeBg.pos");
         _payPaneGivingBtn.visible = false;
         _payPaneBuyBtn.visible = false;
         _payPaneGetBtn.visible = false;
         this._exchangeBtn.visible = false;
         this._scoreTitleField = ComponentFactory.Instance.creatComponentByStylename("littleGame.AwardScoreTitleField");
         this._scoreTitleField.text = LanguageMgr.GetTranslation("littlegame.AwardScore");
         addChild(this._scoreTitleField);
         this._scoreField = ComponentFactory.Instance.creatComponentByStylename("littleGame.AwardScoreField");
         addChild(this._scoreField);
         PositionUtils.setPos(itemCell,"littleGame.GoodItemCell.pos");
         itemCell.cellSize = AwardItemCell_Size;
      }
      
      override protected function creatItemCell() : ShopItemCell
      {
         var _loc1_:Sprite = new Sprite();
         _loc1_.graphics.beginFill(16777215,0);
         _loc1_.graphics.drawRect(0,0,AwardItemCell_Size,AwardItemCell_Size);
         _loc1_.graphics.endFill();
         return CellFactory.instance.createShopItemCell(_loc1_,null,true,true) as ShopItemCell;
      }
      
      override protected function addEvent() : void
      {
         super.addEvent();
         this._exchangeBtn.addEventListener(MouseEvent.CLICK,this.__payPanelClick);
      }
      
      override protected function removeEvent() : void
      {
         super.removeEvent();
         this._exchangeBtn.removeEventListener(MouseEvent.CLICK,this.__payPanelClick);
      }
      
      override protected function __payPanelClick(param1:MouseEvent) : void
      {
         if(_shopItemInfo == null)
         {
            return;
         }
         SoundManager.instance.play("008");
         if(PlayerManager.Instance.Self.bagLocked)
         {
            BaglockedManager.Instance.show();
            return;
         }
         if(PlayerManager.Instance.Self.Score < _shopItemInfo.getItemPrice(1).scoreValue)
         {
            MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.littlegame.scorelack"));
            return;
         }
         SocketManager.Instance.out.sendBuyGoods([_shopItemInfo.GoodsID],[1],[""],[""],[""]);
      }
      
      override public function set shopItemInfo(param1:ShopItemInfo) : void
      {
         super.shopItemInfo = param1;
         _payPaneGivingBtn.visible = false;
         _payPaneBuyBtn.visible = false;
         _payPaneGetBtn.visible = false;
         this._exchangeBtn.visible = param1 != null;
         if(param1)
         {
            this._scoreField.visible = true;
            this._scoreTitleField.visible = true;
            this._scoreField.text = String(param1.AValue1);
         }
         else
         {
            this._scoreField.visible = false;
            this._scoreTitleField.visible = false;
         }
      }
   }
}
