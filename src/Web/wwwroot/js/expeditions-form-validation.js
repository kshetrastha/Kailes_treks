(() => {
  'use strict';

  const forms = document.querySelectorAll('.needs-validation');

  const resetCustomValidity = (form) => {
    form.querySelectorAll('input, select, textarea').forEach((field) => {
      if (field instanceof HTMLInputElement || field instanceof HTMLSelectElement || field instanceof HTMLTextAreaElement) {
        field.setCustomValidity('');
      }
    });
  };

  const getTabTriggerForField = (field) => {
    const tabPane = field.closest('.tab-pane');
    if (!tabPane?.id) return null;
    return document.querySelector(`[data-bs-toggle="tab"][href="#${tabPane.id}"]`);
  };

  const applyCustomRules = (form) => {
    const heroImageUrl = form.querySelector('#HeroImageUrl');
    const heroImageFile = form.querySelector('#HeroImageFile');
    const heroVideoUrl = form.querySelector('#HeroVideoUrl');

    const hasHeroImageUrl = !!heroImageUrl?.value?.trim();
    const hasHeroVideoUrl = !!heroVideoUrl?.value?.trim();
    const hasHeroImageFile = !!(heroImageFile instanceof HTMLInputElement && heroImageFile.files && heroImageFile.files.length > 0);

    if (!hasHeroImageUrl && !hasHeroVideoUrl && !hasHeroImageFile && heroVideoUrl instanceof HTMLInputElement) {
      heroVideoUrl.setCustomValidity('Provide at least one primary media source: image URL, video URL, or uploaded image.');
    }

    const minGroupSize = form.querySelector('#MinGroupSize');
    const maxGroupSize = form.querySelector('#MaxGroupSize');
    if (minGroupSize instanceof HTMLInputElement && maxGroupSize instanceof HTMLInputElement) {
      const minValue = Number(minGroupSize.value || 0);
      const maxValue = Number(maxGroupSize.value || 0);
      if (minValue > 0 && maxValue > 0 && minValue > maxValue) {
        maxGroupSize.setCustomValidity('Max group size must be greater than or equal to min group size.');
      }
    }

    form.querySelectorAll('#departureWrap .departure-row').forEach((row) => {
      const start = row.querySelector('input[name$=".StartDate"]');
      const end = row.querySelector('input[name$=".EndDate"]');
      if (!(start instanceof HTMLInputElement) || !(end instanceof HTMLInputElement)) return;

      const hasAnyDepartureData = !!(start.value || end.value || row.querySelector('input[name$=".ForDays"]')?.value || row.querySelector('input[name$=".GroupSize"]')?.value);
      if (!hasAnyDepartureData) return;

      if (!start.value || !end.value) {
        (end.value ? start : end).setCustomValidity('Both start and end dates are required for departures.');
        return;
      }

      if (new Date(end.value) < new Date(start.value)) {
        end.setCustomValidity('Departure end date must be on or after start date.');
      }
    });

    form.querySelectorAll('#reviewWrap .review-row').forEach((row) => {
      const fullName = row.querySelector('input[name$=".FullName"]');
      const email = row.querySelector('input[name$=".EmailAddress"]');
      const reviewText = row.querySelector('input[name$=".ReviewText"]');
      if (!(fullName instanceof HTMLInputElement) || !(email instanceof HTMLInputElement) || !(reviewText instanceof HTMLInputElement)) return;

      const hasAnyReviewData = !!(fullName.value.trim() || email.value.trim() || reviewText.value.trim());
      if (!hasAnyReviewData) return;

      if (!fullName.value.trim()) fullName.setCustomValidity('Reviewer name is required.');
      if (!email.value.trim()) email.setCustomValidity('Reviewer email is required.');
      if (!reviewText.value.trim()) reviewText.setCustomValidity('Review text is required.');
    });
  };

  forms.forEach((form) => {
    form.addEventListener('submit', (event) => {
      resetCustomValidity(form);
      applyCustomRules(form);

      if (form.checkValidity()) {
        return;
      }

      event.preventDefault();
      event.stopPropagation();
      form.classList.add('was-validated');

      const firstInvalidField = form.querySelector(':invalid');
      if (firstInvalidField instanceof HTMLElement) {
        const tabTrigger = getTabTriggerForField(firstInvalidField);
        if (tabTrigger) {
          bootstrap.Tab.getOrCreateInstance(tabTrigger).show();
        }

        firstInvalidField.focus();
      }
    }, false);
  });
})();
