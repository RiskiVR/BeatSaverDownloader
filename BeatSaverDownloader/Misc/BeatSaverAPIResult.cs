using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LegacyCore.Custom;
using SimpleJSON;

namespace BeatSaverDownloader
{

    public enum SongQueueState { Available, Queued, Downloading, Downloaded, Error }; 
    public enum Sort { Latest, Relevance, Rating, Curated, Random, Duration }

    [Serializable]
    public class DifficultyLevel
    {
        public string difficulty;
        public int difficultyRank;
        public string jsonPath;
        public int? offset;

        public DifficultyLevel(string Difficulty)
        {
            this.difficulty = Difficulty;
        }

        /*
        public DifficultyLevel(CustomLevelInfo.DifficultyLevelInfo difficultyLevel)
        {
            difficulty = difficultyLevel.difficulty;
            difficultyRank = difficultyLevel.difficultyRank;
        }*/

        public DifficultyLevel(CustomLevelInfo.DifficultyBeatmap difficultyLevel)
        {
            this.difficulty = difficultyLevel.difficulty;
        }

    }
    [Serializable]
    public class Song
    {
        public string id;
        public string beatname;
        public string ownerid;
        public string downloads;
        public string upvotes;
        public string plays;
        public string beattext;
        public string uploadtime;
        public string songName;
        public string songSubName;
        public string authorName;
        public string beatsPerMinute;
        public string downvotes;
        public string coverUrl;
        public string downloadUrl;
        public DifficultyLevel[] difficultyLevels;
        public string img;
        public string hash;

        public string path;

        public SongQueueState songQueueState = SongQueueState.Available;
        public Sort songSort;

        public float downloadingProgress = 0f;

        public Song()
        {

        }

        public Song(JSONNode jsonNode)
        {
            id = jsonNode["id"];
            beatname = jsonNode["name"];
            ownerid = jsonNode["uploader"]["id"];
            downloads = jsonNode["stats"]["downloads"];
            upvotes = jsonNode["stats"]["upvotes"];
            downvotes = jsonNode["stats"]["downvotes"];
            plays = jsonNode["stats"]["plays"];
            beattext = jsonNode["description"];
            uploadtime = jsonNode["createdAt"];
            songName = jsonNode["metadata"]["songName"];
            songSubName = jsonNode["metadata"]["songSubName"];
            authorName = jsonNode["metadata"]["songAuthorName"];
            beatsPerMinute = jsonNode["metadata"]["bpm"];
            coverUrl = jsonNode["versions"][0]["coverURL"];
            downloadUrl = jsonNode["versions"][0]["downloadURL"];
            hash = jsonNode["versions"][0]["hash"];
            JSONNode jsonnode = jsonNode["versions"][0]["diffs"];
            difficultyLevels = new DifficultyLevel[jsonnode.Count];
            for (int i = 0; i < jsonnode.Count; i++)
            {
                difficultyLevels[i] = new DifficultyLevel(jsonnode[i]["difficulty"]);
            }
        }

        public static Song FromSearchNode(JSONNode mainNode)
        {
            Song song = new Song();
            song.id = mainNode["id"];
            song.beatname = mainNode["name"];
            song.ownerid = mainNode["uploader"]["id"];
            song.downloads = mainNode["stats"]["downloads"];
            song.upvotes = mainNode["stats"]["upvotes"];
            song.downvotes = mainNode["stats"]["downvotes"];
            song.plays = mainNode["stats"]["plays"];
            song.beattext = mainNode["description"];
            song.uploadtime = mainNode["createdAt"];
            song.songName = mainNode["metadata"]["songName"];
            song.songSubName = mainNode["metadata"]["songSubName"];
            song.authorName = mainNode["metadata"]["songAuthorName"];
            song.beatsPerMinute = mainNode["metadata"]["bpm"];
            song.coverUrl = mainNode["versions"][0]["coverURL"];
            song.downloadUrl = mainNode["versions"][0]["downloadURL"];
            song.hash = mainNode["versions"][0]["hash"];
            JSONNode jsonnode = mainNode["versions"][0]["diffs"];
            song.difficultyLevels = new DifficultyLevel[jsonnode.Count];
            for (int i = 0; i < jsonnode.Count; i++)
            {
                song.difficultyLevels[i] = new DifficultyLevel(jsonnode[i]["difficulty"]);
            }
            return song;
        }

        public bool Compare(Song compareTo)
        {
            if (compareTo != null && songName == compareTo.songName)
            {
                if (difficultyLevels != null && compareTo.difficultyLevels != null)
                {
                    return (songSubName == compareTo.songSubName && authorName == compareTo.authorName && difficultyLevels.Length == compareTo.difficultyLevels.Length);
                }
                else
                {
                    return (songSubName == compareTo.songSubName && authorName == compareTo.authorName);
                }
            }
            else
            {
                return false;
            }
        }

        public Song(CustomLevelSO _data)
        {
            songName = _data.songName;
            songSubName = _data.songSubName;
            authorName = _data.songAuthorName;
            difficultyLevels = ConvertDifficultyLevels(_data.difficultyBeatmaps);
            path = _data.levelInfo.levelPath;
        }


        public DifficultyLevel[] ConvertDifficultyLevels(CustomLevelInfo.DifficultyBeatmap[] _difficultyLevels)
        {
            if (_difficultyLevels != null && _difficultyLevels.Length > 0)
            {
                DifficultyLevel[] buffer = new DifficultyLevel[_difficultyLevels.Length];

                for (int i = 0; i < _difficultyLevels.Length; i++)
                {
                    buffer[i] = new DifficultyLevel(_difficultyLevels[i]);
                }


                return buffer;
            }
            else
            {
                return null;
            }
        }

        
        public DifficultyLevel[] ConvertDifficultyLevels(IStandardLevelDifficultyBeatmap[] _difficultyLevels)
        {
            if (_difficultyLevels != null && _difficultyLevels.Length > 0)
            {
                DifficultyLevel[] buffer = new DifficultyLevel[_difficultyLevels.Length];

                for (int i = 0; i < _difficultyLevels.Length; i++)
                {
                    buffer[i] = new DifficultyLevel(_difficultyLevels[i].difficulty.ToString());
                }


                return buffer;
            }
            else
            {
                return null;
            }
        }

    }
    [Serializable]
    public class RootObject
    {
        public Song[] songs;
    }
}
