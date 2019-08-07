using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appraisit.Models;
using RedditSharp.Things;

namespace Appraisit.Constants
{
    internal class Constants
    {
        public static readonly string redditBaseUrl = "https://reddit.com/";
        public static readonly string redditApiBaseUrl = "https://ssl.reddit.com/api/v1/";
        public static readonly string redditOauthApiBaseUrl = "https://oauth.reddit.com/";

        public readonly static List<string> scopeList = new List<string>
        {
            "creddits",
            "modcontributors",
            "modmail",
            "modconfig",
            "subscribe",
            "structuredstyles",
            "vote",
            "wikiedit",
            "mysubreddits",
            "submit",
            "modlog",
            "modposts",
            "modflair",
            "save",
            "modothers",
            "read",
            "privatemessages",
            "report",
            "identity",
            "livemanage",
            "account",
            "modtraffic",
            "wikiread",
            "edit",
            "modwiki",
            "modself",
            "history",
            "flair",
        };

        
    }
}
