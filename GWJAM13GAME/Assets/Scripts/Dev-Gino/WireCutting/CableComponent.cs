using UnityEngine;
using Random = UnityEngine.Random;


public class CableComponent : MonoBehaviour
{
    public static CableComponent Instance;

    [SerializeField] private WireCable[] wires;
    private bool puzzleSolved = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AssignRandomCorrectWire();
    }

    private void AssignRandomCorrectWire()
    {
        if (wires.Length == 0)
        {
            Debug.LogError("No wires assigned!");
            return;
        }

        foreach (WireCable wire in wires)
        {
            wire.isCorrectWire = false;
        }

        int correctIndex = Random.Range(0, wires.Length);
        wires[correctIndex].isCorrectWire = true;

        Debug.Log($"Correct wire is: {wires[correctIndex].name}");
    }

    public void WireCut(WireCable wire)
    {
        if (puzzleSolved) return;

        if (wire.isCorrectWire)
        {
            Debug.Log("Correct wire cut! Component finished");
            puzzleSolved = true;
        }
        else
        {
            Debug.Log("Wrong wire! Boom!");
        }
    }
}
