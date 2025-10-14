using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Building.Events;
using SDG.Unturned;
using OpenMod.Unturned.Users;

namespace Bl0721e.NoRaid.Events
{
    public class BuildableDamagingEventHandler : IEventListener<UnturnedBuildableDamagingEvent>
	{
		//private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
		//public BuildableDamagingEventHandler(IUnturnedUserDirectory unturnedUserDirectory)
		//{
		//	m_UnturnedUserDirectory = unturnedUserDirectory;
		//}
		public Task HandleEventAsync(object? sender, UnturnedBuildableDamagingEvent e)
		{
			var ownership = e.Buildable.Ownership;
			List<EDamageOrigin> allowedOrigins = new List<EDamageOrigin> {
				EDamageOrigin.Mega_Zombie_Boulder,
				EDamageOrigin.Trap_Wear_And_Tear,
				EDamageOrigin.Plant_Harvested,
				EDamageOrigin.Zombie_Swipe,
				EDamageOrigin.Radioactive_Zombie_Explosion,
				EDamageOrigin.Flamable_Zombie_Explosion,
				EDamageOrigin.Zombie_Electric_Shock,
				EDamageOrigin.Zombie_Stomp,
				EDamageOrigin.Horde_Beacon_Self_Destruct
			};
			if (!ownership.HasOwner)
			{
			//	e.Instigator.PrintMessageAsync("dis is an orphan.");
			//	Parallel.ForEach(m_UnturnedUserDirectory.GetOnlineUsers(), x => x.PrintMessageAsync("[DEBUG]O:null, I:"+e.Instigator!.SteamPlayer.model.name+", DO:"+e.DamageOrigin+", H:"+e.Buildable.State.Health+", IC:"+e.IsCancelled));
				if (e.Instigator != null)
				{
					e.Instigator!.PrintMessageAsync("此物品当前生命值: "+e.Buildable!.State.Health);
				}
				return Task.CompletedTask;
			}
			if (e.Instigator == null)
			{
				if (!allowedOrigins.Contains(e.DamageOrigin))
				{
					e.IsCancelled = true;
				}
				//Parallel.ForEach(m_UnturnedUserDirectory.GetOnlineUsers(), x => x.PrintMessageAsync("[DEBUG]O:"+ownership.OwnerPlayerId+", I:null"+", DO:"+e.DamageOrigin+", H:"+e.Buildable.State.Health+", IC:"+e.IsCancelled));
				return Task.CompletedTask;
			}
			if (e.Instigator.SteamId.m_SteamID.ToString() == ownership.OwnerPlayerId)
			{
				//Parallel.ForEach(m_UnturnedUserDirectory.GetOnlineUsers(), x => x.PrintMessageAsync("[DEBUG]O:"+ownership.OwnerPlayerId+", I:"+e.Instigator!.SteamPlayer.model.name+", DO:"+e.DamageOrigin+", H:"+e.Buildable.State.Health+", IC:"+e.IsCancelled));
				e.Instigator!.PrintMessageAsync("此物品当前生命值: "+e.Buildable!.State.Health);
				return Task.CompletedTask;
			}
			e.IsCancelled = true;
			e.Instigator!.PrintMessageAsync("你不能攻击其他玩家的建筑物");
			//Parallel.ForEach(m_UnturnedUserDirectory.GetOnlineUsers(), x => x.PrintMessageAsync("[DEBUG]O:"+ownership.OwnerPlayerId+", I:"+e.Instigator!.SteamPlayer.model.name+", DO:"+e.DamageOrigin+", H:"+e.Buildable.State.Health+", IC:"+e.IsCancelled));
			return Task.CompletedTask;
		}
	}
}
