using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScreepsSharp.Objects
{
    public struct ScreepsUserData
    {
        [JsonProperty("_id")]
        public string UserId { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("cpu")]
        public int? CPU { get; private set; }

        [JsonProperty("password")]
        public string Password { get; private set; }

        [JsonProperty("notifyPrefs")]
        public ScreepsNotifyPrefs? NotificationPreferances { get; private set; }

        [JsonProperty("credits")]
        public float? Credits { get; private set; }

        [JsonProperty("lastChargeTime")]
        public DateTime? LastChargeTime { get; private set; }

        [JsonProperty("lastTweetTime")]
        public DateTime? LastTweetTime { get; private set; }

        [JsonProperty("github")]
        public GithubData? Github { get; private set; }

        [JsonProperty("steam")]
        public SteamData? Steam { get; private set; }

        [JsonProperty("twitter")]
        public TwitterData? Twitter { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }

        [JsonProperty("badge")]
        public ScreepsBadgeData Badge { get; private set; }

        [JsonProperty("gcl")]
        public ulong GCL { get; private set; }
    }

    public struct ScreepsNotifyPrefs
    {
        [JsonProperty("sendOnline")]
        public bool SendOnline { get; private set; }

        [JsonProperty("errorsInterval")]
        public ulong ErrorsInterval { get; private set; }

        [JsonProperty("disabledOnMessages")]
        public bool DisabledOnMessages { get; private set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; private set; }

        [JsonProperty("interval")]
        public ulong Interval { get; private set; }
    }

    public struct ScreepsBadgeData
    {
        [JsonProperty("type")]
        public int Type { get; private set; }

        [JsonProperty("color1")]
        public string PrimaryColour { get; private set; }

        [JsonProperty("color2")]
        public string SecondaryColour { get; private set; }

        [JsonProperty("color3")]
        public string TertiaryColour { get; private set; }

        [JsonProperty("param")]
        public int Parameter { get; private set; }

        [JsonProperty("flip")]
        public bool Flip { get; private set; }
    }

    public struct TwitterData
    {
        [JsonProperty("followers_count")]
        public int FollowersCount { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }
    }

    public struct GithubData
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("username")]
        public string Username { get; private set; }
    }

    public struct SteamData
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }
    }

    public struct ScreepsSeasonData
    {
        [JsonProperty("_id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("date")]
        public DateTime Date { get; private set; }
    }

    public struct ScreepsUserSeasonData
    {
        [JsonProperty("_id")]
        public string Id { get; private set; }

        [JsonProperty("season")]
        public string Season { get; private set; }

        [JsonProperty("user")]
        public string UserId { get; private set; }

        [JsonProperty("score")]
        public int Score { get; private set; }

        [JsonProperty("rank")]
        public int Rank { get; private set; }
    }

    public struct ScreepsRoomData
    {
        [JsonIgnore]
        public IReadOnlyList<ScreepsRoomTerrain> Terrain { get; internal set; }

        [JsonProperty("ok")]
        public int Okay { get; private set; }

        [JsonProperty("owner")]
        public ScreepsUserData? Owner { get; private set; }

        [JsonProperty("stats")]
        public ScreepsRoomStats Stats { get; private set; }

        [JsonProperty("statsMax")]
        public ScreepsMaxRoomStats MaxStats { get; private set; }

        [JsonProperty("totals")]
        public ScreepsTotalRoomStats StatTotals { get; private set; }
    }

    public struct ScreepsRoomStats
    {
        [JsonProperty("energyHarvested")]
        public IReadOnlyList<KeyValuePair<int, int>> EnergyHarvested { get; private set; }

        [JsonProperty("energyConstruction")]
        public IReadOnlyList<KeyValuePair<int, int>> EnergySpentOnConstruction { get; private set; }

        [JsonProperty("energyCreeps")]
        public IReadOnlyList<KeyValuePair<int, int>> EnergySpentOnCreeps { get; private set; }

        [JsonProperty("energyControl")]
        public IReadOnlyList<KeyValuePair<int, int>> EnergySpentOnController { get; private set; }

        [JsonProperty("creepsProduced")]
        public IReadOnlyList<KeyValuePair<int, int>> CreepsProduced { get; private set; }

        [JsonProperty("creepsLost")]
        public IReadOnlyList<KeyValuePair<int, int>> CreepsLost { get; private set; }

        [JsonProperty("powerProcessed")]
        public IReadOnlyList<KeyValuePair<int, int>> PowerProcessed { get; private set; }
    }

    public struct ScreepsMaxRoomStats
    {
        [JsonProperty("power1440")]
        public int Power_Interval1440 { get; private set; }

        [JsonProperty("powerProcessed1440")]
        public int PowerProcessed_Interval1440 { get; private set; }

        [JsonProperty("energy1440")]
        public int Energy_Interval1440 { get; private set; }

        [JsonProperty("energyControl1440")]
        public int EnergyControl_Interval1440 { get; private set; }

        [JsonProperty("energyConstruction1440")]
        public int EnergyConstruction_Interval1440 { get; private set; }

        [JsonProperty("energyCreeps1440")]
        public int EnergyCreeps_Interval1440 { get; private set; }

        [JsonProperty("creepsLost1440")]
        public int CreepsLost_Interval1440 { get; private set; }

        [JsonProperty("creepsProduced1440")]
        public int CreepsProduced_Interval1440 { get; private set; }

        [JsonProperty("energyHarvested1440")]
        public int EnergyHarvested_Interval1440 { get; private set; }

        [JsonProperty("power180")]
        public int Power_Interval180 { get; private set; }

        [JsonProperty("powerProcessed180")]
        public int PowerProcessed_Interval180 { get; private set; }

        [JsonProperty("creepsProduced180")]
        public int CreepsProduced_Interval180 { get; private set; }

        [JsonProperty("energy180")]
        public int Energy_Interval180 { get; private set; }

        [JsonProperty("energyControl180")]
        public int EnergyControl_Interval180 { get; private set; }

        [JsonProperty("energyHarvested180")]
        public int EnergyHarvested_Interval180 { get; private set; }

        [JsonProperty("energyConstruction180")]
        public int EnergyConstruction_Interval180 { get; private set; }

        [JsonProperty("power8")]
        public int Power_Interval8 { get; private set; }

        [JsonProperty("powerProcessed8")]
        public int PowerProcessed_Interval8 { get; private set; }

        [JsonProperty("energy8")]
        public int Energy_Interval8 { get; private set; }

        [JsonProperty("energyControl8")]
        public int EnergyControl_Interval8 { get; private set; }

        [JsonProperty("creepsLost8")]
        public int CreepsLost_Interval8 { get; private set; }

        [JsonProperty("energyCreeps8")]
        public int EnergyCreeps_Interval8 { get; private set; }

        [JsonProperty("energyConstruction8")]
        public int EnergyConstruction_Interval8 { get; private set; }

        [JsonProperty("creepsProduced8")]
        public int CreepsProduced_Interval8 { get; private set; }

        [JsonProperty("energyHarvested8")]
        public int EnergyHarvested_Interval8 { get; private set; }
    }

    public struct ScreepsTotalRoomStats
    {
        [JsonProperty("energyHarvested")]
        public int TotalEnergyHarvested { get; private set; }

        [JsonProperty("creepsProduced")]
        public int TotalCreepsProduced { get; private set; }

        [JsonProperty("energyConstruction")]
        public int TotalEnergySpentOnConstruction { get; private set; }

        [JsonProperty("energyControl")]
        public int TotalEnergySpentOnController { get; private set; }

        [JsonProperty("energyCreeps")]
        public int TotalEnergySpentOnCreeps { get; private set; }
    }

    public struct ScreepsRoomTerrain
    {
        [JsonProperty("room")]
        public string Room { get; private set; }

        [JsonProperty("x")]
        public int X { get; private set; }

        [JsonProperty("y")]
        public int Y { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }
    }

    public struct ScreepsUserMessage
    {
        [JsonProperty("_id")]
        public string Id { get; private set; }

        [JsonProperty("message")]
        public ScreepsMessage Message { get; private set; }
    }

    public struct ScreepsMessage
    {
        [JsonProperty("_id")]
        public string Id { get; private set; }

        [JsonProperty("user")]
        public string User { get; private set; }

        [JsonProperty("respondent")]
        public string Respondent { get; private set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; private set; }

        [JsonProperty("type")]
        public int Type { get; private set; }

        [JsonProperty("text")]
        public string Text { get; private set; }

        [JsonProperty("unread")]
        public bool IsUnread { get; private set; }
    }
}
