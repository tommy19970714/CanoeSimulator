using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to your non-flat water system.
/// </summary>
public class WaterInterface : MonoBehaviour {

    // Replace "WaterSystem" with your type.

    // WaterSystem myWaterSystem;

    private void Start()
    {
        // Replace the following line with the one corresponding to your type.

        // myWaterSystem = GetComponent<WaterSystem>();
    }

    /// <summary>
    /// Return water height y in world coordinates at world point x, z
    /// </summary>
    public float GetWaterHeightAtLocation(float x, float z)
    {
        // Replace the "return 0" statement with the function from your water system, e.g:

        // return myWaterSystem.GetWaterHeightAtLocation(float x, float z);

        // Do not forget to uncheck "Flat Water" under Additional Options of FloatingObject.

        return 0;
    }

}
