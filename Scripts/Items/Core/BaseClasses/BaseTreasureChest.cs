using System;

namespace Server.Items
{
    public class BaseTreasureChest : LockableContainer
    {
        private TreasureLevel I_TreasureLevel;
        private short I_MaxSpawnTime = 60;
        private short I_MinSpawnTime = 10;
        private TreasureResetTimer I_ResetTimer;
        public BaseTreasureChest(int itemID) : this(itemID, TreasureLevel.Level2)
        {
        }

        public BaseTreasureChest(int itemID, TreasureLevel level) : base(itemID)
        {
            this.I_TreasureLevel = level;
            this.Locked = true;
            this.Movable = false;

            this.SetLockLevel();
            this.GenerateTreasure();
        }

        public BaseTreasureChest(Serial serial)
            : base(serial)
        {
        }

        public enum TreasureLevel
        {
            Level1, 
            Level2, 
            Level3, 
            Level4, 
            Level5,
            Level6,
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public TreasureLevel Level
        {
            get
            {
                return this.I_TreasureLevel;
            }
            set
            {
                this.I_TreasureLevel = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public short MaxSpawnTime
        {
            get
            {
                return this.I_MaxSpawnTime;
            }
            set
            {
                this.I_MaxSpawnTime = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public short MinSpawnTime
        {
            get
            {
                return this.I_MinSpawnTime;
            }
            set
            {
                this.I_MinSpawnTime = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public override bool Locked
        {
            get
            {
                return base.Locked;
            }
            set
            {
                if (base.Locked != value)
                {
                    base.Locked = value;
					
                    if (!value)
                        this.StartResetTimer();
                }
            }
        }
        public override bool IsDecoContainer
        {
            get
            {
                return false;
            }
        }
        public override string DefaultName
        {
            get
            {
                if (this.Locked)
                    return "a locked treasure chest";

                return "a treasure chest";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((byte)this.I_TreasureLevel);
            writer.Write(this.I_MinSpawnTime);
            writer.Write(this.I_MaxSpawnTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            this.I_TreasureLevel = (TreasureLevel)reader.ReadByte();
            this.I_MinSpawnTime = reader.ReadShort();
            this.I_MaxSpawnTime = reader.ReadShort();

            if (!this.Locked)
                this.StartResetTimer();
        }

        public void ClearContents()
        {
            for (int i = this.Items.Count - 1; i >= 0; --i)
            {
                if (i < this.Items.Count)
                    this.Items[i].Delete();
            }
        }

        public void Reset()
        {
            if (this.I_ResetTimer != null)
            {
                if (this.I_ResetTimer.Running)
                    this.I_ResetTimer.Stop();
            }

            this.Locked = true;
            this.ClearContents();
            this.GenerateTreasure();
        }

        protected virtual void SetLockLevel()
        {
            switch( this.I_TreasureLevel )
            {
                case TreasureLevel.Level1:
                    this.RequiredSkill = this.LockLevel = 5;
                    break;
                case TreasureLevel.Level2:
                    this.RequiredSkill = this.LockLevel = 20;
                    break;
                case TreasureLevel.Level3:
                    this.RequiredSkill = this.LockLevel = 50;
                    break;
                case TreasureLevel.Level4:
                    this.RequiredSkill = this.LockLevel = 70;
                    break;
                case TreasureLevel.Level5:
                    this.RequiredSkill = this.LockLevel = 90;
                    break;
                case TreasureLevel.Level6:
                    this.RequiredSkill = this.LockLevel = 100;
                    break;
            }
        }

        protected virtual void GenerateTreasure()
        {
            int MinGold = 1;
            int MaxGold = 2;

            switch( this.I_TreasureLevel )
            {
                case TreasureLevel.Level1:
                    MinGold = 100;
                    MaxGold = 300;
                    break;
                case TreasureLevel.Level2:
                    MinGold = 300;
                    MaxGold = 600;
                    break;
                case TreasureLevel.Level3:
                    MinGold = 600;
                    MaxGold = 900;
                    break;
                case TreasureLevel.Level4:
                    MinGold = 900;
                    MaxGold = 1200;
                    break;
                case TreasureLevel.Level5:
                    MinGold = 1200;
                    MaxGold = 5000;
                    break;
                case TreasureLevel.Level6:
                    MinGold = 5000;
                    MaxGold = 9000;
                    break;
            }

            this.DropItem(new Gold(MinGold, MaxGold));
        }

        private void StartResetTimer()
        {
            if (this.I_ResetTimer == null)
                this.I_ResetTimer = new TreasureResetTimer(this);
            else
                this.I_ResetTimer.Delay = TimeSpan.FromMinutes(Utility.Random(this.I_MinSpawnTime, this.I_MaxSpawnTime));

            this.I_ResetTimer.Start();
        }

        private class TreasureResetTimer : Timer
        {
            private readonly BaseTreasureChest I_Chest;
            public TreasureResetTimer(BaseTreasureChest chest) : base(TimeSpan.FromMinutes(Utility.Random(chest.MinSpawnTime, chest.MaxSpawnTime)))
            {
                this.I_Chest = chest;
                this.Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                this.I_Chest.Reset();
            }
        }
        ; 
    }
}