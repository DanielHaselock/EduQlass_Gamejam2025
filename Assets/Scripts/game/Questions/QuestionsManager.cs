using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class QuestionsManager : MonoBehaviour
{
    [Header("Objects")]

    [SerializeField] GameObject QuestionsUI;
    [SerializeField] TextMeshProUGUI questionsText;
    [SerializeField] TextMeshProUGUI Answers1Text;
    [SerializeField] TextMeshProUGUI Answers2Text;
    [SerializeField] TextMeshProUGUI Answers3Text;
    [SerializeField] TextMeshProUGUI Answers4Text;

    [Header("Time settings")]
    [SerializeField] float timeBetweenQuestions = 15;
    [SerializeField] float timeforEachQuestion = 10;

    private int correctAnswer = 1;
    Fetch fetch;
    private List<PlayerInputs> players;

    private bool[] previousActivity;
    private int amountPlayerAnswered = 0;

    private void Start()
    {
        QuestionsUI.SetActive(false);
        //setInputsActive(false);
        previousActivity = new bool[2];
        fetch = GetComponent<Fetch>();
    }
    public void setPlayers(List<GameObject> p_players)//called by PlayerSpawnManager
    {
        players = new List<PlayerInputs>();

        foreach (GameObject p in p_players)
        {
            if (p.GetComponent<PlayerInputs>())
                players.Add(p.GetComponent<PlayerInputs>());
        }

        players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").FindAction("QuestionInputs").started += OnPlayer1ButtonSelected;
        players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
        players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").FindAction("QuestionInputs").started += OnPlayer2ButtonSelected;
        players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
    }


    public void setInputsActive(bool a)
    {
        if (a)
        {
            players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
            players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
        }
        else
        {
            players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
            players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
        }
    }

    private void disableSpecificPlayerInput(int player)
    {
        if (player == 1)
        {
            players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
        }
        else if (player == 2)
        {
            players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
        }
    }

    public void Pause(bool shouldPause)
    {
        if (shouldPause)
        {
            previousActivity[0] = players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").enabled;
            previousActivity[1] = players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").enabled;

            players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
            players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
        }
        else
        {
            if (previousActivity[0])
            {
                players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
            }

            if (previousActivity[1])
            {
                players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Enable();
            }
        }

    }

    private void setQuestionsActive(bool a)
    {
        QuestionsUI.SetActive(a);
    }

    private void setQuestion(string text)
    {
        questionsText.text = text;
    }

    private void setAnswer1(string text)
    {
        Answers1Text.text = text;
    }
    private void setAnswer2(string text)
    {
        Answers2Text.text = text;
    }
    private void setAnswer3(string text)
    {
        Answers3Text.text = text;
    }

    private void setAnswer4(string text)
    {
        Answers4Text.text = text;
    }

    private void setCorrectAnswer(int p_Correct)
    {
        correctAnswer = Mathf.Clamp(p_Correct, 1, 4);
    }

    public void OnPlayer1ButtonSelected(InputAction.CallbackContext context)
    {

        Vector2 input = context.ReadValue<Vector2>();

        bool IsCorrectAnswer = checkAnswer(input);
        Debug.Log("Player 1 selected: " + IsCorrectAnswer);
        disableSpecificPlayerInput(1);

        if (IsCorrectAnswer)
            players[0].GetComponent<Power>().addPower();
        else
            players[0].GetComponent<Power>().noAddPower();

        amountPlayerAnswered++;
        checkPlayersAnswered();
    }

    public void OnPlayer2ButtonSelected(InputAction.CallbackContext context)
    {
        
        Vector2 input = context.ReadValue<Vector2>();
        bool IsCorrectAnswer = checkAnswer(input);
        Debug.Log("Player 2 selected: " + IsCorrectAnswer);
        disableSpecificPlayerInput(2);

        if (IsCorrectAnswer)
            players[1].GetComponent<Power>().addPower();
        else
            players[1].GetComponent<Power>().noAddPower();

        amountPlayerAnswered++;
        checkPlayersAnswered();

    }

    private void checkPlayersAnswered()
    {
        if (amountPlayerAnswered == 2)
        {
            amountPlayerAnswered = 0;

            StopAllCoroutines();
            endQuestion();
        }
    }

    public void startQuestions()
    {
        StartCoroutine(WaitForNextQuestion());
    }

    private void endQuestion()
    {
        //Show answer here
        switch(correctAnswer)
        {
            case 1:
                Answers1Text.color = Color.green;
                Answers2Text.color = Color.red;
                Answers3Text.color = Color.red;
                Answers4Text.color = Color.red;
                break;
            case 2:
                Answers1Text.color = Color.red;
                Answers2Text.color = Color.green;
                Answers3Text.color = Color.red;
                Answers4Text.color = Color.red;
                break;
            case 3:
                Answers1Text.color = Color.red;
                Answers2Text.color = Color.red;
                Answers3Text.color = Color.green;
                Answers4Text.color = Color.red;
                break;

            case 4:
                Answers1Text.color = Color.red;
                Answers2Text.color = Color.red;
                Answers3Text.color = Color.red;
                Answers4Text.color = Color.green;
                break;
        }

        setInputsActive(false);

        StartCoroutine(ShowAnswerToQuestion());
    }

    private void hideQuestion()
    {
        setQuestionsActive(false);
        
        StartCoroutine(WaitForNextQuestion());
    }


    public void stopQuestions()
    {
        StopAllCoroutines();
        setQuestionsActive(false);
        setInputsActive(false);
    }

    private IEnumerator ShowAnswerToQuestion()
    {
        yield return new WaitForSeconds(3);

        Answers1Text.color = Color.white;
        Answers2Text.color = Color.white;
        Answers3Text.color = Color.white;
        Answers4Text.color = Color.white;

        hideQuestion();
    }

    private IEnumerator WaitForNextQuestion()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        getNextQuestion();
        StartCoroutine(WaitInQuestion());
    }

    private IEnumerator WaitInQuestion()
    {
        yield return new WaitForSeconds(timeforEachQuestion);
        endQuestion();
    }

    private void getNextQuestion() //TODO Get info for question here
    {
        if(fetch == null)
            fetch = GetComponent<Fetch>();

        Question q = fetch.GetQuestion();
        System.Random rnd = new System.Random();
        int correctAnswerSpot = rnd.Next(1, 5);

        setQuestion(q.question);

        switch (correctAnswerSpot)
        {
            case 1:
                setAnswer1(q.answer);
                setAnswer2(q.other_question);
                setAnswer3(q.other_question2);
                setAnswer4(q.other_question3);
                break;
            case 2:
                setAnswer1(q.other_question);
                setAnswer2(q.answer);
                setAnswer3(q.other_question2);
                setAnswer4(q.other_question3);
                break;
            case 3:
                setAnswer1(q.other_question);
                setAnswer2(q.other_question2);
                setAnswer3(q.answer);
                setAnswer4(q.other_question3);
                break;
            case 4:
                setAnswer1(q.other_question);
                setAnswer2(q.other_question2);
                setAnswer3(q.other_question3);
                setAnswer4(q.answer);
                break;
            default:
                throw new Exception("WRONG INPUT FROM THE RANDOM");
        }

        amountPlayerAnswered = 0;
        setCorrectAnswer(correctAnswerSpot);
        setQuestionsActive(true);
        setInputsActive(true);
    }

    private bool checkAnswer(Vector2 input)
    {
        int answer = 0;

        if (input.y > 0) //Up = answer 1
        {
            answer = 1;
        }
        else if (input.x > 0) //right = answer 2
        {
            answer = 2;
        }
        else if (input.y < 0) //down = answer 3
        {
            answer = 3;
        }
        else if (input.x < 0) //left = answer 4
        {
            answer = 4;
        }

        return answer == correctAnswer;
    }

    private void OnDestroy()
    {
        if (!players[0] || players[0].GetComponent<PlayerInput>() == null)
            return;
        if (!players[1] || players[1].GetComponent<PlayerInput>() == null)
            return;


        players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").FindAction("QuestionInputs").started -= OnPlayer1ButtonSelected;
        players[0].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
        players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").FindAction("QuestionInputs").started -= OnPlayer2ButtonSelected;
        players[1].GetComponent<PlayerInput>().actions.FindActionMap("Questions").Disable();
    }

}
