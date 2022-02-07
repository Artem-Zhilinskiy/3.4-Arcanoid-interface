using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControls : MonoBehaviour
{

    //Объявить делегат для события
    public delegate void SphereLeftDelegate();

    //Объявление события
    public event SphereLeftDelegate SphereLeftEvent;

    [Header("Шар"), SerializeField]
    public Transform _sphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Регистрация удара
    private void OnTriggerEnter(Collider _mustBeSphereCollider)
    {
        if ((_sphere.GetComponent<Collider>() == _mustBeSphereCollider) && (SphereLeftEvent != null))
        {
            SphereLeftEvent();
        }

    }
}
