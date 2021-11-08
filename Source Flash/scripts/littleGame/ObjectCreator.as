package littleGame
{
   import littleGame.data.LittleObjectType;
   import littleGame.interfaces.ILittleObject;
   import littleGame.object.BigBoguInhaled;
   import littleGame.object.BoguGiveUp;
   import littleGame.object.NormalBoguInhaled;
   import road7th.comm.PackageIn;
   
   public class ObjectCreator
   {
       
      
      public function ObjectCreator()
      {
         super();
      }
      
      public static function CreatObject(param1:String, param2:PackageIn = null) : ILittleObject
      {
         var _loc3_:ILittleObject = null;
         switch(param1)
         {
            case LittleObjectType.NormalBoguInhaled:
               _loc3_ = new NormalBoguInhaled();
               break;
            case LittleObjectType.BigBoguInhaled:
               _loc3_ = new BigBoguInhaled();
               break;
            case LittleObjectType.BoguGiveup:
               _loc3_ = new BoguGiveUp();
         }
         return _loc3_;
      }
   }
}
