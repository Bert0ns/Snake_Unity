using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameSnake : MonoBehaviour
{
    public static int SCREEN_WIDTH = 64;//64
    public static int SCREEN_HEIGHT = 48;//48

    public static Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

    private static int difficultyLevel = 0;

    [SerializeField] private GameObject restartMenu;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private GameObject buttonUnPause;
    [SerializeField] private GameObject controlsButtons;

    private bool dir_up = false;
    private bool dir_down = false;
    private bool dir_right = false;
    private bool dir_left = false;

    public static float speed = 0.15f;
    private float timer = 0f;

    public int[] posPlayer;
    private static int playerAge = 3;

    //public static bool playerIsDead = false;

    void Start()
    {
        restartMenu.SetActive(false);
        controlsButtons.SetActive(true);

        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
   
        ChangeDifficulty();
        //playerIsDead = false;
        playerAge = 3;
        ScoreTextUpdate();
        PlaceCells();
        CreatePlayer(32, 24);
        RandomDir();

        SpawnFood();
        SpawnFood();
        SpawnFood();
        SpawnFood();
        SpawnFood();
    }
    void Update()
    {
        UserInput();
        if (timer > speed)
        {
            //playerAge++;
            PlayerMov();
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    private void PlayerMov()
    {
        if (dir_up)
        {
            if (grid[posPlayer[0], posPlayer[1] + 1].isAlive == true || grid[posPlayer[0], posPlayer[1] + 1].isBorder == true || grid[posPlayer[0], posPlayer[1] + 1].isPlayer == true)
            {
                PlayerDead();
            }
            else
            {
                if (grid[posPlayer[0], posPlayer[1] + 1].isFood == true)
                {
                    playerAge++;
                    SpawnFood();
                    ScoreTextUpdate();
                }
                grid[posPlayer[0], posPlayer[1]].SetHead(false);
                posPlayer[1]++;
                grid[posPlayer[0], posPlayer[1]].SetState(3);
                grid[posPlayer[0], posPlayer[1]].Age = playerAge;
                grid[posPlayer[0], posPlayer[1]].SetHead(true);
            }
        }
        else if (dir_down)
        {
            if (grid[posPlayer[0], posPlayer[1] - 1].isAlive == true || grid[posPlayer[0], posPlayer[1] - 1].isBorder == true || grid[posPlayer[0], posPlayer[1] - 1].isPlayer == true)
            {
                PlayerDead();
            }
            else
            {
                if (grid[posPlayer[0], posPlayer[1] - 1].isFood == true)
                {
                    playerAge++;
                    SpawnFood();
                    ScoreTextUpdate();
                }
                grid[posPlayer[0], posPlayer[1]].SetHead(false);
                posPlayer[1]--;
                grid[posPlayer[0], posPlayer[1]].SetState(3);
                grid[posPlayer[0], posPlayer[1]].Age = playerAge;
                grid[posPlayer[0], posPlayer[1]].SetHead(true);
            }
        }
        else if (dir_right)
        {
            if (grid[posPlayer[0] + 1, posPlayer[1]].isAlive == true || grid[posPlayer[0] + 1, posPlayer[1]].isBorder == true || grid[posPlayer[0] + 1, posPlayer[1]].isPlayer == true)
            {
                PlayerDead();
            }
            else
            {
                if (grid[posPlayer[0] + 1, posPlayer[1]].isFood == true)
                {
                    playerAge++;
                    SpawnFood();
                    ScoreTextUpdate();
                }
                grid[posPlayer[0], posPlayer[1]].SetHead(false);
                posPlayer[0]++;
                grid[posPlayer[0], posPlayer[1]].SetState(3);
                grid[posPlayer[0], posPlayer[1]].Age = playerAge;
                grid[posPlayer[0], posPlayer[1]].SetHead(true);
            }
        }
        else if (dir_left)
        {
            if (grid[posPlayer[0] - 1, posPlayer[1]].isAlive == true || grid[posPlayer[0] - 1, posPlayer[1]].isBorder == true || grid[posPlayer[0] - 1, posPlayer[1]].isPlayer == true)
            {
                PlayerDead();
            }
            else
            {
                if (grid[posPlayer[0] - 1, posPlayer[1]].isFood == true)
                {
                    playerAge++;
                    SpawnFood();
                    ScoreTextUpdate();
                }
                grid[posPlayer[0], posPlayer[1]].SetHead(false);
                posPlayer[0]--;
                grid[posPlayer[0], posPlayer[1]].SetState(3);
                grid[posPlayer[0], posPlayer[1]].Age = playerAge;
                grid[posPlayer[0], posPlayer[1]].SetHead(true);
            }
        }
    }
    private void SpawnFood()
    {
        bool spawned = false;

        for(int y = 0; y < SCREEN_HEIGHT - 1; y++)
        {
            for (int x = 0; x < SCREEN_HEIGHT - 1; x++)
            {
                if(!(grid[x, y].isAlive || grid[x, y].isBorder || grid[x, y].isPlayer))
                {
                    if(1 > UnityEngine.Random.Range(0, 10000))
                    {
                        grid[x, y].SetState(4);
                        spawned = true;
                    }
                }
                if(spawned)
                {
                    break;
                }
            }
            if (spawned)
            {
                break;
            }
        }
        
        if(spawned == false)
        {
            SpawnFood();
        }
    }
    private void PlayerDead()
    {
        Time.timeScale = 0f;
        //playerIsDead = true;
        controlsButtons.SetActive(false);
        buttonPause.SetActive(false);
        restartMenu.SetActive(true);
    }
    private void UserInput()
    {
        //select direction
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            GoUp();
        }        
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoDown();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            GoRight();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoLeft();
        }
    }
    private void ScoreTextUpdate()
    {
        scoreText.text = (playerAge - 1).ToString();
        if(playerAge - 1 > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", playerAge - 1);
            bestScoreText.text = (playerAge - 1).ToString();
        }
    }
    private void ChangeDifficulty()
    {
        difficultyLevel = PlayerPrefs.GetInt("Difficulty", 0);

        switch(difficultyLevel)
        {
            case 1:
                Debug.Log("Set difficulty on easy");
                speed = 0.13f;
                break;
            case 2:
                Debug.Log("Set difficulty on hard");
                speed = 0.09f;
                break;
            default:
                Debug.Log("Set difficulty on hell");
                speed = 0.145f;
                break;
        }
    }
    public void GoUp()
    {
        dir_up = true;
        dir_down = false;
        dir_left = false;
        dir_right = false;
        Debug.Log("vai SU");
    }
    public void GoDown()
    {
        dir_up = false;
        dir_down = true;
        dir_left = false;
        dir_right = false;
        Debug.Log("vai GIU");
    }
    public void GoRight()
    {
        dir_up = false;
        dir_down = false;
        dir_left = false;
        dir_right = true;
        Debug.Log("vai a destra");
    }
    public void GoLeft()
    {
        dir_up = false;
        dir_down = false;
        dir_left = true;
        dir_right = false;
        Debug.Log("vai a sinistra");
    }
    public void Restart()
    {
        //playerIsDead = false;
        //buttonRestart.SetActive(false);
        buttonPause.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitApplication()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        buttonPause.SetActive(false);
        buttonUnPause.SetActive(true);
        Debug.Log("Game paused");
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        buttonPause.SetActive(true);
        buttonUnPause.SetActive(false);
        Debug.Log("Game unpaused");
    }
    public void SetDifficultyLevel(int difficulty)
    {
        difficultyLevel = difficulty;
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                grid[x, y] = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                if(x == SCREEN_WIDTH - 1 || x == 0 || y == SCREEN_HEIGHT - 1 || y == 0)
                {
                    //border
                    grid[x, y].SetState(2);
                }
                else
                {
                    //in bounds
                    grid[x, y].SetState(0);
                }
            }
        }
    }
    private void CreatePlayer(int x, int y)
    {
        //grid[x, y].SetState(0);
        grid[x, y].SetState(3);
        grid[x, y].Age = playerAge;// grid[x, y].Age = playerAge++;
        posPlayer[0] = x;
        posPlayer[1] = y;
    }
    private void RandomDir()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 25)
        {
            dir_right = true;
        }
        else if ( rand >= 25 || rand < 50)
        {
            dir_left = true;
        }
        else if ( rand >= 50 || rand < 75)
        {
            dir_up = true;
        }
        else
        {
            dir_down = true;
        }
    }
}
