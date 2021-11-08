package cmodule.decry
{
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si32;
   import avm2.intrinsics.memory.si8;
   import avm2.intrinsics.memory.sxi8;
   
   public final class FSM___ultoa extends Machine
   {
      
      public static const intRegCount:int = 11;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public var i8:int;
      
      public var i7:int;
      
      public var i9:int;
      
      public function FSM___ultoa()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___ultoa = null;
         _loc1_ = new FSM___ultoa();
         FSM___ultoa.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 0;
               this.i0 = li32(mstate.ebp + 8);
               this.i1 = li32(mstate.ebp + 12);
               this.i2 = li32(mstate.ebp + 16);
               this.i3 = li32(mstate.ebp + 20);
               this.i4 = li32(mstate.ebp + 24);
               this.i5 = li32(mstate.ebp + 28);
               this.i6 = si8(li8(mstate.ebp + 32));
               this.i7 = li32(mstate.ebp + 36);
               if(this.i2 != 8)
               {
                  if(this.i2 != 10)
                  {
                     if(this.i2 == 16)
                     {
                        this.i3 = 0;
                        while(true)
                        {
                           this.i2 = this.i0 & 15;
                           this.i5 = this.i3 ^ -1;
                           this.i2 = this.i4 + this.i2;
                           this.i2 = li8(this.i2);
                           this.i5 = this.i1 + this.i5;
                           si8(this.i2,this.i5);
                           this.i3 = this.i3 + 1;
                           this.i2 = this.i0 >>> 4;
                           if(uint(this.i0) >= uint(16))
                           {
                              this.i0 = this.i2;
                              continue;
                           }
                           break;
                        }
                     }
                     else
                     {
                        state = 1;
                        mstate.esp = mstate.esp - 4;
                        FSM___ultoa.start();
                        return;
                     }
                  }
                  else
                  {
                     if(uint(this.i0) <= uint(9))
                     {
                        this.i0 = this.i0 + 48;
                        si8(this.i0,this.i1 + -1);
                        this.i0 = this.i1 + -1;
                     }
                     else
                     {
                        if(this.i0 >= 0)
                        {
                           this.i3 = 0;
                           this.i4 = this.i1;
                        }
                        else
                        {
                           this.i3 = 1;
                           this.i2 = uint(this.i0) / uint(10);
                           this.i4 = this.i2 * 10;
                           this.i0 = this.i0 - this.i4;
                           this.i0 = this.i0 + 48;
                           si8(this.i0,this.i1 + -1);
                           this.i0 = this.i1 + -1;
                           this.i4 = this.i0;
                           this.i0 = this.i2;
                        }
                        this.i1 = this.i7;
                        loop1:
                        while(true)
                        {
                           this.i2 = this.i1 + 1;
                           this.i7 = this.i1;
                           if(this.i5 != 0)
                           {
                              while(true)
                              {
                                 this.i8 = this.i0 / 10;
                                 this.i8 = this.i8 * 10;
                                 this.i8 = this.i0 - this.i8;
                                 this.i8 = this.i8 + 48;
                                 si8(this.i8,this.i4 + -1);
                                 this.i8 = li8(this.i7);
                                 this.i3 = this.i3 + 1;
                                 this.i9 = this.i4 + -1;
                                 this.i10 = this.i8 << 24;
                                 this.i10 = this.i10 >> 24;
                                 if(this.i10 == this.i3)
                                 {
                                    this.i8 = this.i8 & 255;
                                    if(this.i8 != 127)
                                    {
                                       if(this.i0 >= 10)
                                       {
                                          si8(this.i6,this.i4 + -2);
                                          this.i3 = li8(this.i2);
                                          this.i4 = this.i4 + -2;
                                          if(this.i3 == 0)
                                          {
                                             this.i3 = 0;
                                          }
                                          else
                                          {
                                             this.i3 = this.i1 + 1;
                                             this.i2 = this.i0 / 10;
                                             this.i0 = this.i0 + 9;
                                             if(uint(this.i0) <= uint(18))
                                             {
                                                this.i3 = this.i4;
                                                break loop1;
                                             }
                                             this.i7 = 0;
                                             this.i1 = this.i3;
                                             this.i0 = this.i2;
                                             this.i3 = this.i7;
                                             continue loop1;
                                          }
                                       }
                                       addr381:
                                       this.i8 = this.i0 / 10;
                                       this.i0 = this.i0 + 9;
                                       if(uint(this.i0) <= uint(18))
                                       {
                                          break;
                                       }
                                       this.i0 = this.i8;
                                       continue;
                                    }
                                 }
                                 this.i4 = this.i9;
                                 §§goto(addr381);
                              }
                              this.i3 = this.i4;
                              break;
                           }
                           this.i3 = this.i4;
                           while(true)
                           {
                              this.i4 = this.i0 / 10;
                              this.i1 = this.i4 * 10;
                              this.i1 = this.i0 - this.i1;
                              this.i1 = this.i1 + 48;
                              si8(this.i1,this.i3 + -1);
                              this.i3 = this.i3 + -1;
                              this.i0 = this.i0 + 9;
                              if(uint(this.i0) <= uint(18))
                              {
                                 break;
                              }
                              this.i0 = this.i4;
                           }
                           break;
                        }
                     }
                     addr707:
                     mstate.eax = this.i0;
                     mstate.esp = mstate.ebp;
                     mstate.ebp = li32(mstate.esp);
                     mstate.esp = mstate.esp + 4;
                     mstate.esp = mstate.esp + 4;
                     mstate.gworker = caller;
                     return;
                  }
               }
               else
               {
                  this.i4 = 0;
                  while(true)
                  {
                     this.i2 = this.i0 | 48;
                     this.i5 = this.i4 ^ -1;
                     this.i2 = this.i2 & 55;
                     this.i5 = this.i1 + this.i5;
                     si8(this.i2,this.i5);
                     this.i4 = this.i4 + 1;
                     this.i6 = this.i0 >>> 3;
                     if(uint(this.i0) >= uint(8))
                     {
                        this.i0 = this.i6;
                        continue;
                     }
                     break;
                  }
                  if(this.i3 != 0)
                  {
                     this.i3 = this.i2 & 255;
                     if(this.i3 != 48)
                     {
                        this.i3 = 48;
                        this.i0 = this.i4 + -1;
                        this.i0 = -2 - this.i0;
                        this.i0 = this.i1 + this.i0;
                        si8(this.i3,this.i0);
                     }
                     §§goto(addr707);
                  }
                  this.i3 = this.i5;
               }
               addr703:
               this.i0 = this.i3;
               §§goto(addr707);
            case 1:
               this.i3 = this.i5;
               §§goto(addr703);
         }
      }
   }
}
