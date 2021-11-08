package cmodule.decry
{
   public class MemUser
   {
       
      
      public function MemUser()
      {
         super();
      }
      
      public final function _mrd(param1:int) : Number
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readDouble();
      }
      
      public final function _mrf(param1:int) : Number
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readFloat();
      }
      
      public final function _mr32(param1:int) : int
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readInt();
      }
      
      public final function _mru8(param1:int) : int
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readUnsignedByte();
      }
      
      public final function _mw32(param1:int, param2:int) : void
      {
         MemUser.ds.position = param1;
         MemUser.ds.writeInt(param2);
      }
      
      public final function _mrs8(param1:int) : int
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readByte();
      }
      
      public final function _mw16(param1:int, param2:int) : void
      {
         MemUser.ds.position = param1;
         MemUser.ds.writeShort(param2);
      }
      
      public final function _mw8(param1:int, param2:int) : void
      {
         MemUser.ds.position = param1;
         MemUser.ds.writeByte(param2);
      }
      
      public final function _mrs16(param1:int) : int
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readShort();
      }
      
      public final function _mru16(param1:int) : int
      {
         MemUser.ds.position = param1;
         return MemUser.ds.readUnsignedShort();
      }
      
      public final function _mwd(param1:int, param2:Number) : void
      {
         MemUser.ds.position = param1;
         MemUser.ds.writeDouble(param2);
      }
      
      public final function _mwf(param1:int, param2:Number) : void
      {
         MemUser.ds.position = param1;
         MemUser.ds.writeFloat(param2);
      }
   }
}
