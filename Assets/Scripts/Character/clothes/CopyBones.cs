//Script for video tutorial https://www.youtube.com/watch?v=sc6kq1TV8Ek by TheoremGames
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CopyBones : MonoBehaviour
{

    public GameObject SourceObject;

    // Use this for initialization
    void Start()
    {
        Copy();
    }

    void Copy()
    {
        if (SourceObject == null) return;

        var sourceRenderer = SourceObject.GetComponent<SkinnedMeshRenderer>();
        var targetRenderer = GetComponent<SkinnedMeshRenderer>();

        if (sourceRenderer == null) return;
        if (targetRenderer == null) return;

        targetRenderer.bones =
            sourceRenderer.bones.Where(b => targetRenderer.bones.Any(t => t.name == b.name)).ToArray();


    }
}