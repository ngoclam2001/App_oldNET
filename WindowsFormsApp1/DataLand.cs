using System.Collections.Generic;

namespace ClassDataLand
{
    public class Person
    {
        public string PersonName { get; set; }
        public string PersonCCCD { get; set; }
    }

    public class Land
    {
        public string LandNumber { get; set; }
        public string LandMapNumber { get; set; }
        public int LandArea { get; set; }
        public string LandClass { get; set; }
        public string LandUseDate { get; set; }
        public string LandUse { get; set; }
        public string LandAddress { get; set; }
        public string LandPurpose { get; set; }
    }

    public class Asset
    {
        public string AssetName { get; set; }
        public string AssetArea { get; set; }
        public string AssetAreaUse { get; set; }
        public string AssetNumberFloor { get; set; }
        public string AssetStructure { get; set; }
        public string AssetLevel { get; set; }
        public string AssetUse { get; set; }
        public string AssetUseTime { get; set; }
        public string AssetAddress { get; set; }
    }

    public class DataLand
    {
        public string Id { get; set; }
        public Dictionary<int, Person> Muc1 { get; set; }
        public Dictionary<int, Land> Muc2 { get; set; }
        public Dictionary<int, Asset> Muc3 { get; set; }
        public Dictionary<int, string> Muc4 { get; set; }
        public bool IsCheckMuc2 { get; set; }
        public bool IsCheckMuc3 { get; set; }
    }
}
