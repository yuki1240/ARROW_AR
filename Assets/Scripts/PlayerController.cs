using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bow = null;

    [SerializeField] private ARTrackedImageManager imageManager = null;

    public TextMeshProUGUI text1 = null;

    private Camera arCamera = null;

    // private Vector3 markerPos = Vector3.zero;

    private void Start()
    {
        arCamera = Camera.main;

        // �}�[�J�[�F�����̃C�x���g�n���h���[�̐ݒ�
        // ARTrackedImagesChangedEventArgs�N���X���擾�\
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void Update()
    {
        var offset = arCamera;


        var camForwardPos = arCamera.transform.forward;
        bow.transform.position = camForwardPos;

        var camRot = arCamera.transform.rotation;
        bow.transform.rotation = camRot;

    }

    /// <summary>
    /// �F�������摜�}�[�J�[�̏ꏊ���擾
    /// </summary>
    /// <param name="trackedImage">�F�������摜�}�[�J�[�̏��</param>
    private Vector3 GetMarkerPosition(ARTrackedImage trackedImage)
    {
        var markerPos = trackedImage.transform.position;
        return markerPos;
    }

    /// <summary>
    /// �|��̔��ˏ���
    /// </summary>
    /// <param name="trackedImage">�F�������摜�}�[�J�[�̏��</param>
    private void ArrowFire(ARTrackedImage trackedImage)
    {
        Debug.Log($"MarkerPos�F{GetMarkerPosition(trackedImage)}");
        text1.text = $"MarkerPos�F{GetMarkerPosition(trackedImage)}";
    }

    /// <summary>
    /// TrackedImagesChanged���̏���
    /// </summary>
    /// <param name="eventArgs">���o�C�x���g�Ɋւ������</param>
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // �}�[�J�[�����߂ĔF�������Ƃ�
        foreach (var trackedImage in eventArgs.added)
        {
            ArrowFire(trackedImage);
        }

        // �����}�[�J�[��F�������Ƃ��i2��ڈȍ~�j
        foreach (var trackedImage in eventArgs.updated)
        {
            ArrowFire(trackedImage);
        }
    }
}
