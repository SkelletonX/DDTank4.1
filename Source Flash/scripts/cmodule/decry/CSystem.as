package cmodule.decry
{
   public interface CSystem
   {
       
      
      function tell(param1:int) : int;
      
      function access(param1:int, param2:int) : int;
      
      function fsize(param1:int) : int;
      
      function open(param1:int, param2:int, param3:int) : int;
      
      function lseek(param1:int, param2:int, param3:int) : int;
      
      function setup(param1:Function) : void;
      
      function psize(param1:int) : int;
      
      function ioctl(param1:int, param2:int, param3:int) : int;
      
      function read(param1:int, param2:int, param3:int) : int;
      
      function getenv() : Object;
      
      function close(param1:int) : int;
      
      function getargv() : Array;
      
      function exit(param1:int) : void;
      
      function write(param1:int, param2:int, param3:int) : int;
   }
}
