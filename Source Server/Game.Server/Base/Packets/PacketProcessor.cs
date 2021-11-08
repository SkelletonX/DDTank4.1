// Decompiled with JetBrains decompiler
// Type: Game.Base.Packets.PacketProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Events;
using Game.Server;
using Game.Server.Packets;
using Game.Server.Packets.Client;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace Game.Base.Packets
{
  public class PacketProcessor
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected static readonly IPacketHandler[] m_packetHandlers = new IPacketHandler[512];
    protected IPacketHandler m_activePacketHandler;
    protected GameClient m_client;
    protected int m_handlerThreadID;

    public PacketProcessor(GameClient client)
    {
      this.m_client = client;
    }

    public void HandlePacket(GSPacketIn packet)
    {
      int code = (int) packet.Code;
      Statistics.BytesIn += (long) packet.Length;
      ++Statistics.PacketsIn;
      IPacketHandler packetHandler = (IPacketHandler) null;
      if (code < PacketProcessor.m_packetHandlers.Length)
      {
        packetHandler = PacketProcessor.m_packetHandlers[code];
        if (packetHandler == null)
          return;
        try
        {
          packetHandler.ToString();
          if(bool.Parse(ConfigurationManager.AppSettings["Debug"]))
          { 
          Console.WriteLine("Pacote Carregado {0}", (ePackageType)code);
          }
        }
        catch
        {
          Console.WriteLine("Pacote Não Encontrado {0}", (ePackageType)code);
        }
      }
      else if (PacketProcessor.log.IsErrorEnabled)
      {
        PacketProcessor.log.ErrorFormat("Received packet code is outside of m_packetHandlers array bounds! " + this.m_client.ToString());
        PacketProcessor.log.Error((object) Marshal.ToHexDump(string.Format("===> <{2}> Packet 0x{0:X2} (0x{1:X2}) length: {3} (ThreadId={4})", (object) code, (object) (code ^ 168), (object) this.m_client.TcpEndpoint, (object) packet.Length, (object) Thread.CurrentThread.ManagedThreadId), packet.Buffer));
      }
      if (packetHandler == null)
        return;
      long tickCount = (long) Environment.TickCount;
      try
      {
        if (this.m_client != null)
        {
          if (packet != null && this.m_client.TcpEndpoint != "not connected")
            packetHandler.HandlePacket(this.m_client, packet);
        }
      }
      catch (Exception ex)
      {
        if (PacketProcessor.log.IsErrorEnabled)
        {
          string tcpEndpoint = this.m_client.TcpEndpoint;
          PacketProcessor.log.Error((object) ("Error while processing packet (handler=" + packetHandler.GetType().FullName + "  client: " + tcpEndpoint + ")"), ex);
          PacketProcessor.log.Error((object) Marshal.ToHexDump("Package Buffer:", packet.Buffer, 0, packet.Length));
        }
      }
      long num = (long) Environment.TickCount - tickCount;
      this.m_activePacketHandler = (IPacketHandler) null;
      if (PacketProcessor.log.IsDebugEnabled)
        PacketProcessor.log.Debug((object) ("Package process Time:" + (object) num + "ms!"));
      if (num <= 1500L)
        return;
      string tcpEndpoint1 = this.m_client.TcpEndpoint;
      if (!PacketProcessor.log.IsWarnEnabled)
        return;
      PacketProcessor.log.Warn((object) ("(" + tcpEndpoint1 + ") Handle packet Thread " + (object) Thread.CurrentThread.ManagedThreadId + " " + (object) packetHandler + " took " + (object) num + "ms!"));
    }

    [ScriptLoadedEvent]
    public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
    {
      Array.Clear((Array) PacketProcessor.m_packetHandlers, 0, PacketProcessor.m_packetHandlers.Length);
      int num = PacketProcessor.SearchPacketHandlers("v168", Assembly.GetAssembly(typeof (GameServer)));
      if (!PacketProcessor.log.IsInfoEnabled)
        return;
      PacketProcessor.log.Info((object) ("PacketProcessor: Loaded " + (object) num + " handlers from GameServer Assembly!"));
    }

    public static void RegisterPacketHandler(int packetCode, IPacketHandler handler)
    {
      PacketProcessor.m_packetHandlers[packetCode] = handler;
    }

    protected static int SearchPacketHandlers(string version, Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Server.Packets.Client.IPacketHandler") != (Type) null)
        {
          PacketHandlerAttribute[] customAttributes = (PacketHandlerAttribute[]) type.GetCustomAttributes(typeof (PacketHandlerAttribute), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            PacketProcessor.RegisterPacketHandler(customAttributes[0].Code, (IPacketHandler) Activator.CreateInstance(type));
          }
        }
      }
      return num;
    }
  }
}
