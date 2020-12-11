using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public struct PosRot
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    
    [System.Serializable]
    public struct WaveEnemyDescriptor
    {
        public GameObject prefab;
        public int amountToSpawn;
        public PosRot[] positionsAndRotations;
    }

    [System.Serializable]
    public struct Wave
    {
        public AudioClip audioclipToPlayBeforeWaveStart;
        public AudioClip audioclipToPlayAfterWaveWon;
        public WaveEnemyDescriptor[] enemiesToSpawn;
    }
    
    [CreateAssetMenu(fileName = "LevelDetail", menuName = "ScriptableObjects/LevelDetailsScriptableObject", order = 1)]
    public class LevelDetailsScriptableObject : ScriptableObject
    {
        public Wave[] waves;
    }
}
