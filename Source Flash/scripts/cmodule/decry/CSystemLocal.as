package cmodule.decry
{
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.net.URLLoader;
   import flash.net.URLLoaderDataFormat;
   import flash.net.URLRequest;
   import flash.text.TextField;
   import flash.text.TextFieldType;
   import flash.text.TextFormat;
   import flash.utils.ByteArray;
   
   public class CSystemLocal implements CSystem
   {
       
      
      private const statCache:Object = {};
      
      private var forceSync:Boolean;
      
      private const fds:Array = [];
      
      public function CSystemLocal(param1:Boolean = false)
      {
         super();
         this.forceSync = param1;
         var CSystemLocal:* = new TextField();
         CSystemLocal.width = Boolean(CSystemLocal)?Number(CSystemLocal.stage.stageWidth):Number(800);
         CSystemLocal.height = Boolean(CSystemLocal)?Number(CSystemLocal.stage.stageHeight):Number(600);
         CSystemLocal.multiline = true;
         CSystemLocal.defaultTextFormat = new TextFormat("Courier New");
         CSystemLocal.type = TextFieldType.INPUT;
         CSystemLocal.doubleClickEnabled = true;
         this.fds[0] = new TextFieldI(CSystemLocal);
         this.fds[1] = new TextFieldO(CSystemLocal,CSystemLocal == null);
         this.fds[2] = new TextFieldO(CSystemLocal,true);
         if(CSystemLocal && CSystemLocal)
         {
            CSystemLocal.addChild(CSystemLocal);
         }
         else
         {
            log(3,"local system w/o gsprite");
         }
      }
      
      public function getargv() : Array
      {
         return CSystemLocal;
      }
      
      public function lseek(param1:int, param2:int, param3:int) : int
      {
         var _loc4_:IO = this.fds[param1];
         if(param3 == 0)
         {
            _loc4_.position = param2;
         }
         else if(param3 == 1)
         {
            _loc4_.position = _loc4_.position + param2;
         }
         else if(param3 == 2)
         {
            _loc4_.position = _loc4_.size + param2;
         }
         return _loc4_.position;
      }
      
      public function open(param1:int, param2:int, param3:int) : int
      {
         var _loc4_:String = CSystemLocal.gworker.stringFromPtr(param1);
         if(param2 != 0)
         {
            log(3,"failed open(" + _loc4_ + ") flags(" + param2 + ")");
            return -1;
         }
         var _loc5_:Object = this.fetch(_loc4_);
         if(_loc5_.pending)
         {
            throw new AlchemyBlock();
         }
         if(_loc5_.size < 0)
         {
            log(3,"failed open(" + _loc4_ + ") doesn\'t exist");
            return -1;
         }
         var _loc6_:int = 0;
         while(this.fds[_loc6_])
         {
            _loc6_++;
         }
         var _loc7_:ByteArrayIO = new ByteArrayIO();
         _loc7_.byteArray = new ByteArray();
         _loc7_.byteArray.writeBytes(_loc5_.data);
         _loc7_.byteArray.position = 0;
         this.fds[_loc6_] = _loc7_;
         log(4,"open(" + _loc4_ + "): " + _loc7_.size);
         return _loc6_;
      }
      
      public function psize(param1:int) : int
      {
         var _loc2_:String = CSystemLocal.gworker.stringFromPtr(param1);
         var _loc3_:Object = this.fetch(_loc2_);
         if(_loc3_.pending)
         {
            throw new AlchemyBlock();
         }
         if(_loc3_.size < 0)
         {
            log(3,"psize(" + _loc2_ + ") failed");
         }
         else
         {
            log(3,"psize(" + _loc2_ + "): " + _loc3_.size);
         }
         return _loc3_.size;
      }
      
      public function read(param1:int, param2:int, param3:int) : int
      {
         return this.fds[param1].read(param2,param3);
      }
      
      public function getenv() : Object
      {
         return CSystemLocal;
      }
      
      public function write(param1:int, param2:int, param3:int) : int
      {
         return this.fds[param1].write(param2,param3);
      }
      
      public function access(param1:int, param2:int) : int
      {
         var _loc3_:String = CSystemLocal.gworker.stringFromPtr(param1);
         if(param2 & ~4)
         {
            log(3,"failed access(" + _loc3_ + ") mode(" + param2 + ")");
            return -1;
         }
         var _loc4_:Object = this.fetch(_loc3_);
         if(_loc4_.pending)
         {
            throw new AlchemyBlock();
         }
         log(3,"access(" + _loc3_ + "): " + (_loc4_.size >= 0));
         if(_loc4_.size < 0)
         {
            return -1;
         }
         return 0;
      }
      
      public function exit(param1:int) : void
      {
         log(3,"exit: " + param1);
         shellExit(param1);
      }
      
      public function fsize(param1:int) : int
      {
         return this.fds[param1].size;
      }
      
      public function tell(param1:int) : int
      {
         return this.fds[param1].position;
      }
      
      public function ioctl(param1:int, param2:int, param3:int) : int
      {
         return -1;
      }
      
      public function close(param1:int) : int
      {
         var _loc2_:int = this.fds[param1].close();
         this.fds[param1] = null;
         return _loc2_;
      }
      
      private function fetch(param1:String) : Object
      {
         var gf:ByteArray = null;
         var request:URLRequest = null;
         var loader:URLLoader = null;
         var path:String = param1;
         var res:Object = this.statCache[path];
         if(!res)
         {
            gf = CSystemLocal[path];
            if(gf)
            {
               res = {
                  "pending":false,
                  "size":gf.length,
                  "data":gf
               };
               this.statCache[path] = res;
               return res;
            }
         }
         if(this.forceSync)
         {
            return res || {
               "size":-1,
               "pending":false
            };
         }
         if(!res)
         {
            request = new URLRequest(path);
            loader = new URLLoader();
            loader.dataFormat = URLLoaderDataFormat.BINARY;
            loader.addEventListener(Event.COMPLETE,function(param1:Event):void
            {
               statCache[path].data = loader.data;
               statCache[path].size = loader.data.length;
               statCache[path].pending = false;
            });
            loader.addEventListener(IOErrorEvent.IO_ERROR,function(param1:Event):void
            {
               statCache[path].size = -1;
               statCache[path].pending = false;
            });
            this.statCache[path] = res = {"pending":true};
            loader.load(request);
         }
         return res;
      }
      
      public function setup(param1:Function) : void
      {
         param1();
      }
   }
}
