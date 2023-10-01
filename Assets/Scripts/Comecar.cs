using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comecar : MonoBehaviour
{
    [SerializeField] Grid grid;

    private void Start()
    {
        grid.DesenharGrid();
    }
}
