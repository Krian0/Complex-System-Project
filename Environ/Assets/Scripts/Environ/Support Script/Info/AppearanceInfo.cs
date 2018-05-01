namespace EnvironInfo
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New AppearanceInfo", order = 1)]
    public class AppearanceInfo : ScriptableObject
    {
        public Texture objectTexture;
        public Material objectMaterial;
        public ParticleSystem objectParticle;

        public bool particlesOn;
    }
}