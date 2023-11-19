using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowPlayer : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(PlayerManager.Instance.transform);
    }
}
