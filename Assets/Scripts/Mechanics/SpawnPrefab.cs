using SuperstarDJ.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           return GameObject.Instantiate ( prefab, new Vector3 ( 0, 0, 0 ), Quaternion.identity );
        }


    }
}
