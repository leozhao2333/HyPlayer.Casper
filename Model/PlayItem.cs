﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace HyPlayer.Casper.Model;

public class ProvidableItem
{
    public string ActualId;
    public string Name;
    public string PlaySourceType;
    public string ProviderId;
    public ProvidableItemType Type;

    public ProvidableItem(string id)
    {
        ProviderId = Id.Substring(0, 3);
        PlaySourceType = Id.Substring(3, 2);
        ActualId = Id.Substring(5);
    }

    public ProvidableItem(string providerId, string playSourceType, string actualId)
    {
        ProviderId = providerId;
        PlaySourceType = playSourceType;
        ActualId = actualId;
    }

    protected ProvidableItem()
    {
    }

    public string Id => ProviderId + PlaySourceType + ActualId;
    public string InProviderId => PlaySourceType + ActualId;
}

public enum ProvidableItemType
{
    Single, // 单曲
    SongContainer, // 歌曲容器 (内容仅为歌曲)
    MixedContent, // 混合内容 (包含歌曲和歌单)
}

public class SongContainer : ProvidableItem
{
    public string Creator;
    public string CreatorId; // 此处 ID 需要加 ProviderId
    public string Description;
    public PlayListSourceType PlayListSourceType;
}

public class SingleSong : ProvidableItem
{
    public Album Album;
    public List<Artist> Artists;
    public bool Available;
    public string Description;
    public TimeSpan Duration;
    public string TranslatedName;
    public string ArtistsString => string.Join(" / ", Artists.Select(t => t.Name));
}

public abstract class Album : SongContainer
{
    public string PicUrl;
    public abstract Task<BitmapImage> GetCoverImage(int picSizeX = -1, int picSizeY = -1);
    public abstract Task<IRandomAccessStream> GetCoverImageStream(int picSizeX = -1, int picSizeY = -1);
}

public class Artist : ProvidableItem
{
    // Empty
}

public class SongLyric
{
    public static SongLyric PureSong = new()
        { HaveTranslation = false, LyricTime = TimeSpan.Zero, PureLyric = "纯音乐 请欣赏" };

    public static SongLyric NoLyric = new()
        { HaveTranslation = false, LyricTime = TimeSpan.Zero, PureLyric = "无歌词 请欣赏" };

    public static SongLyric LoadingLyric = new()
        { HaveTranslation = false, LyricTime = TimeSpan.Zero, PureLyric = "加载歌词中..." };

    public bool HaveTranslation;
    public TimeSpan LyricTime;
    public string PureLyric;
    public string Translation;
}

public enum PlayListSourceType
{
    Liner,
    Interactive // 用于私人 FM 之类的场景, 播放列表并不确定, 需要在播放时更新
}