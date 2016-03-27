using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Movement;
using Server.Network;

namespace Server.Multis
{
	public class RowBoat : BaseShip
    {				
        protected int NorthID { get { return 0x3C; } }

        #region Properties
        [CommandProperty(AccessLevel.GameMaster)]
        public RowBoatRope Rope { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public RowBoatHelm Helm { get; set; }
        #endregion

        /*
		private RowBoatHelm m_Helm;
        private RowBoatRope m_Rope;
		
        #region Properties
        [CommandProperty(AccessLevel.GameMaster)]
        public RowBoatRope Rope { get { return m_Rope; } set { m_Rope = value; } }		
        #endregion
         */
		
        [Constructable]
        public RowBoat()
            : this(0x3C, 100)
        {
        }		

        protected RowBoat(int ItemID, ushort maxDurability)
            : base(ItemID, maxDurability)
        {
            Helm = new RowBoatHelm(this, 0x3EBC, new Point3D(0, 4, 1));
            Rope = new RowBoatRope(this, 0x14F8, new Point3D(0, -2, 4)); 
        }

        public RowBoat(Serial serial)
            : base(serial)
        { }
  
        protected override void UpdateContainedItemsFacing(Direction oldFacing, Direction newFacing, int count)
        {
            base.UpdateContainedItemsFacing(oldFacing, newFacing, count);
        }	
	
        protected override int GetMultiId(Direction newFacing)
        {
            return NorthID + ((int)newFacing / 2);
        }

        protected override bool IsEnabledLandID(int landID)
        {
            if (landID > 167 && landID < 172)
                return true;

            if (landID == 310 || landID == 311)
                return true;

            return false;
        }

        protected override bool IsEnabledStaticID(int staticID)
        {
            if (staticID > 0x1795 && staticID < 0x17B3)
                return true;

            return false;
        }
		
		protected override bool BeginMove(Direction dir, SpeedCode speed)	
		{
			speed = SpeedCode.Slow;
			
			return base.BeginMove(dir, speed);
		}

        #region Serialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        Helm = reader.ReadItem() as RowBoatHelm;
                        Rope = reader.ReadItem() as RowBoatRope;

                        break;
                    }
            }	
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);


            writer.Write((int)1);

            writer.Write((Item)Helm);
            writer.Write((Item)Rope);

        }
        #endregion

        public class DecayTimer : Timer
        {
            private RowBoat _rowBoat;
            private int _count;

            public DecayTimer(RowBoat rowBoat)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(5.0))
            {
                _rowBoat = rowBoat;

                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (_count == 5)
                {
                    _rowBoat.Delete();
                    Stop();
                }
                else
                {
                    _rowBoat.Location = new Point3D(_rowBoat.X, _rowBoat.Y, _rowBoat.Z - 1);

                    ++_count;
                }
            }
        }	
    }
}