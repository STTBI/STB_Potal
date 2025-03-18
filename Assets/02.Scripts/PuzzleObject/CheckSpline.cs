using UnityEngine;

public class CheckSpline : MonoBehaviour , ICheckObj
{
    public bool isTrue { get; set; }
    private Material _material;

    private void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _material.color = Color.red;
    }
    public void False()
    {
        _material.color = Color.red;
        isTrue = false;
    }

    public void True()
    {
        _material.color = Color.green;
        isTrue = true;
    }
}
