package cmodule.decry
{
   import flash.events.Event;
   import flash.events.IOErrorEvent;
   import flash.events.TimerEvent;
   import flash.net.URLLoader;
   import flash.net.URLLoaderDataFormat;
   import flash.net.URLRequest;
   import flash.utils.Timer;
   
   public class CRunner implements Debuggee
   {
       
      
      var timer:Timer;
      
      var forceSyncSystem:Boolean;
      
      var suspended:int = 0;
      
      var debugger:GDBMIDebugger;
      
      public function CRunner(param1:Boolean = false)
      {
         super();
         if(CRunner)
         {
            log(1,"More than one CRunner!");
         }
         var CRunner:* = this;
         this.forceSyncSystem = param1;
      }
      
      public function cancelDebug() : void
      {
         this.debugger = null;
      }
      
      public function get isRunning() : Boolean
      {
         return this.suspended <= 0;
      }
      
      public function createArgv(param1:Array) : Array
      {
         return this.rawAllocStringArray(param1).concat(0);
      }
      
      public function createEnv(param1:Object) : Array
      {
         var _loc3_:* = null;
         var _loc2_:Array = [];
         for(_loc3_ in param1)
         {
            _loc2_.push(_loc3_ + "=" + param1[_loc3_]);
         }
         return this.rawAllocStringArray(_loc2_).concat(0);
      }
      
      public function startInit() : void
      {
         log(2,"Static init...");
         modStaticInit();
         var args:Array = CRunner.system.getargv();
         var env:Object = CRunner.system.getenv();
         var argv:Array = this.createArgv(args);
         var envp:Array = this.createEnv(env);
         var startArgs:Array = [args.length].concat(argv,envp);
         var ap:int = this.rawAllocIntArray(startArgs);
         CRunner.ds.length = CRunner.ds.length + 4095 & ~4095;
         CRunner.push(ap);
         CRunner.push(0);
         log(2,"Starting work...");
         this.timer = new Timer(1);
         this.timer.addEventListener(TimerEvent.TIMER,function(param1:TimerEvent):void
         {
            work();
         });
         try
         {
            CRunner.start();
         }
         catch(e:AlchemyExit)
         {
            CRunner.system.exit(e.rv);
            return;
         }
         catch(e:AlchemyYield)
         {
         }
         catch(e:AlchemyDispatch)
         {
         }
         catch(e:AlchemyBlock)
         {
         }
         this.startWork();
      }
      
      private function startWork() : void
      {
         if(!this.timer.running)
         {
            this.timer.delay = 1;
            this.timer.start();
         }
      }
      
      public function work() : void
      {
         var startTime:Number = NaN;
         var checkInterval:int = 0;
         var ms:int = 0;
         if(!this.isRunning)
         {
            return;
         }
         try
         {
            startTime = new Date().time;
            while(true)
            {
               checkInterval = 1000;
               while(checkInterval > 0)
               {
                  try
                  {
                     while(checkInterval-- > 0)
                     {
                        CRunner.gworker.work();
                     }
                  }
                  catch(e:AlchemyDispatch)
                  {
                     continue;
                  }
               }
               if(new Date().time - startTime >= 1000 * 10)
               {
                  throw new AlchemyYield();
               }
            }
            return;
         }
         catch(e:AlchemyExit)
         {
            timer.stop();
            CRunner.system.exit(e.rv);
            return;
         }
         catch(e:AlchemyYield)
         {
            ms = e.ms;
            timer.delay = ms > 0?Number(ms):Number(1);
            return;
         }
         catch(e:AlchemyBlock)
         {
            timer.delay = 10;
            return;
         }
         catch(e:AlchemyBreakpoint)
         {
            throw e;
         }
      }
      
      public function startSystemBridge(param1:String, param2:int) : void
      {
         log(3,"bridge: " + param1 + " port: " + param2);
         CRunner.system = new CSystemBridge(param1,param2);
         CRunner.system.setup(this.startInit);
      }
      
      public function rawAllocString(param1:String) : int
      {
         var _loc2_:int = CRunner.ds.length;
         CRunner.ds.length = CRunner.ds.length + (param1.length + 1);
         CRunner.ds.position = _loc2_;
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            CRunner.ds.writeByte(param1.charCodeAt(_loc3_));
            _loc3_++;
         }
         CRunner.ds.writeByte(0);
         return _loc2_;
      }
      
      public function rawAllocStringArray(param1:Array) : Array
      {
         var _loc2_:Array = [];
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            _loc2_.push(this.rawAllocString(param1[_loc3_]));
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function resume() : void
      {
         if(!--this.suspended)
         {
            this.startWork();
         }
      }
      
      public function startSystem() : void
      {
         var request:URLRequest = null;
         var loader:URLLoader = null;
         if(!this.forceSyncSystem)
         {
            request = new URLRequest(".swfbridge");
            loader = new URLLoader();
            loader.dataFormat = URLLoaderDataFormat.TEXT;
            loader.addEventListener(Event.COMPLETE,function(param1:Event):void
            {
               var _loc2_:XML = new XML(loader.data);
               if(_loc2_ && _loc2_.name() == "bridge" && _loc2_.host && _loc2_.port)
               {
                  startSystemBridge(_loc2_.host,_loc2_.port);
               }
               else
               {
                  startSystemLocal();
               }
            });
            loader.addEventListener(IOErrorEvent.IO_ERROR,function(param1:Event):void
            {
               startSystemLocal();
            });
            loader.load(request);
            return;
         }
         this.startSystemLocal(true);
      }
      
      public function rawAllocIntArray(param1:Array) : int
      {
         var _loc2_:int = CRunner.ds.length;
         CRunner.ds.length = CRunner.ds.length + (param1.length + 1) * 4;
         CRunner.ds.position = _loc2_;
         var _loc3_:int = 0;
         while(_loc3_ < param1.length)
         {
            CRunner.ds.writeInt(param1[_loc3_]);
            _loc3_++;
         }
         return _loc2_;
      }
      
      public function startSystemLocal(param1:Boolean = false) : void
      {
         log(3,"local system");
         CRunner.system = new CSystemLocal(param1);
         CRunner.system.setup(this.startInit);
      }
      
      public function suspend() : void
      {
         this.suspended++;
         if(this.timer && this.timer.running)
         {
            this.timer.stop();
         }
      }
   }
}
