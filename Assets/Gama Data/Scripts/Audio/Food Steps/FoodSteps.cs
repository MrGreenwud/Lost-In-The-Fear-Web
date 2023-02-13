using System;
using UnityEngine;
using Unity.Profiling;
using Random = UnityEngine.Random;
using MaterialType = FoodStepSurface.MaterialType;
using SurfaceType = FoodStepSurface.SurfaceType;

[RequireComponent(typeof(AudioSource))]
public class FoodSteps : MonoBehaviour
{
    [Serializable]
    public class FoodStepsAudio
    {
        [SerializeField] private MaterialType m_MaterialType;

        [SerializeField] private AudioClip[] m_WalkAudioSampls;
        [SerializeField] private AudioClip[] m_RunAudioSampls;

        public MaterialType GetMaterialType() => m_MaterialType;
        
        public AudioClip[] GetWalkAudioSampls()
        {
            AudioClip[] tempAudioClips = new AudioClip[m_WalkAudioSampls.Length];

            for(int i = 0; i < m_WalkAudioSampls.Length; i++)
                tempAudioClips[i] = m_WalkAudioSampls[i];

            return tempAudioClips;
        }

        public AudioClip[] GetRunAudioSampls()
        {
            AudioClip[] tempAudioClips = new AudioClip[m_RunAudioSampls.Length];

            for (int i = 0; i < m_RunAudioSampls.Length; i++)
                tempAudioClips[i] = m_RunAudioSampls[i];

            return tempAudioClips;
        }
    }

    private AudioSource m_AudioSource;

    [SerializeField] private LayerMask m_GroundLayer;

    [Space(10)]

    [SerializeField] private FoodStepsAudio[] m_FoodStepsAudio;

    private Terrain m_Terrain;
    private TerrainData m_TerrainData;
    private Vector2 m_AlphaMap;
    private float[,,] m_SplatmapData;
    private int m_TextureCount;

    private static ProfilerMarker s_FoodSteps = new ProfilerMarker(ProfilerCategory.Scripts, "Food steps");
    private static ProfilerMarker s_GetTerrain = new ProfilerMarker(ProfilerCategory.Scripts, "Get Terrain");

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void Step(bool isRun)
    {
        s_FoodSteps.Begin();

        FoodStepSurface foodStepSurface = GetSurface();

        if (foodStepSurface.GetSurfaceType() == SurfaceType.Tarrain)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 20, 0), Vector3.down, out RaycastHit hit, 1000, m_GroundLayer))
            {
                int textereIndex = GetTerrainTextureIndex(Terrain.activeTerrain, transform.position);

                if (textereIndex == 0)
                    Play(MaterialType.Grass, isRun);
                else
                    Play(MaterialType.Gravle, isRun);
            }
        }
        else
        {
            Play(foodStepSurface.GetMaterialType(), isRun);
        }

        s_FoodSteps.End();
    }

    private void Play(MaterialType materialType, bool isRun)
    {
        if (m_AudioSource.isPlaying == true) return;

        AudioClip[] audioSampls = GetSempls(materialType, isRun);
        if (audioSampls == null || audioSampls.Length == 0) return;

        AudioClip sample = audioSampls[Random.Range(0, audioSampls.Length)];
        if (sample == null) return;

        m_AudioSource.PlayOneShot(sample);
    }

    private AudioClip[] GetSempls(MaterialType materialType, bool isRun)
    {
        for (int i = 0; i < m_FoodStepsAudio.Length; i++)
        {
            if (m_FoodStepsAudio[i].GetMaterialType() == materialType)
            {
                if (isRun == false)
                    return m_FoodStepsAudio[i].GetWalkAudioSampls();
                else
                    return m_FoodStepsAudio[i].GetRunAudioSampls();
            }
        }

        return null;
    }

    private FoodStepSurface GetSurface()
    {
        if(Physics.Raycast(transform.position + new Vector3(0, 20, 0), Vector3.down, out RaycastHit hit, 1000, m_GroundLayer))
            if (hit.collider.TryGetComponent<FoodStepSurface>(out FoodStepSurface foodStepSurface))
                return foodStepSurface;

        return null;
    }

    private Vector3 ConverToSplatmapCordinate(Terrain terrain, Vector3 position)
    {
        float positionX = ((position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
        float positionZ = ((position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
        return new Vector3(positionX, 0, positionZ);
    }

    private int GetTerrainTextureIndex(Terrain terrain, Vector3 position)
    {
        s_GetTerrain.Begin();

        if (m_Terrain == null)
        {
            m_TerrainData = terrain.terrainData;
            m_AlphaMap = new Vector2(m_TerrainData.alphamapWidth, m_TerrainData.alphamapHeight);
            m_SplatmapData = m_TerrainData.GetAlphamaps(0, 0, (int)m_AlphaMap.x, (int)m_AlphaMap.y);
            m_TextureCount = m_SplatmapData.Length / ((int)m_AlphaMap.x * (int)m_AlphaMap.y);
            m_Terrain = terrain;
        }

        s_GetTerrain.End();

        Vector3 terranCordinate = ConverToSplatmapCordinate(terrain, position);

        int activeIndex = 0;
        float largestOpasity = 0f;

        for(int i = 0; i < m_TextureCount; i++)
        {
            if(largestOpasity < m_SplatmapData[(int)terranCordinate.z, (int)terranCordinate.x, i])
            {
                activeIndex = i;
                largestOpasity = m_SplatmapData[(int)terranCordinate.z, (int)terranCordinate.x, i];
            }
        }

        return activeIndex;
    }
}
