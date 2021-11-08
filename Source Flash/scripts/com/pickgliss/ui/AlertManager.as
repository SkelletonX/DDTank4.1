package com.pickgliss.ui
{
   import com.pickgliss.action.AlertAction;
   import com.pickgliss.events.ComponentEvent;
   import com.pickgliss.manager.CacheSysManager;
   import com.pickgliss.toplevel.StageReferance;
   import com.pickgliss.ui.controls.alert.BaseAlerFrame;
   import com.pickgliss.ui.vo.AlertInfo;
   import com.pickgliss.utils.ObjectUtils;
   import com.pickgliss.utils.StringUtils;
   import flash.events.Event;
   
   public final class AlertManager
   {
      
      private static var _instance:AlertManager;
      
      public static var DEFAULT_CONFIRM_LABEL:String = "Ok";
       
      
      private var _layerType:int;
      
      private var _simpleAlertInfo:AlertInfo;
      
      public function AlertManager()
      {
         super();
      }
      
      public static function get Instance() : AlertManager
      {
         if(_instance == null)
         {
            _instance = new AlertManager();
         }
         return _instance;
      }
      
      public function alert(param1:String, param2:AlertInfo, param3:int = 0, param4:String = null) : BaseAlerFrame
      {
         var _loc5_:BaseAlerFrame = ComponentFactory.Instance.creat(param1);
         _loc5_.addEventListener(ComponentEvent.PROPERTIES_CHANGED,this.__onAlertSizeChanged);
         _loc5_.addEventListener(Event.REMOVED_FROM_STAGE,this.__onAlertRemoved);
         _loc5_.info = param2;
         if(param4 && CacheSysManager.isLock(param4))
         {
            CacheSysManager.getInstance().cache(param4,new AlertAction(_loc5_,this._layerType,param3));
         }
         else
         {
            LayerManager.Instance.addToLayer(_loc5_,this._layerType,_loc5_.info.frameCenter,param3);
            StageReferance.stage.focus = _loc5_;
         }
         return _loc5_;
      }
      
      private function __onAlertRemoved(param1:Event) : void
      {
         var _loc2_:BaseAlerFrame = param1.currentTarget as BaseAlerFrame;
         _loc2_.removeEventListener(ComponentEvent.PROPERTIES_CHANGED,this.__onAlertSizeChanged);
         _loc2_.removeEventListener(Event.REMOVED_FROM_STAGE,this.__onAlertRemoved);
      }
      
      private function __onAlertSizeChanged(param1:ComponentEvent) : void
      {
         var _loc2_:BaseAlerFrame = param1.currentTarget as BaseAlerFrame;
         if(_loc2_.info.frameCenter)
         {
            _loc2_.x = (StageReferance.stageWidth - _loc2_.width) / 2;
            _loc2_.y = (StageReferance.stageHeight - _loc2_.height) / 2;
         }
      }
      
      public function setup(param1:int, param2:AlertInfo) : void
      {
         this._simpleAlertInfo = param2;
         this._layerType = param1;
      }
      
      public function simpleAlert(param1:String, param2:String, param3:String = "", param4:String = "", param5:Boolean = false, param6:Boolean = false, param7:Boolean = false, param8:int = 0, param9:String = null) : BaseAlerFrame
      {
         if(StringUtils.isEmpty(param3))
         {
            param3 = DEFAULT_CONFIRM_LABEL;
         }
         var _loc10_:AlertInfo = new AlertInfo();
         ObjectUtils.copyProperties(_loc10_,this._simpleAlertInfo);
         _loc10_.sound = this._simpleAlertInfo.sound;
         _loc10_.data = param2;
         _loc10_.autoDispose = param5;
         _loc10_.title = param1;
         _loc10_.submitLabel = param3;
         _loc10_.cancelLabel = param4;
         _loc10_.enableHtml = param6;
         _loc10_.mutiline = param7;
         if(StringUtils.isEmpty(param4))
         {
            _loc10_.showCancel = false;
         }
         return this.alert(ComponentSetting.SIMPLE_ALERT_STYLE,_loc10_,param8,param9);
      }
   }
}
