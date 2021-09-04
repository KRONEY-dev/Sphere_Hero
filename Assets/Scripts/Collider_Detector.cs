using System;
using UnityEngine;

public class Collider_Detector : MonoBehaviour
{
    public bool Win = false;
    public bool Can_Win = false;
    private DateTime Last_Triger_Time;

    private void FixedUpdate()
    {
        if (DateTime.Now > Last_Triger_Time.AddSeconds(2) && Can_Win == true && Win == false)
        {
            Win = true;
        }
    }

    private void OnTriggerStay()
    {
        Last_Triger_Time = DateTime.Now;
    }
}
