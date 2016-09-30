using UnityEngine;
using System.Collections;


public class FloatAround : MonoBehaviour
{
    public float Amplitude = 0.1f;          //Set in Inspector 
    public float SpeedX = 1.5f;
    public float SpeedY = 1.5f; //Set in Inspector 
    private float _tempX;
    private float _tempY;
    private Vector3 _tempPos;

    void Start()
    {
        _tempX = transform.position.x;
        _tempY = transform.position.y;

        Amplitude = Random.Range(0f, 0.1f);
        SpeedX = Random.Range(-3f, 3f);
        SpeedY = Random.Range(-3f, 3f);
    }

    void Update()
    {
        _tempPos.y = _tempY + Amplitude * Mathf.Sin(SpeedX * Time.time);
        _tempPos.x = _tempX + Amplitude * Mathf.Sin(SpeedY * Time.time);
        transform.position = _tempPos;
    }
}
