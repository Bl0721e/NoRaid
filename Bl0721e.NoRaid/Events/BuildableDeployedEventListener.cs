using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Building.Events;
using SDG.Unturned;
using OpenMod.Unturned.Users;
using UnityEngine;

namespace Bl0721e.NoRaid.Events
{
    public class BuildableDeployedEventListener : IEventListener<UnturnedBuildableDeployedEvent>
	{
		private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
		public BuildableDeployedEventListener(IUnturnedUserDirectory unturnedUserDirectory)
		{
			m_UnturnedUserDirectory = unturnedUserDirectory;
		}
		public async Task HandleEventAsync(object? sender, UnturnedBuildableDeployedEvent @event)
		{
			string message = "";
			var position = @event.Buildable.Transform.Position;
			Vector3 v3 = new UnityEngine.Vector3(position.X, position.Y, position.Z);
			SteamPlayer Player = PlayerTool.getSteamPlayer(Convert.ToUInt64(@event.Buildable.Ownership.OwnerPlayerId));
			if (LevelNavigation.checkSafeFakeNav(v3) && Player != null)
			{
				message = "[提示]此建筑位于资源区内, 将不会受到任何保护";
			}
			if (message == "" || Player == null)
			{
				return;
			}
			await UniTask.SwitchToMainThread();
			ChatManager.serverSendMessage(message, Color.red, toPlayer: Player);
		}
	}
}
