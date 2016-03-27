using Server;
using System;
using Server.Gumps;
using Server.Accounting;
using Server.Commands;
using Server.Network;
using Server.AccountConfiguration;

namespace Server.Gumps
{
    public class SetAccount : Gump
    {
        public static void Initialize()
        {
            if (AccountConfig.AccountAccountSecurityEnabled)
            {
                CommandSystem.Register("AccountSecurity", AccessLevel.Player, new CommandEventHandler(AccountSecurity_OnCommand));
            }
        }

        public static void AccountSecurity_OnCommand(CommandEventArgs e)
        {
            Account acct = e.Mobile.Account as Account;
            string MailString = acct.GetTag("EMailRecieved");
            if (MailString != null)
            {
                e.Mobile.SendMessage("E-Mail has already been set");
            }
            else
            {
                e.Mobile.CloseGump(typeof(SetAccount));
                e.Mobile.SendGump(new SetAccount());
            }
        }

        public SetAccount()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(130, 74, 414, 239, 9200);
            AddAlphaRegion(141, 109, 182, 164);
            AddAlphaRegion(333, 114, 201, 19);
            AddAlphaRegion(335, 140, 201, 19);
            AddAlphaRegion(336, 166, 201, 19);
            AddAlphaRegion(334, 193, 201, 19);
            AddAlphaRegion(335, 219, 201, 19);
            AddAlphaRegion(334, 246, 201, 19);
            AddLabel(262, 85, 2727, @"Set E-Mail and Password");
            AddLabel(151, 114, 2727, @"Magic Word:");
            AddLabel(151, 141, 2727, @"E-Mail:");
            AddLabel(149, 167, 2727, @"Confirm E-Mail:");
            AddLabel(149, 194, 2727, @"Current Password:");
            AddLabel(147, 218, 2727, @"New Password:");
            AddLabel(149, 246, 2727, @"Confirm New Password:");
            AddTextEntry(334, 115, 200, 20, 1175, 1, "");
            AddTextEntry(334, 140, 200, 20, 1175, 2, "");
            AddTextEntry(335, 166, 200, 20, 1175, 3, "");
            AddTextEntry(335, 193, 200, 20, 1175, 4, "");
            AddTextEntry(335, 219, 200, 20, 1175, 5, "");
            AddTextEntry(334, 245, 200, 20, 1175, 6, "");
            AddLabel(400, 286, 2727, @"@GOW, 2015");
            AddButton(144, 281, 247, 248, 1, GumpButtonType.Reply, 0);
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;
            switch (info.ButtonID)
            {
                case 0:
                    {
                        m.SendMessage(1278, "Your e-mail or Password has not been set.");
                        return;
                    }
                case 1:
                    {
                        string MagicWord = GetString(info, 1);
                        string EMAIL = GetString(info, 2);
                        string confirmEMAIL = GetString(info, 3);

                        string origPass = GetString(info, 4);
                        string newPass = GetString(info, 5);
                        string confirmNewPass = GetString(info, 6);

                        if (newPass != confirmNewPass) //Two "New password" fields do not match
                        {
                            m.SendMessage(37, "The 'Confirm New Password' value does not match the 'New Password'. Remember it is cAsE sEnSaTiVe. ");
                            m.CloseGump(typeof(SetAccount));
                            m.SendGump(new SetAccount());
                            return;
                        }

                        for (int i = 0; i < newPass.Length; ++i)
                        {
                            if (!(char.IsLetterOrDigit(newPass[i]))) //Char is NOT a letter or digit
                            {
                                m.SendMessage(37, "Passwords may only consist of letters (A - Z) and Digits (0 - 9).");
                                return;
                            }
                        }

                        Account acct = m.Account as Account;

                        if (EMAIL.Length <= 6 || MagicWord.Length <= 3 || EMAIL.Length > 40 || MagicWord.Length > 40)
                        {
                            acct.RemoveTag("EMAIL");
                            m.CloseGump(typeof(SetAccount));
                            m.SendGump(new SetAccount());
                            m.SendMessage(37, "Your e-mail must be at least 7 letters or numbers.  Magic Word must be at least 4 letters or numbers.");
                            return;
                        }

                        if (EMAIL != confirmEMAIL)
                        {
                            m.SendMessage(37, "The 'E-mail' values do not match. Remember it is cAsE sEnSaTiVe. ");
                            m.CloseGump(typeof(SetAccount));
                            m.SendGump(new SetAccount());
                            return;
                        }

                        if (!(acct.CheckPassword(origPass))) //"Current Password" value is incorrect
                        {
                            m.SendMessage(37, "The 'Current Password' value is incorrect. [ " + origPass + " ].");
                            m.CloseGump(typeof(SetAccount));
                            m.SendGump(new SetAccount());
                            return;
                        }

                        if (EMAIL != null && MagicWord != null && (acct.CheckPassword(origPass)) && (newPass == confirmNewPass))
                        {
                            acct.SetPassword(newPass);
                            acct.SetTag("EMailRecieved", " ( " + EMAIL + " ) ");
                            acct.SetTag("MagicWord", " ( " + MagicWord + " ) ");

                            m.SendMessage(68, "Your Magic Word is '" + MagicWord + "' and your e-mail is '" + EMAIL + " and your account ( " + acct.Username + " ) password has been changed to '" + newPass + "'. Remember this!");
                        }

                        break;
                    }
            }
        }
    }
}