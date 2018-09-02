using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScreepsSharp.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class ScreepsSharpClient
{
    private readonly string _requestBase = "https://screeps.com/api";
    private HttpClient _client = new HttpClient();

    private string Token { get; set; }

    /// <summary>
    /// Signs in a user using the provided email and password
    /// </summary>
    /// <param name="email">Your email</param>
    /// <param name="password">Your password</param>
    /// <returns></returns>
    public async Task<bool> SignInAsync(string email, string password)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"{_requestBase}/auth/signin")
        {
            Content = new StringContent($"{{\"email\":\"{email}\",\"password\":\"{password}\"}}")
        };
        req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            Token = cont["token"].ToString();
            _client.DefaultRequestHeaders.Add("X-Token", Token);
            _client.DefaultRequestHeaders.Add("X-Username", Token);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Gets the user data for the user who is currently signed in
    /// </summary>
    /// <returns></returns>
    public async Task<ScreepsUserData?> GetCurrentUserAsync()
    {
        if (Token == null) return null;
        var req = new HttpRequestMessage(HttpMethod.Get, $"{_requestBase}/auth/me");
        req.Headers.Add("X-Token", Token);
        req.Headers.Add("X-Username", Token);
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ScreepsUserData>(cont);
        }
        else return null;
    }

    /// <summary>
    /// Gets overview data of a given room in a given shard
    /// </summary>
    /// <param name="room">Name of the room, IE: W25N25</param>
    /// <param name="shard">Name of the shard, IE: shard2</param>
    /// <param name="interval">Set to either 8, 180, or 1440</param>
    /// <returns></returns>
    public async Task<ScreepsRoomData?> GetRoomOverviewAsync(string room, string shard, int interval = 180)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{_requestBase}/game/room-overview?interval={interval}&shard={shard}&room={room}");
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            var obj = JsonConvert.DeserializeObject<ScreepsRoomData>(cont);
            req = new HttpRequestMessage(HttpMethod.Get, $"{_requestBase}/game/room-terrain?room={room}&shard={shard}");
            res = await _client.SendAsync(req).ConfigureAwait(false);
            cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            var terobj = JsonConvert.DeserializeObject<IReadOnlyList<ScreepsRoomTerrain>>(JObject.Parse(cont).SelectToken("terrain").ToString());
            obj.Terrain = terobj;
            return obj;
        }
        else return null;
    }

    /// <summary>
    /// Gets a room's terrain data
    /// </summary>
    public async Task<IReadOnlyList<ScreepsRoomTerrain>> GetRoomTerrainAsync(string room)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{_requestBase}/game/room-terrain?room={room}");
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsRoomTerrain>>(JObject.Parse(cont).SelectToken("terrain").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Returns a partial user object for the user provided
    /// </summary>
    /// <param name="username">The target's in-game username</param>
    /// <returns></returns>
    public async Task<ScreepsUserData?> GetUserDataAsync(string username)
    {
        var res = await _client.GetAsync($"{_requestBase}/user/find?username={username}").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<ScreepsUserData?>(cont.SelectToken("user").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Gets the seasons history
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<ScreepsSeasonData>> GetSeasonsAsync()
    {
        var res = await _client.GetAsync($"{_requestBase}/leaderboard/seasons").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsSeasonData>>(cont.SelectToken("seasons").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Gets a user's season data from a given season, format is year-month
    /// </summary>
    /// <param name="username">The target's in-game username</param>
    /// <param name="season">Season, IE: 2018-08</param>
    /// <param name="mode">I actually have no idea but the default is "world"</param>
    /// <returns></returns>
    public async Task<ScreepsUserSeasonData?> GetUserSingleSeasonDataAsync(string username, string season, string mode = "world")
    {
        if (string.IsNullOrEmpty(season)) season = DateTime.Now.ToString(@"yyyy-MM");
        var res = await _client.GetAsync($"{_requestBase}/leaderboard/find?mode={mode}&season={season}&username={username}").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ScreepsUserSeasonData>(cont);
        }
        else return null;
    }

    /// <summary>
    /// Gets all of the user's seasons history
    /// </summary>
    /// <param name="username">The target's in-game username</param>
    /// <param name="mode">I actually have no idea but the default is "world"</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ScreepsUserSeasonData>> GetUserSeasonsDataAsync(string username, string mode = "world")
    {
        var res = await _client.GetAsync($"{_requestBase}/leaderboard/find?mode={mode}&username={username}").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsUserSeasonData>>(cont.SelectToken("list").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Gets all of the signed in user's messages
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<ScreepsUserMessage>> GetUserMessagesAsync()
    {
        var res = await _client.GetAsync($"{_requestBase}/user/messages/index").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsUserMessage>>(cont.SelectToken("messages").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Gets messages from a specific user
    /// </summary>
    /// <param name="_id">The string Id of the user</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ScreepsMessage>> GetUserMessagesAsync(string _id)
    {
        var res = await _client.GetAsync($"{_requestBase}/user/messages/list?respondent={_id}").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsMessage>>(cont.SelectToken("messages").ToString());
        }
        else return null;
    }

    /// <summary>
    /// Gets the world status. Ususally returns "Normal" and it is unknown what else it returns
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetWorldStatusAsync()
    {
        var res = await _client.GetAsync($"{_requestBase}/user/world-status").ConfigureAwait(false);
        if (res.IsSuccessStatusCode) return JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false)).SelectToken("status").ToString();
        else return null;
    }
}