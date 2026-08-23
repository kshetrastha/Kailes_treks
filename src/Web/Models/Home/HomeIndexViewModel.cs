using TravelCleanArch.Application.Abstractions.Travel;
using TravelCleanArch.Domain.Entities;

namespace TravelCleanArch.Web.Models.Home;

public sealed class HomeReviewViewModel
{
    public string ReviewerName { get; init; } = string.Empty;
    public string? ReviewerRole { get; init; }
    public string ReviewText { get; init; } = string.Empty;
    public int Rating { get; init; } = 5;
    public string? ReviewerImagePath { get; init; }
}

public sealed class HomeIndexViewModel
{
    public string WhyWithUsHeader { get; init; } = "Because we are the best";
    public string WhyWithUsTitle { get; init; } = "Why with us?";
    public string WhyWithUsDescription { get; init; } = string.Empty;
    public string? WhyWithUsBackgroundImagePath { get; init; }
    //public IReadOnlyCollection<WhyWithUsItemViewModel> WhyWithUsItems { get; init; } = [];
    //public IReadOnlyCollection<WhyWithUsItemViewModel> WhyWithUsItems { get; init; } = [];
    public IReadOnlyList<WhyWithUsItemViewModel> WhyWithUsItems { get; init; }

    public string WhoWeAreHeader { get; init; } = "Leading Expedition Operator";
    public string WhoWeAreTitle { get; init; } = "Who we are?";
    public string WhoWeAreDescription { get; init; } = string.Empty;
    public string? WhoWeAreBackgroundImagePath { get; init; }
    public IReadOnlyList<WhoWeAreItemViewModel> WhoWeAreItems { get; init; } = [];
    public IReadOnlyList<BlogCardViewModel> RecentBlogs { get; init; } = [];
    public Banner GetBannerContent { get; set; } = new Banner();
    public List<TrekkingCountryGroupDto> TrekkingCountryGroupDtos { get; set; } =new List<TrekkingCountryGroupDto>();
     public List<TrekkingCountryPackageCountDto> TrekkingCountryPackageCountDtos { get; set; } = new List<TrekkingCountryPackageCountDto>();
    public IReadOnlyList<HomeReviewViewModel> Reviews { get; init; } = [];

    /// <summary>Content for the home page About section, managed under Admin / Company / About Us.</summary>
    public AboutUsPageViewModel? AboutUs { get; init; }
}
