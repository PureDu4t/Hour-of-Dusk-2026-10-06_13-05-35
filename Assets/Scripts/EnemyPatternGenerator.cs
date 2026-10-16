using UnityEngine;

public class EnemyPatternGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[] RandomizePitches()
    {
        int[] _pitches = new int[8];

        for (int i = 0; i < _pitches.Length; i++)
        {
            _pitches[i] = Random.Range(1, 13);
        }
        return _pitches;
    }
}
