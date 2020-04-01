using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    // References
    [SerializeField]
    WorldGenerator worldGenerator;

    public WorldGenerator getWorldGenerator()
    {
        return worldGenerator;
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogError("More than one instance of GameConroller exists! Self Destruction");
            Destroy(this);
        } else {
            instance = this;
        }
    }
}
