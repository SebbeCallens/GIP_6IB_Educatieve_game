using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelBoxScript : MonoBehaviour
{
    private GameObject[,] _slots;

    public GameObject[,] Slots
    {
        get { return _slots; }
        set { _slots = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuildSlotMatrix(int row, int col)
    {
        _slots = new GameObject[row, col];
    }
}
