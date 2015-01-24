using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Door[] _doors;

    public bool HasDoor
    {
        get
        {
            return _doors != null;
        }
    }

    public Door FirstDoor
    {
        get
        {
            return _doors[0];
        }
    }
}
