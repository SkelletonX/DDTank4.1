package ddt.data
{
   public class ServerInfo
   {
      
      public static const MAINTAIN:int = 1;
      
      public static const ALL_FULL:int = 5;
      
      public static const UNIMPEDED:int = 2;
      
      public static const HALF:int = 4;
       
      
      public var Name:String;
      
      public var ID:Number;
      
      public var IP:String;
      
      public var Port:Number;
      
      public var Remark:String;
      
      public var State:int;
      
      public var MustLevel:int;
      
      public var LowestLevel:int;
      
      public var Online:int;
      
      public function ServerInfo()
      {
         super();
      }
   }
}
