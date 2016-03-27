using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

//LevelOne loc = 

namespace Server.Items
{
    public class ShameAltarAddon : BaseAddon
    {
        public override int LabelNumber { get { return 1151636; } } // Guardian's Altar
        private DateTime LastUse;
        //public virtual TimeSpan Delay{ get{ return TimeSpan.FromHours( 1.0 ); } }

        bool mActive = true;

        public enum DungeonLevel
        { LevelOne = 0, LevelTwo, LevelThree, Boss }

        public DungeonLevel mDungeonLevel;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active { get { return mActive; } set { mActive = value; } }

        private static int[,] m_AddOnComplexComponents = new int[,] 
        {
			  {13826, 1, 1, 0}, {13827, 0, 1, 0}, {13828, 1, 0, 0}// 1	2	3	
			, {15778, 1, 1, 10}// 4	
		};

        [Constructable]
        public ShameAltarAddon(DungeonLevel lvl)
        {
            for (int i = 0; i < m_AddOnComplexComponents.Length / 4; i++)
                AddComponent(new AddonComponent(m_AddOnComplexComponents[i, 0]), m_AddOnComplexComponents[i, 1], m_AddOnComplexComponents[i, 2], m_AddOnComplexComponents[i, 3]);
            mDungeonLevel = lvl;
            Hue = 1378;
        }

        public void HueShift()
        {
            if (Active)
            {
                Hue = 1378;
            }
            else
            {
                Hue = 2700;
            }
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null) ac.Name = name;
            if (hue != 0) ac.Hue = hue;
            if (lightsource != -1) ac.Light = (LightType)lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

        public override void OnComponentUsed(AddonComponent c, Mobile from)
        {
            if (!Active)
            {
                from.SendLocalizedMessage(1151621);//The champion for this dungeon level has already been summoned.
                return;
            }
            else
            {
                if (mDungeonLevel == DungeonLevel.LevelOne)
                {
                    LevelOne(c, from);
                }
                else if (mDungeonLevel == DungeonLevel.LevelTwo)
                {
                    LevelTwo(c, from);
                }
                else if (mDungeonLevel == DungeonLevel.LevelThree)
                {
                    LevelThree(c, from);
                }
                else if (mDungeonLevel == DungeonLevel.Boss)
                {
                    Boss(c, from);
                }
            }
        }

        // Missing check for defeat 
        //msg The champion has recently been defeated, and cannot be summoned again for a few minutes. 1151622
        public void LevelOne(AddonComponent c, Mobile from)
        {
            Container pack = from.Backpack;
            int shamecrystal = 10;

            if (pack != null && pack.ConsumeTotal(typeof(ShameCrystal), 10))
            {
                Mobile QuartzElementalGuardian = new QuartzElementalGuardian(this);
                QuartzElementalGuardian.MoveToWorld(new Point3D(5389, 11, 30), Map);
                from.SendLocalizedMessage(1151620);//The champion accepts your challenge. You have one hour to find and defeat him!
                Active = false;
                HueShift();
            }
            else
            {
                from.SendLocalizedMessage(1151625, shamecrystal.ToString());
                //You are not yet worthy of challenging the champion. 1151623
                //Summoning the dungeon level's champion costs you ~1_COST~ Crystals of Shame. 1151625
            }
        }

        public void LevelTwo(AddonComponent c, Mobile from)
        {
            Container pack = from.Backpack;
            int shamecrystal = 20;

            if (pack != null && pack.ConsumeTotal(typeof(ShameCrystal), 20))
            {
                Mobile FlameElementalGuardian = new FlameElementalGuardian(this);
                FlameElementalGuardian.MoveToWorld(new Point3D(5564, 115, 3), Map);
                from.SendLocalizedMessage(1151620);//The champion accepts your challenge. You have one hour to find and defeat him!
                Active = false;
                HueShift();
            }
            else
            {
                from.SendLocalizedMessage(1151625, shamecrystal.ToString());
                //You are not yet worthy of challenging the champion. 1151623
                //Summoning the dungeon level's champion costs you ~1_COST~ Crystals of Shame. 1151625
            }
        }

        public void LevelThree(AddonComponent c, Mobile from)
        {
            Container pack = from.Backpack;
            int shamecrystal = 30;

            if (pack != null && pack.ConsumeTotal(typeof(ShameCrystal), 30))
            {
                Mobile WindElementalGuardian = new WindElementalGuardian(this);
                WindElementalGuardian.MoveToWorld(new Point3D(5620, 231, 0), Map);
                from.SendLocalizedMessage(1151620);//The champion accepts your challenge. You have one hour to find and defeat him!
                Active = false;
                HueShift();
            }
            else
            {
                from.SendLocalizedMessage(1151625, shamecrystal.ToString());
                //You are not yet worthy of challenging the champion. 1151623
                //Summoning the dungeon level's champion costs you ~1_COST~ Crystals of Shame. 1151625
            }
        }

        public void Boss(AddonComponent c, Mobile from)
        {

        }

        public ShameAltarAddon(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
            writer.Write((bool)mActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
            mActive = reader.ReadBool();

        }
    }
}
