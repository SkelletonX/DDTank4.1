package littleGame
{
   import com.pickgliss.loader.BaseLoader;
   import com.pickgliss.loader.LoaderEvent;
   import com.pickgliss.loader.LoaderManager;
   import com.pickgliss.ui.core.Disposeable;
   import ddt.manager.PathManager;
   import flash.events.Event;
   import flash.events.EventDispatcher;
   import flash.events.IOErrorEvent;
   import flash.utils.ByteArray;
   
   [Event(name="complete",type="com.pickgliss.loader.LoaderEvent")]
   public class MonsterLoader extends EventDispatcher implements Disposeable
   {
       
      
      private var _loaded:int;
      
      private var _total:int;
      
      private var _monsters:Array;
      
      private var _loaders:Vector.<BaseLoader>;
      
      private var _shutdown:Boolean = false;
      
      public function MonsterLoader(param1:Array)
      {
         this._loaders = new Vector.<BaseLoader>();
         this._monsters = param1;
         this._total = this._monsters.length;
         super();
      }
      
      public function dispose() : void
      {
         this._loaders = null;
      }
      
      public function startup() : void
      {
         var _loc1_:String = null;
         var _loc2_:BaseLoader = null;
         for each(_loc1_ in this._monsters)
         {
            _loc2_ = LoaderManager.Instance.creatLoader(PathManager.solveASTPath(_loc1_),BaseLoader.BYTE_LOADER);
            if(CharacterFactory.Instance.hasFile(_loc2_.url))
            {
               this._loaded++;
               this.complete();
            }
            else
            {
               _loc2_.addEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
               _loc2_.addEventListener(LoaderEvent.COMPLETE,this.__dataComplete);
               LoaderManager.Instance.startLoad(_loc2_);
               this._loaders.push(_loc2_);
            }
         }
      }
      
      private function __loaderError(param1:LoaderEvent) : void
      {
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__dataComplete);
         _loc2_.removeEventListener(LoaderEvent.LOAD_ERROR,this.__loaderError);
      }
      
      public function shutdown() : void
      {
         this._shutdown = true;
      }
      
      private function __dataComplete(param1:Event) : void
      {
         var _loc2_:BaseLoader = param1.currentTarget as BaseLoader;
         _loc2_.removeEventListener(IOErrorEvent.IO_ERROR,this.__loaderError);
         _loc2_.removeEventListener(LoaderEvent.COMPLETE,this.__dataComplete);
         var _loc3_:ByteArray = _loc2_.content as ByteArray;
         CharacterFactory.Instance.addFile(_loc2_.url,_loc3_);
         var _loc4_:int = this._loaders.indexOf(_loc2_);
         if(_loc4_ >= 0)
         {
            this._loaders.splice(_loc4_,1);
         }
         this._loaded++;
         this.complete();
      }
      
      private function complete() : void
      {
         if(this._loaded >= this._total && !this._shutdown)
         {
            dispatchEvent(new LoaderEvent(LoaderEvent.COMPLETE,null));
         }
      }
      
      public function get progress() : int
      {
         return this._loaded / this._total * 100;
      }
      
      public function unload() : void
      {
      }
   }
}
