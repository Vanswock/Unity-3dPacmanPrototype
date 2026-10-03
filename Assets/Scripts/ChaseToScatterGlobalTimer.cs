using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ChaseToScatterGlobalTimer : MonoBehaviour
{
    public string mode { get; private set; } = "Scatter";
    float scatterTimer = 15f;
    float chaseTimer = 15f;


    private void Start()
    {
        StartCoroutine(GhostModeLoop());
    }
    IEnumerator GhostModeLoop()
    {
        while (true)
        {

            if (scatterTimer >= 1)
            { 
                    mode = "Scatter";
                yield return new WaitForSeconds(scatterTimer);

                mode = "Chase";
                yield return new WaitForSeconds(chaseTimer);
                // Make the next Scatter period half as long.

                scatterTimer /= 2f;
            }
            if(scatterTimer < 1)
            {
                mode = "Chase";
                yield break;
            }



        }
    }
}
