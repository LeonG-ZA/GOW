using System;
using Server.Items;

namespace Server.Mobiles
{
    public class ClassicMessenger : BaseClassicEscortable
    {
        #region BBS Quests [Locations]
        private static readonly string[] m_MLTownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",   
            "New Haven", "Buccaneer's Den"/*, 
            "Magincia"*/

        };
        private static readonly string[] m_TownNames = new string[]
        {
            "Cove", "Britain", "Jhelom",
			"Minoc", "Trinsic", "Moonglow", 
			"Vesper", "Yew", "Skara Brae",
			"Nujel'm", "Serpent's Hold",    
            "Ocllo"/*, "Magincia"*/

        };
        #endregion

        [Constructable]
        public ClassicMessenger()
        {
            this.Title = "the Messenger";
        }

        public ClassicMessenger(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach
        {
            get
            {
                return true;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }// Do not display 'the ClassicMessenger' when single-clicking

        public override string[] GetPossibleDestinations()
        {
            #region BBS Quests
            if (Map == Map.Trammel)
                return m_MLTownNames;

            else
                return m_TownNames;
            #endregion
        }

        public override void InitOutfit()
        {
            if (this.Female)
                this.AddItem(new PlainDress());
            else
                this.AddItem(new Shirt(GetRandomHue()));

            int lowHue = GetRandomHue();

            this.AddItem(new ShortPants(lowHue));

            if (this.Female)
                this.AddItem(new Boots(lowHue));
            else
                this.AddItem(new Shoes(lowHue));

            //if ( !Female )
            //AddItem( new BodySash( lowHue ) );

            //AddItem( new Cloak( GetRandomHue() ) );

            //if ( !Female )
            //AddItem( new Longsword() );

            switch ( Utility.Random(4) )
            {
                case 0:
                    this.AddItem(new ShortHair(Utility.RandomHairHue()));
                    break;
                case 1:
                    this.AddItem(new TwoPigTails(Utility.RandomHairHue()));
                    break;
                case 2:
                    this.AddItem(new ReceedingHair(Utility.RandomHairHue()));
                    break;
                case 3:
                    this.AddItem(new KrisnaHair(Utility.RandomHairHue()));
                    break;
            }

            this.PackGold(200, 250);
        }

        public override void OnDeath(Container c)
        {

            if (0.01 > Utility.RandomDouble())
                c.DropItem(new Sandals(1672));//Abysmal Red
            else
                c.DropItem(new Sandals(Utility.RandomRedHue()));//reds

            base.OnDeath(c);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;

            this.SpawnMercenary(caster);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            SpawnMercenary(attacker);

        }

        public void SpawnMercenary(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int mercenarys = 0;

            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m is ClassicMercenary)
                    ++mercenarys;
            }

            if (mercenarys < 2)
            {
                BaseCreature mercenary = new ClassicMercenary();

                mercenary.Team = this.Team;

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 7; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, this.Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                mercenary.MoveToWorld(loc, map);

                mercenary.Combatant = target;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private static int GetRandomHue()
        {
            switch ( Utility.Random(6) )
            {
                default:
                case 0:
                    return 0;
                case 1:
                    return Utility.RandomBlueHue();
                case 2:
                    return Utility.RandomGreenHue();
                case 3:
                    return Utility.RandomRedHue();
                case 4:
                    return Utility.RandomYellowHue();
                case 5:
                    return Utility.RandomNeutralHue();
            }
        }
    }
}