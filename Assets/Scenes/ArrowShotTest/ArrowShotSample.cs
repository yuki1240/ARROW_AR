using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShotSample : MonoBehaviour
{
    [SerializeField]
    private Transform shotPoint = default;
    [SerializeField]
    private GameObject arrowPrefab = default;
    [SerializeField]
    private float shotSpeed = 25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }

    private void Shot()
    {
        var arrow = Instantiate(arrowPrefab, shotPoint.position, shotPoint.rotation);
        var arrowRigidBody = arrow.GetComponent<Rigidbody>();
        var force = shotPoint.forward * shotSpeed;
        arrowRigidBody.AddForce(force, ForceMode.VelocityChange);
    }
}
