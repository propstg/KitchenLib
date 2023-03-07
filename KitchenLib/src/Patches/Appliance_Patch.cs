﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using KitchenData;

namespace KitchenLib.src.Patches
{
	[HarmonyPatch(typeof(Appliance), "SetupForGame")]
	public class Appliance_Patch
	{
		private static Dictionary<int, int> PurchaseCostOverrides = new Dictionary<int, int>();
		public static void AddPurchaseCostOverride(int id, int cost)
		{
			PurchaseCostOverrides[id] = cost;
		}
		public static void RemovePurchaseCostOverride(int id)
		{
			PurchaseCostOverrides.Remove(id);
		}
		static void Postfix(Appliance __instance)
		{
			if (PurchaseCostOverrides.ContainsKey(__instance.ID))
			{
				__instance.PurchaseCost = PurchaseCostOverrides[__instance.ID];
			}
		}
	}
}
