using UnityEngine;

public class CellScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (transform.childCount > 0)
        {
            GetComponentInChildren<MeatScript>().Clicked();
        }
    }
}
