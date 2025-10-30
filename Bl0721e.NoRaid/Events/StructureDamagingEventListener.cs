using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Building.Events;
using SDG.Unturned;
using OpenMod.Unturned.Users;
using UnityEngine;

using Color = System.Drawing.Color;

namespace Bl0721e.NoRaid.Events
{
    public class StructureDamagingEventHandler : IEventListener<UnturnedStructureDamagingEvent>
	{
		//private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
		//public StructureDamagingEventHandler(IUnturnedUserDirectory unturnedUserDirectory)
		//{
		//	m_UnturnedUserDirectory = unturnedUserDirectory;
		//}
		public async Task HandleEventAsync(object? sender, UnturnedStructureDamagingEvent @event)
		{
			string message = "";
			Color color = Color.FromName("White");
			List<EDamageOrigin> allowedOrigins = new List<EDamageOrigin> {
				EDamageOrigin.Carepackage_Timeout,
				EDamageOrigin.Charge_Self_Destruct,
				EDamageOrigin.Zombie_Fire_Breath,
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
			if (@event.Instigator == null)
			{
				if (!allowedOrigins.Contains(@event.DamageOrigin))
				{
					@event.IsCancelled = true;
				}
			}
			else if (@event.DamageOrigin == EDamageOrigin.Vehicle_Bumper && @event.Instigator.CurrentVehicle!.Asset.VehicleType == "train")
			{
				return;
			}
			else
			{
				var position = @event.Buildable.Transform.Position;
				bool hasAccess = await @event.Buildable.Ownership.HasAccessAsync(@event.Instigator);
				bool ignoreNav = false;
				Vector3 v3 = new UnityEngine.Vector3(position.X, position.Y, position.Z);
				if (!@event.Buildable.Ownership.HasOwner || hasAccess || LevelNavigation.checkSafeFakeNav(v3))
				{
					var health = 0.0;
					if (@event.Buildable.State.Health > @event.DamageAmount)
					{
						health = @event.Buildable.State.Health - @event.DamageAmount;
					}
					message = $"此物品当前生命值: {health}";
				}
				else
				{
					@event.IsCancelled = true;
					message = "你不能攻击其他玩家的建筑物";
					color = Color.FromName("Crimson");
				}
			}
			if (message == "")
			{
				return;
			}
			await @event.Instigator!.PrintMessageAsync(message, color);
		}
	}
}
