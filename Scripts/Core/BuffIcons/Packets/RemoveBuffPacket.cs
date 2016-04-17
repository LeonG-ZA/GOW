using Server.Network;
using Server;
using Server.Buff.Icons;

public sealed class RemoveBuffPacket : Packet
{
    public RemoveBuffPacket(Mobile mob, BuffInfo info)
        : this(mob, info.ID)
    {
    }

    public RemoveBuffPacket(Mobile mob, BuffIcon iconID)
        : base(0xDF)
    {
        EnsureCapacity(13);
        m_Stream.Write((int)mob.Serial);

        m_Stream.Write((short)iconID);	//ID
        m_Stream.Write((short)0x0);	//Type 0 for removal. 1 for add 2 for Data

        m_Stream.Fill(4);
    }
}