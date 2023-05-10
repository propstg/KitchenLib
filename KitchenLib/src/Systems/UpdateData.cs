﻿using Kitchen;
using Kitchen.NetworkSupport;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using KitchenMods;
using Steamworks;
using System.Net;
using UnityEngine;
using System.Threading;
using KitchenLib.Fun;
using System.Collections.Generic;
using System;
using System.Management;
using Microsoft.Win32;

namespace KitchenLib.Systems
{
	public class UpdateData : GameSystemBase, IModSystem
	{
		public static List<string> capes = new List<string>
		{
			"itsHappening",
			"staff",
			"kitchenlib",
			"support",
			"twitch",
			"easter2023",
			"gears2023",
			"discordboost"
		};
		public static void CheckAllData(bool isForced)
		{
			if (Main.manager.GetPreference<PreferenceBool>("datacollection").Value && Main.manager.GetPreference<PreferenceBool>("over13").Value)
			{
				if (CheckForInternetConnection())
				{
					if (NetworkUtils.Get("https://raw.githubusercontent.com/StarFluxMods/starfluxmods.github.io/master/AccessRemoteServer").Contains("1"))
					{
						CollectData("http://api.plateupmodding.com", isForced);
					}
				}
			}
		}
		public static void RunInNewThread(bool isForced = false)
		{
			new Thread(delegate () {
				CheckAllData(isForced);
			}).Start();
		}
		protected override void Initialise()
		{
			RunInNewThread();
		}
		protected override void OnUpdate() { }

		public static void CollectData(string url, bool forced = false)
		{
			string steamID = StringUtils.GetInt32HashCode(SteamPlatform.Steam.Me.ID.ToString()).ToString();
			string steamName = SteamClient.Name;
			string gameVersion = Application.version;
			string klVersion = Main.MOD_VERSION;

			//Data Update
			try
			{
				string lobby = SteamPlatform.Steam.CurrentInviteLobby.Id.ToString();

				while (SteamPlatform.Steam.CurrentInviteLobby.Id.ToString() == "0")
				{
					Thread.Sleep(1000);
				}

				string urlBuilder = $"{url}";
				urlBuilder += $"?syncver=0.6.8";
				urlBuilder += $"&?mode=update";
				urlBuilder += $"&steamID={steamID}";
				urlBuilder += $"&steamName={steamName}";
				urlBuilder += $"&gameVersion={gameVersion}";
				urlBuilder += $"&klVersion={klVersion}";
				urlBuilder += $"&lobbyID={lobby}";
				urlBuilder += $"&resolution={Screen.currentResolution.width + "x" + Screen.currentResolution.height}";
				if (forced)
					urlBuilder += "&forced=1";
				Main.LogInfo(urlBuilder);
				NetworkUtils.Get(urlBuilder);
				
				char[] cosmetic = NetworkUtils.Get($"{url}?syncver=" + Main.MOD_VERSION + "&mode=cosmetic&steamID={steamID}").ToCharArray();

				foreach (string cape in capes)
				{
					Main.cosmeticManager.GetPreference<PreferenceBool>(cape).Set(cosmetic[capes.IndexOf(cape)] == '1');
				}
			}
			catch
			{
			}
		}

		public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
		{
			try
			{
				url = "http://raw.githubusercontent.com";

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.KeepAlive = false;
				request.Timeout = timeoutMs;
				using (var response = (HttpWebResponse)request.GetResponse())
					return true;
			}
			catch
			{
				return false;
			}
		}
	}
}