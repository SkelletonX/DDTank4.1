package cmodule.decry
{
   import avm2.intrinsics.memory.li16;
   import avm2.intrinsics.memory.li32;
   import avm2.intrinsics.memory.li8;
   import avm2.intrinsics.memory.si16;
   import avm2.intrinsics.memory.si32;
   
   public final class FSM___sfvwrite extends Machine
   {
      
      public static const intRegCount:int = 20;
      
      public static const NumberRegCount:int = 0;
       
      
      public var i10:int;
      
      public var i11:int;
      
      public var i12:int;
      
      public var i13:int;
      
      public var i14:int;
      
      public var i15:int;
      
      public var i17:int;
      
      public var i19:int;
      
      public var i16:int;
      
      public var i0:int;
      
      public var i1:int;
      
      public var i2:int;
      
      public var i3:int;
      
      public var i4:int;
      
      public var i5:int;
      
      public var i6:int;
      
      public var i7:int;
      
      public var i8:int;
      
      public var i9:int;
      
      public var i18:int;
      
      public function FSM___sfvwrite()
      {
         super();
      }
      
      public static function start() : void
      {
         var _loc1_:FSM___sfvwrite = null;
         _loc1_ = new FSM___sfvwrite();
         FSM___sfvwrite.gworker = _loc1_;
      }
      
      override public final function work() : void
      {
         loop11:
         switch(state)
         {
            case 0:
               mstate.esp = mstate.esp - 4;
               si32(mstate.ebp,mstate.esp);
               mstate.ebp = mstate.esp;
               mstate.esp = mstate.esp - 0;
               this.i0 = li32(mstate.ebp + 12);
               this.i1 = li32(mstate.ebp + 8);
               this.i2 = li32(this.i0 + 8);
               this.i3 = this.i0 + 8;
               if(this.i2 == 0)
               {
                  addr72:
                  this.i0 = 0;
                  addr1633:
                  mstate.eax = this.i0;
                  mstate.esp = mstate.ebp;
                  mstate.ebp = li32(mstate.esp);
                  mstate.esp = mstate.esp + 4;
                  mstate.esp = mstate.esp + 4;
                  mstate.gworker = caller;
                  return;
               }
               this.i2 = li16(this.i1 + 12);
               this.i4 = this.i1 + 12;
               this.i5 = this.i2 & 8;
               if(this.i5 != 0)
               {
                  this.i5 = li32(this.i1 + 16);
                  if(this.i5 == 0)
                  {
                     this.i2 = this.i2 & 512;
                     if(this.i2 == 0)
                     {
                     }
                  }
                  addr177:
                  this.i0 = li32(this.i0);
                  this.i2 = li16(this.i4);
                  this.i5 = li32(this.i0);
                  this.i6 = li32(this.i0 + 4);
                  this.i7 = this.i2 & 2;
                  if(this.i7 != 0)
                  {
                     this.i2 = this.i5;
                     this.i5 = this.i6;
                     loop5:
                     while(true)
                     {
                        this.i6 = 0;
                        addr245:
                        while(true)
                        {
                           if(this.i5 != 0)
                           {
                              break loop5;
                           }
                           this.i2 = li32(this.i0 + 8);
                           this.i5 = li32(this.i0 + 12);
                           this.i0 = this.i0 + 8;
                           continue loop5;
                        }
                     }
                     mstate.esp = mstate.esp - 12;
                     this.i7 = uint(this.i5) < uint(1025)?int(this.i5):int(1024);
                     this.i8 = this.i2 + this.i6;
                     si32(this.i1,mstate.esp);
                     si32(this.i8,mstate.esp + 4);
                     si32(this.i7,mstate.esp + 8);
                     state = 2;
                     mstate.esp = mstate.esp - 4;
                     FSM___sfvwrite.start();
                     return;
                  }
                  this.i7 = this.i1 + 8;
                  this.i2 = this.i2 & 1;
                  if(this.i2 != 0)
                  {
                     this.i2 = 0;
                     this.i8 = this.i1 + 20;
                     this.i9 = this.i1 + 16;
                     this.i10 = this.i1;
                     while(true)
                     {
                        this.i11 = 0;
                        this.i12 = this.i5;
                        this.i13 = this.i11;
                        loop3:
                        while(true)
                        {
                           this.i14 = this.i5 + this.i13;
                           if(this.i6 != 0)
                           {
                              if(this.i11 == 0)
                              {
                                 if(this.i6 != 0)
                                 {
                                    this.i2 = this.i6 + 1;
                                    this.i11 = this.i13 + this.i12;
                                    while(true)
                                    {
                                       this.i15 = li8(this.i11);
                                       this.i16 = this.i11;
                                       if(this.i15 == 10)
                                       {
                                          this.i2 = this.i16;
                                          break;
                                       }
                                       this.i2 = this.i2 + -1;
                                       this.i11 = this.i11 + 1;
                                       if(this.i2 != 1)
                                       {
                                          continue;
                                       }
                                    }
                                    if(this.i2 != 0)
                                    {
                                       this.i11 = 1;
                                       this.i2 = this.i2 + 1;
                                       this.i2 = this.i2 - this.i14;
                                    }
                                    else
                                    {
                                       this.i2 = 1;
                                       this.i15 = this.i6 + 1;
                                       this.i11 = this.i2;
                                       this.i2 = this.i15;
                                    }
                                 }
                                 this.i2 = 0;
                                 §§goto(addr1130);
                              }
                              this.i15 = li32(this.i7);
                              this.i16 = li32(this.i8);
                              this.i17 = li32(this.i10);
                              this.i18 = li32(this.i9);
                              this.i19 = uint(this.i2) <= uint(this.i6)?int(this.i2):int(this.i6);
                              this.i15 = this.i16 + this.i15;
                              if(uint(this.i17) > uint(this.i18))
                              {
                                 if(this.i19 > this.i15)
                                 {
                                    this.i16 = this.i17;
                                    this.i17 = this.i15;
                                    memcpy(this.i16,this.i14,this.i17);
                                    this.i14 = li32(this.i10);
                                    this.i14 = this.i14 + this.i15;
                                    si32(this.i14,this.i10);
                                    mstate.esp = mstate.esp - 4;
                                    si32(this.i1,mstate.esp);
                                    state = 7;
                                    mstate.esp = mstate.esp - 4;
                                    FSM___sfvwrite.start();
                                    return;
                                 }
                              }
                              if(this.i16 <= this.i19)
                              {
                                 mstate.esp = mstate.esp - 12;
                                 si32(this.i1,mstate.esp);
                                 si32(this.i14,mstate.esp + 4);
                                 si32(this.i16,mstate.esp + 8);
                                 state = 8;
                                 mstate.esp = mstate.esp - 4;
                                 FSM___sfvwrite.start();
                                 return;
                              }
                              addr1498:
                              addr1498:
                              this.i15 = this.i17;
                              this.i16 = this.i19;
                              memcpy(this.i15,this.i14,this.i16);
                              this.i14 = li32(this.i7);
                              this.i14 = this.i14 - this.i19;
                              si32(this.i14,this.i7);
                              this.i14 = li32(this.i10);
                              this.i14 = this.i14 + this.i19;
                              si32(this.i14,this.i10);
                              this.i14 = this.i19;
                              while(true)
                              {
                                 this.i15 = this.i2 - this.i14;
                                 if(this.i2 != this.i14)
                                 {
                                    this.i2 = this.i11;
                                    addr1568:
                                    while(true)
                                    {
                                       this.i11 = li32(this.i3);
                                       this.i16 = this.i11 - this.i14;
                                       si32(this.i16,this.i3);
                                       this.i6 = this.i6 - this.i14;
                                       this.i13 = this.i13 + this.i14;
                                       if(this.i11 != this.i14)
                                       {
                                          this.i11 = this.i2;
                                          this.i2 = this.i15;
                                          continue loop3;
                                       }
                                       break loop11;
                                    }
                                 }
                                 else
                                 {
                                    mstate.esp = mstate.esp - 4;
                                    si32(this.i1,mstate.esp);
                                    state = 9;
                                    mstate.esp = mstate.esp - 4;
                                    FSM___sfvwrite.start();
                                    return;
                                 }
                              }
                           }
                           else
                           {
                              break;
                           }
                        }
                        this.i5 = li32(this.i0 + 8);
                        this.i6 = li32(this.i0 + 12);
                        this.i0 = this.i0 + 8;
                     }
                  }
                  else
                  {
                     this.i2 = this.i1 + 20;
                     this.i8 = this.i1 + 16;
                     this.i9 = this.i1;
                     while(true)
                     {
                        this.i10 = 0;
                        loop1:
                        while(true)
                        {
                           this.i11 = this.i5 + this.i10;
                           if(this.i6 != 0)
                           {
                              this.i12 = li16(this.i4);
                              this.i12 = this.i12 & 16896;
                              if(this.i12 == 16896)
                              {
                                 this.i12 = li32(this.i7);
                                 if(uint(this.i12) < uint(this.i6))
                                 {
                                    this.i12 = li32(this.i9);
                                    this.i13 = li32(this.i8);
                                    this.i14 = this.i6 + 128;
                                    this.i12 = this.i12 - this.i13;
                                    this.i15 = this.i14 + this.i12;
                                    si32(this.i14,this.i7);
                                    si32(this.i15,this.i2);
                                    mstate.esp = mstate.esp - 8;
                                    this.i14 = this.i15 + 1;
                                    si32(this.i13,mstate.esp);
                                    si32(this.i14,mstate.esp + 4);
                                    state = 3;
                                    mstate.esp = mstate.esp - 4;
                                    FSM___sfvwrite.start();
                                    return;
                                 }
                              }
                              addr667:
                              while(true)
                              {
                                 this.i12 = li16(this.i4);
                                 this.i13 = li32(this.i7);
                                 this.i12 = this.i12 & 512;
                                 if(this.i12 != 0)
                                 {
                                    this.i13 = uint(this.i13) > uint(this.i6)?int(this.i6):int(this.i13);
                                    if(this.i13 <= 0)
                                    {
                                       this.i13 = this.i6;
                                    }
                                    else
                                    {
                                       this.i12 = li32(this.i9);
                                       this.i14 = this.i13;
                                       memcpy(this.i12,this.i11,this.i14);
                                       this.i11 = li32(this.i7);
                                       this.i11 = this.i11 - this.i13;
                                       si32(this.i11,this.i7);
                                       this.i11 = li32(this.i9);
                                       this.i13 = this.i11 + this.i13;
                                       si32(this.i13,this.i9);
                                       this.i13 = this.i6;
                                    }
                                 }
                                 else
                                 {
                                    this.i12 = li32(this.i9);
                                    this.i14 = li32(this.i8);
                                    if(uint(this.i12) > uint(this.i14))
                                    {
                                       if(uint(this.i13) < uint(this.i6))
                                       {
                                          this.i14 = this.i13;
                                          memcpy(this.i12,this.i11,this.i14);
                                          this.i11 = li32(this.i9);
                                          this.i11 = this.i11 + this.i13;
                                          si32(this.i11,this.i9);
                                          mstate.esp = mstate.esp - 4;
                                          si32(this.i1,mstate.esp);
                                          state = 5;
                                          mstate.esp = mstate.esp - 4;
                                          FSM___sfvwrite.start();
                                          return;
                                       }
                                    }
                                    this.i13 = li32(this.i2);
                                    if(uint(this.i13) <= uint(this.i6))
                                    {
                                       mstate.esp = mstate.esp - 12;
                                       si32(this.i1,mstate.esp);
                                       si32(this.i11,mstate.esp + 4);
                                       si32(this.i13,mstate.esp + 8);
                                       state = 6;
                                       mstate.esp = mstate.esp - 4;
                                       FSM___sfvwrite.start();
                                       return;
                                    }
                                    this.i13 = this.i12;
                                    this.i12 = this.i6;
                                    memcpy(this.i13,this.i11,this.i12);
                                    this.i13 = li32(this.i7);
                                    this.i13 = this.i13 - this.i6;
                                    si32(this.i13,this.i7);
                                    this.i13 = li32(this.i9);
                                    this.i13 = this.i13 + this.i6;
                                    si32(this.i13,this.i9);
                                    this.i13 = this.i6;
                                 }
                                 addr1022:
                                 while(true)
                                 {
                                    this.i11 = this.i13;
                                    this.i12 = li32(this.i3);
                                    this.i13 = this.i12 - this.i11;
                                    si32(this.i13,this.i3);
                                    this.i6 = this.i6 - this.i11;
                                    this.i10 = this.i10 + this.i11;
                                    if(this.i12 != this.i11)
                                    {
                                       continue loop1;
                                    }
                                    break loop11;
                                 }
                              }
                           }
                           else
                           {
                              break;
                           }
                        }
                        this.i5 = li32(this.i0 + 8);
                        this.i6 = li32(this.i0 + 12);
                        this.i0 = this.i0 + 8;
                     }
                  }
               }
               mstate.esp = mstate.esp - 4;
               si32(this.i1,mstate.esp);
               state = 1;
               mstate.esp = mstate.esp - 4;
               FSM___sfvwrite.start();
               return;
            case 1:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               if(this.i2 != 0)
               {
                  this.i0 = -1;
                  §§goto(addr1633);
               }
               else
               {
                  §§goto(addr177);
               }
            case 2:
               this.i7 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               if(this.i7 >= 1)
               {
                  this.i8 = li32(this.i3);
                  this.i9 = this.i8 - this.i7;
                  si32(this.i9,this.i3);
                  this.i5 = this.i5 - this.i7;
                  this.i6 = this.i6 + this.i7;
                  if(this.i8 != this.i7)
                  {
                     §§goto(addr245);
                  }
                  else
                  {
                     break;
                  }
               }
               else
               {
                  addr1614:
                  this.i0 = -1;
                  this.i1 = li16(this.i4);
                  this.i1 = this.i1 | 64;
                  si16(this.i1,this.i4);
                  §§goto(addr1633);
               }
            case 3:
               this.i14 = mstate.eax;
               mstate.esp = mstate.esp + 8;
               if(this.i14 == 0)
               {
                  if(this.i13 != 0)
                  {
                     mstate.esp = mstate.esp - 4;
                     si32(this.i13,mstate.esp);
                     state = 4;
                     mstate.esp = mstate.esp - 4;
                     FSM___sfvwrite.start();
                     return;
                  }
               }
               addr645:
               si32(this.i14,this.i8);
               if(this.i14 != 0)
               {
                  this.i12 = this.i14 + this.i12;
                  si32(this.i12,this.i9);
                  §§goto(addr667);
               }
               else
               {
                  §§goto(addr1614);
               }
            case 4:
               mstate.esp = mstate.esp + 4;
               §§goto(addr645);
            case 5:
               this.i11 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               if(this.i11 == 0)
               {
                  §§goto(addr1022);
               }
               else
               {
                  §§goto(addr1614);
               }
            case 6:
               this.i11 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               if(this.i11 >= 1)
               {
                  this.i13 = this.i11;
                  §§goto(addr1022);
               }
               else
               {
                  §§goto(addr1614);
               }
            case 7:
               this.i14 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               if(this.i14 == 0)
               {
                  this.i14 = this.i15;
                  §§goto(addr1498);
               }
               else
               {
                  §§goto(addr1614);
               }
            case 8:
               this.i14 = mstate.eax;
               mstate.esp = mstate.esp + 12;
               if(this.i14 >= 1)
               {
                  §§goto(addr1498);
               }
               else
               {
                  §§goto(addr1614);
               }
            case 9:
               this.i2 = mstate.eax;
               mstate.esp = mstate.esp + 4;
               if(this.i2 == 0)
               {
                  this.i2 = 0;
                  §§goto(addr1568);
               }
               else
               {
                  §§goto(addr1614);
               }
         }
         §§goto(addr72);
      }
   }
}
