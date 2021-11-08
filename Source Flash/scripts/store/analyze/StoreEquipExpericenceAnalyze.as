package store.analyze
{
   import com.pickgliss.loader.DataAnalyzer;
   import road7th.data.DictionaryData;
   
   public class StoreEquipExpericenceAnalyze extends DataAnalyzer
   {
       
      
      public var expericence:Array;
      
      public var necklaceStrengthExpList:DictionaryData;
      
      public var necklaceStrengthPlusList:DictionaryData;
      
      public function StoreEquipExpericenceAnalyze(param1:Function)
      {
         super(param1);
      }
      
      override public function analyze(param1:*) : void
      {
         var _loc3_:XMLList = null;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc2_:XML = new XML(param1);
         this.expericence = [];
         this.necklaceStrengthExpList = new DictionaryData();
         this.necklaceStrengthPlusList = new DictionaryData();
         if(_loc2_.@value == "true")
         {
            _loc3_ = _loc2_..item;
            _loc4_ = 0;
            while(_loc4_ < _loc3_.length())
            {
               this.expericence[_loc4_] = int(_loc3_[_loc4_].@Exp);
               _loc5_ = int(_loc3_[_loc4_].@NecklaceStrengthExp);
               _loc6_ = int(_loc3_[_loc4_].@NecklaceStrengthPlus);
               this.necklaceStrengthExpList.add(_loc4_,_loc5_);
               this.necklaceStrengthPlusList.add(_loc4_,_loc6_);
               _loc4_++;
            }
            onAnalyzeComplete();
         }
         else
         {
            message = _loc2_.@message;
            onAnalyzeError();
            onAnalyzeComplete();
         }
      }
   }
}
