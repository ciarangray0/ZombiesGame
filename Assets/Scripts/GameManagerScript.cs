using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
  public enum gameState { Menu, Playing};
  public gameState currentState = gameState.Menu;
  public static Vector3 screenBottomLeft, screenTopRight; 
  public static float screenWidth, screenHeight;
  private static GameManagerScript instance;
  public GameObject character;

    // Start is called before the first frame update
        void Start() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
         }
         else
         {
             Destroy(gameObject);
             return;
         }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void onStartGameButtonClick() {
      changeScene(gameState.Playing);
      Debug.Log("Button clicked!");
    }

    private void changeScene(gameState newGameState) {
      currentState = newGameState;
      switch(newGameState) {
        case gameState.Menu :
        Debug.Log("switching to menu");
        SceneManager.LoadScene("MenuScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
        break;

        case gameState.Playing :
        Debug.Log("switching to game");
        SceneManager.LoadScene("GameScene");
        SceneManager.sceneLoaded += OnSceneLoaded; // Attach the event for scene load
        break;
      }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // If the "GameScene" is loaded and game state is Playing, start the game
    if (scene.name == "GameScene" && currentState == gameState.Playing)
    {
        Debug.Log("calling startGame()");
        startGame();
        Debug.Log("GameScene loaded, calling startGame()");
    }
    // If the "MenuScene" is loaded, display the high score
    else if (scene.name == "MenuScene" && currentState == gameState.Menu)
    {
       Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(onStartGameButtonClick);
            Debug.Log("StartButton's OnClick listener added.");
        }
        else
        {
            Debug.LogError("StartButton not found in MenuScene.");
        }
    }

    // Detach event to prevent duplicate calls
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

    private void startGame() {
        //assigning values to my screen position variables
        screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,30f)); 
        screenTopRight = Camera.main.ViewportToWorldPoint (new Vector3(1f,1f,30f)); 
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.z - screenBottomLeft.z;
      //position camera
      Camera.main.transform.position = new Vector3(0f, 10f, 0f);
      Camera.main.transform.LookAt(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f)); //point camera down
      spawnCharacter();
          if (character != null)
    {
        Camera.main.transform.LookAt(character.transform);
    }
    else
    {
        Debug.LogError("Character could not be instantiated.");
    }
    }

    private void spawnCharacter() {
        GameObject playerCharacter = GameObject.Instantiate(character);
}

    private void gameOver() {
    Debug.Log("Game Over!");
    changeScene(gameState.Menu);
    
    // Destroy all game objects with Rigidbody components
    foreach (Rigidbody rb in GameObject.FindObjectsOfType<Rigidbody>()) {
        Destroy(rb.gameObject);
    }
}
}

    