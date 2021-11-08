package email.view
{
   import bagAndInfo.cell.BagCell;
   import bagAndInfo.cell.DragEffect;
   import bagAndInfo.cell.LinkedBagCell;
   import ddt.data.goods.InventoryItemInfo;
   import ddt.manager.DragManager;
   import ddt.manager.LanguageMgr;
   import ddt.manager.MessageTipManager;
   import ddt.manager.PlayerManager;
   import flash.events.MouseEvent;
   
   public class EmaillBagCell extends LinkedBagCell
   {
       
      
      public function EmaillBagCell()
      {
         super(null);
         _bg.alpha = 0;
      }
      
      override public function dragDrop(param1:DragEffect) : void
      {
         if(PlayerManager.Instance.Self.bagLocked)
         {
            return;
         }
         var _loc2_:InventoryItemInfo = param1.data as InventoryItemInfo;
         if(_loc2_ && param1.action == DragEffect.MOVE)
         {
            param1.action = DragEffect.NONE;
            if(_loc2_.IsBinds)
            {
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.view.emailII.EmaillIIBagCell.isBinds"));
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("此物品已经绑定"));
            }
            else if(_loc2_.getRemainDate() <= 0)
            {
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("tank.view.emailII.EmaillIIBagCell.RemainDate"));
               MessageTipManager.getInstance().show(LanguageMgr.GetTranslation("此物品已经过期"));
            }
            else
            {
               bagCell = param1.source as BagCell;
               param1.action = DragEffect.LINK;
            }
            DragManager.acceptDrag(this);
         }
      }
      
      override protected function onMouseOver(param1:MouseEvent) : void
      {
         buttonMode = true;
      }
      
      override public function dispose() : void
      {
         super.dispose();
      }
   }
}
