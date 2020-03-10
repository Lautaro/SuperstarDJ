using UnityEngine;
using Random = UnityEngine.Random;

namespace SuperstarDJ
{
    public static class ProjectTools
    {
        public static Vector3 GetRandomWithinBounds( Bounds bounds )
        {
            return bounds.center + new Vector3 (
                ( Random.value - 0.5f ) * bounds.size.x,
                ( Random.value - 0.5f ) * bounds.size.y,
                ( Random.value - 0.5f ) * bounds.size.z
             );
        }

        public static Vector2 GetRandomPlaceWithinScreen(float limitToCenter = 1f)
        {
            float randomY = Random.Range
                 (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            spawnX *= limitToCenter;
            randomY *= limitToCenter;
            return new Vector2(spawnX, randomY);
        }

        public static Vector3 GetSymmetricalVector( float size )
        {
            return new Vector3 ( size, size, size );
        }

    }
}