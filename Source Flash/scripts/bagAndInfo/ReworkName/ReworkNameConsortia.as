package bagAndInfo.ReworkName
{
   import com.pickgliss.events.FrameEvent;
   import com.pickgliss.ui.AlertManager;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.LayerManager;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.utils.ObjectUtils;
   import ddt.data.analyze.ReworkNameAnalyzer;
   import ddt.manager.LanguageMgr;
   import ddt.manager.PlayerManager;
   import ddt.manager.SocketManager;
   import ddt.utils.FilterWordManager;
   import flash.events.Event;
   import road7th.utils.StringHelper;
   
   public class ReworkNameConsortia extends ReworkNameFrame
   {
       
      
      public function ReworkNameConsortia()
      {
         super();
         _path = "ConsortiaNameCheck.ashx";
         _nicknameDetail = LanguageMgr.GetTranslation("tank.view.ConsortiaReworkNameView.consortiaNameAlert");
      }
      
      override protected function configUi() : void
      {
         super.configUi();
         titleText = LanguageMgr.GetTranslation("tank.view.ReworkNameView.consortiaReworkName");
         _tittleField.text = LanguageMgr.GetTranslation("tank.view.ReworkNameView.consortiaInputName");
         _resultField.text = LanguageMgr.GetTranslation("tank.view.ConsortiaReworkNameView.consortiaNameAlert");
         if(_nicknameInput)
         {
            ObjectUtils.disposeObject(_nicknameInput);
            _nicknameInput = null;
         }
         _nicknameInput = ComponentFactory.Instance.creatComponentByStylename("bagAndInfo.reworkname.ConsortiaInput");
         addToContent(_nicknameInput);
      }
      
      override protected function __onInputChange(param1:Event) : void
      {
         super.__onInputChange(param1);
         StringHelper.checkTextFieldLength(_nicknameInput,12);
      }
      
      override protected function nameInputCheck() : Boolean
      {
         var _loc1_:BaseAlerFrame = null;
         if(_nicknameInput.text != "")
         {
            if(FilterWordManager.isGotForbiddenWords(_nicknameInput.text,"name"))
            {
               _loc1_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.RenameFrame.Consortia.FailWord"),LanguageMgr.GetTranslation("ok"),"",false,false,false,LayerManager.ALPHA_BLOCKGOUND);
               _loc1_.addEventListener(FrameEvent.RESPONSE,__onAlertResponse);
               return false;
            }
            if(FilterWordManager.IsNullorEmpty(_nicknameInput.text))
            {
               _loc1_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.RenameFrame.Consortia.space"),LanguageMgr.GetTranslation("ok"),"",false,false,false,LayerManager.ALPHA_BLOCKGOUND);
               _loc1_.addEventListener(FrameEvent.RESPONSE,__onAlertResponse);
               return false;
            }
            if(FilterWordManager.containUnableChar(_nicknameInput.text))
            {
               _loc1_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.RenameFrame.Consortia.string"),LanguageMgr.GetTranslation("ok"),"",false,false,false,LayerManager.ALPHA_BLOCKGOUND);
               _loc1_.addEventListener(FrameEvent.RESPONSE,__onAlertResponse);
               return false;
            }
            return true;
         }
         _loc1_ = AlertManager.Instance.simpleAlert(LanguageMgr.GetTranslation("AlertDialog.Info"),LanguageMgr.GetTranslation("tank.RenameFrame.Consortia.input"),LanguageMgr.GetTranslation("ok"),"",false,false,false,LayerManager.ALPHA_BLOCKGOUND);
         _loc1_.addEventListener(FrameEvent.RESPONSE,__onAlertResponse);
         return false;
      }
      
      override protected function setCheckTxt(param1:String) : void
      {
         if(param1 == LanguageMgr.GetTranslation("tank.view.ConsortiaReworkNameView.consortiaNameAlert4"))
         {
            state = Aviable;
            _isCanRework = true;
         }
         else
         {
            state = Unavialbe;
         }
         _resultField.text = param1;
      }
      
      override protected function submitCheckCallBack(param1:ReworkNameAnalyzer) : void
      {
         var _loc3_:String = null;
         complete = true;
         var _loc2_:XML = param1.result;
         this.setCheckTxt(_loc2_.@message);
         if(this.nameInputCheck() && _isCanRework)
         {
            _loc3_ = _nicknameInput.text;
            SocketManager.Instance.out.sendUseConsortiaReworkName(PlayerManager.Instance.Self.ConsortiaID,_bagType,_place,_loc3_);
            reworkNameComplete();
         }
      }
   }
}
