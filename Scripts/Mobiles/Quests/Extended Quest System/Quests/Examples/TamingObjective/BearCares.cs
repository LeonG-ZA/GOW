//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 1/11/2013 2:26:16 AM
//=================================================

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class BearCares : BaseQuest
	{

		//This is the Quest Title the player sees at the top of the Gump
		public override object Title{ get{ return "Bear Cares"; } }
		//This tells the story of the quest
		public override object Description { get { return "In the forrest behind me it would seem the bears are getting to close to town. These are not your ordinary black or brown bears. It be grizzly bears so watch you back. I need you to tame them and release them."; } }
		//This decides how the npc reacts in text the player refusing the quest
		public override object Refuse{ get{ return "Its your back when your sleeping at night out there and not mine."; } }
		//This is what the npc says when the player returns without completing the objective(s)
		public override object Uncomplete{ get{ return "Grow some balls will you. If you got the skill to tame them please help out your fellow townsfolk."; } }
		//This is what the Quest Giver says when the player completes the quest.
		public override object Complete{ get{ return "Great work."; } }

		public BearCares() : base()
		{
			//Tame Objective
			AddObjective(new TamingObjective(typeof(GrizzlyBear),"Grizzly Bear",5));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward("3 Random Magic Items"));
		}

		public override void GiveRewards()
		{
			Item item;
			//Add Reward Item #1
			item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
			if( item is BaseWeapon )
				BaseRunicTool.ApplyAttributesTo((BaseWeapon)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );
			if( item is BaseArmor )
				BaseRunicTool.ApplyAttributesTo((BaseArmor)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );
			if( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo((BaseJewel)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );
			if( item is BaseHat )
				BaseRunicTool.ApplyAttributesTo((BaseHat)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );
			if(!Owner.AddToBackpack( item ) )
			{
				item.MoveToWorld(Owner.Location,Owner.Map);
			}

			//Add Reward Item #2
			item = Loot.RandomArmor();
			BaseRunicTool.ApplyAttributesTo((BaseArmor)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );
            if (!Owner.AddToBackpack(item))
                item.MoveToWorld(Owner.Location,Owner.Map);

			//Add Reward Item #3
			item = Loot.RandomJewelry();
			BaseRunicTool.ApplyAttributesTo((BaseJewel)item,  Utility.RandomMinMax( 3,5 ), 50, 100 );

			if(!Owner.AddToBackpack( item ) )
				item.MoveToWorld(Owner.Location,Owner.Map);
			
			base.GiveRewards();
		}

		public override bool CanOffer()
		{
			return (Owner.Skills[SkillName.AnimalTaming].Base >= 59.1);
		}
	}
}
