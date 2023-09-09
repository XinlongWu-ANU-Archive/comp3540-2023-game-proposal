using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject TitleScreen;
    public GameObject MainGame;
    public GameObject GameOverScreen;
    public GameObject IntrduceScreen;
    public GameObject pauseScreen;
    public AudioClip TitleBGM;
    public AudioClip ForestBGM;
    public AudioClip IntrduceBGM;
    public AudioClip OptionBGM;
    private AudioSource BGMSource;
    private AudioSource OptionSource;

    public bool isGameOver = false;
    public bool isGameStart = false;
    public bool isGamePause = false;
    // Start is called before the first frame update
    void Start()
    {
        OptionSource = GetComponent<AudioSource>();
        BGMSource = Camera.main.GetComponent<AudioSource>();
        BGMSource.clip = TitleBGM;
        BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
        else if (!isGameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OptionSource.PlayOneShot(OptionBGM);
                IntrduceScreen.SetActive(false);
                MainGame.SetActive(true);

                BGMSource.clip = ForestBGM;
                BGMSource.Play();
                isGameStart = true;
            }
        }
        else if (isGameStart && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePause) {
                Continue();
            }
            else {
                Pause();

            }
        }
    }

    public void Exit()
    {
        OptionSource.PlayOneShot(OptionBGM);
        Application.Quit();
    }

    public void Play()
    {
        OptionSource.PlayOneShot(OptionBGM);
        TitleScreen.gameObject.SetActive(false);

        IntrduceScreen.SetActive(true);

        BGMSource.clip = IntrduceBGM;
        BGMSource.Play();
    }

    public void GameOver()
    {
        isGameOver = true;
        isGameStart = false;
        MainGame.SetActive(false);
        GameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        isGamePause = false;
        MainGame.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void Pause()
    {
        //Time.timeScale = 0;
        isGamePause = true;
        MainGame.SetActive(false);
        pauseScreen.SetActive(true);
    }
}
