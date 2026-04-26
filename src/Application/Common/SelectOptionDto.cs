namespace TravelCleanArch.Application.Common;

public sealed record SelectOptionDto(
 string Value,
 string Text,
 bool Selected = false
);
