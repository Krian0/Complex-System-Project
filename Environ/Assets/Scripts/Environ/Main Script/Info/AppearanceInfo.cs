namespace Environ.Info
{
    using UnityEngine;
    using System.Collections.Generic;
    using Environ.Support.Enum.Damage;

    [CreateAssetMenu(fileName = "NewAppearanceInfo.asset", menuName = "Environ/Info/New Appearance Info", order = 2)]
    public class AppearanceInfo : EnvironInfoBase
    {
        public ParticleSystem particle;
        public Material material;
        public uint priority;
        public MeshRenderer mRenderer;

        public bool particlesOn;
        public bool materialOn;

        public bool hideOnResistance;
        public List<DType> hideIDList;


        public void Setup(GameObject targetObj)
        {
            if (particle)
            {
                particle = Instantiate(particle);
                particle.transform.position = targetObj.transform.position;
                particle.transform.SetParent(targetObj.transform);

                particlesOn = true;
            }

            mRenderer = targetObj.GetComponent<MeshRenderer>();
            materialOn = true;
        }

        public void SetupRenderer()
        {
            if (!mRenderer)
                return;

            if (!material)
                material = mRenderer.material;
            else
                mRenderer.material = material;
        }

        public void UpdateAppearance()
        {
            if (particle)
            {
                if (particlesOn && !particle.isPlaying)
                    particle.Play();

                else if (!particlesOn && particle.isPlaying)
                    particle.Stop();
            }
        }


        public void SetRendererMaterial()
        {
            if (mRenderer)
                mRenderer.material = material;
        }

        public void SetRendererMaterial(Material mat)
        {
            if (mRenderer)
                mRenderer.material = mat;
        }

        public void StopAndDestroy()
        {
            if (particle)
            {
                particle.Stop();
                Destroy(particle.gameObject, 4);
            }
        }

        public void TurnOn()
        {
            particlesOn = true;
            materialOn = true;
        }

        public void TurnOff()
        {
            particlesOn = false;
            materialOn = false;
        }

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