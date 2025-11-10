using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    //[SerializeField] private EdgeCollider2D _collider;

    private readonly List<Vector2> _points = new List<Vector2>();
    void Start()
    {
        //_collider.transform.position -= transform.position;
    }


    void Update()
    {

    }

    public void SetPosition(Vector3 pos)
    {
        if (!CanAppend(pos)) return;

        _points.Add(pos);

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, pos);

        //_renderer.Simplify(1);

        //_collider.points = _points.ToArray();
    }

    public List<Vector2> GetPoints()
    {
        return _points;
    }

    public void DeletePosition(Vector2 vecToDestroy)
    {
        _points.Remove(vecToDestroy);
    }

    private bool CanAppend(Vector2 pos)
    {
        if (_renderer.positionCount == 0) return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), pos) > DrawController.RESOLUTION;
    }
}
