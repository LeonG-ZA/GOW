using System;
using Server;
using Server.MainConfiguration;
using Server.CharConfiguration;

namespace Server.Items
{
    public class SeaChart : MapItem
    {
        [Constructable]
        public SeaChart()
        {
            this.SetDisplay(0, 0, 5119, 4095, 400, 400);
        }

        public SeaChart(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1015232;
            }
        }// sea chart
        public override void CraftInit(Mobile from)
        {
            if (CharConfig.CharHSCartographyEnabled)
            {
                Map map = from.Map;

                double skillValue = from.Skills[SkillName.Cartography].Value;
                int dist = 0; int size = 0;

                if (map == Map.Trammel || map == Map.Felucca)
                { dist = 64 + (int)(skillValue * 10); size = 24 + (int)(skillValue * 3.3); }
                if (map == Map.Ilshenar)
                { dist = 40 + (int)(skillValue * 4); size = 24 + (int)(skillValue * 2.5); }
                if (map == Map.Malas)
                { dist = 50 + (int)(skillValue * 5); size = 24 + (int)(skillValue * 2.8); }
                if (map == Map.Tokuno)
                { dist = 40 + (int)(skillValue * 4); size = 24 + (int)(skillValue * 2.4); }
                if (map == Map.TerMur)
                { dist = 40 + (int)(skillValue * 3); size = 24 + (int)(skillValue * 2); }

                if (dist < 200)
                    dist = 200;

                if (size < 200)
                    size = 200;
                else if (size > 400)
                    size = 400;

                SetDisplay(from.X - dist, from.Y - dist, from.X + dist, from.Y + dist, size, size, from.Map);
            }
            else
            {

                double skillValue = from.Skills[SkillName.Cartography].Value;
                int dist = 64 + (int)(skillValue * 10);

                if (dist < 200)
                    dist = 200;

                int size = 24 + (int)(skillValue * 3.3);

                if (size < 200)
                    size = 200;
                else if (size > 400)
                    size = 400;

                this.SetDisplay(from.X - dist, from.Y - dist, from.X + dist, from.Y + dist, size, size);
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (CharConfig.CharHSCartographyEnabled)
            {
                Map map = this.DisplayMap;
                string mDesc = "";

                if (map == Map.Trammel)
                    mDesc = "for Trammel";
                if (map == Map.Felucca)
                    mDesc = "for Felucca";
                if (map == Map.Ilshenar)
                    mDesc = "for Ilshenar";
                if (map == Map.Malas)
                    mDesc = "for Malas";
                if (map == Map.Tokuno)
                    mDesc = "for Tokuno Islands";
                if (map == Map.TerMur)
                    mDesc = "for Ter Mur";

                list.Add(1053099, String.Format("<BASEFONT COLOR=#DDCC22>\t{0}<BASEFONT COLOR=#FFFFFF>", mDesc));
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}