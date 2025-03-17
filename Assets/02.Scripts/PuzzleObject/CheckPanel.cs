using UnityEngine;

public class CheckPanel : MonoBehaviour, ICheckObj
{
    public Material[] mat = new Material[2];

    public bool isTrue { get; set; }
    private Material _material;

    private void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<MeshRenderer>().material = mat[0];
        _material.color = Color.red;
    }

    public void False()
    {
        gameObject.GetComponent<MeshRenderer>().material = mat[0];
        _material.color = Color.red;
        isTrue = false;
    }

    public void True()
    {
        gameObject.GetComponent<MeshRenderer>().material = mat[1];
        _material.color = Color.green;
        isTrue = true;
    }
}
