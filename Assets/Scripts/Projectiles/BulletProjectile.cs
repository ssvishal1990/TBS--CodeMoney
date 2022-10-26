using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] GameObject trailRendererGameObject;
    [SerializeField] Transform bulletHitVFXPrefab;
    float moveSpeed;
    private Vector3 targetPosition;


    public void Setup(Vector3 targetPosition, float moveSpeed)
    {
        this.targetPosition = targetPosition;
        this.moveSpeed = moveSpeed;
    }

    private void Update()
    {

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRendererGameObject.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
        }
    }
}
