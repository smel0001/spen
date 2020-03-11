using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectManager
{
    #region Singleton
    private static readonly ObjectManager t_Instance = new ObjectManager();

    public static ObjectManager Instance
    {
        get {return t_Instance;}
    }
    #endregion

    static ObjectManager()
    {
        //Construct
        map = GameObject.Find("Clickable").GetComponent<Tilemap>();
    }

    private static Tilemap map;

    private Dictionary<int, System.Tuple<int, int>> idGrid = new Dictionary<int, System.Tuple<int, int>>();
    private HashSet<System.Tuple<int, int>> filledGridLocs = new HashSet<System.Tuple<int, int>>();
    private Dictionary<int, GameObject> objectDict = new Dictionary<int, GameObject>();

    public bool AddObject(GameObject obj, Vector3 pos)
    {
        GameObject spawnedObj = Object.Instantiate(obj, pos, Quaternion.identity);
        objectDict.Add(spawnedObj.GetInstanceID(), spawnedObj);
        return true;
    }

    public bool AddObjectToGrid(GameObject obj, Vector3 pos)
    {
        Vector3Int CellPos = map.WorldToCell(pos);
        int x = CellPos.x;
        int y = CellPos.y;

        System.Tuple<int, int> gridIndex = new System.Tuple<int, int>(x,y);

        if (filledGridLocs.Contains(gridIndex))
        {
            return false;
        }
        else
        { 
            GameObject spawnedObj = Object.Instantiate(obj, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
            int instanceId = spawnedObj.GetInstanceID();
            objectDict.Add(instanceId, spawnedObj);
            filledGridLocs.Add(gridIndex);
            idGrid.Add(instanceId, gridIndex);
            return true;
        }
    }

    private bool IsGridSpaceEmpty(int x, int y)
    {
        if (filledGridLocs.Contains(new System.Tuple<int, int>(x,y)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool RemoveObject(int instanceId)
    {
        if (objectDict.ContainsKey(instanceId))
        {
            Object.Destroy(objectDict[instanceId]);
            objectDict.Remove(instanceId);

            if (idGrid.ContainsKey(instanceId))
            {
                filledGridLocs.Remove(idGrid[instanceId]);
                idGrid.Remove(instanceId);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //Debug Function
    public int GetCount()
    {
        return objectDict.Count;
    }
}
