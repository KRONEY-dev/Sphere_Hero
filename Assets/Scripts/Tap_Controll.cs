using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tap_Controll : MonoBehaviour
{
    [SerializeField]
    private GameObject Pre_Shoot_Sphere;
    private float Pre_Shoot_Sphere_Radius = 0.15f;
    private readonly float Pre_Shoot_Sphere_Start_Radius = 0.15f;

    [SerializeField]
    private GameObject Sphere_Line;

    [SerializeField]
    private GameObject Shoot_Prefab;
    private GameObject Shoot_Obj;
    private Coroutine Current_Shoot_Coroutine;

    private Coroutine Curent_Preview_Coroutine;
    private float Sphere_Hero_Minimal_Radius;

    private Vector3 touch_Position;

    private Ray Camera_Ray, Shoot_Ray;
    private RaycastHit Camera_Hit, Shoot_Hit;

    private List<Collider> CurrentSelectable = new List<Collider>();
    private List<Collider >Last_Selectable = new List<Collider>();

    [SerializeField]
    private GameObject Game_Over_Layer;
    [SerializeField]
    private GameObject Level_Complete_Layer;


    Collider_Detector Collider_Detector_Obj;


    private void Start()
    {
        Sphere_Hero_Minimal_Radius = transform.localScale.x / 100 * 35;
        Collider_Detector_Obj = Sphere_Line.GetComponent<Collider_Detector>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !Game_Over_Layer.activeSelf && Current_Shoot_Coroutine == null)
        {
            Set_Direction_and_Hit_Preview();
        }

        if (Collider_Detector_Obj.Win == true)
        {
            Level_Complete();
        }
    }

    private void Set_Direction_and_Hit_Preview()
    {
        touch_Position = Input.GetTouch(0).position;

        Camera_Ray = Camera.main.ScreenPointToRay(touch_Position);
        Debug.DrawRay(Camera_Ray.origin, Camera_Ray.direction * 20f, Color.yellow);

        if (Physics.Raycast(Camera_Ray, out Camera_Hit))
        {
            Shoot_Ray = new Ray(transform.position, Camera_Hit.point - transform.position);

            Vector3 Temp = Shoot_Ray.direction;
            Temp.y = 0;
            Shoot_Ray.direction = Temp;

            Debug.DrawRay(Shoot_Ray.origin, Shoot_Ray.direction * 20f, Color.red);

            if (Physics.SphereCast(Shoot_Ray, Pre_Shoot_Sphere_Radius, out Shoot_Hit))
            { //Hit_Preview
                Collider[] overlapped_Colliders = Physics.OverlapSphere(Shoot_Hit.point, Pre_Shoot_Sphere_Radius);

                CurrentSelectable.Clear();
                foreach (var item in overlapped_Colliders)
                {
                    if (item.GetComponent<Selectable>())
                    {
                        CurrentSelectable.Add(item);
                    }
                }

                if (Last_Selectable != null && CurrentSelectable != Last_Selectable)
                {
                    foreach (var item in Last_Selectable)
                    {
                        item.GetComponent<Selectable>().Deselect();
                    }
                    foreach (var item in CurrentSelectable)
                    {
                        item.GetComponent<Selectable>().Select();
                    }
                }

                if (Curent_Preview_Coroutine == null)
                {
                    transform.localScale -= Vector3_With_One_Parameter(Pre_Shoot_Sphere_Radius);
                    Curent_Preview_Coroutine = StartCoroutine(Shoot_Create());
                }
                if(transform.localScale.x < Sphere_Hero_Minimal_Radius)
                {
                    Game_Over();
                }

                Last_Selectable.Clear();
                Last_Selectable.AddRange(CurrentSelectable.ToArray());
            }
        }
    }

    private IEnumerator Shoot_Create()
    {
        float scale_dicrement = 0.01f;
        Vector3 Sphere_Scale = Vector3_With_One_Parameter(scale_dicrement);
        while (Input.touchCount > 0)
        {
            if(transform.localScale.x >= Sphere_Hero_Minimal_Radius)
            {
                transform.localScale -= Sphere_Scale;
                Sphere_Line.transform.localScale = new Vector3(transform.localScale.x / 10f, Sphere_Line.transform.localScale.y, Sphere_Line.transform.localScale.z);
                Pre_Shoot_Sphere_Radius += scale_dicrement;
                Pre_Shoot_Sphere_Radius += scale_dicrement;
            }
            else
            {
                Game_Over();
            }
            yield return null;
        }
        Pre_Shoot_Sphere.transform.position = Vector3.zero;
        Pre_Shoot_Sphere.transform.localScale = Vector3_With_One_Parameter(Pre_Shoot_Sphere_Start_Radius);
        Current_Shoot_Coroutine = StartCoroutine(Start_Shoot());

        Curent_Preview_Coroutine = null;
        yield break;
    }

    private IEnumerator Start_Shoot()
    {
        Shoot_Obj = Instantiate(Shoot_Prefab, transform.position, new Quaternion());
        Shoot_Obj.transform.localScale = Vector3_With_One_Parameter(Pre_Shoot_Sphere_Radius);
        Shoot_Prefab_Script Shoot_Obj_Shoot_Prefab_Script = Shoot_Obj.GetComponent<Shoot_Prefab_Script>();
        Shoot_Obj_Shoot_Prefab_Script.End_Point = Shoot_Hit.point;
        Shoot_Obj_Shoot_Prefab_Script.Selectable = Last_Selectable;

        float lerp_index = 0;
        float Animation_Duration = 1.5f;
        while (Shoot_Obj.gameObject)//Shoot move by lerp
        {
            Shoot_Obj.transform.position = Vector3.Lerp(transform.position, Shoot_Obj_Shoot_Prefab_Script.End_Point, lerp_index);
            lerp_index += Time.deltaTime / Animation_Duration;
            yield return null;
        }
        Collider_Detector_Obj.Can_Win = true;
        Pre_Shoot_Sphere_Radius = Pre_Shoot_Sphere_Start_Radius;
        Current_Shoot_Coroutine = null;
        yield break;
    }

    private void Game_Over()
    {
        Collider_Detector_Obj.Win = false;
        Game_Over_Layer.SetActive(true);
    }

    private void Level_Complete()
    {
        gameObject.GetComponent<Animator>().Play("Win_Animation");
    }

    private void Win_Anim_Complite_Check()
    {
        Level_Complete_Layer.SetActive(true);
    }

    private Vector3 Vector3_With_One_Parameter(float Parameter)
    {
        return new Vector3(Parameter, Parameter, Parameter);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Shoot_Hit.point, Pre_Shoot_Sphere_Radius);
        if (Current_Shoot_Coroutine == null)
        {
            Pre_Shoot_Sphere.transform.position = Shoot_Hit.point;
            Pre_Shoot_Sphere.transform.localScale = Vector3_With_One_Parameter(Pre_Shoot_Sphere_Radius);
        }
    }
}