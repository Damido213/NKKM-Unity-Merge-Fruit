using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public List<GameObject> Animals; // List of animals to spawn
    public TextMeshProUGUI scoreText; 
    public int currentIndex = 0; // Index of the current animal

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if simmilar name, merge animals
        if (SimilarName(collision.gameObject.name, gameObject.name))
        {
            // Check if the current animal is not the last one in the list
            if (currentIndex != 5)
            {
                //Merge animals
                if (this.gameObject.transform.position.y < collision.gameObject.transform.position.y)
                {
                    SpawnAnimal();
                    Destroy(collision.gameObject);
                    Destroy(gameObject);

                    // Update score
                    int score = int.Parse(scoreText.text);
                    score += currentIndex;
                    scoreText.text = score.ToString();
                }
            }
        }

        
    }

    // Check if the names are similar
    bool SimilarName(string name1, string name2)
    {
        string pattern = @"^([a-zA-Z_]+)";
        var match1 = Regex.Match(name1, pattern);
        var match2 = Regex.Match(name2, pattern);

        if (match1.Success && match2.Success)
        {
            return match1.Groups[1].Value == match2.Groups[1].Value;
        }

        return false;
    }

    void SpawnAnimal()
    {
        if (Animals != null && Animals.Count > 0)
        {
            currentIndex = (currentIndex + 1) % Animals.Count;

            Instantiate(Animals[currentIndex], transform.position, transform.rotation);
            // Spawn the next animal at the position of the current animal
        }
    }

    void Start()
    {
        // Find the score text object in the scene
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score_number").GetComponent<TextMeshProUGUI>();
        }
    }
}

