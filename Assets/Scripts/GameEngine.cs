using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{

    public Text _infoTextScore;
    public Text _infoTextTime;
    public Text _gameOver;
    public bool isGameOver;
    private float timeOut = 10.5f;
    public GameObject _rest;
    public Camera _camera;
    public ParticleSystem _trail;

    private LineRenderer line;
    private LineRenderer showFigure;
    private bool isMousePressed;
    private List<Vector2> pointsList;
    private Vector2 mousePos;
    private float curTimeOut;
    private bool isStart;
    private int score;
    struct MyLine
    {
        public Vector2 StartPoint;
        public Vector2 EndPoint;
    }
    void Awake()
    {
        line = gameObject.AddComponent<LineRenderer>();
        isMousePressed = false;
        pointsList = new List<Vector2>();
        DrawFigure(4);
        _trail.startLifetime = .2f;
        _trail.startSpeed = .1f;
        _trail.startSize = .5f;
        _trail.simulationSpace = ParticleSystemSimulationSpace.World;
        _trail.maxParticles = 30;
        _trail.startColor = new Color(0, 1, 1, 1);
        _trail.emissionRate = 0;

    }
    // Дай бог здоровья, кто будет это тестить...
    // Use this for initialization
    void Start()
    {

        Restart();
        _infoTextTime.text = "";
        _gameOver.text = "";
        _infoTextScore.text = "Score: 0";

    }

    public void Restart()
    {
        isGameOver = false;
        _infoTextScore.text = "Score: 0";
        _gameOver.text = "";
        _infoTextTime.text = "";
        Time.timeScale = 1;
        isStart = true;
        curTimeOut = timeOut;
    }

    void OnGUI()
    {
        if (isStart)
        {
            _infoTextScore.text = "Score: " + score.ToString();

            if (((int)(curTimeOut * 10f)) / 10f == (int)curTimeOut)
            {
                _infoTextTime.text = (((int)(curTimeOut * 10f)) / 10f).ToString() + ".0";
            }
            else
            {
                _infoTextTime.text = (((int)(curTimeOut * 10f)) / 10f).ToString();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (isGameOver)
            {
                _rest.SetActive(true);
            }
            else _rest.SetActive(false);

            Vector2 curScreenPoint = new Vector2(Input.mousePosition.x,
                                                 Input.mousePosition.y);
            Vector2 mouse = _camera.ScreenToWorldPoint(curScreenPoint);
            if (Input.GetMouseButtonDown(0))
            {
                isMousePressed = true;
                pointsList.RemoveRange(0, pointsList.Count);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isMousePressed = false;
            }
            if (isMousePressed)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (!pointsList.Contains(mousePos))
                {
                    pointsList.Add(mousePos);
                    line.SetVertexCount(pointsList.Count);
                    line.SetPosition(pointsList.Count - 1, (Vector2)pointsList[pointsList.Count - 1]);
                    _trail.transform.position = mouse;
                    _trail.emissionRate = 200;
                }
            }
            else _trail.emissionRate = 0;
            curTimeOut -= Time.deltaTime;
            if (curTimeOut <= 0)
            {
                isGameOver = true;
                _gameOver.text = "Вы проиграли, очков заработано: " + score.ToString();
                Time.timeScale = 0;
            }
        }
    }
    private void DrawFigure(int edges)
    {
        showFigure = GetComponent<LineRenderer>();
        showFigure.SetVertexCount(edges + 1);
        showFigure.SetWidth(0.1f, 0.1f);
        showFigure.useWorldSpace = true;
        showFigure.SetPosition(0, new Vector2(-2, 2));
        showFigure.SetPosition(1, new Vector2(2, 2));
        showFigure.SetPosition(2, new Vector2(2, -2));
        showFigure.SetPosition(3, new Vector2(-2, -2));
        showFigure.SetPosition(4, new Vector2(-2, 2));
    }
}
