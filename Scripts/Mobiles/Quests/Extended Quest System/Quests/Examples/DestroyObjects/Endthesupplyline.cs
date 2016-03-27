//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 1/10/2013 12:13:45 AM
//=================================================

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    //this quest is for the DestroyObjectObjective Example Quest
    //Quest Giver: GuardJedrek
    //Quest Item: BreakableCreate

	public class Endthesupplyline : BaseQuest
	{
		//This is the Quest Title the player sees at the top of the Gump
		public override object Title{ get{ return "End the supply line"; } }
		//This tells the story of the quest
		public override object Description { get { return "To the north east you will find a camp of orcs. They have slowly been moving south towards the settlements of our people. The first thing we need to do is stop the supply lines. I need you to destroy as many supply crates as you see around the camp."; } }
		//This decides how the npc reacts in text the player refusing the quest
		public override object Refuse{ get{ return "It won't be long till they move past this fort. We need all the help we can get. If you feel the need to help come back to me and we shall talk."; } }
		//This is what the npc says when the player returns without completing the objective(s)
		public override object Uncomplete{ get{ return "You need to stop the supply line, not stair at it soldier. Go do the task you was given."; } }
		//This is what the Quest Giver says when the player completes the quest.
		public override object Complete{ get{ return "Thank you soldier. You have done a great deed for your kingdom."; } }

		public Endthesupplyline() : base()
		{
			//Obtain Objective #1
            AddObjective(new DestroyObjectObjective(typeof(BreakableCrate), "Orcish Supply Crates", 10));
            AddReward( new BaseReward(typeof(Gold),1500,"1500 Gold"));
            AddReward(new BaseReward(typeof(Bandage), 20, "20 Bandages"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
		}

		public override bool CanOffer()
		{
			return true;
		}
	}
}
