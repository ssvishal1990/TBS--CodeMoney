using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float zoomAmount = 1f;
    [SerializeField] float zoomSpeed = 2f;


    [SerializeField] private Vector3 targetFollowOffSet;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCammera;


    private const float MIN_FOLLOW_Y_CONSTANT = 2f;
    private const float MAX_FOLLOW_Y_CONSTANT = 12f;

    private CinemachineTransposer cinemachineTransposer;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCammera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffSet = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        CameraMovement();
        CameraRotation();
        CameraZoom();
    }

    private void CameraZoom()
    {
        
        Vector3 followOffset = cinemachineTransposer.m_FollowOffset;


        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffSet.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffSet.y += zoomAmount;
        }
        followOffset.y = Mathf.Clamp(targetFollowOffSet.y, MIN_FOLLOW_Y_CONSTANT, MAX_FOLLOW_Y_CONSTANT);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset,
                                                            targetFollowOffSet, 
                                                            Time.deltaTime *zoomSpeed);
    }

    private void CameraRotation()
    {
        Vector3 rotationVector = new Vector3();
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += -1f;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void CameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputDir.z += -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputDir.x += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDir.x += 1f;
        }

        Vector3 moveVector = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
