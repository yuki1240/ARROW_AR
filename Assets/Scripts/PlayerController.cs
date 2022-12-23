using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null;

    [SerializeField] private ARTrackedImageManager imageManager = null;

    public TextMeshProUGUI text1 = null;

    private Camera arCamera = null;

    private void Start()
    {
        arCamera = Camera.main;

        // マーカー認識時のイベントハンドラーの設定
        // ARTrackedImagesChangedEventArgsクラスが取得可能
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;

        InvokeRepeating("Shot", 0f, 1f);
    }

    private void Update()
    {
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Ended)
        {
            return;
        }

        // GameObject arrowObj = Instantiate(arrow, arCamera.transform.position, arCamera.transform.rotation);
        // arrowObj.transform.parent = arCamera.transform;

        //arrowRb = arrowObj.GetComponent<Rigidbody>();
        //arrowRb.AddForce(arCamera.transform.forward * 5, ForceMode.Impulse);
    }

    private void Shot()
    {
        GameObject arrow = Instantiate(arrowPrefab, arCamera.transform.position, arCamera.transform.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        // 弓の速度を設定
        arrowRb.AddForce(transform.forward * 5);

        // 5秒後に砲弾を破壊する
        Destroy(arrow, 5.0f);
    }



















    /// <summary>
    /// 認識した画像マーカーの場所を取得
    /// </summary>
    /// <param name="trackedImage">認識した画像マーカーの情報</param>
    private Vector3 GetMarkerPosition(ARTrackedImage trackedImage)
    {
        var markerPos = trackedImage.transform.position;
        return markerPos;
    }

    /// <summary>
    /// 弓矢の発射処理
    /// </summary>
    /// <param name="trackedImage">認識した画像マーカーの情報</param>
    private void ArrowFire(ARTrackedImage trackedImage)
    {
        Debug.Log($"MarkerPos：{GetMarkerPosition(trackedImage)}");
        text1.text = $"MarkerPos：{GetMarkerPosition(trackedImage)}";
    }

    /// <summary>
    /// TrackedImagesChanged時の処理
    /// </summary>
    /// <param name="eventArgs">検出イベントに関する引数</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // マーカーを初めて認識したとき
        foreach (var trackedImage in eventArgs.added)
        {
            ArrowFire(trackedImage);
        }

        // 同じマーカーを認識したとき（2回目以降）
        foreach (var trackedImage in eventArgs.updated)
        {
            ArrowFire(trackedImage);
        }
    }
}
