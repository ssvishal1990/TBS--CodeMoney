using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    private Vector3 targetPosition;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (!checkIfReached())
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));

        }

    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private bool checkIfReached()
    {
        int distance =  (int)Mathf.Round(Vector3.Distance(transform.position, targetPosition));
        if (distance == 0)
        {
            return true;
        }else
        {
            return false;
        }
    }
}
