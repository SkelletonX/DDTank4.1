package com.pickgliss.loader
{
   import com.pickgliss.events.UIModuleEvent;
   import com.pickgliss.ui.ComponentFactory;
   import com.pickgliss.ui.ComponentSetting;
   import deng.fzip.FZip;
   import deng.fzip.FZipFile;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.external.ExternalInterface;
   import flash.utils.ByteArray;
   
   [Event(name="uiMoudleProgress",type="com.pickgliss.events.UIModuleEvent")]
   [Event(name="uiModuleError",type="com.pickgliss.events.UIModuleEvent")]
   [Event(name="uiModuleComplete",type="com.pickgliss.events.UIModuleEvent")]
   public class UIModuleLoader extends EventDispatcher
   {
      
      public static const XMLPNG:String = "xml.png";
      
      public static const CONFIG_ZIP:String = "configZip";
      
      public static const CONFIG_XML:String = "configXml";
      
      private static var _baseUrl:String = "";
      
      private static var _instance:UIModuleLoader;
       
      
      private var _uiModuleLoadMode:String = "configXml";
      
      private var _loadingLoaders:Vector.<BaseLoader>;
      
      private var _queue:Vector.<String>;
      
      private var _backupUrl:String = "";
      
      private var _zipPath:String = "";
      
      private var _zipLoadComplete:Boolean = true;
      
      private var _zipLoader:BaseLoader;
      
      private var _isSecondLoad:Boolean = false;
      
      public function UIModuleLoader()
      {
         super();
         this._queue = new Vector.<String>();
         this._loadingLoaders = new Vector.<BaseLoader>();
      }
      
      public static function get Instance() : UIModuleLoader
      {
         if(_instance == null)
         {
            _instance = new UIModuleLoader();
         }
         return _instance;
      }
      
      public function addUIModlue(param1:String) : void
      {
         if(this._queue.indexOf(param1) != -1)
         {
            return;
         }
         this._queue.push(param1);
         if(!this.isLoading && this._zipLoadComplete)
         {
            this.loadNextModule();
         }
      }
      
      public function addUIModuleImp(param1:String, param2:String = null) : void
      {
         var _loc3_:int = this._queue.indexOf(param1);
         if(_loc3_ != -1)
         {
            this._queue.splice(_loc3_,1);
         }
         if(this._zipLoadComplete)
         {
            this.loadModuleConfig(param1,param2);
         }
         else
         {
            this._queue.unshift(param1);
         }
      }
      
      public function setup(param1:String = "", param2:String = "") : void
      {
         _baseUrl = param1;
         this._backupUrl = param2;
         ComponentSetting.FLASHSITE = _baseUrl;
         ComponentSetting.BACKUP_FLASHSITE = this._backupUrl;
         this._zipPath = _baseUrl + ComponentSetting.getUIConfigZIPPath();
         this._uiModuleLoadMode = CONFIG_ZIP;
         this._zipLoadComplete = false;
         this.loadZipConfig();
      }
      
      public function get baseUrl() : String
      {
         return _baseUrl;
      }
      
      private function loadZipConfig() : void
      {
         if(this._uiModuleLoadMode == CONFIG_XML)
         {
            return;
         }
         this._zipLoader = LoaderManager.Instance.creatLoader(this._zipPath,BaseLoader.BYTE_LOADER);
         this._zipLoader.addEventListener(LoaderEvent.COMPLETE,this.__onLoadZipComplete);
         LoaderManager.Instance.startLoad(this._zipLoader);
      }
      
      private function __onLoadZipComplete(param1:LoaderEvent) : void
      {
         var _loc2_:ByteArray = this._zipLoader.content;
         this.analyMd5(_loc2_);
      }
      
      public function analyMd5(param1:ByteArray) : void
      {
         var _loc2_:ByteArray = null;
         if(ComponentSetting.USEMD5 && (ComponentSetting.md5Dic[XMLPNG] || this.hasHead(param1)))
         {
            if(this.compareMD5(param1))
            {
               _loc2_ = new ByteArray();
               param1.position = 37;
               param1.readBytes(_loc2_);
               this.zipLoad(_loc2_);
            }
            else
            {
               if(this._isSecondLoad)
               {
                  if(ExternalInterface.available)
                  {
                     ExternalInterface.call("alert",this._zipPath + ":is old");
                  }
               }
               else
               {
                  this._zipPath = this._zipPath.replace(ComponentSetting.FLASHSITE,ComponentSetting.BACKUP_FLASHSITE);
                  this._zipLoader.url = this._zipPath + "?rnd=" + Math.random();
                  this._zipLoader.isLoading = false;
                  this._zipLoader.loadFromWeb();
               }
               this._isSecondLoad = true;
            }
         }
         else
         {
            this.zipLoad(param1);
         }
      }
      
      private function compareMD5(param1:ByteArray) : Boolean
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc2_:ByteArray = new ByteArray();
         _loc2_.writeUTFBytes(ComponentSetting.md5Dic[XMLPNG]);
         _loc2_.position = 0;
         param1.position = 5;
         while(_loc2_.bytesAvailable > 0)
         {
            _loc3_ = _loc2_.readByte();
            _loc4_ = param1.readByte();
            if(_loc3_ != _loc4_)
            {
               return false;
            }
         }
         return true;
      }
      
      private function hasHead(param1:ByteArray) : Boolean
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc2_:ByteArray = new ByteArray();
         _loc2_.writeUTFBytes(ComponentSetting.swf_head);
         _loc2_.position = 0;
         param1.position = 0;
         while(_loc2_.bytesAvailable > 0)
         {
            _loc3_ = _loc2_.readByte();
            _loc4_ = param1.readByte();
            if(_loc3_ != _loc4_)
            {
               return false;
            }
         }
         return true;
      }
      
      private function zipLoad(param1:ByteArray) : void
      {
         var _loc2_:FZip = new FZip();
         _loc2_.addEventListener(Event.COMPLETE,this.__onZipParaComplete);
         _loc2_.loadBytes(param1);
      }
      
      private function __onZipParaComplete(param1:Event) : void
      {
         var _loc5_:FZipFile = null;
         var _loc6_:XML = null;
         this._zipLoader.removeEventListener(LoaderEvent.COMPLETE,this.__onLoadZipComplete);
         var _loc2_:FZip = param1.currentTarget as FZip;
         _loc2_.removeEventListener(Event.COMPLETE,this.__onZipParaComplete);
         var _loc3_:int = _loc2_.getFileCount();
         var _loc4_:int = 0;
         while(_loc4_ < _loc3_)
         {
            _loc5_ = _loc2_.getFileAt(_loc4_);
            _loc6_ = new XML(_loc5_.content.toString());
            ComponentFactory.Instance.setup(_loc6_);
            _loc4_++;
         }
         this._zipLoadComplete = true;
         this.loadNextModule();
      }
      
      public function get isLoading() : Boolean
      {
         return this._loadingLoaders.length > 0;
      }
      
      private function __onConfigLoadComplete(param1:LoaderEvent) : void
      {
         var _loc2_:XML = null;
         var _loc3_:String = null;
         param1.loader.removeEventListener(LoaderEvent.COMPLETE,this.__onConfigLoadComplete);
         param1.loader.removeEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
         this._loadingLoaders.splice(this._loadingLoaders.indexOf(param1.loader),1);
         if(param1.loader.isSuccess)
         {
            _loc2_ = new XML(param1.loader.content);
            _loc3_ = _loc2_.@source;
            ComponentFactory.Instance.setup(_loc2_);
            this.loadModuleUI(_loc3_,param1.loader.loadProgressMessage,param1.loader.loadCompleteMessage);
         }
         else
         {
            this.removeLastLoader(param1.loader);
            dispatchEvent(new UIModuleEvent(UIModuleEvent.UI_MODULE_COMPLETE,param1.loader));
            this.loadNextModule();
         }
      }
      
      private function __onLoadError(param1:LoaderEvent) : void
      {
         dispatchEvent(new UIModuleEvent(UIModuleEvent.UI_MODULE_ERROR,param1.loader));
      }
      
      private function __onResourceComplete(param1:LoaderEvent) : void
      {
         param1.loader.removeEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
         param1.loader.removeEventListener(LoaderEvent.PROGRESS,this.__onResourceProgress);
         param1.loader.removeEventListener(LoaderEvent.COMPLETE,this.__onResourceComplete);
         this.removeLastLoader(param1.loader);
         dispatchEvent(new UIModuleEvent(UIModuleEvent.UI_MODULE_COMPLETE,param1.loader));
         this.loadNextModule();
      }
      
      private function removeLastLoader(param1:BaseLoader) : void
      {
         this._loadingLoaders.splice(this._loadingLoaders.indexOf(param1),1);
         this._queue.splice(this._queue.indexOf(param1.loadProgressMessage),1);
      }
      
      private function __onResourceProgress(param1:LoaderEvent) : void
      {
         dispatchEvent(new UIModuleEvent(UIModuleEvent.UI_MODULE_PROGRESS,param1.loader));
      }
      
      private function loadNextModule() : void
      {
         if(this._queue.length <= 0)
         {
            return;
         }
         var _loc1_:String = this._queue[0];
         if(!this.isLoadingModule(_loc1_))
         {
            this.loadModuleConfig(_loc1_);
         }
      }
      
      private function isLoadingModule(param1:String) : Boolean
      {
         var _loc2_:int = 0;
         while(_loc2_ < this._loadingLoaders.length)
         {
            if(this._loadingLoaders[_loc2_].loadProgressMessage == param1)
            {
               return true;
            }
            _loc2_++;
         }
         return false;
      }
      
      private function loadModuleConfig(param1:String, param2:String = "") : void
      {
         var _loc3_:BaseLoader = null;
         if(this._uiModuleLoadMode == CONFIG_XML)
         {
            _loc3_ = LoaderManager.Instance.creatLoader(_baseUrl + ComponentSetting.getUIConfigXMLPath(param1),BaseLoader.TEXT_LOADER);
            _loc3_.loadProgressMessage = param1;
            _loc3_.loadCompleteMessage = param2;
            _loc3_.addEventListener(LoaderEvent.COMPLETE,this.__onConfigLoadComplete);
            _loc3_.addEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
            _loc3_.loadErrorMessage = "加载UI配置文件" + _loc3_.url + "出现错误";
            this._loadingLoaders.push(_loc3_);
            LoaderManager.Instance.startLoad(_loc3_,true);
         }
         else
         {
            this.loadModuleUI(_baseUrl + ComponentSetting.getUISourcePath(param1),param1,param2);
         }
      }
      
      private function loadModuleUI(param1:String, param2:String = "", param3:String = "") : void
      {
         var _loc4_:BaseLoader = LoaderManager.Instance.creatLoader(param1,BaseLoader.MODULE_LOADER);
         _loc4_.loadProgressMessage = param2;
         _loc4_.loadCompleteMessage = param3;
         _loc4_.loadErrorMessage = "加载ui资源：" + _loc4_.url + "出现错误";
         _loc4_.addEventListener(LoaderEvent.LOAD_ERROR,this.__onLoadError);
         _loc4_.addEventListener(LoaderEvent.PROGRESS,this.__onResourceProgress);
         _loc4_.addEventListener(LoaderEvent.COMPLETE,this.__onResourceComplete);
         this._loadingLoaders.push(_loc4_);
         LoaderManager.Instance.startLoad(_loc4_,true);
      }
   }
}
