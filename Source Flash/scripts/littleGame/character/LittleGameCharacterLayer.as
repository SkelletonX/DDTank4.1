package littleGame.character
{
   import ddt.data.EquipType;
   import ddt.data.goods.ItemTemplateInfo;
   import ddt.manager.PathManager;
   import ddt.view.character.BaseLayer;
   
   public class LittleGameCharacterLayer extends BaseLayer
   {
       
      
      private var _sex:Boolean;
      
      private var _specialType:int;
      
      public function LittleGameCharacterLayer(param1:ItemTemplateInfo, param2:String = "", param3:Boolean = true, param4:int = 1, param5:int = 0, param6:int = 0)
      {
         this._sex = param3;
         this._specialType = param5;
         super(param1,param2,false,1,String(param6));
      }
      
      override protected function getUrl(param1:int) : String
      {
         if(this._specialType > 0)
         {
            return PathManager.solveLitteGameCharacterPath(this._specialType,this._sex,1,param1,_pic);
         }
         if(_info.CategoryID != EquipType.CLOTH)
         {
            return PathManager.solveSceneCharacterLoaderPath(_info.CategoryID,_info.Pic,this._sex,_info.NeedSex == 1,String(param1),1,"");
         }
         return PathManager.solveLitteGameCharacterPath(_info.CategoryID,this._sex,1,param1,_pic);
      }
   }
}
