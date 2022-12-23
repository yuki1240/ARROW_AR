using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// �摜�}�[�J�[�����Ή��̃T���v��
/// </summary>
public class MultiMarker : MonoBehaviour
{
    /// <summary>
    /// �}�[�J�[�p�I�u�W�F�N�g�̃v���n�u
    /// </summary>
    [SerializeField] private GameObject[] _arPrefabs;

    /// <summary>
    /// ARTrackedImageManager
    /// </summary>
    [SerializeField] private ARTrackedImageManager _imageManager;

    /// <summary>
    /// �}�[�J�[�p�I�u�W�F�N�g�̃v���n�u�ƕ������R�Â�������
    /// </summary>
    private readonly Dictionary<string, GameObject> _markerNameAndPrefabDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        _imageManager.trackedImagesChanged += OnTrackedImagesChanged;

        //��������� �摜�̖��O��AR�I�u�W�F�N�g��Prefab��R�Â���
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
    /// �F�������摜�}�[�J�[�ɉ����ĕR�Â���AR�I�u�W�F�N�g��\��
    /// </summary>
    /// <param name="trackedImage">�F�������摜�}�[�J�[</param>
    private void ActivateARObject(ARTrackedImage trackedImage)
    {
        //�F�������摜�}�[�J�[�̖��O���g���Ď�������C�ӂ̃I�u�W�F�N�g����������o��
        var arObject = _markerNameAndPrefabDictionary[trackedImage.referenceImage.name];
        var imageMarkerTransform = trackedImage.transform;

        //�ʒu���킹
        var markerFrontRotation = imageMarkerTransform.rotation * Quaternion.Euler(90f, 0f, 0f);
        arObject.transform.SetPositionAndRotation(imageMarkerTransform.transform.position, markerFrontRotation);
        arObject.transform.SetParent(imageMarkerTransform);

        //�g���b�L���O�̏�Ԃɉ�����AR�I�u�W�F�N�g�̕\����؂�ւ�
        arObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
    }

    /// <summary>
    /// TrackedImagesChanged���̏���
    /// </summary>
    /// <param name="eventArgs">���o�C�x���g�Ɋւ������</param>
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