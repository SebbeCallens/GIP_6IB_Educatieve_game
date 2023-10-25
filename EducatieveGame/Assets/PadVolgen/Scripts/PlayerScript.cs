using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerScript : MonoBehaviour
{
    private bool _isMoving;
    private Vector2 _origPos, _targetPos;
    private float _timeToMove = 0.2f;
    [SerializeField] private GameObject _tile;

    public bool GetIsMoving()
    {
        return _isMoving;
    }

    public Vector2 GetOrigPos()
    {
        return _origPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !_isMoving)
        {
            StartCoroutine(MovePlayer(Vector2.up));
        }
        if (Input.GetKey(KeyCode.A) && !_isMoving)
        {
            StartCoroutine(MovePlayer(Vector2.left));
        }
        if (Input.GetKey(KeyCode.S) && !_isMoving)
        {
            StartCoroutine(MovePlayer(Vector2.down));
        }
        if (Input.GetKey(KeyCode.D) && !_isMoving)
        {
            StartCoroutine(MovePlayer(Vector2.right));
        }
    }

    public IEnumerator MovePlayer(Vector2 direction)
    {
        _isMoving = true;

        float elapsedTime = 0;

        _origPos = transform.position;
        _targetPos = _origPos + (direction);
        if ((_targetPos.x >= 0 && _targetPos.x <= 7) && (_targetPos.y >= 0 && _targetPos.y <= 7))
        {
            while (elapsedTime < _timeToMove)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(_origPos, _targetPos, (elapsedTime / _timeToMove));
                yield return null;
            }

            transform.position = _targetPos;
        }

        _isMoving = false;
    }
}
