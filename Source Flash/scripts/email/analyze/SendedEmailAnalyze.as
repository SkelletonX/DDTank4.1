package email.analyze
{
   import com.pickgliss.loader.DataAnalyzer;
   import com.pickgliss.utils.ObjectUtils;
   import email.data.EmailInfoOfSended;
   import flash.utils.describeType;
   
   public class SendedEmailAnalyze extends DataAnalyzer
   {
       
      
      private var _list:Array;
      
      public function SendedEmailAnalyze(param1:Function)
      {
         super(param1);
      }
      
      override public function analyze(param1:*) : void
      {
         var _loc3_:XMLList = null;
         var _loc4_:XML = null;
         var _loc5_:int = 0;
         var _loc6_:EmailInfoOfSended = null;
         this._list = new Array();
         var _loc2_:XML = new XML(param1);
         if(_loc2_.@value == "true")
         {
            _loc3_ = _loc2_.Item;
            _loc4_ = describeType(new EmailInfoOfSended());
            _loc5_ = 0;
            while(_loc5_ < _loc3_.length())
            {
               _loc6_ = new EmailInfoOfSended();
               ObjectUtils.copyPorpertiesByXML(_loc6_,_loc3_[_loc5_]);
               this.list.push(_loc6_);
               _loc5_++;
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
      
      public function get list() : Array
      {
         return this._list;
      }
   }
}
