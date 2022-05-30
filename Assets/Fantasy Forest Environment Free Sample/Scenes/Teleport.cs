using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public void TeleportPlayer(Transform pos)
    {
        transform.position = pos.position;
    }
}
