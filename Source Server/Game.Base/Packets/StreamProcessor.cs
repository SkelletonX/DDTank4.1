// Decompiled with JetBrains decompiler
// Type: Game.Base.Packets.StreamProcessor
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using Road.Base.Packets;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Base.Packets
{
  public class StreamProcessor
  {
    public static byte[] KEY = new byte[8]
    {
      (byte) 174,
      (byte) 191,
      (byte) 86,
      (byte) 120,
      (byte) 171,
      (byte) 205,
      (byte) 239,
      (byte) 241
    };
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected readonly BaseClient m_client;
    protected int m_firstPkgOffset;
    protected int m_sendBufferLength;
    protected bool m_sendingTcp;
    protected Queue m_tcpQueue;
    protected byte[] m_tcpSendBuffer;
    private FSM receive_fsm;
    private SocketAsyncEventArgs send_event;
    private FSM send_fsm;

    public StreamProcessor(BaseClient client)
    {
      this.m_client = client;
      this.m_client.resetKey();
      this.m_tcpSendBuffer = client.SendBuffer;
      this.m_tcpQueue = new Queue(256);
      this.send_event = new SocketAsyncEventArgs();
      this.send_event.UserToken = (object) this;
      this.send_event.Completed += new EventHandler<SocketAsyncEventArgs>(StreamProcessor.AsyncTcpSendCallback);
      this.send_event.SetBuffer(this.m_tcpSendBuffer, 0, 0);
      this.send_fsm = new FSM(2059198199, 1501, nameof (send_fsm));
      this.receive_fsm = new FSM(2059198199, 1501, nameof (receive_fsm));
    }

    private static void AsyncSendTcpImp(object state)
    {
      StreamProcessor streamProcessor = state as StreamProcessor;
      BaseClient client = streamProcessor.m_client;
      try
      {
        StreamProcessor.AsyncTcpSendCallback((object) streamProcessor, streamProcessor.send_event);
      }
      catch (Exception ex)
      {
        StreamProcessor.log.Error((object) nameof (AsyncSendTcpImp), ex);
        client.Disconnect();
      }
    }

    private static void AsyncTcpSendCallback(object sender, SocketAsyncEventArgs e)
    {
      StreamProcessor userToken = (StreamProcessor) e.UserToken;
      BaseClient client = userToken.m_client;
      try
      {
        Queue tcpQueue = userToken.m_tcpQueue;
        if (tcpQueue == null || !client.Socket.Connected)
          return;
        int bytesTransferred = e.BytesTransferred;
        byte[] tcpSendBuffer = userToken.m_tcpSendBuffer;
        int num1 = 0;
        if (bytesTransferred != e.Count && userToken.m_sendBufferLength > bytesTransferred)
        {
          num1 = userToken.m_sendBufferLength - bytesTransferred;
          Array.Copy((Array) tcpSendBuffer, bytesTransferred, (Array) tcpSendBuffer, 0, num1);
        }
        e.SetBuffer(0, 0);
        int offset = userToken.m_firstPkgOffset;
        lock (tcpQueue.SyncRoot)
        {
          int num2 = 0;
          if (tcpQueue.Count > 0)
          {
            PacketIn packetIn;
            do
            {
              packetIn = (PacketIn) tcpQueue.Peek();
              int num3 = !client.Encryted ? packetIn.CopyTo(tcpSendBuffer, num1, offset) : packetIn.CopyTo3(tcpSendBuffer, num1, offset, client.SEND_KEY, ref client.numPacketProcces);
              if (num3 == 0)
                ++num2;
              else
                num2 = 0;
              offset += num3;
              num1 += num3;
              if (packetIn.Length <= offset)
              {
                tcpQueue.Dequeue();
                offset = 0;
                if (client.Encryted)
                {
                  userToken.send_fsm.UpdateState();
                  packetIn.isSended = true;
                }
              }
              if (tcpSendBuffer.Length != num1)
              {
                if (num2 > 5)
                  goto label_18;
              }
              else
                break;
            }
            while (tcpQueue.Count > 0);
            goto label_19;
label_18:
            packetIn.isSended = true;
          }
label_19:
          userToken.m_firstPkgOffset = offset;
          if (num1 <= 0)
          {
            userToken.m_sendingTcp = false;
            return;
          }
        }
        userToken.m_sendBufferLength = num1;
        e.SetBuffer(0, num1);
        if (client.SendAsync(e))
          return;
        StreamProcessor.AsyncTcpSendCallback(sender, e);
      }
      catch (Exception ex)
      {
        StreamProcessor.log.Error((object) nameof (AsyncTcpSendCallback), ex);
        StreamProcessor.log.WarnFormat("It seems <{0}> went linkdead. Closing connection. (SendTCP, {1}: {2})", (object) client, (object) ex.GetType(), (object) ex.Message);
        client.Disconnect();
      }
    }

    public static byte[] cloneArrary(byte[] arr, int length = 8)
    {
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
        numArray[index] = arr[index];
      return numArray;
    }

    public static byte[] decryptBytes(byte[] param1, int curOffset, int param2, byte[] param3)
    {
      byte[] numArray = new byte[param2];
      for (int index = 0; index < param2; ++index)
        numArray[index] = param1[index];
      for (int index = 0; index < param2; ++index)
      {
        if (index > 0)
        {
          param3[index % 8] = (byte) ((int) param3[index % 8] + (int) param1[curOffset + index - 1] ^ index);
          numArray[index] = (byte) ((uint) param1[curOffset + index] - (uint) param1[curOffset + index - 1] ^ (uint) param3[index % 8]);
        }
        else
          numArray[0] = (byte) ((uint) param1[curOffset] ^ (uint) param3[0]);
      }
      return numArray;
    }

    public void Dispose()
    {
      this.send_event.Dispose();
      this.m_tcpQueue.Clear();
    }

    public static string PrintArray(byte[] arr, int length = 8)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      for (int index = 0; index < length; ++index)
        stringBuilder.AppendFormat("{0} ", (object) arr[index]);
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    public static string PrintArray(byte[] arr, int first, int length)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      for (int index = first; index < first + length; ++index)
        stringBuilder.AppendFormat("{0} ", (object) arr[index]);
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    public void ReceiveBytes(int numBytes)
    {
      lock (this)
      {
        byte[] packetBuf = this.m_client.PacketBuf;
        int num = this.m_client.PacketBufSize + numBytes;
        if (num < 20)
        {
          this.m_client.PacketBufSize = num;
        }
        else
        {
          this.m_client.PacketBufSize = 0;
          int index = 0;
          int length;
          do
          {
            int count1 = 0;
            if (this.m_client.Encryted)
            {
              int count2 = this.receive_fsm.count;
              byte[] numArray1 = StreamProcessor.cloneArrary(this.m_client.RECEIVE_KEY, 8);
              for (; index + 4 < num; ++index)
              {
                byte[] numArray2 = StreamProcessor.decryptBytes(packetBuf, index, 8, numArray1);
                if (((int) numArray2[0] << 8) + (int) numArray2[1] == 29099)
                {
                  count1 = ((int) numArray2[2] << 8) + (int) numArray2[3];
                  break;
                }
              }
            }
            else
            {
              for (; index + 4 < num; ++index)
              {
                if (((int) packetBuf[index] << 8) + (int) packetBuf[index + 1] == 29099)
                {
                  count1 = ((int) packetBuf[index + 2] << 8) + (int) packetBuf[index + 3];
                  break;
                }
              }
            }
            if ((count1 == 0 || count1 >= 20) && count1 <= 8192)
            {
              length = num - index;
              if (length >= count1 && (uint) count1 > 0U)
              {
                GSPacketIn pkg = new GSPacketIn(new byte[8192], 8192);
                if (this.m_client.Encryted)
                  pkg.CopyFrom3(packetBuf, index, 0, count1, this.m_client.RECEIVE_KEY);
                else
                  pkg.CopyFrom(packetBuf, index, 0, count1);
                pkg.ReadHeader();
                try
                {
                  this.m_client.OnRecvPacket(pkg);
                }
                catch (Exception ex)
                {
                  if (StreamProcessor.log.IsErrorEnabled)
                    StreamProcessor.log.Error((object) "HandlePacket(pak)", ex);
                }
                index += count1;
              }
              else
                goto label_28;
            }
            else
              goto label_25;
          }
          while (num - 1 > index);
          goto label_29;
label_25:
          this.m_client.PacketBufSize = 0;
          if (!this.m_client.Strict)
            return;
          this.m_client.Disconnect();
          return;
label_28:
          Array.Copy((Array) packetBuf, index, (Array) packetBuf, 0, length);
          this.m_client.PacketBufSize = length;
label_29:
          if (num - 1 != index)
            return;
          packetBuf[0] = packetBuf[index];
          this.m_client.PacketBufSize = 1;
        }
      }
    }

    public void SendTCP(GSPacketIn packet)
    {
      packet.WriteHeader();
      packet.Offset = 0;
      if (!this.m_client.Socket.Connected)
        return;
      try
      {
        Statistics.BytesOut += (long) packet.Length;
        ++Statistics.PacketsOut;
        lock (this.m_tcpQueue.SyncRoot)
        {
          this.m_tcpQueue.Enqueue((object) packet);
          if (this.m_sendingTcp)
            return;
          this.m_sendingTcp = true;
        }
        if (this.m_client.AsyncPostSend)
          ThreadPool.QueueUserWorkItem(new WaitCallback(StreamProcessor.AsyncSendTcpImp), (object) this);
        else
          StreamProcessor.AsyncTcpSendCallback((object) this, this.send_event);
      }
      catch (Exception ex)
      {
        StreamProcessor.log.Error((object) nameof (SendTCP), ex);
        StreamProcessor.log.WarnFormat("It seems <{0}> went linkdead. Closing connection. (SendTCP, {1}: {2})", (object) this.m_client, (object) ex.GetType(), (object) ex.Message);
        this.m_client.Disconnect();
      }
    }

    public void SetFsm(int adder, int muliter)
    {
      this.send_fsm.Setup(adder, muliter);
      this.receive_fsm.Setup(adder, muliter);
    }
  }
}
