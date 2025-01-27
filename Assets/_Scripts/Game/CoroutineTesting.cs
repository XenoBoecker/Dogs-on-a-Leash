using System.Collections;
using UnityEngine;

public class CoroutineTesting : MonoBehaviour
{
    void Start()
    {
        // Start the main coroutine
        StartCoroutine(MainCoroutine());
    }

    // Main Coroutine
    private IEnumerator MainCoroutine()
    {
        Debug.Log("Starting Main Coroutine");

        // Call the first sub-coroutine
        yield return StartCoroutine(FirstStep());

        // Call the second sub-coroutine
        yield return StartCoroutine(SecondStep());

        // Call the third sub-coroutine
        yield return StartCoroutine(ThirdStep());

        Debug.Log("Main Coroutine Finished");
    }

    private IEnumerator FirstStep()
    {
        Debug.Log("First Step Started");
        yield return new WaitForSeconds(1f); // Simulate some work
        Debug.Log("First Step Completed");
    }

    private IEnumerator SecondStep()
    {
        Debug.Log("Second Step Started");
        yield return new WaitForSeconds(2f); // Simulate more work
        Debug.Log("Second Step Completed");
    }

    private IEnumerator ThirdStep()
    {
        Debug.Log("Third Step Started");
        yield return new WaitForSeconds(1.5f); // Simulate final work
        Debug.Log("Third Step Completed");
    }
}