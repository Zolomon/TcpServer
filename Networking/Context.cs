using System;
namespace Networking
{
	public class Context
	{
		private State state;
		
		public Context (State state)
		{
			this.state = state;
		}
		
		public State State {
			get {return state;}
			set {
				state = value;
				Console.WriteLine("Current State: {0}", state.GetType().Name);
			}
		}
		
		public void Request()
		{
			state.Handle(this);
		}
	}
}

