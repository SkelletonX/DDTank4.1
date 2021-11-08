package littleGame
{
   import com.pickgliss.loader.BaseLoader;
   import com.pickgliss.loader.LoaderEvent;
   import com.pickgliss.loader.LoaderManager;
   import ddt.interfaces.IProcessObject;
   import ddt.manager.PathManager;
   import ddt.manager.ProcessManager;
   import ddt.utils.RequestVairableCreater;
   import flash.display.Loader;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.net.URLLoader;
   import flash.net.URLVariables;
   import flash.system.ApplicationDomain;
   import flash.system.LoaderContext;
   import flash.utils.ByteArray;
   import flash.utils.Endian;
   import littleGame.data.Grid;
   import littleGame.model.Scenario;
   
   [Event(name="complete",type="com.pickgliss.loader.LoaderEvent")]
   public class ScenarioLoader extends EventDispatcher implements IProcessObject
   {
      
      private static const GridProcessCount:int = 5000;
      
      private static const ConfigReady:int = 1;
      
      private static const ConfigStartProcess:int = 2;
      
      private static const ConfigEndProcess:int = 3;
      
      private static const ConfigComplete:int = 4;
       
      
      private var _onProcess:Boolean = false;
      
      private var _loaded:int = 0;
      
      private var _total:int = 2;
      
      private var _w:int;
      
      private var _h:int;
      
      private var _localW:int;
      
      private var _localH:int;
      
      private var _configBytes:ByteArray;
      
      private var _grid:Grid;
      
      private var _resLoader:BaseLoader;
      
      private var _configLoader:BaseLoader;
      
      private var _scene:Scenario;
      
      private var _objectLoaders:Vector.<BaseLoader>;
      
      private var _objectLoaded:int = 0;
      
      private var _configLoaded:int = 0;
      
      private var _configProcessState:int = 0;
      
      private var _configReady:Boolean = false;
      
      public function ScenarioLoader(param1:Scenario)
      {
         this._scene = param1;
         super();
      }
      
      public function startup() : void
      {
         this.loadObjects();
         this.loadConfig();
         this.loadResource();
      }
      
      private function loadObjects() : void
      {
         var _loc4_:BaseLoader = null;
         var _loc1_:Array = this._scene.objects.split(",");
         this._total = _loc1_.length + 2;
         this._objectLoaders = new Vector.<BaseLoader>();
         var _loc2_:int = _loc1_.length;
         var _loc3_:int = 0;
         while(_loc3_ < _loc2_)
         {
            _loc4_ = LoaderManager.Instance.creatLoader(PathManager.solveLittleGameObjectPath(_loc1_[_loc3_]),BaseLoader.MODULE_LOADER);
            _loc4_.addEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
            _loc4_.addEventListener(LoaderEvent.COMPLETE,this.__objectComplete);
            LoaderManager.Instance.startLoad(_loc4_);
            this._objectLoaders.push(_loc4_);
            _loc3_++;
         }
      }
      
      private function __loaderError(param1:LoaderEvent) : void
      {
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioConfigComplete);
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__objectComplete);
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioResComplete);
         _loc2_.removeEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
      }
      
      private function __objectComplete(param1:LoaderEvent) : void
      {
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__objectComplete);
         _loc2_.removeEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
         this._objectLoaded++;
         this._loaded++;
         this.complete();
      }
      
      public function get progress() : int
      {
         return (this._resLoader.progress / this._total + this._configLoader.progress / this._total + this._objectLoaded / this._total + this._configLoaded / this._total) * 100;
      }
      
      public function shutdown() : void
      {
         this._resLoader.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioResComplete);
         this._configLoader.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioConfigComplete);
      }
      
      private function loadResource() : void
      {
         this._resLoader = LoaderManager.Instance.creatLoader(PathManager.solveLittleGameResPath(this._scene.id),BaseLoader.MODULE_LOADER);
         this._resLoader.addEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
         this._resLoader.addEventListener(LoaderEvent.COMPLETE,this.__scenarioResComplete);
         LoaderManager.Instance.startLoad(this._resLoader);
      }
      
      private function __scenarioResComplete(param1:LoaderEvent) : void
      {
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioResComplete);
         _loc2_.removeEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
         this._loaded++;
         this.complete();
      }
      
      private function loadConfig() : void
      {
         var _loc1_:URLVariables = RequestVairableCreater.creatWidthKey(true);
         _loc1_["rnd"] = Math.random();
         this._configLoader = LoaderManager.Instance.creatLoader(PathManager.solveLittleGameConfigPath(this._scene.id),BaseLoader.BYTE_LOADER,_loc1_);
         this._configLoader.addEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
         this._configLoader.addEventListener(LoaderEvent.COMPLETE,this.__scenarioConfigComplete);
         LoaderManager.Instance.startLoad(this._configLoader);
      }
      
      private function __scenarioConfigComplete(param1:LoaderEvent) : void
      {
         this._loaded++;
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__scenarioConfigComplete);
         _loc2_.removeEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
         this._configBytes = _loc2_.content as ByteArray;
         this._configBytes.uncompress();
         this._configBytes.endian = Endian.LITTLE_ENDIAN;
         this._w = this._configBytes.readInt();
         this._h = this._configBytes.readInt();
         this._grid = new Grid(this._h,this._w);
         this._total = this._total + this._w * this._h / GridProcessCount;
         this._configReady = true;
         this.processConfig();
      }
      
      private function processConfig() : void
      {
         ProcessManager.Instance.addObject(this);
         this._configProcessState = ConfigEndProcess;
      }
      
      private function configProcessComplete() : void
      {
         this._configProcessState = ConfigComplete;
         ProcessManager.Instance.removeObject(this);
      }
      
      private function loadConfigData() : void
      {
         var _loc3_:int = 0;
         this._configProcessState = ConfigStartProcess;
         var _loc1_:int = this._localW + GridProcessCount / this._h;
         _loc1_ = _loc1_ > this._w?int(this._w):int(_loc1_);
         var _loc2_:int = this._localW;
         while(_loc2_ < _loc1_)
         {
            _loc3_ = 0;
            while(_loc3_ < this._h)
            {
               this._grid.setNodeWalkAble(_loc2_,_loc3_,this._configBytes.readByte() == 1?Boolean(true):Boolean(false));
               this._configBytes.readByte();
               _loc3_++;
            }
            _loc2_++;
         }
         this._localW = _loc1_;
         if(this._localW < this._w)
         {
            this._loaded++;
            this._configLoaded++;
            this._configProcessState = ConfigEndProcess;
         }
         else
         {
            this.configProcessComplete();
            this.complete();
         }
      }
      
      private function __resDataComplete(param1:Event) : void
      {
         var _loc2_:URLLoader = param1.currentTarget as URLLoader;
         _loc2_.removeEventListener(Event.COMPLETE,this.__resDataComplete);
         var _loc3_:ByteArray = _loc2_.data as ByteArray;
         var _loc4_:Loader = new Loader();
         _loc4_.contentLoaderInfo.addEventListener(Event.COMPLETE,this.__resComplete);
         _loc4_.loadBytes(_loc3_,new LoaderContext(false,ApplicationDomain.currentDomain));
      }
      
      private function __resComplete(param1:Event) : void
      {
         param1.currentTarget.removeEventListener(Event.COMPLETE,this.__resComplete);
         this._loaded++;
         this.complete();
      }
      
      private function complete() : void
      {
         if(this._loaded >= this._total)
         {
            dispatchEvent(new LoaderEvent(LoaderEvent.COMPLETE,null));
         }
      }
      
      public function get grid() : Grid
      {
         return this._grid;
      }
      
      public function dispose() : void
      {
         this._grid = null;
         this._objectLoaders = null;
         this._resLoader = null;
         this._configLoader = null;
         this._scene = null;
         this._configBytes = null;
         this._configLoader = null;
      }
      
      public function unload() : void
      {
      }
      
      public function get onProcess() : Boolean
      {
         return this._onProcess;
      }
      
      public function set onProcess(param1:Boolean) : void
      {
         this._onProcess = false;
      }
      
      public function process(param1:Number) : void
      {
         if(this._configReady && this._configProcessState == ConfigEndProcess)
         {
            this.loadConfigData();
         }
      }
   }
}
