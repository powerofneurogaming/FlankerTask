using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    public int totalQuestions;

    private Question previousQuestion;
    private Question currentQuestion;
    private int randQuestionIndex;

    [SerializeField]
    private int numQuestions;

    [SerializeField]
    public GameObject arrows;

    [SerializeField]
    private float questionTransitionTime = 1f;

    private void Start()
    {


        StartFlankerTask();

    }

    //public void startGame()
    //{
    //    string name = playerName.GetComponent<Text>().text;
    //    if (name.Length <= 0)
    //    {
    //        PlayerPrefs.SetString("PlayerName", "NoName");
    //        SceneManager.LoadScene("Flanker Main");

    //        StartFlankerTask();

    //    }
    //    else
    //    {
    //        PlayerPrefs.SetString("PlayerName", name);
    //        SceneManager.LoadScene("Flanker Main");
    //        StartFlankerTask();


    //    }


    //}

    public void StartFlankerTask()
    {

        randQuestionIndex = Random.Range(0, questions.Length);
        int currentlevel = PlayerPrefs.GetInt("PlayerLevel");
        
        currentQuestion = questions[randQuestionIndex];

        Debug.Log(currentQuestion.flankerArrows);
        Debug.Log(currentlevel.ToString());

        if (previousQuestion == null)
        {
            arrows.GetComponent<Text>().text = currentQuestion.flankerArrows;

        } else {
            randQuestionIndex = Random.Range(0, questions.Length);
            currentQuestion = questions[randQuestionIndex];

            arrows.GetComponent<Text>().text = currentQuestion.flankerArrows;
        }
        currentlevel++;
        PlayerPrefs.SetInt("PlayerLevel", currentlevel);
        previousQuestion = currentQuestion;
    }

    IEnumerator TransitionToNextQuestion()
    {
        int currentLevel = PlayerPrefs.GetInt("PlayerLevel");
        yield return new WaitForSeconds(questionTransitionTime);
        if (currentLevel > totalQuestions)
        {
            GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");

            foreach (GameObject obj in buttons)
            {
                obj.active = false;
            }

            string playerScore = PlayerPrefs.GetInt("PlayerScore").ToString();
            arrows.GetComponent<Text>().text = "You scored " + playerScore;

        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    
    public void userSelectRight()
    {
        if (!currentQuestion.isLeft)
        {
            Debug.Log("Correct");
            answerCorrect();

        } else {
            Debug.Log("Incorrect");
        }
        StartCoroutine(TransitionToNextQuestion());
    }
    public void userSelectLeft()
    {
        if (currentQuestion.isLeft)
        {
            Debug.Log("Correct");
            answerCorrect();

        }
        else {
            Debug.Log("Incorrect");

        }
        StartCoroutine(TransitionToNextQuestion());
    }

    public void answerCorrect()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score++;
        PlayerPrefs.SetInt("PlayerScore", score);
    }
}
