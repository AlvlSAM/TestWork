using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[CreateAssetMenu(fileName = "GameLogix", menuName = "New game logix")]
public class GameLogix : ScriptableObject
{


    #region Private Defenition

    private delegate bool Check(float yPos, Zones zone);

    private Check inZone = (float pos, Zones zone) => (zone.TopBoard >= pos) && (zone.LowBoard <= pos);

    [System.Serializable]
    private class Zones
    {
        public string NameZone;
        public float TopBoard = 0;
        public float LowBoard = 0;

    }

    [SerializeField]
    private List<Object> SkyObjects;
    [SerializeField]
    private List<Object> SeaObjects;
    [SerializeField]
    private List<Object> BichObjects;

    private Dictionary<int, int> bichObjectsCount = null;
    private Dictionary<int, int> seaObjectsCount = null;
    private Dictionary<int, int> skyObjectsCount = null;

    [SerializeField]
    private List<Zones> zones;

    private void GetZone(Zones zone, float yPos, out Dictionary<int, int> zoneCounter, out List<Object> zoneObjects, out float coefSize)
    {
        zoneCounter = BichObjectsCount;
        zoneCounter = SeaObjectsCount;
        zoneCounter = SkyObjectsCount;
        coefSize = 0;

        switch (zone.NameZone)
        {
            case "Bich":
                {
                    coefSize = (yPos - zone.TopBoard) / (zone.LowBoard - zone.TopBoard);
                    zoneCounter = bichObjectsCount;
                    zoneObjects = BichObjects;
                    return;
                }
            case "Sea":
                {
                    coefSize = (yPos - zone.TopBoard) / (zone.LowBoard - zone.TopBoard);
                    zoneCounter = seaObjectsCount;
                    zoneObjects = SeaObjects;
                    return;
                }
            case "Sky":
                {
                    coefSize = (yPos - zone.LowBoard) / (zone.TopBoard - zone.LowBoard);
                    zoneCounter = skyObjectsCount;
                    zoneObjects = SkyObjects;
                    return;
                }
        }
        zoneCounter = null;
        zoneObjects = null;
    }
    #endregion

    #region Features
    public Dictionary<int, int> BichObjectsCount
    {
        get
        {
            if (bichObjectsCount == null)
            {
                bichObjectsCount = new Dictionary<int, int>();
                for (int i = 0; i < BichObjects.Count; i++)
                {
                    bichObjectsCount.Add(i, BichObjects[i].Count);
                }
                return bichObjectsCount;
            }
            return bichObjectsCount;
        }
        
    }

    public Dictionary<int, int> SeaObjectsCount
    {
        get
        {
            if (seaObjectsCount == null)
            {
                seaObjectsCount = new Dictionary<int, int>();
                for (int i = 0; i < SeaObjects.Count; i++)
                {
                    seaObjectsCount.Add(i, SeaObjects[i].Count);
                }
                return seaObjectsCount;
            }
            return seaObjectsCount;
        }

    }

    public Dictionary<int, int> SkyObjectsCount
    {
        get
        {
            if (skyObjectsCount == null)
            {
                skyObjectsCount = new Dictionary<int, int>();
                for (int i = 0; i < SkyObjects.Count; i++)
                {
                    skyObjectsCount.Add(i, SkyObjects[i].Count);
                }
                return skyObjectsCount;
            }
            return skyObjectsCount;
        }

    }

    #endregion

    #region Public Methods
    public GameObject CreatedObject(float yPos, out string Parent)
    {
        Parent = null;
        Dictionary<int, int> zoneCounter = new Dictionary<int, int>();
        List<Object> zoneObjects = new List<Object>();
        float scaleSize = 0;
        foreach(var z in zones)
        {
            if(inZone(yPos, z))
            {
                GetZone(z, yPos, out zoneCounter, out zoneObjects, out scaleSize);
                Parent = z.NameZone;
                break;
            }
        }
        
        Random.InitState(Random.Range(1, 100));

        List<int> objs = new List<int>();
        foreach (var obj in zoneCounter)
        {
            if (obj.Value > 0)
            {
                objs.Add(obj.Key);
            }
        }

        if (objs.Count() > 0)
        {
            int rand = Random.Range(0, objs.Count() - 1);
            Object g = zoneObjects[objs.ElementAt(rand)];

            zoneCounter[objs.ElementAt(rand)] -= 1;

            GameObject gO = g.pref;

            gO.transform.localScale = new Vector3(scaleSize * (g.MaxSize - g.MinSize) + g.MinSize, scaleSize * (g.MaxSize - g.MinSize) + g.MinSize);
            return gO;
        }
       
        return null;

    }

    public GameObject skyObject
    {
        get
        {
            Random.InitState(Random.Range(1, 100));

            List<int> objs = new List<int>();
            foreach (var obj in SkyObjectsCount)
            {
                if (obj.Value > 0)
                {
                    objs.Add(obj.Key);
                }
            }

            if (objs.Count() > 0)
            {
                int rand = Random.Range(0, objs.Count() - 1);
                skyObjectsCount[objs.ElementAt(rand)] -= 1;
                return SkyObjects.ElementAt(objs.ElementAt(rand)).pref;
            }
            return null;

        }
    }

    public GameObject seaObject
    {
        get
        {
            Random.InitState(Random.Range(1, 100));

            List<int> objs = new List<int>();
            foreach (var obj in SeaObjectsCount)
            {
                if (obj.Value > 0)
                {
                    objs.Add(obj.Key);
                }
            }

            if (objs.Count() > 0)
            {
                int rand = Random.Range(0, objs.Count() - 1);
                seaObjectsCount[objs.ElementAt(rand)] -= 1;
                return SeaObjects.ElementAt(objs.ElementAt(rand)).pref;
            }
            return null;

        }
    }
    
    #endregion
}