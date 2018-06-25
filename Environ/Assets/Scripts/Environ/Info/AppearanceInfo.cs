namespace Environ.Info
{
    using System.Collections.Generic;
    using UnityEngine;
    using Environ.Support.Base;
    using Environ.Support.Enum.Damage;

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New Appearance Info", order = 2)]
    public class AppearanceInfo : EnvironBase
    {
        #region Variables
        public ParticleSystem particle;
        public Material material;
        public uint priority;
        public MeshRenderer mRenderer;

        public bool particlesOn;
        public bool materialOn;

        public bool hideOnResistance;
        public List<DamageType> hideIDList;

        public bool canUseMaterial { get { return material && materialOn; } }
        #endregion


        #region Setup and Update Functions
        ///<summary> Sets variables up for use. </summary>
        public void Setup(GameObject targetObj)
        {
            mRenderer = targetObj.GetComponent<MeshRenderer>();

            if (material)
                materialOn = true;

            if (particle)
            {
                particle = Instantiate(particle, targetObj.transform);
                particlesOn = true;
            }
        }

        ///<summary> Sets variables up for use as an Appearance for an EnvironObject. </summary>
        public void SetupRenderer()
        {
            if (!mRenderer)
                return;

            if (!material)
                material = mRenderer.material;
            else
                mRenderer.material = material;
        }

        ///<summary> Updates particles to play or stop based on the particlesOn value. </summary>
        public void UpdateInfo()
        {
            if (!particle)
                return;

            if (particlesOn && !particle.isPlaying)
                particle.Play();
            else if (!particlesOn && particle.isPlaying)
                particle.Stop();
        }
        #endregion


        #region Set Material Functions
        ///<summary> Sets the first material of the MeshRenderer to material. </summary>
        public void SetRendererMaterial()
        {
            if (mRenderer)
                mRenderer.material = material;
        }

        ///<summary> Sets the first material of the MeshRenderer to the given material. </summary>
        public void SetRendererMaterial(Material mat)
        {
            if (mRenderer)
                mRenderer.material = mat;
        }
        #endregion


        #region Appearance Control Functions
        ///<summary> Stops and destroys the particle system after the given waitTime (default 3f). </summary>
        public void StopAndDestroy(float waitTime = 3f)
        {
            if (!particle)
                return;

            particle.Stop();
            Destroy(particle.gameObject, waitTime);
        }

        ///<summary> Sets particlesOn and materialOn to true. Allows the particle system to play and the material to be applied to the target MeshRenderer. </summary>
        public void TurnOn()
        {
            particlesOn = true;
            materialOn = true;
        }

        ///<summary> Sets particlesOn and materialOn to false. Stops the particle system and prevents the material from being applied to the target MeshRenderer. </summary>
        public void TurnOff()
        {
            particlesOn = false;
            materialOn = false;
        }
        #endregion


        #region Helper Functions
        ///<summary> Checks if material can currently be applied, and if this AppearanceInfo takes priority over the given AppearanceInfo. </summary>
        public bool IsPriority(AppearanceInfo otherAppearance)
        {
            return canUseMaterial && priority < otherAppearance.priority;
        }
        #endregion


        #region Operator Overrides
        public static bool operator ==(AppearanceInfo a, AppearanceInfo b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.uniqueID == b.uniqueID;
        }

        public static bool operator !=(AppearanceInfo a, AppearanceInfo b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            return this == (AppearanceInfo)o;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}