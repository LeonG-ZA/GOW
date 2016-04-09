using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Multis;

namespace Server.Items
{
    public class RowBoatHelm : BaseShipItem, IFacingChange
    {
        private RowBoat m_RowBoat;

        [CommandProperty(AccessLevel.GameMaster)]
        public RowBoat RowBoat { get { return m_RowBoat; } }

        public RowBoatHelm(RowBoat rowBoat, int northItemID, Point3D initOffset)
            : base(rowBoat, northItemID, initOffset)
        {
            m_RowBoat = rowBoat;
            Name = "Wheel";
        }

        public RowBoatHelm(Serial serial)
            : base(serial)
        {
        }
		
        public void SetFacing(Direction oldFacing, Direction newFacing)
        {
            switch (newFacing)
            {
                case Direction.South: ItemID = 0x3EC4; break;
                case Direction.North: ItemID = 0x3EBC; break;
                case Direction.West: ItemID = 0x3E76; break;
                case Direction.East: ItemID = 0x3E63; break;
            }

            if (oldFacing == Server.Direction.North)
            {
                Location = (new Point3D(X, Y, Z));
            }
            else if (newFacing == Server.Direction.North)
            {
                switch (oldFacing)
                {
                    case Server.Direction.South: Location = (new Point3D(X, Y, Z)); break;
                    case Server.Direction.East: Location = (new Point3D(X, Y, Z)); break;
                    case Server.Direction.West: Location = (new Point3D(X, Y, Z)); break;
                }
            }
        }		

        public override void OnDoubleClick(Mobile from)
        {
            if (m_RowBoat != null && Transport.IsDriven)
            {
                Transport.LeaveCommand(from);
                from.SendMessage("You are no longer piloting this vessel");
            }
            else
            {
                if (m_RowBoat != null)
                {
					if (!from.Mounted)
					{
						if (from == m_RowBoat.Owner)
						{
							from.SendMessage("You are now piloting this vessel");
							Transport.TakeCommand(from);
						}
					}
					else
					{
						from.SendMessage("You can not control the Transport while mounted");
					}                   
                }
            }
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
                        m_RowBoat = reader.ReadItem() as RowBoat;
                        break;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); //current version is 1

            // version 1
            writer.Write((RowBoat)m_RowBoat);
        }
        #endregion
    }

    public class RowBoatRope : RowBoatItem
    {
        private RowBoat m_RowBoat;
        //private BoatRopeSide m_Side;

        [CommandProperty(AccessLevel.GameMaster, true)]
        public override bool ShareHue
        {
            get { return false; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public RowBoat RowBoat { get { return m_RowBoat; } set { m_RowBoat = value; } }

        public RowBoatRope(RowBoat rowBoat, int northItemID, Point3D initOffset)
            : base(rowBoat, northItemID, initOffset)
        {
            m_RowBoat = rowBoat;
            Movable = false;
            Name = "Mooring Line";
        }

        public RowBoatRope(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClickDead(Mobile from)
        {
            base.OnDoubleClick(from);
            from.MoveToWorld(this.Location, this.Map);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_RowBoat != null && !m_RowBoat.Contains(from))
            {
                this.m_RowBoat.Refresh();
                from.SendMessage("Welcome aboard !");
                base.OnDoubleClick(from);
                from.MoveToWorld(this.Location, this.Map);
				Transport.Embark(from);
            }
            else if (m_RowBoat != null && m_RowBoat.Contains(from))
            {
                Map map = Map;

                if (map == null)
                    return;

                int rx = 0, ry = 0;

				if (m_RowBoat.Facing == Direction.North)
					rx = 1;
				else if (m_RowBoat.Facing == Direction.South)
					rx = -1;
				else if (m_RowBoat.Facing == Direction.East)
					ry = 1;
				else if (m_RowBoat.Facing == Direction.West)
					ry = -1;
				
				for (int i = 1; i <= 6; ++i)
                {
                    int x = X + (i * rx);
                    int y = Y + (i * ry);
                    int z;

                    for (int j = -16; j <= 16; ++j)
                    {
                        z = from.Z + j;

                        if (map.CanFit(x, y, z, 16, false, false) && !Server.Spells.SpellHelper.CheckMulti(new Point3D(x, y, z), map) && !Region.Find(new Point3D(x, y, z), map).IsPartOf(typeof(Factions.StrongholdRegion)))
                        {
                            if (i == 1 && j >= -2 && j <= 2)
                                return;

                            from.Location = new Point3D(x, y, z);
							Transport.Disembark(from);
                            return;
                        }
                    }

                    z = map.GetAverageZ(x, y);

                    if (map.CanFit(x, y, z, 16, false, false) && !Server.Spells.SpellHelper.CheckMulti(new Point3D(x, y, z), map) && !Region.Find(new Point3D(x, y, z), map).IsPartOf(typeof(Factions.StrongholdRegion)))
                    {
                        if (i == 1)
                            return;

                        from.Location = new Point3D(x, y, z);
						Transport.Disembark(from);
                        return;
                    }
                }	

				rx = 0; 
				ry = 0;
				
				if (m_RowBoat.Facing == Direction.North)
					rx = -1;
				else if (m_RowBoat.Facing == Direction.South)
					rx = 1;
				else if (m_RowBoat.Facing == Direction.East)
					ry = -1;
				else if (m_RowBoat.Facing == Direction.West)
					ry = 1;
                
                for (int i = 1; i <= 6; ++i)
                {
                    int x = X + (i * rx);
                    int y = Y + (i * ry);
                    int z;

                    for (int j = -16; j <= 16; ++j)
                    {
                        z = from.Z + j;

                        if (map.CanFit(x, y, z, 16, false, false) && !Server.Spells.SpellHelper.CheckMulti(new Point3D(x, y, z), map) && !Region.Find(new Point3D(x, y, z), map).IsPartOf(typeof(Factions.StrongholdRegion)))
                        {
                            if (i == 1 && j >= -2 && j <= 2)
                                return;

                            from.Location = new Point3D(x, y, z);
							Transport.Disembark(from);
                            return;
                        }
                    }

                    z = map.GetAverageZ(x, y);

                    if (map.CanFit(x, y, z, 16, false, false) && !Server.Spells.SpellHelper.CheckMulti(new Point3D(x, y, z), map) && !Region.Find(new Point3D(x, y, z), map).IsPartOf(typeof(Factions.StrongholdRegion)))
                    {
                        if (i == 1)
                            return;

                        from.Location = new Point3D(x, y, z);
						Transport.Disembark(from);
                        return;
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version

            writer.Write(m_RowBoat);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_RowBoat = reader.ReadItem() as RowBoat;

                        if (m_RowBoat == null)
                            Delete();

                        break;
                    }
            }
        }
    }
}
