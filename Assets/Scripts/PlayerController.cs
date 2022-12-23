using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null;

    // [SerializeField] private ARTrackedImageManager imageManager = null;

    // public TextMeshProUGUI text1 = null;

    private Camera arCamera = null;

    private void Start()
    {
        arCamera = Camera.main;

        InvokeRepeating("Shot", 0f, 1f);
    }

    private void Update()
    {

    }

    private void Shot()
    {
        GameObject arrow = Instantiate(arrowPrefab, arCamera.transform.position, arCamera.transform.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        // 弓の速度を設定
        arrowRb.AddForce(arCamera.transform.forward * 5, ForceMode.Impulse);

        // n病後に弓の消去
        Destroy(arrow, 3.0f);
    }
}