using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance { set; get; }

    private const bool SHOW_COLLIDER = true;

    //Level spawning mechanics
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 10;
    private const int MAX_SEGMENTS_ON_SCREEN = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continuousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;

    //List of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> pieces = new List<Piece>(); // All the pieces in the pool

    //List of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    public List<Segment> activeSegments = new List<Segment>();

    //Gameplay
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    private void Start()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            //Generate segment
            GenerateSegment();
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continuousSegments * 0.25f))
        {
            //Spawn transition segment
            continuousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continuousSegments++;
        }
    }

    private void SpawnSegment()
    {
        List<Segment> possibleSegment = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSegment.Count);

        // Segment s = possibleSegment[id];
    }

    private void SpawnTransition()
    {

    }

    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);
        
        if (p == null)
        {
            GameObject go = null;
            if (pt == PieceType.ramp)
            {
                go = ramps[visualIndex].gameObject;
            }
            else if (pt == PieceType.longBlock)
            {
                go = longBlocks[visualIndex].gameObject;
            }
            else if (pt == PieceType.jump)
            {
                go = jumps[visualIndex].gameObject;
            }
            else if (pt == PieceType.slide)
            {
                go = slides[visualIndex].gameObject;
            }

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }
}
