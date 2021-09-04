using System.Collections.Generic;
using UnityEngine;

public class Shoot_Prefab_Script : MonoBehaviour
{
    public Vector3 End_Point;
    public List<Collider> Selectable = new List<Collider>();

    private void Update()
    {
        if (transform.position == End_Point)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        foreach (var item in Selectable)
        {
            item.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
