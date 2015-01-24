using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Button _doorToggle;

    [SerializeField]
    Door _door;

    void Awake()
    {
        _doorToggle.onClick.AddListener(OnDoorToggleClicked);
    }

    void OnDoorToggleClicked()
    {
        _door.SetState(!_door.Open);
    }

}