using System.Collections;
using UnityEngine;

public class HandleLaneChange : MonoBehaviour
{
    public CameraManager cameraManager;
    public Animator animator;
    public void HandleLanePressButton()
    {
        StopAllCoroutines();
        StartCoroutine(HandleLaneChangeCoroutine());

    }

    IEnumerator HandleLaneChangeCoroutine()
    {
        animator.enabled = true;
        yield return new WaitForSeconds(5f);
        cameraManager.Next();
        animator.enabled = false;
    }
}
