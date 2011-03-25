using System;
namespace Networking
{
	public abstract class State
	{
		public abstract void Handle (Context context);
	}

	public class StatePlayerA : State
	{
		public override void Handle (Context context)
		{
			context.State = new StatePlayerB ();
		}
	}

	public class StatePlayerB : State
	{
		public override void Handle (Context context)
		{
			context.State = new StatePlayerA ();
		}
	}
}

