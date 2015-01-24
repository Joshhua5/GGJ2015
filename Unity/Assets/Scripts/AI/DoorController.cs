using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Button _doorToggle;

    void Awake()
    {
        _doorToggle.onClick.AddListener(OnDoorToggleClicked);
    }

    void OnDoorToggleClicked()
    {
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        foreach(var door in doors)
        {
            door.SetState(!door.Open);
        }
    }
}