(function(){
  const form = document.querySelector('form');
  if (!form) return;

  const itineraryBody = document.querySelector('#itineraryTable tbody');
  let itIdx = itineraryBody ? itineraryBody.querySelectorAll('tr').length : 0;
  let g = document.querySelectorAll('#gearWrap .gear-row').length;
  let m = document.querySelectorAll('#mapWrap .map-row').length;
  let me = document.querySelectorAll('#mediaWrap .media-row').length;

  const addRow = (containerId, html) => {
    document.getElementById(containerId)?.insertAdjacentHTML('beforeend', html);
  };

  document.getElementById('addItinerary')?.addEventListener('click', () => {
    itineraryBody?.insertAdjacentHTML(
      'beforeend',
      `<tr><td><input type="hidden" name="Itineraries[${itIdx}].Id" value="0" /><input name="Itineraries[${itIdx}].SeasonTitle" class="form-control"/></td><td><input name="Itineraries[${itIdx}].SortOrder" class="form-control" value="${itIdx}"/></td><td><textarea class="form-control itin-days" data-index="${itIdx}"></textarea></td></tr>`
    );
    itIdx++;
  });

  document.getElementById('addGear')?.addEventListener('click', () => {
    addRow('gearWrap', `<div class='row mb-1 gear-row'><input type='hidden' name='GearLists[${g}].Id' value='0' /><input type='hidden' name='GearLists[${g}].ExistingPath' value='' /><input type='hidden' name='GearLists[${g}].ExistingImagePath' value='' /><div class='col'><input name='GearLists[${g}].ShortDescription' class='form-control' placeholder='description'/></div><div class='col'><input type='file' name='GearLists[${g}].UploadFile' class='form-control'/></div><div class='col'><input type='file' name='GearLists[${g}].UploadImage' class='form-control'/></div></div>`);
    g++;
  });

  document.getElementById('addMap')?.addEventListener('click', () => {
    addRow('mapWrap', `<div class='row mb-1 map-row'><input type='hidden' name='Maps[${m}].Id' value='0' /><input type='hidden' name='Maps[${m}].ExistingPath' value='' /><div class='col'><input name='Maps[${m}].Title' class='form-control' placeholder='title'/></div><div class='col'><input name='Maps[${m}].Notes' class='form-control' placeholder='notes'/></div><div class='col'><input type='file' name='Maps[${m}].UploadFile' class='form-control'/></div></div>`);
    m++;
  });

  document.getElementById('addMedia')?.addEventListener('click', () => {
    addRow('mediaWrap', `<div class='row mb-1 media-row'><input type='hidden' name='Media[${me}].Id' value='0' /><input type='hidden' name='Media[${me}].ExistingPath' value='' /><div class='col'><input type='file' name='Media[${me}].PhotoFile' class='form-control'/></div><div class='col'><input name='Media[${me}].VideoUrl' class='form-control' placeholder='video url'/></div><div class='col'><input name='Media[${me}].Caption' class='form-control' placeholder='caption'/></div><div class='col'><input name='Media[${me}].SortOrder' class='form-control' placeholder='sort'/></div></div>`);
    me++;
  });

  form.addEventListener('submit', () => {
    document.querySelectorAll('.itin-days').forEach(t => {
      const i = t.dataset.index;
      (t.value || '').split('\n').map(line => line.trim()).filter(Boolean).forEach((line, dIdx) => {
        const parts = line.split(':');
        const hasId = parts.length >= 5;
        const existingId = hasId ? parts[0] : '0';
        const day = hasId ? parts[1] : parts[0];
        const short = hasId ? parts[2] : parts[1];
        const meals = hasId ? parts[3] : parts[2];
        const acc = hasId ? parts[4] : parts[3];
        form.insertAdjacentHTML('beforeend',
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].Id' value='${existingId || 0}'/><input type='hidden' name='Itineraries[${i}].Days[${dIdx}].DayNumber' value='${day || dIdx + 1}'/>` +
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].ShortDescription' value='${short || ''}'/>` +
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].Meals' value='${meals || ''}'/>` +
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].AccommodationType' value='${acc || ''}'/>`
        );
      });
    });

    const c = (document.getElementById('costText')?.value || '').split('\n').map(x => x.trim()).filter(Boolean);
    c.forEach((line, i) => {
      const [id, t, tp, s] = line.split('|');
      form.insertAdjacentHTML('beforeend', `<input type='hidden' name='CostItems[${i}].Id' value='${id || 0}'/><input type='hidden' name='CostItems[${i}].Title' value='${t || ''}'/><input type='hidden' name='CostItems[${i}].Type' value='${tp || 'Inclusion'}'/><input type='hidden' name='CostItems[${i}].SortOrder' value='${s || 0}'/><input type='hidden' name='CostItems[${i}].IsActive' value='true'/>`);
    });

    const d = (document.getElementById('departureText')?.value || '').split('\n').map(x => x.trim()).filter(Boolean);
    d.forEach((line, i) => {
      const [id, st, en, fd, ss, gs] = line.split('|');
      form.insertAdjacentHTML('beforeend', `<input type='hidden' name='FixedDepartures[${i}].Id' value='${id || 0}'/><input type='hidden' name='FixedDepartures[${i}].StartDate' value='${st}'/><input type='hidden' name='FixedDepartures[${i}].EndDate' value='${en}'/><input type='hidden' name='FixedDepartures[${i}].ForDays' value='${fd || 0}'/><input type='hidden' name='FixedDepartures[${i}].Status' value='${ss || 'BookingOpen'}'/><input type='hidden' name='FixedDepartures[${i}].GroupSize' value='${gs || ''}'/>`);
    });

    const h = (document.getElementById('highlightText')?.value || '').split('\n').map(x => x.trim()).filter(Boolean);
    h.forEach((line, i) => {
      const [id, t, s] = line.split('|');
      form.insertAdjacentHTML('beforeend', `<input type='hidden' name='Highlights[${i}].Id' value='${id || 0}'/><input type='hidden' name='Highlights[${i}].Text' value='${t || ''}'/><input type='hidden' name='Highlights[${i}].SortOrder' value='${s || 0}'/>`);
    });

    const r = (document.getElementById('reviewText')?.value || '').split('\n').map(x => x.trim()).filter(Boolean);
    r.forEach((line, i) => {
      const [id, n, e, ra, st, tx] = line.split('|');
      form.insertAdjacentHTML('beforeend', `<input type='hidden' name='Reviews[${i}].Id' value='${id || 0}'/><input type='hidden' name='Reviews[${i}].FullName' value='${n || ''}'/><input type='hidden' name='Reviews[${i}].EmailAddress' value='${e || ''}'/><input type='hidden' name='Reviews[${i}].Rating' value='${ra || 5}'/><input type='hidden' name='Reviews[${i}].ModerationStatus' value='${st || 'Pending'}'/><input type='hidden' name='Reviews[${i}].ReviewText' value='${tx || ''}'/>`);
    });
  });
})();
