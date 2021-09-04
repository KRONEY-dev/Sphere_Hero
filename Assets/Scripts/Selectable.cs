using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private Material material;
    private new Renderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    public void Select()
    {
        renderer.material.color = Color.yellow;
    }

    public void Deselect()
    {
        renderer.material = material;
    }
}
