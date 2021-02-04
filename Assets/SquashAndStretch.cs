using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteAlways]
public class SquashAndStretch : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private int smoothingItterations;
    private Vector3 previousPosition;

    List<Vector3> previousScales;

    private void Start()
    {
        previousScales = new List<Vector3>();
    }

    private void Update()
    {
        var positionDiff = previousPosition - transform.position;

        positionDiff = new Vector3(Mathf.Abs(positionDiff.x), Mathf.Abs(positionDiff.y), Mathf.Abs(positionDiff.z));
        var volumePreservation = 1f / (positionDiff.magnitude * amount + 1);

        previousScales.Add(Vector3.one * volumePreservation + Vector3.Scale(positionDiff * amount, positionDiff * amount));
        if (previousScales.Count > smoothingItterations)
            previousScales.RemoveAt(0);

        var avgScale = Vector3.zero;
        foreach (var scale in previousScales)
        {
            avgScale += scale;
        }
        avgScale /= previousScales.Count;

        transform.localScale = avgScale;
        previousPosition = transform.position;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }
}