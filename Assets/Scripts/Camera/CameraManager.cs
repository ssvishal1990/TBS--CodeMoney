using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void showActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void hideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
