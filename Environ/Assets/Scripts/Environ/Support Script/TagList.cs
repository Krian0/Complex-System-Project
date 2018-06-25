namespace Environ.Support.TagList
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Enum.Tag;

    [Serializable]
    public class EnvironTagList
    {
        #region Variables
        public List<ObjectTag> objectTags;
        #endregion


        #region Matching Tag Functions
        ///<summary> Checks for matching objects between objectTags and the given EnvironTagList. </summary>
        public bool MatchingTags(EnvironTagList tagList)
        {
            return objectTags.Intersect(tagList.objectTags).Any();
        }

        ///<summary> Checks for matching objects between objectTags and the given ObjectTag IEnumerable. </summary>
        public bool MatchingTags(IEnumerable<ObjectTag> tagList)
        {
            return objectTags.Intersect(tagList).Any();
        }
        #endregion
    }
}