using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipList : MonoBehaviour
{
	private static ShipList _instance;
    public static ShipList Instance { get { return _instance; } }

    //I know this is far from the best way of doing this.
    [SerializeField] List<Ship> _list;
    public static List<Ship> list;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;         
        }
        else
        {
            Destroy(this.gameObject);
        }

        list = _list;
    }
}
