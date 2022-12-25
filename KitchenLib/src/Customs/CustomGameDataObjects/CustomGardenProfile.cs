using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGardenProfile : CustomGameDataObject
    {
		public virtual Appliance SpawnHolder { get; internal set; }
		public virtual List<GardenProfile.SpawnProbability> Spawns { get { return new List<GardenProfile.SpawnProbability>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GardenProfile result = ScriptableObject.CreateInstance<GardenProfile>();
			GardenProfile empty = ScriptableObject.CreateInstance<GardenProfile>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<GardenProfile>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.SpawnHolder != SpawnHolder) result.SpawnHolder = SpawnHolder;
            if (empty.Spawns != Spawns) result.Spawns = Spawns;

            gameDataObject = result;
        }
    }
}