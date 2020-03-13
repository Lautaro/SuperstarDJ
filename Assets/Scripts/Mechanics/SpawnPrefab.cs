using SuperstarDJ.Enums;
using UnityEngine;

namespace SuperstarDJ.Mechanics
{
    public class SpawnPrefab
    {
        static SpawnPrefab instance;
        public static SpawnPrefab Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new SpawnPrefab ();
                };
                return instance;
            }
        }

        public GameObject Spawn( string PrefabName )
        {
            var prefab = Resources.Load<GameObject> ( Prefabs.PathToPrefabsFolder + PrefabName );
            var returnObject = GameObject.Instantiate ( prefab, new Vector3 ( 0, 0, 0 ), Quaternion.identity );

            return returnObject;
        }


    }
}
