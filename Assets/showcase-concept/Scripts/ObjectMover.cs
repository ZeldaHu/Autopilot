using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject startGameObject;
    public GameObject endGameObject;
    public GameObject objectPrefab;
    public float instantiateInterval = 1f; // Time between each instantiation
    public int maxInstantiations = 10; // Maximum number of GameObjects to instantiate
    public float moveSpeed = 5f; // Speed at which objects move from start to end
    public float alphaMultiplier = 1f; // Multiplier for alpha fading effect

    private int currentInstantiations = 0;
    private float timer = 0f;

    void Start()
    {
        // Start the instantiation process
        InvokeRepeating("InstantiateObject", 0, instantiateInterval);
    }

    void Update()
    {
        // Cancel repeating calls if the maximum number of instantiations is reached
        if (currentInstantiations >= maxInstantiations)
        {
            CancelInvoke("InstantiateObject");
        }
    }

    void InstantiateObject()
    {
        if (currentInstantiations < maxInstantiations)
        {
            GameObject obj = Instantiate(objectPrefab, startGameObject.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform); // Set the instantiated object as a child of the ObjectMover
            SpriteFader fader = obj.AddComponent<SpriteFader>(); // Add SpriteFader component to handle movement and fading
            fader.startGameObject = startGameObject;
            fader.endGameObject = endGameObject;
            fader.moveSpeed = moveSpeed;
            fader.alphaMultiplier = alphaMultiplier;
            currentInstantiations++;
        }
    }
}
public class SpriteFader : MonoBehaviour
{
    public GameObject startGameObject;
    public GameObject endGameObject;
    public float moveSpeed = 5f;
    public float alphaMultiplier = 1f; // Use this to tune the transparency effect

    private SpriteRenderer spriteRenderer;
    private float fullDistance;

    void Start()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>()){
  spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    
        fullDistance = Vector3.Distance(startGameObject.transform.position, endGameObject.transform.position);
    }

    void Update()
    {
        MoveAndFade();
    }

void MoveAndFade()
{
    // Ensure distance is calculated correctly with respect to its scope
    float totalDistance = Vector3.Distance(startGameObject.transform.position, endGameObject.transform.position);
    
    // Move towards the end GameObject
    float step = moveSpeed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, endGameObject.transform.position, step);

    // Calculate distance from the closest point (start or end)
    float distanceToStart = Vector3.Distance(transform.position, startGameObject.transform.position);
    float distanceToEnd = Vector3.Distance(transform.position, endGameObject.transform.position);
    float closerDistance = Mathf.Min(distanceToStart, distanceToEnd);

    // Adjust alpha based on proximity to the closest point, making it more transparent as it gets closer
    // Now correctly using totalDistance for relative calculations
    float midpointDistance = totalDistance / 2f; // Halfway point between start and end
    float alpha = (closerDistance / midpointDistance) * alphaMultiplier;
    alpha = Mathf.Clamp(alpha, 0f, 1f); // Ensure alpha remains within valid range

    if (spriteRenderer!=null)
    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

    // If the GameObject reaches the end position, move it back to the start
    if (transform.position == endGameObject.transform.position)
    {
        transform.position = startGameObject.transform.position;
    }
}

}


