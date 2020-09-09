using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockPlacer : MonoBehaviour
{
    public int rows;

    public int columns;
    // Start is called before the first frame update
    void Start()
    {
        PlaceBlocks();
    }

    public void Reset()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        PlaceBlocks();
    }

    public void PlaceBlocks()
    {
        for (int j=0;j<columns;j++)
        {
        
            float chance = Mathf.Pow(j,2f) * .02f;
            float chance2 = Mathf.Pow(j-5,1.7f) * .02f;
            for (int i=0;i<rows;i++)
            {
                //look upon this ungodly line of code and despair
                Instantiate(Resources.Load((Random.Range(0f, 1f) < chance) ? ((Random.Range(0f, 1f) < chance2) ? "Block2" : "Block1") : "Block"), new Vector3(i-Mathf.Floor(rows/2), j*.25f+1, 0), quaternion.identity, transform);
            }
        }
    }
}
