using System;
using System.Collections.Generic;

public class Cursors
{
    public object previous { get; set; }
    public string next { get; set; }
}

public class Tag
{
    public string uri { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class Gltf
{
    public int size { get; set; }
}

public class Archives
{
    public Gltf gltf { get; set; }
}

public class JImage
{
    public string uid { get; set; }
    public int width { get; set; }
    public int size { get; set; }
    public string url { get; set; }
    public int height { get; set; }
}

public class Avatar
{
    public string uri { get; set; }
    public IList<JImage> images { get; set; }
}

public class User
{
    public string uid { get; set; }
    public string account { get; set; }
    public string uri { get; set; }
    public Avatar avatar { get; set; }
    public string displayName { get; set; }
    public string profileUrl { get; set; }
    public string username { get; set; }
}

public class Category
{
    public string name { get; set; }
}
public class Thumbnails
{
    public IList<JImage> images { get; set; }
}

public class Result
{
    public string name { get; set; }
    public DateTime? staffpickedAt { get; set; }
    public string uri { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime publishedAt { get; set; }
    public int animationCount { get; set; }
    public IList<Tag> tags { get; set; }
    public bool isDownloadable { get; set; }
    public Archives archives { get; set; }
    public string embedUrl { get; set; }
    public int commentCount { get; set; }
    public bool isAgeRestricted { get; set; }
    public string uid { get; set; }
    public string description { get; set; }
    public User user { get; set; }
    public int vertexCount { get; set; }
    public int likeCount { get; set; }
    public IList<Category> categories { get; set; }
    public int faceCount { get; set; }
    public int viewCount { get; set; }
    public string viewerUrl { get; set; }
    public Thumbnails thumbnails { get; set; }
}

public class JSonResponceObject
{
    public Cursors cursors { get; set; }
    public object previous { get; set; }
    public string next { get; set; }
    public IList<Result> results { get; set; }
}