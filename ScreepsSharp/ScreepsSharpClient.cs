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
    private HttpClient _client = new HttpClient();

    private string Token { get; set; }

    /// <summary>
    /// Signs in a user using the provided email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> SignInAsync(string email, string password)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "https://screeps.com/api/auth/signin")
        {
            Content = new StringContent($"{{\"email\":\"{email}\",\"password\":\"{password}\"}}")
        };
        req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            Token = cont["token"].ToString();
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
        var req = new HttpRequestMessage(HttpMethod.Get, $"https://screeps.com/api/auth/me");
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
    /// <param name="room"></param>
    /// <param name="shard"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public async Task<ScreepsRoomData?> GetRoomOverviewAsync(string room, string shard, int interval = 180)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"https://screeps.com/api/game/room-overview?interval={interval}&shard={shard}&room={room}");
        req.Headers.Add("X-Token", Token);
        req.Headers.Add("X-Username", Token);
        var res = await _client.SendAsync(req).ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ScreepsRoomData>(cont);
        }
        else return null;
    }

    /// <summary>
    /// Returns a partial user object for the user provided
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<ScreepsUserData?> GetUserDataAsync(string username)
    {
        var res = await _client.GetAsync($"https://screeps.com/api/user/find?username={username}").ConfigureAwait(false);
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
        var res = await _client.GetAsync($"https://screeps.com/api/leaderboard/seasons").ConfigureAwait(false);
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
    /// <param name="username"></param>
    /// <param name="season"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public async Task<ScreepsUserSeasonData?> GetUserSingleSeasonDataAsync(string username, string season, string mode = "world")
    {
        if (string.IsNullOrEmpty(season)) season = DateTime.Now.ToString(@"yyyy-MM");
        var res = await _client.GetAsync($"https://screeps.com/api/leaderboard/find?mode={mode}&season={season}&username={username}").ConfigureAwait(false);
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
    /// <param name="username"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ScreepsUserSeasonData>> GetUserSeasonsDataAsync(string username, string mode = "world")
    {
        var res = await _client.GetAsync($"https://screeps.com/api/leaderboard/find?mode={mode}&username={username}").ConfigureAwait(false);
        if (res.IsSuccessStatusCode)
        {
            var cont = JObject.Parse(await res.Content.ReadAsStringAsync().ConfigureAwait(false));
            return JsonConvert.DeserializeObject<IReadOnlyList<ScreepsUserSeasonData>>(cont.SelectToken("list").ToString());
        }
        else return null;
    }
}