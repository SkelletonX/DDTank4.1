namespace Game.Base
{
	public abstract class AbstractCommandHandler
	{
		public virtual void DisplayMessage(BaseClient client, string format, params object[] args)
		{
			DisplayMessage(client, string.Format(format, args));
		}

		public virtual void DisplayMessage(BaseClient client, string message)
		{
			client?.DisplayMessage(message);
		}

		public virtual void DisplaySyntax(BaseClient client)
		{
			if (client == null)
			{
				return;
			}
			CmdAttribute[] array = (CmdAttribute[])GetType().GetCustomAttributes(typeof(CmdAttribute), inherit: false);
			if (array.Length != 0)
			{
				client.DisplayMessage(array[0].Description);
				string[] usage = array[0].Usage;
				foreach (string msg in usage)
				{
					client.DisplayMessage(msg);
				}
			}
		}
	}
}
