using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = ObjectPools.Find(x => x.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() {LookupString = objectToSpawn.name,};
            
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault(obj => obj != null);

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            
            spawnableObject.transform.rotation = spawnRotation;
            
            pool.InactiveObjects.Remove(spawnableObject);
            
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0,obj.name.Length - 7); //(Clone) yazısını silmek için
        
        PooledObjectInfo pool = ObjectPools.Find(x => x.LookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Object is not pooled: "  + obj.name);
        }
        else
        {
            obj.SetActive(false);
            
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
