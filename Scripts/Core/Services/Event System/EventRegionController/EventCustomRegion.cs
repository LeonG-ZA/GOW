using Server.Items;

namespace Server.Regions
{
	public class EventCustomRegion : CustomRegion
	{
		private EventRegionControl control;
		public EventCustomRegion(EventRegionControl c):base(c)
		{
			control = c;
		}
		public override void OnEnter(Mobile m)
		{
			base.OnEnter(m);
			control.OnEnter(m);
		}
		public override void OnExit(Mobile m)
		{
			base.OnExit(m);
			control.OnExit(m);
		}
		public override bool OnBeforeDeath(Mobile m)
		{
			control.OnDeath(m);
			return base.OnBeforeDeath(m);
		}
	}
}
