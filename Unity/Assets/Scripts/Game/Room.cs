using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Door[] _doors;

	public PowerNode powerNode;

    public GameObject Fire;

    public bool HasDoor
    {
        get
        {
            return _doors != null;
        }
    }

    public bool HasPower() { return powerNode.isActive(); }

    public Door FirstDoor
    {
        get
        {
            return _doors[0];
        }
    } 
}
