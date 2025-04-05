using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnAnimal : MonoBehaviour
{
    public List<GameObject> Animals; // List of animals to spawn
    public int animal; // Index of the current animal
    public TextMeshProUGUI scoreText;
    private GameObject currentAnimal;
    private bool isMovingToMouse = false;
    private Vector3 targetPosition; // Target position for the animal to move to
    public float moveSpeed = 8f;
    private bool isResetting = false;

    void Start()
    {
        Spawn(); // Spawn an animal at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isResetting)
        {
            ResetGame(); // Reset the game when R is pressed
        }

        if (Input.GetMouseButtonDown(0) && currentAnimal != null && !isMovingToMouse)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0f;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            worldMousePos.z = 0f;
            // Set the target position to the mouse position

            targetPosition = new Vector3(worldMousePos.x, currentAnimal.transform.position.y, 0f);
            isMovingToMouse = true; // Start moving the animal to the mouse position
        }

        if (isMovingToMouse && currentAnimal != null)
        {
            // Move the animal towards the target position
            Vector3 newPosition = Vector3.MoveTowards(currentAnimal.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            newPosition.z = 0f;
            currentAnimal.transform.position = newPosition;


            // Check if the animal has reached the target position
            if (Mathf.Abs(currentAnimal.transform.position.x - targetPosition.x) < 0.01f)
            {
                isMovingToMouse = false;

                // Unfreeze animal
                Rigidbody2D rb = currentAnimal.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                }

                // Spawn a new animal after a delay
                Invoke("Spawn", 0.5f);
            }
        }
    }

    void Spawn()
    {
        isResetting = false;

        // Get a random animal from the list
        animal = Random.Range(0, 3); 
        Vector3 spawnPosition = new Vector3(0, 8, 0); // Middle of the screen, on top of the screen

        // Spawn animal at the specified position
        currentAnimal = Instantiate(Animals[animal], spawnPosition, Quaternion.identity);

        // Freeze animal's position
        Rigidbody2D rb = currentAnimal.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void DeleteAllAnimals()
    {
        // Find all animals in the scene
        Rigidbody2D[] allRigidbodies = FindObjectsOfType<Rigidbody2D>();

        // Destroy each animal
        foreach (Rigidbody2D rb in allRigidbodies)
        {
            Destroy(rb.gameObject); 
        }

        currentAnimal = null;
    }

    void ResetGame()
    {
        isResetting = true;
        isMovingToMouse = false;
        targetPosition = Vector3.zero;
        scoreText.text = "0"; 

        DeleteAllAnimals();

        Invoke("Spawn", 0.15f); 
    }
}
