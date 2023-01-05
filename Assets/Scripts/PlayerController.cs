using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform shotPoint = default;

    [SerializeField]
    private GameObject arrowPrefab = default;

    [SerializeField]
    private float shotSpeed = 25f;

    // public TextMeshProUGUI text1 = null;

    private Camera arCamera = default;

    public GameObject target = default;

    public float throwingAngle = default;

    private bool touchFlag;

    [SerializeField]
    private ARTrackedImageManager arTrackedImageManager;

    private void Awake()
    {
        // イベントハンドラーの登録
        arTrackedImageManager.trackedImagesChanged += OnTargetActive;
    }

    private void Start()
    {
        arCamera = Camera.main;

        // InvokeRepeating("Shot", 0f, 1f);
    }

    private void Update()
    {
        // エディタ
        if (Application.isEditor)
        {
            // 押した瞬間
            if (Input.GetMouseButtonDown(0))
            {
                this.touchFlag = true;
                Shot();
            }
        }
        // 端末
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    this.touchFlag = true;
                    Shot();
                }
            }
        }

    }

    private void Shot()
    {
        var arrow = Instantiate(arrowPrefab, shotPoint.position, shotPoint.rotation);
        var arrowRigidBody = arrow.GetComponent<Rigidbody>();
        var force = shotPoint.forward * shotSpeed;
        float angle = throwingAngle;
        
        Vector3 velocity = CalculateVelocity(shotPoint.position, target.transform.position, angle);
        velocity = (target.transform.position - shotPoint.position).normalized * shotSpeed;
        Debug.Log("Shot----------");
        Debug.Log($"Point: {shotPoint.position}, Target: {target.transform.position}, Velocity: {velocity}");
        arrowRigidBody.AddForce(velocity, ForceMode.VelocityChange);

        //if (target.activeSelf)
        //{
        //    arrowRigidBody.AddForce(velocity, ForceMode.VelocityChange);
        //}
        //else
        //{
        //    arrowRigidBody.AddForce(force, ForceMode.VelocityChange);
        //}

        // n秒後に弓の消去
        Destroy(arrow, 3.0f);
        this.touchFlag = false;
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }

    public void OnTargetActive(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 初めて認識した時
        foreach (var trackedImage in eventArgs.added)
        {
            target.SetActive(true);
            target.transform.position = trackedImage.transform.position;
            target.transform.SetParent(trackedImage.transform);
            Debug.Log("-----------------認識した----------------------");
        }

        // trackingしているとき
        foreach (var trackedImage in eventArgs.updated)
        {
            target.SetActive(true);
            target.transform.position = trackedImage.transform.position;
            target.transform.SetParent(trackedImage.transform);
            Debug.Log($"arCameraPos：{arCamera.transform.position}, makerPos：{trackedImage.transform.position}");
        }
    }

    private void StatusCheck(ARTrackedImage trackedImage)
    {
        //トラッキングの状態に応じてTargetを非表示
        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            target.SetActive(false);
            return;
        }

        //var trackingPos = trackedImage.transform.position;
        //target.transform.position = new Vector3(trackingPos.x, trackingPos.y, 10);

        //var dic = target.transform.position;

        Debug.Log($"!!!!!!!!!---------認識中----------!!!!!!!!!：");
        // Debug.Log($"targetPos：{dic}");
        Debug.Log($"arCameraPos：{arCamera.transform.position}");
    }
}