using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var agent = other.gameObject.GetComponent<Agent>();
        Game.Instance.PlayerEscaped(agent);
    }
}
