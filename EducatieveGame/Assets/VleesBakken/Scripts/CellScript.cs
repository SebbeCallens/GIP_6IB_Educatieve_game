using UnityEngine;

public class CellScript : MonoBehaviour
{
    private void OnMouseDown() //vlees aanklikken
    {
        if (transform.childCount > 0 && Time.timeScale != 0)
        {
            GetComponentInChildren<MeatScript>().Clicked();
        }
    }
}
