using System.Collections;
using UnityEngine;

[System.Serializable] // Makes it visible in the Unity Inspector
public class CameraStage
{
    // public Vector3 position;
    // public Quaternion rotation;
    // public float fieldOfView;
    //  public float orthographicSize;
    // public bool orthographic;
    public Camera cameraReference;
    public GameObject[] objectToActivate;
}

public class CameraManager : MonoBehaviour
{
    public CameraStage[] stages; // Array of stages
    public int currentStageIndex = 0; // Current stage index
    public Camera mainCamera; // Reference to the main camera
    private bool isTransitioning = false; // To prevent calling Next() while already transitioning

    // Assuming a fixed time for interpolation for simplicity
    public float transitionDuration = 2.0f;

    [ContextMenu("Next")]
    public void Next()
    {
        if (isTransitioning || stages.Length == 0)
            return; // Prevent updates during transitions or if there are no stages

        isTransitioning = true;
        int nextStageIndex = (currentStageIndex + 1) % stages.Length; // Loop back to 0 if at the last stage

        StartCoroutine(TransitionToStage(nextStageIndex));
    }

    private IEnumerator TransitionToStage(int nextStageIndex)
    {
        CameraStage currentStage = stages[currentStageIndex];
        CameraStage nextStage = stages[nextStageIndex];

        // Deactivate the current object
             for (int i = 0; i < currentStage.objectToActivate.Length; i++)
        {
            currentStage.objectToActivate[i].SetActive(false);
        }



        float elapsedTime = 0;
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float startFOV = mainCamera.fieldOfView;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                nextStage.cameraReference.transform.position,
                t
            );
            mainCamera.transform.rotation = Quaternion.Lerp(
                startRotation,
                nextStage.cameraReference.transform.rotation,
                t
            );
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, nextStage.cameraReference.fieldOfView, t);
            mainCamera.orthographic = nextStage.cameraReference.orthographic;
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = nextStage.cameraReference.orthographicSize;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Directly set to ensure accuracy
        mainCamera.transform.position = nextStage.cameraReference.transform.position;
        mainCamera.transform.rotation = nextStage.cameraReference.transform.rotation;
        mainCamera.fieldOfView = nextStage.cameraReference.fieldOfView;
        mainCamera.orthographic = nextStage.cameraReference.orthographic;
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = nextStage.cameraReference.orthographicSize;
        }
       

        for (int i = 0; i < nextStage.objectToActivate.Length; i++)
        {
            nextStage.objectToActivate[i].SetActive(true);
        }

        currentStageIndex = nextStageIndex;
        isTransitioning = false;
    }
}
