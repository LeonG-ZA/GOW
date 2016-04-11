namespace Server.Items
{
    public interface IToggler
    {
        bool Toggle(byte state, Mobile who, int sid);
        bool Deleted { get; }
        IToggler Link { get; set; }
    }
}