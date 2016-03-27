using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class BagofCleanupPigments : Bag
    {
        [Constructable]
        public BagofCleanupPigments():base()
        {
            Name = "Cleanup 2014 Pigment Bag";
            Hue = 0x07A8;

            DropItem(new AuraofAmberPigment());
            DropItem(new BlackandGreenPigment());
            DropItem(new DarkVoidPigment());
            DropItem(new DeepBluePigment());
            DropItem(new DeepVioletPigment());
            DropItem(new GleamingFuchsiaPigment());
            DropItem(new GlossyBluePigment());
            DropItem(new LiquidSunshinePigment());
            DropItem(new MotherofPearlPigment());
            DropItem(new MurkyAmberPigment());
            DropItem(new MurkySeagreenPigment());
            DropItem(new PolishedBronzePigment());
            DropItem(new ReflectiveShadowPigment());
            DropItem(new ShadowyBluePigment());
            DropItem(new StarBluePigment());
            DropItem(new VibrantCrimsonPigment());
            DropItem(new VibrantSeagreenPigment());
        }
        public BagofCleanupPigments(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}