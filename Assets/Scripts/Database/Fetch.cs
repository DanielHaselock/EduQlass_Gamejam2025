using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

[Serializable]
public class QuestionList
{
    public List<Question> questions;
}

[Serializable]
public class Question
{
    [field: SerializeField] public int question_id;
    [field: SerializeField] public string question;
    [field: SerializeField] public string answer;
    [field: SerializeField] public string other_question;
    [field: SerializeField] public string other_question2;
    [field: SerializeField] public string other_question3;
    [field: SerializeField] public string subject;

    public override string ToString()
    {
        return $"Q: {question} | A: {answer}";
    }
}

public class Fetch : MonoBehaviour
{
    public string jsonFileName = "questions.json";
    private List<Question> loadedQuestions;

    void Awake()
    {
        LoadQuestionsFromJson();
    }

    private void init()
    {
        LoadQuestionsFromJson();
    }

    private void LoadQuestionsFromJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log(jsonData);
            QuestionList list = JsonUtility.FromJson<QuestionList>(jsonData);
            loadedQuestions = list.questions;
            //foreach (var item in loadedQuestions)
            //{
            //    Debug.Log(item);
            //}
            if (loadedQuestions != null)
            {
                Debug.Log($"Loaded {loadedQuestions.Count} questions from {jsonFileName}");
            }
            else
            {
                Debug.LogError($"Failed to parse JSON data from {jsonFileName}. Ensure the JSON structure is a direct array of question objects.");
                loadedQuestions = new List<Question>();
            }
        }
        else
        {
            Debug.LogError($"JSON file not found at: {filePath}. Make sure '{jsonFileName}' is in your StreamingAssets folder.");
            loadedQuestions = new List<Question>();
        }
    }

    public Question GetQuestion(string subject)
    {
        if (loadedQuestions != null && loadedQuestions.Count > 0)
        {
            List<Question> subjectQuestions = loadedQuestions.FindAll(e => e.subject == subject);
            if (subjectQuestions.Count > 0)
            {
                int randomIndex = Random.Range(0, subjectQuestions.Count);
                return subjectQuestions[randomIndex];
            }
            else
            {
                Debug.LogWarning($"No questions found for subject: {subject}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No questions loaded. Cannot get a question by subject.");
            return null;
        }
    }

    public Question GetQuestion()
    {
        if (loadedQuestions == null)
            init();

        if (loadedQuestions != null && loadedQuestions.Count > 0)
        {
            int randomIndex = Random.Range(0, loadedQuestions.Count);
            return loadedQuestions[randomIndex];
        }
        else
        {
            Debug.LogWarning("No questions loaded. Cannot get a random question.");
            return null;
        }
    }
}