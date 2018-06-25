using System;

namespace Environ.Support.Enum
{

    namespace General
    {
        public enum TransferCondition
        {
            COLLISION_ENTER = 0,
            COLLISION_EXIT,
            COLLISION_STAY,
            TRIGGER_ENTER,
            TRIGGER_EXIT,
            TRIGGER_STAY
        }

        public enum TerminalCondition
        {
            NONE = 0,
            COLLISION_EXIT,
            TRIGGER_EXIT,
            TIMER_ZERO
        }

        public enum Similarity
        {
            UNIQUE = 0,
            STANDARD,
            SELECTIVE
        }
    }

    namespace Damage
    {
        //These can be changed
        public enum DamageType
        {
            NONE = 0,
            FIRE,
            WATER,
            NATURE,
            FROST,
            ELECTRICITY,
            PHYSICAL
        }

        public enum DamageLimit
        {
            NONE = 0,
            TIME_LIMIT,
            ATTACK_LIMIT,
            DAMAGE_LIMIT
        }
    }

    namespace Resistance
    {
        public enum ResistanceType
        {
            NULLIFY_DAMAGE = 0,
            REDUCE_DAMAGE,
            MULTIPLY_DAMAGE,
            HEAL
        }
    }

    namespace Destruct
    {
        public enum DestroyCondition
        {
            COLLISION_ENTER = 0,
            COLLISION_EXIT,
            TRIGGER_ENTER,
            TRIGGER_EXIT,
            TIMER_ZERO,
            ZERO_HITPOINTS,
            EFFECT_DAMAGE_TYPE
        }
    }

    namespace Tag
    {
        //These can be changed
        public enum ObjectTag
        {
            PLAYER = 0,
            DOOR,
            SPHERE,
        }
    }
}
