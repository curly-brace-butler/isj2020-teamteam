using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShake : MonoBehaviour
{

    /*    // Link to post-processing volume object in editor
        public GameObject postObject;

        private ChromaticAberration caLayer = null;

        public void Awake()
        {
            PostProcessVolume volume = postObject.GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings(out caLayer);
        }
    */


    public Transform cameraTransform;

    public void DuplicateHasBeenKilled()
    {
        StartCoroutine(this.Shake(.15f, .4f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Debug.Log("Trigger camera shake duration: " + duration);
        // Save the original position of the camera so we can reset. Then, start a timer.
        Vector3 originalPosition = cameraTransform.localPosition;
        // TODO: Save the post-processing volume's chromatic abberation intensity
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Offset the camera to a randomized position (x,y)
            cameraTransform.localPosition = new Vector3(x, y, originalPosition.z);

            // TODO: Offset the post-processing volume's chromatic abberation intensity

            // Halt until frame rendered
            elapsed += Time.deltaTime;
            Debug.Log("Elapsed: " + elapsed + "/" + duration);
            yield return null;
        }

        // Reset to original position and chromatic abberation intensity
        cameraTransform.localPosition = originalPosition;
        Debug.Log("Camera reset to original pos");

        // TODO: Reset the abberation intensity
    }
}
