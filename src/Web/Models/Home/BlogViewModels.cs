namespace TravelCleanArch.Web.Models.Home;

public sealed class BlogCardViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Summary { get; init; }
    public string? ThumbnailImagePath { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
    public bool IsFeatured { get; init; }
}

public sealed class BlogListViewModel
{
    public BlogCardViewModel? FeaturedPost { get; init; }
    public IReadOnlyList<BlogCardViewModel> OtherPosts { get; init; } = [];
}

public sealed class BlogDetailViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? Summary { get; init; }
    public string ContentHtml { get; init; } = string.Empty;
    public string? HeroImagePath { get; init; }
    public DateTime? PublishedOnUtc { get; init; }
    public IReadOnlyList<BlogCardViewModel> LatestPosts { get; init; } = [];
}
