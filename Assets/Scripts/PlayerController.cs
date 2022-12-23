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

        // �}�[�J�[�F�����̃C�x���g�n���h���[�̐ݒ�
        // ARTrackedImagesChangedEventArgs�N���X���擾�\
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

        // �|�̑��x��ݒ�
        arrowRb.AddForce(transform.forward * 5);

        // 5�b��ɖC�e��j�󂷂�
        Destroy(arrow, 5.0f);
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
