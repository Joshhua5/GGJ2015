using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoSingleton<Game>
{
    private int _deaths = 0;
    private int _surviviors = 0;
    private int _score = 0;
    public int Score { get { return _score; } }

    [SerializeField]
    private GameObject _agents;

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _survivorLabel;

    [SerializeField]
    private Text _deathLabel;

    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private Text _gameOverScore;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Button _exitButton;



    private List<Agent> _agentList;


    void Awake()
    {
        _agentList = new List<Agent>(_agents.GetComponentsInChildren<Agent>());
        Debug.Log(_agentList.Count);
        _scoreLabel.text = "Score: " + 0;

        foreach (var agent in _agentList)
        {
            agent.OnAgentDeath += agent_OnAgentDeath;
        }

        UpdateLabels();

        _gameOverPanel.SetActive(false);
    }

    public void OnRestartButtonClick()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel("Level1");

        _deaths = 0;
        _surviviors = 0;
        _score = 0;
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    void agent_OnAgentDeath(Agent agent)
    {
        _deaths++;

        _agentList.Remove(agent);
        Destroy(agent.gameObject);

        UpdateLabels();
    }

    public void PlayerEscaped(Agent agent)
    {
        _score += (int)agent.Health;
        _surviviors++;

        _agentList.Remove(agent);
        Destroy(agent.gameObject);

        UpdateLabels();
    }

    private void UpdateLabels()
    {
        _scoreLabel.text = "SCORE: " + _score;
        _survivorLabel.text = "SURVIVIORS: " + _surviviors;
        _deathLabel.text = "LOST SOULS: " + _deaths;

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (_agentList.Count == 0)
        {
            Time.timeScale = 0.001f;
            _gameOverPanel.SetActive(true);
            _gameOverScore.text = "Score: " + _score;
        }
    }
}
