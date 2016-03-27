using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RandomBlackRock : BaseReagent
	{
		
		[Constructable]
		public RandomBlackRock() : this( 1 )
		{
		}

		[Constructable]
        public RandomBlackRock(int amount)
            : base(GetRandomID())
		{
            Name = "a piece of blackrock"; //not sure if this is correct name
            Hue = 1;
            Stackable = false; // not sure if this is stackable
            Weight = Utility.Random(8, 22); //weight is between 8 and 22 stones
		}

        public RandomBlackRock(Serial serial)
            : base(serial)
		{
		}

		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
        #region randomize
        private static int[] m_ItemID = new int[] { 0x136C, 0x136B };

        public static int GetRandomID()
        {
            return m_ItemID[Utility.Random(m_ItemID.Length)];
        }
        #endregion
    }
}