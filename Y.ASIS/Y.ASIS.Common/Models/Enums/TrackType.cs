namespace Y.ASIS.Models.Enums
{
    /// <summary>
    /// 股道类型
    /// </summary>
    public enum TrackType
    {
        /// <summary>
        /// 一股道一列位一车 Track-Position-Train
        /// </summary>
        TPT,

        /// <summary>
        /// 一股道一列位两车 Track-Position-Train-Train
        /// </summary>
        TPTT,

        /// <summary>
        /// 一股道两列位两车 Track-Position-Position-Train-Train
        /// </summary>
        TPPTT
    }
}
