using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFade : MonoBehaviour, IPooledObject
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _fadeAmount;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _lerpRandomness;
    private float _startWidth;
    private float _endWidth;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private void Awake()
    {
        _startWidth = _line.startWidth;
        _endWidth = _line.endWidth;
    }

    public void SetPositions(Vector3 pos0, Vector3 pos1)
    {
        _startPoint = pos0;
        _endPoint = pos1;

        _line.SetPosition(0, _startPoint);
        _line.SetPosition(1, _endPoint);


        ResetProperties();
    }

    public void OnObjectInstantiated()
    {
        _lerpSpeed += _lerpSpeed * Random.Range(-_lerpRandomness, _lerpRandomness);
        float lifetime = 1f;
        Invoke("Death", lifetime);
    }
    private void Update()
    {
        Fade();
        LerpStartPoint();
    }
    private void ResetProperties()
    {
        _line.startWidth = _startWidth;
        _line.endWidth = _endWidth;
        transform.position = Vector3.zero;
    }
    private void Death() => gameObject.SetActive(false);
    private void Fade()
    {
        _line.startWidth /= _fadeAmount;
        _line.endWidth /= _fadeAmount;
    }
    private void LerpStartPoint()
    {
        _startPoint = Vector3.MoveTowards(_startPoint, _endPoint, _lerpSpeed * Time.deltaTime);
        _line.SetPosition(0, _startPoint);
    }
}
