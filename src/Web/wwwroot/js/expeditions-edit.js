(function () {
  const form = document.querySelector('form');
  if (!form) return;

  const itineraryBody = document.querySelector('#itineraryTable tbody');

  const addHtml = (containerId, html) => {
    document.getElementById(containerId)?.insertAdjacentHTML('beforeend', html);
  };

  const reindexCollection = (containerSelector, rowSelector, prefix) => {
    const container = document.querySelector(containerSelector);
    if (!container) return;

    container.querySelectorAll(rowSelector).forEach((row, index) => {
      row.querySelectorAll('[name]').forEach((input) => {
        input.name = input.name.replace(new RegExp(`${prefix}\\[\\d+\\]`), `${prefix}[${index}]`);
      });
      if (row.classList.contains('itinerary-row')) {
        const textArea = row.querySelector('.itin-days');
        if (textArea) {
          textArea.dataset.index = `${index}`;
        }
      }
    });
  };

  document.getElementById('addItinerary')?.addEventListener('click', () => {
    const index = itineraryBody ? itineraryBody.querySelectorAll('.itinerary-row').length : 0;
    itineraryBody?.insertAdjacentHTML('beforeend',
      `<tr class="itinerary-row" data-index="${index}">
        <td><input type="hidden" name="Itineraries[${index}].Id" value="0" /><input name="Itineraries[${index}].SeasonTitle" class="form-control"/></td>
        <td><input name="Itineraries[${index}].SortOrder" class="form-control" value="${index}"/></td>
        <td><textarea class="form-control itin-days" data-index="${index}"></textarea></td>
        <td><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></td>
      </tr>`);
  });

  document.getElementById('addCost')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#costWrap .cost-row').length;
    addHtml('costWrap', `<div class="row g-1 mb-2 cost-row">
      <input type="hidden" name="CostItems[${idx}].Id" value="0" />
      <div class="col-md-3"><input name="CostItems[${idx}].Title" class="form-control" placeholder="title" /></div>
      <div class="col-md-2"><input name="CostItems[${idx}].Type" class="form-control" value="Inclusion" /></div>
      <div class="col-md-2"><input name="CostItems[${idx}].SortOrder" class="form-control" value="${idx}" /></div>
      <div class="col-md-2"><select name="CostItems[${idx}].IsActive" class="form-select"><option value="true" selected>Active</option><option value="false">Inactive</option></select></div>
      <div class="col-md-3"><input name="CostItems[${idx}].ShortDescription" class="form-control" placeholder="description" /></div>
      <div class="col-12 text-end"><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div>
    </div>`);
  });

  document.getElementById('addDeparture')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#departureWrap .departure-row').length;
    addHtml('departureWrap', `<div class="row g-1 mb-2 departure-row">
      <input type="hidden" name="FixedDepartures[${idx}].Id" value="0" />
      <div class="col-md-2"><input type="date" name="FixedDepartures[${idx}].StartDate" class="form-control" /></div>
      <div class="col-md-2"><input type="date" name="FixedDepartures[${idx}].EndDate" class="form-control" /></div>
      <div class="col-md-2"><input name="FixedDepartures[${idx}].ForDays" class="form-control" placeholder="days" /></div>
      <div class="col-md-3"><input name="FixedDepartures[${idx}].Status" class="form-control" value="BookingOpen" /></div>
      <div class="col-md-2"><input name="FixedDepartures[${idx}].GroupSize" class="form-control" placeholder="group" /></div>
      <div class="col-md-1"><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div>
    </div>`);
  });

  document.getElementById('addGear')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#gearWrap .gear-row').length;
    addHtml('gearWrap', `<div class='row mb-1 gear-row'><input type='hidden' name='GearLists[${idx}].Id' value='0' /><input type='hidden' name='GearLists[${idx}].ExistingPath' value='' /><input type='hidden' name='GearLists[${idx}].ExistingImagePath' value='' /><div class='col'><input name='GearLists[${idx}].ShortDescription' class='form-control' placeholder='description'/></div><div class='col'><input type='file' name='GearLists[${idx}].UploadFile' class='form-control'/></div><div class='col'><input type='file' name='GearLists[${idx}].UploadImage' class='form-control'/></div><div class='col-12 text-end'><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });

  document.getElementById('addMap')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#mapWrap .map-row').length;
    addHtml('mapWrap', `<div class='row mb-1 map-row'><input type='hidden' name='Maps[${idx}].Id' value='0' /><input type='hidden' name='Maps[${idx}].ExistingPath' value='' /><div class='col'><input name='Maps[${idx}].Title' class='form-control' placeholder='title'/></div><div class='col'><input name='Maps[${idx}].Notes' class='form-control' placeholder='notes'/></div><div class='col'><input type='file' name='Maps[${idx}].UploadFile' class='form-control'/></div><div class='col-12 text-end'><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });

  document.getElementById('addMedia')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#mediaWrap .media-row').length;
    addHtml('mediaWrap', `<div class='row mb-1 media-row'><input type='hidden' name='Media[${idx}].Id' value='0' /><input type='hidden' name='Media[${idx}].ExistingPath' value='' /><div class='col'><input type='file' name='Media[${idx}].PhotoFile' class='form-control'/></div><div class='col'><input name='Media[${idx}].VideoUrl' class='form-control' placeholder='video url'/></div><div class='col'><input name='Media[${idx}].Caption' class='form-control' placeholder='caption'/></div><div class='col'><input name='Media[${idx}].SortOrder' class='form-control' placeholder='sort'/></div><div class='col-12 text-end'><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });

  document.getElementById('addHighlight')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#highlightWrap .highlight-row').length;
    addHtml('highlightWrap', `<div class="row g-1 mb-2 highlight-row"><input type="hidden" name="Highlights[${idx}].Id" value="0" /><div class="col-md-10"><input name="Highlights[${idx}].Text" class="form-control" placeholder="highlight" /></div><div class="col-md-1"><input name="Highlights[${idx}].SortOrder" class="form-control" value="${idx}" /></div><div class="col-md-1"><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });


  document.getElementById('addFaq')?.addEventListener('click', () => {
    addHtml('faqWrap', `<div class="row g-1 mb-2 faq-row"><div class="col-md-4"><input class="form-control faq-question" placeholder="question" /></div><div class="col-md-6"><input class="form-control faq-answer" placeholder="answer" /></div><div class="col-md-1"><input class="form-control faq-order" placeholder="order" /></div><div class="col-md-1"><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });

  document.getElementById('addReview')?.addEventListener('click', () => {
    const idx = document.querySelectorAll('#reviewWrap .review-row').length;
    addHtml('reviewWrap', `<div class="row g-1 mb-2 review-row"><input type="hidden" name="Reviews[${idx}].Id" value="0" /><input type="hidden" name="Reviews[${idx}].ExistingPhotoPath" value="" /><div class="col-md-2"><input name="Reviews[${idx}].FullName" class="form-control" placeholder="name" /></div><div class="col-md-2"><input name="Reviews[${idx}].EmailAddress" class="form-control" placeholder="email" /></div><div class="col-md-1"><input name="Reviews[${idx}].Rating" value="5" class="form-control" /></div><div class="col-md-2"><input name="Reviews[${idx}].ModerationStatus" value="Pending" class="form-control" /></div><div class="col-md-4"><input name="Reviews[${idx}].ReviewText" class="form-control" placeholder="review" /></div><div class="col-md-1"><button type="button" class="btn btn-sm btn-outline-danger remove-row">Remove</button></div></div>`);
  });

  form.addEventListener('click', (e) => {
    const target = e.target;
    if (!(target instanceof HTMLElement) || !target.classList.contains('remove-row')) return;

    const row = target.closest('.itinerary-row, .cost-row, .departure-row, .gear-row, .map-row, .media-row, .highlight-row, .faq-row, .review-row');
    row?.remove();

    reindexCollection('#itineraryTable tbody', '.itinerary-row', 'Itineraries');
    reindexCollection('#costWrap', '.cost-row', 'CostItems');
    reindexCollection('#departureWrap', '.departure-row', 'FixedDepartures');
    reindexCollection('#gearWrap', '.gear-row', 'GearLists');
    reindexCollection('#mapWrap', '.map-row', 'Maps');
    reindexCollection('#mediaWrap', '.media-row', 'Media');
    reindexCollection('#highlightWrap', '.highlight-row', 'Highlights');
    reindexCollection('#reviewWrap', '.review-row', 'Reviews');
  });

  form.addEventListener('submit', () => {
    form.querySelectorAll('.generated-itinerary-day').forEach((el) => el.remove());

    const faqLines = [];
    document.querySelectorAll('#faqWrap .faq-row').forEach((row, idx) => {
      const q = row.querySelector('.faq-question')?.value?.trim() || '';
      const a = row.querySelector('.faq-answer')?.value?.trim() || '';
      const o = row.querySelector('.faq-order')?.value?.trim() || `${idx}`;
      if (q && a) faqLines.push(`${q}|${a}|${o}`);
    });

    const faqText = document.getElementById('FaqsText');
    if (faqText) faqText.value = faqLines.join('\n');

    document.querySelectorAll('.itin-days').forEach((textarea) => {
      const i = textarea.dataset.index;
      if (!i) return;

      (textarea.value || '')
        .split('\n')
        .map((line) => line.trim())
        .filter(Boolean)
        .forEach((line, dIdx) => {
          const parts = line.split(':');
          const hasId = parts.length >= 5;
          const existingId = hasId ? parts[0] : '0';
          const day = hasId ? parts[1] : parts[0];
          const short = hasId ? parts[2] : parts[1];
          const meals = hasId ? parts[3] : parts[2];
          const acc = hasId ? parts[4] : parts[3];

          [
            [`Itineraries[${i}].Days[${dIdx}].Id`, existingId || '0'],
            [`Itineraries[${i}].Days[${dIdx}].DayNumber`, day || `${dIdx + 1}`],
            [`Itineraries[${i}].Days[${dIdx}].ShortDescription`, short || ''],
            [`Itineraries[${i}].Days[${dIdx}].Meals`, meals || ''],
            [`Itineraries[${i}].Days[${dIdx}].AccommodationType`, acc || '']
          ].forEach(([name, value]) => {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.className = 'generated-itinerary-day';
            input.name = name;
            input.value = value;
            form.appendChild(input);
          });
        });
    });
  });
})();
