package email.view
{
   import ddt.data.goods.InventoryItemInfo;
   
   public class DiamondOfStrip extends DiamondBase
   {
       
      
      public function DiamondOfStrip()
      {
         super();
         countTxt.visible = false;
         diamondBg.visible = false;
      }
      
      override protected function update() : void
      {
         var _loc1_:* = undefined;
         _loc1_ = _info.getAnnexByIndex(index);
         if(_loc1_ && _loc1_ is String)
         {
            _cell.visible = false;
            centerMC.visible = true;
            mouseEnabled = true;
            if(_loc1_ == "gold")
            {
               centerMC.setFrame(3);
               countTxt.text = String(_info.Gold);
            }
            else if(_loc1_ == "money")
            {
               centerMC.setFrame(2);
               countTxt.text = String(_info.Money);
            }
            else if(_loc1_ == "gift")
            {
               centerMC.setFrame(6);
               countTxt.text = String(_info.GiftToken);
            }
            else if(_loc1_ == "medal")
            {
               centerMC.setFrame(7);
               countTxt.text = _info.Medal.toString();
            }
         }
         else if(_loc1_)
         {
            _cell.visible = true;
            centerMC.visible = false;
            _cell.info = _loc1_ as InventoryItemInfo;
            mouseEnabled = true;
         }
         else
         {
            centerMC.visible = true;
            _cell.visible = false;
            if(_info.IsRead)
            {
               centerMC.setFrame(5);
            }
            else
            {
               centerMC.setFrame(4);
            }
            mouseEnabled = false;
         }
      }
      
      override public function dispose() : void
      {
         super.dispose();
      }
   }
}
