using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelCleanArch.Application.Abstractions.Persistence;
using TravelCleanArch.Application.Abstractions.Security;
using TravelCleanArch.Domain.Constants;
using TravelCleanArch.Domain.Entities.Master;
using TravelCleanArch.Domain.Entities.Media;
using TravelCleanArch.Infrastructure.Persistence;
using TravelCleanArch.Web.Areas.Admin.Models;

namespace TravelCleanArch.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppRoles.Admin)]
[Route("admin/service-regions")]
public sealed class ServiceRegionsController(
    IUnitOfWork uow,
    ICurrentUser currentUser,
    AppDbContext dbContext,
    IWebHostEnvironment env) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? name = null, int? serviceTypeId = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, name, serviceTypeId, ct);
    }

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexPost(int page = 1, int pageSize = 10, string? name = null, int? serviceTypeId = null, CancellationToken ct = default)
    {
        return await BuildIndexResult(page, pageSize, name, serviceTypeId, ct);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadServiceTypeOptionsAsync(null, ct);
        return View("Upsert", new ServiceRegionFormViewModel());
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var item = await uow.ServiceRegionService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        var (bannerImagePath, bannerShortDescription, bannerDescription, dashboardImagePath) = await GetExistingMediaPathsAsync(item.Id, ct);
        var existingBannerImages = await GetBannerImagesAsync(item.Id, ct);
        await LoadServiceTypeOptionsAsync(item.ServiceTypeId, ct);

        return View("Upsert", new ServiceRegionFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            ServiceTypeId = item.ServiceTypeId,
            Reason = item.Reason,
            SlugURL = item.SlugURL,
            Ordering = item.Ordering,
            ExistingBannerImagePath = bannerImagePath,
            PrimaryBannerShortDescription = bannerShortDescription,
            PrimaryBannerDescription = bannerDescription,
            ExistingDashboardImagePath = dashboardImagePath,
            BannerImages = existingBannerImages
        });
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRegionFormViewModel model, CancellationToken ct)
    {
        await LoadServiceTypeOptionsAsync(model.ServiceTypeId, ct);
        if (!await ValidateModelAsync(model, ct)) return View("Upsert", model);

        var now = DateTime.UtcNow;
        var entity = new ServiceRegion
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            ServiceTypeId = model.ServiceTypeId,
            Reason = model.Reason.Trim(),
            SlugURL = string.IsNullOrWhiteSpace(model.SlugURL) ? null : model.SlugURL.Trim(),
            Ordering = model.Ordering,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        await uow.ServiceRegionService.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);

        await UpsertMediaAsync(entity.Id, FileMappingMediaTypes.BannerImage, model.BannerImage, model.PrimaryBannerShortDescription, model.PrimaryBannerDescription, ct);
        await UpsertBannerImagesAsync(entity.Id, model.BannerImages, ct);
        await UpsertMediaAsync(entity.Id, FileMappingMediaTypes.DashboardImage, model.DashboardImage, null, null, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service region created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/edit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceRegionFormViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();

        await LoadServiceTypeOptionsAsync(model.ServiceTypeId, ct);
        if (!await ValidateModelAsync(model, ct))
        {
            var existing = await GetExistingMediaPathsAsync(id, ct);
            model.ExistingBannerImagePath = existing.BannerImagePath;
            model.PrimaryBannerShortDescription = existing.BannerShortDescription;
            model.PrimaryBannerDescription = existing.BannerDescription;
            model.ExistingDashboardImagePath = existing.DashboardImagePath;
            model.BannerImages = await GetBannerImagesAsync(id, ct);
            return View("Upsert", model);
        }

        var item = await uow.ServiceRegionService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        item.Name = model.Name.Trim();
        item.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        item.ServiceTypeId = model.ServiceTypeId;
        item.Reason = model.Reason.Trim();
        item.SlugURL = string.IsNullOrWhiteSpace(model.SlugURL) ? null : model.SlugURL.Trim();
        item.Ordering = model.Ordering;
        item.UpdatedAtUtc = DateTime.UtcNow;
        item.UpdatedBy = currentUser.UserId;

        await UpsertMediaAsync(item.Id, FileMappingMediaTypes.BannerImage, model.BannerImage, model.PrimaryBannerShortDescription, model.PrimaryBannerDescription, ct);
        await UpsertBannerImagesAsync(item.Id, model.BannerImages, ct);
        await UpsertMediaAsync(item.Id, FileMappingMediaTypes.DashboardImage, model.DashboardImage, null, null, ct);

        await uow.SaveChangesAsync(ct);
        TempData["SuccessMessage"] = "Service region updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var item = await uow.ServiceRegionService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();

        await RemoveAllMediaAsync(item.Id, ct);
        uow.ServiceRegionService.Remove(item);
        await uow.SaveChangesAsync(ct);

        TempData["SuccessMessage"] = "Service region deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> BuildIndexResult(int page, int pageSize, string? name, int? serviceTypeId, CancellationToken ct)
    {
        var allowedPageSizes = new[] { 10, 20, 50, 100, -1 };
        var currentPage = Math.Max(1, page);
        var currentPageSize = allowedPageSizes.Contains(pageSize) ? pageSize : 10;
        var currentName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        var currentServiceTypeId = serviceTypeId is > 0 ? serviceTypeId : null;

        ViewBag.PageSize = currentPageSize;
        ViewBag.Name = currentName;
        ViewBag.ServiceTypeId = currentServiceTypeId;

        var serviceTypeFilterOptions = await GetServiceTypeFilterOptionsAsync(currentServiceTypeId, ct);
        ViewBag.ServiceTypeOptions = serviceTypeFilterOptions;

        var model = await uow.ServiceRegionService.ListOrderedAsync(currentPage, currentPageSize, currentName, currentServiceTypeId, ct);

        var ids = model.Items.Select(x => x.Id).ToArray();
        var media = await dbContext.FileMappings
            .AsNoTracking()
            .Include(x => x.FileDetail)
            .Where(x =>
                x.TargetType == FileMappingTargetTypes.ServiceRegion &&
                ids.Contains(x.TargetId) &&
                (x.MediaType == FileMappingMediaTypes.BannerImage || x.MediaType == FileMappingMediaTypes.DashboardImage))
            .ToListAsync(ct);

        ViewBag.BannerImageMap = media
            .Where(x => x.MediaType == FileMappingMediaTypes.BannerImage && x.FileDetail is not null)
            .GroupBy(x => x.TargetId)
            .ToDictionary(x => x.Key, x => x.Select(y => y.FileDetail.FileURL).FirstOrDefault() ?? string.Empty);

        ViewBag.DashboardImageMap = media
            .Where(x => x.MediaType == FileMappingMediaTypes.DashboardImage && x.FileDetail is not null)
            .GroupBy(x => x.TargetId)
            .ToDictionary(x => x.Key, x => x.Select(y => y.FileDetail.FileURL).FirstOrDefault() ?? string.Empty);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView("_ServiceRegionsTable", model);
        }

        return View(nameof(Index), model);
    }

    private async Task<bool> ValidateModelAsync(ServiceRegionFormViewModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            ModelState.AddModelError(nameof(model.Name), "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(model.Reason))
        {
            ModelState.AddModelError(nameof(model.Reason), "Reason is required.");
        }

        var serviceTypeExists = model.ServiceTypeId > 0 &&
                                await uow.ServiceTypeService.AnyAsync(x => x.Id == model.ServiceTypeId, ct);
        if (!serviceTypeExists)
        {
            ModelState.AddModelError(nameof(model.ServiceTypeId), "Select a valid service type.");
        }

        ValidateImageInput(model.BannerImage, nameof(model.BannerImage));
        ValidateImageInput(model.DashboardImage, nameof(model.DashboardImage));
        for (var i = 0; i < model.BannerImages.Count; i++)
        {
            var image = model.BannerImages[i];
            ValidateImageInput(image.File, $"BannerImages[{i}].File");
        }

        return ModelState.IsValid;
    }

    private void ValidateImageInput(IFormFile? file, string fieldName)
    {
        if (file is not { Length: > 0 })
        {
            return;
        }

        if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(fieldName, "Only image files are allowed.");
        }
    }

    private async Task LoadServiceTypeOptionsAsync(int? selectedId, CancellationToken ct)
    {
        var items = await uow.ServiceTypeService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), selectedId == x.Id))
            .ToListAsync(ct);

        items.Insert(0, new SelectListItem("-- Select Service Type --", string.Empty, !selectedId.HasValue || selectedId.Value <= 0));
        ViewBag.ServiceTypeOptions = items;
    }

    private async Task<List<SelectListItem>> GetServiceTypeFilterOptionsAsync(int? selectedId, CancellationToken ct)
    {
        var items = await uow.ServiceTypeService.Query()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString(), selectedId == x.Id))
            .ToListAsync(ct);

        items.Insert(0, new SelectListItem("All", string.Empty, !selectedId.HasValue));
        return items;
    }

    private async Task<(string? BannerImagePath, string? BannerShortDescription, string? BannerDescription, string? DashboardImagePath)> GetExistingMediaPathsAsync(int serviceRegionId, CancellationToken ct)
    {
        var media = await dbContext.FileMappings
            .AsNoTracking()
            .Include(x => x.FileDetail)
            .Where(x => x.TargetType == FileMappingTargetTypes.ServiceRegion && x.TargetId == serviceRegionId)
            .ToListAsync(ct);

        var banner = media
            .Where(x => x.MediaType == FileMappingMediaTypes.BannerImage)
            .Select(x => x.FileDetail?.FileURL)
            .FirstOrDefault();

        var dashboard = media
            .Where(x => x.MediaType == FileMappingMediaTypes.DashboardImage)
            .Select(x => x.FileDetail?.FileURL)
            .FirstOrDefault();

        var bannerShortDescription = media
            .Where(x => x.MediaType == FileMappingMediaTypes.BannerImage)
            .Select(x => x.FileDetail != null ? x.FileDetail.ShortDescription : null)
            .FirstOrDefault();

        var bannerDescription = media
            .Where(x => x.MediaType == FileMappingMediaTypes.BannerImage)
            .Select(x => x.FileDetail != null ? x.FileDetail.Description : null)
            .FirstOrDefault();

        return (banner, bannerShortDescription, bannerDescription, dashboard);
    }

    private async Task<List<ServiceRegionBannerImageInput>> GetBannerImagesAsync(int serviceRegionId, CancellationToken ct)
    {
        return await dbContext.FileMappings
            .AsNoTracking()
            .Include(x => x.FileDetail)
            .Where(x =>
                x.TargetType == FileMappingTargetTypes.ServiceRegion &&
                x.TargetId == serviceRegionId &&
                x.MediaType == FileMappingMediaTypes.BannerImage &&
                x.FileDetail != null)
            .OrderBy(x => x.Id)
            .Select(x => new ServiceRegionBannerImageInput
            {
                ExistingFileDetailId = x.FileDetailId,
                ExistingPath = x.FileDetail.FileURL,
                ShortDescription = x.FileDetail.ShortDescription,
                Description = x.FileDetail.Description
            })
            .ToListAsync(ct);
    }

    private async Task UpsertBannerImagesAsync(int serviceRegionId, List<ServiceRegionBannerImageInput> bannerImages, CancellationToken ct)
    {
        if (bannerImages.Count == 0)
        {
            return;
        }

        foreach (var image in bannerImages.Where(x => !x.Remove))
        {
            if (image.ExistingFileDetailId is > 0)
            {
                var existingFileDetail = await dbContext.FileDetails.FirstOrDefaultAsync(x => x.Id == image.ExistingFileDetailId.Value, ct);
                if (existingFileDetail is null)
                {
                    continue;
                }

                existingFileDetail.ShortDescription = string.IsNullOrWhiteSpace(image.ShortDescription) ? null : image.ShortDescription.Trim();
                existingFileDetail.Description = string.IsNullOrWhiteSpace(image.Description) ? null : image.Description.Trim();
                existingFileDetail.UpdatedAtUtc = DateTime.UtcNow;
                existingFileDetail.UpdatedBy = currentUser.UserId;
                continue;
            }

            if (image.File is not { Length: > 0 })
            {
                continue;
            }

            var fileUrl = await SaveImageAsync(image.File, "banner", ct);
            var now = DateTime.UtcNow;
            var fileDetail = new FileDetail
            {
                FileURL = fileUrl,
                OriginalName = Path.GetFileName(image.File.FileName),
                ContentType = image.File.ContentType,
                FileName = Path.GetFileName(fileUrl),
                ShortDescription = string.IsNullOrWhiteSpace(image.ShortDescription) ? null : image.ShortDescription.Trim(),
                Description = string.IsNullOrWhiteSpace(image.Description) ? null : image.Description.Trim(),
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                CreatedBy = currentUser.UserId,
                UpdatedBy = currentUser.UserId
            };

            var mapping = new FileMapping
            {
                FileDetail = fileDetail,
                TargetId = serviceRegionId,
                TargetType = FileMappingTargetTypes.ServiceRegion,
                MediaType = FileMappingMediaTypes.BannerImage,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
                CreatedBy = currentUser.UserId,
                UpdatedBy = currentUser.UserId
            };

            await dbContext.FileMappings.AddAsync(mapping, ct);
        }
    }

    private async Task UpsertMediaAsync(int serviceRegionId, string mediaType, IFormFile? file, string? shortDescription, string? description, CancellationToken ct)
    {
        if (file is not { Length: > 0 })
        {
            return;
        }

        var oldMappings = await dbContext.FileMappings
            .Include(x => x.FileDetail)
            .Where(x =>
                x.TargetType == FileMappingTargetTypes.ServiceRegion &&
                x.TargetId == serviceRegionId &&
                x.MediaType == mediaType)
            .ToListAsync(ct);

        var oldMappingIds = oldMappings.Select(x => x.Id).ToHashSet();
        var oldFileDetails = oldMappings
            .Where(x => x.FileDetail is not null)
            .Select(x => x.FileDetail!)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        if (oldMappings.Count > 0)
        {
            dbContext.FileMappings.RemoveRange(oldMappings);
        }

        foreach (var oldFile in oldFileDetails)
        {
            var hasOtherMappings = await dbContext.FileMappings
                .AnyAsync(x => x.FileDetailId == oldFile.Id && !oldMappingIds.Contains(x.Id), ct);
            if (hasOtherMappings)
            {
                continue;
            }

            DeletePhysicalFile(oldFile.FileURL);
            dbContext.FileDetails.Remove(oldFile);
        }

        var fileUrl = await SaveImageAsync(file, mediaType == FileMappingMediaTypes.BannerImage ? "banner" : "dashboard", ct);
        var now = DateTime.UtcNow;

        var fileDetail = new FileDetail
        {
            FileURL = fileUrl,
            OriginalName = Path.GetFileName(file.FileName),
            ContentType = file.ContentType,
            FileName = Path.GetFileName(fileUrl),
            ShortDescription = string.IsNullOrWhiteSpace(shortDescription) ? null : shortDescription.Trim(),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        var mapping = new FileMapping
        {
            FileDetail = fileDetail,
            TargetId = serviceRegionId,
            TargetType = FileMappingTargetTypes.ServiceRegion,
            MediaType = mediaType,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            CreatedBy = currentUser.UserId,
            UpdatedBy = currentUser.UserId
        };

        await dbContext.FileMappings.AddAsync(mapping, ct);
    }

    private async Task RemoveAllMediaAsync(int serviceRegionId, CancellationToken ct)
    {
        var mappings = await dbContext.FileMappings
            .Include(x => x.FileDetail)
            .Where(x => x.TargetType == FileMappingTargetTypes.ServiceRegion && x.TargetId == serviceRegionId)
            .ToListAsync(ct);

        if (mappings.Count == 0)
        {
            return;
        }

        var mappingIds = mappings.Select(x => x.Id).ToHashSet();
        var fileDetails = mappings
            .Where(x => x.FileDetail is not null)
            .Select(x => x.FileDetail!)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        dbContext.FileMappings.RemoveRange(mappings);

        foreach (var fileDetail in fileDetails)
        {
            var hasOtherMappings = await dbContext.FileMappings
                .AnyAsync(x => x.FileDetailId == fileDetail.Id && !mappingIds.Contains(x.Id), ct);
            if (hasOtherMappings)
            {
                continue;
            }

            DeletePhysicalFile(fileDetail.FileURL);
            dbContext.FileDetails.Remove(fileDetail);
        }
    }

    private async Task<string> SaveImageAsync(IFormFile file, string prefix, CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName);
        var folder = Path.Combine(env.WebRootPath, "uploads", "service-regions");
        Directory.CreateDirectory(folder);

        var fileName = $"service-region-{prefix}-{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(folder, fileName);
        await using var stream = System.IO.File.Create(fullPath);
        await file.CopyToAsync(stream, ct);

        return Path.Combine("uploads", "service-regions", fileName).Replace('\\', '/');
    }

    private void DeletePhysicalFile(string? fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        var relativePath = fileUrl.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(env.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
}
