using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// 画像マーカー複数対応のサンプル
/// </summary>
public class MultiMarker : MonoBehaviour
{
    /// <summary>
    /// マーカー用オブジェクトのプレハブ
    /// </summary>
    [SerializeField] private GameObject[] _arPrefabs;

    /// <summary>
    /// ARTrackedImageManager
    /// </summary>
    [SerializeField] private ARTrackedImageManager _imageManager;

    /// <summary>
    /// マーカー用オブジェクトのプレハブと文字列を紐づけた辞書
    /// </summary>
    private readonly Dictionary<string, GameObject> _markerNameAndPrefabDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        _imageManager.trackedImagesChanged += OnTrackedImagesChanged;

        //辞書を作る 画像の名前とARオブジェクトのPrefabを紐づける
        for (var i = 0; i < _arPrefabs.Length; i++)
        {
            var arPrefab = Instantiate(_arPrefabs[i]);
            _markerNameAndPrefabDictionary.Add(_imageManager.referenceLibrary[i].name, arPrefab);
            arPrefab.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    /// <summary>
    /// 認識した画像マーカーに応じて紐づいたARオブジェクトを表示
    /// </summary>
    /// <param name="trackedImage">認識した画像マーカー</param>
    private void ActivateARObject(ARTrackedImage trackedImage)
    {
        //認識した画像マーカーの名前を使って辞書から任意のオブジェクトを引っ張り出す
        var arObject = _markerNameAndPrefabDictionary[trackedImage.referenceImage.name];
        var imageMarkerTransform = trackedImage.transform;

        //位置合わせ
        var markerFrontRotation = imageMarkerTransform.rotation * Quaternion.Euler(90f, 0f, 0f);
        arObject.transform.SetPositionAndRotation(imageMarkerTransform.transform.position, markerFrontRotation);
        arObject.transform.SetParent(imageMarkerTransform);

        //トラッキングの状態に応じてARオブジェクトの表示を切り替え
        arObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
    }

    /// <summary>
    /// TrackedImagesChanged時の処理
    /// </summary>
    /// <param name="eventArgs">検出イベントに関する引数</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            ActivateARObject(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            ActivateARObject(trackedImage);
        }
    }
}