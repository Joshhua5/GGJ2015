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
    private Agent[] _agents;

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Text _survivorLabel;

    [SerializeField]
    private Text _deathLabel;

    private List<Agent> _agentList;


    void Awake()
    {
        _agentList = new List<Agent>(_agents);
        _scoreLabel.text = "Score: " + 0;

        foreach(var agent in _agentList)
        {
            agent.OnAgentDeath += agent_OnAgentDeath;
        }

        UpdateLabels();
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
        _scoreLabel.text = "Score: " + _score;
        _survivorLabel.text = "SURVIVIORS: " + _surviviors;
        _deathLabel.text = "SOULS LOST: " + _deaths;
    }
}
