using UnityEngine;

public class HandZoneRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}
