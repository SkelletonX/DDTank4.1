package bagAndInfo.info
{
   import com.pickgliss.events.ListItemEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.controls.ComboBox;
   import com.pickgliss.ui.controls.list.VectorListModel;
   import com.pickgliss.ui.core.Disposeable;
   import ddt.events.EffortEvent;
   import ddt.events.PlayerPropertyEvent;
   import ddt.manager.EffortManager;
   import ddt.manager.LanguageMgr;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.manager.SoundManager;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   
   public class PlayerInfoEffortHonorView extends Sprite implements Disposeable
   {
       
      
      private var _nameChoose:ComboBox;
      
      private var _honorArray:Array;
      
      public function PlayerInfoEffortHonorView()
      {
         super();
         this.init();
      }
      
      private function init() : void
      {
         this._nameChoose = ComponentFactory.Instance.creatComponentByStylename("personInfoViewNameChoose");
         addChild(this._nameChoose);
         EffortManager.Instance.addEventListener(EffortEvent.FINISH,this.__upadte);
         this._nameChoose.button.addEventListener(MouseEvent.CLICK,this.__buttonClick);
         PlayerManager.Instance.Self.addEventListener(PlayerPropertyEvent.PROPERTY_CHANGE,this.__propertyChange);
         this.setlist(EffortManager.Instance.getHonorArray());
         this.update();
      }
      
      private function __propertyChange(param1:PlayerPropertyEvent) : void
      {
         if(param1.changedProperties["honor"] == true)
         {
            if(PlayerManager.Instance.Self.honor != "")
            {
               this._nameChoose.textField.text = PlayerManager.Instance.Self.honor;
            }
            else
            {
               this._nameChoose.textField.text = LanguageMgr.GetTranslation("bagAndInfo.info.PlayerInfoEffortHonorView.selecting");
            }
         }
      }
      
      private function __buttonClick(param1:MouseEvent) : void
      {
         SoundManager.instance.play("008");
      }
      
      private function __upadte(param1:EffortEvent) : void
      {
         this.setlist(EffortManager.Instance.getHonorArray());
         this.update();
      }
      
      private function update() : void
      {
         this._nameChoose.beginChanges();
         this._nameChoose.selctedPropName = "text";
         var _loc1_:VectorListModel = this._nameChoose.listPanel.vectorListModel;
         _loc1_.clear();
         if(!this._honorArray)
         {
            return;
         }
         if(this._honorArray.length == 0)
         {
            _loc1_.append("");
         }
         var _loc2_:int = 0;
         while(_loc2_ < this._honorArray.length)
         {
            _loc1_.append(this._honorArray[_loc2_]);
            _loc2_++;
         }
         this._nameChoose.listPanel.list.updateListView();
         this._nameChoose.listPanel.list.addEventListener(ListItemEvent.LIST_ITEM_CLICK,this.__itemClick);
         this._nameChoose.commitChanges();
         if(PlayerManager.Instance.Self.honor != "")
         {
            this._nameChoose.textField.text = PlayerManager.Instance.Self.honor;
         }
         else
         {
            this._nameChoose.textField.text = LanguageMgr.GetTranslation("bagAndInfo.info.PlayerInfoEffortHonorView.selecting");
         }
      }
      
      private function __itemClick(param1:ListItemEvent) : void
      {
         SoundManager.instance.play("008");
         var _loc2_:String = this._honorArray[param1.index];
         if(_loc2_)
         {
            SocketManager.Instance.out.sendReworkRank(_loc2_);
         }
         else
         {
            SocketManager.Instance.out.sendReworkRank("");
            this._nameChoose.textField.text = LanguageMgr.GetTranslation("bagAndInfo.info.PlayerInfoEffortHonorView.selecting");
         }
      }
      
      public function setlist(param1:Array) : void
      {
         this._honorArray = [];
         this._honorArray = param1;
         if(!this._honorArray)
         {
            return;
         }
      }
      
      public function dispose() : void
      {
         this._nameChoose.button.removeEventListener(MouseEvent.CLICK,this.__buttonClick);
         EffortManager.Instance.removeEventListener(EffortEvent.FINISH,this.__upadte);
         PlayerManager.Instance.Self.removeEventListener(PlayerPropertyEvent.PROPERTY_CHANGE,this.__propertyChange);
         this._nameChoose.dispose();
         this._nameChoose = null;
         if(this.parent)
         {
            this.parent.removeChild(this);
         }
      }
   }
}
