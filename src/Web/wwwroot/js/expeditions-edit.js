(function(){
  const form=document.querySelector('form'); if(!form) return;
  let itIdx=document.querySelectorAll('#itineraryTable tbody tr').length;
  const add=(id,html)=>document.getElementById(id).insertAdjacentHTML('beforeend',html);
  document.getElementById('addItinerary')?.addEventListener('click',()=>{
    add('itineraryTable',`<tbody><tr><td><input name="Itineraries[${itIdx}].SeasonTitle" class="form-control"/></td><td><input name="Itineraries[${itIdx}].SortOrder" class="form-control" value="${itIdx}"/></td><td><textarea class="form-control itin-days" data-index="${itIdx}"></textarea></td></tr></tbody>`);itIdx++;});
  let g=0,m=0,me=0;
  document.getElementById('addGear')?.addEventListener('click',()=>add('gearWrap',`<div class='row mb-1'><div class='col'><input name='GearLists[${g}].ShortDescription' class='form-control' placeholder='description'/></div><div class='col'><input type='file' name='GearLists[${g}].UploadFile' class='form-control'/></div></div>`),g++);
  document.getElementById('addMap')?.addEventListener('click',()=>add('mapWrap',`<div class='row mb-1'><div class='col'><input name='Maps[${m}].Title' class='form-control' placeholder='title'/></div><div class='col'><input type='file' name='Maps[${m}].UploadFile' class='form-control'/></div></div>`),m++);
  document.getElementById('addMedia')?.addEventListener('click',()=>add('mediaWrap',`<div class='row mb-1'><div class='col'><input type='file' name='Media[${me}].PhotoFile' class='form-control'/></div><div class='col'><input name='Media[${me}].VideoUrl' class='form-control' placeholder='video url'/></div><div class='col'><input name='Media[${me}].Caption' class='form-control' placeholder='caption'/></div></div>`),me++);

  form.addEventListener('submit',()=>{
    document.querySelectorAll('.itin-days').forEach(t=>{
      const i=t.dataset.index;
      (t.value||'').split('\n').filter(Boolean).forEach((line,dIdx)=>{
        const [day,short,meals,acc]=line.split(':');
        form.insertAdjacentHTML('beforeend',`<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].DayNumber' value='${day||dIdx+1}'/>`+
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].ShortDescription' value='${short||''}'/>`+
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].Meals' value='${meals||''}'/>`+
          `<input type='hidden' name='Itineraries[${i}].Days[${dIdx}].AccommodationType' value='${acc||''}'/>`);
      });
    });

    const c=(document.getElementById('costText')?.value||'').split('\n').filter(Boolean);
    c.forEach((line,i)=>{const [t,tp,s]=line.split('|');form.insertAdjacentHTML('beforeend',`<input type='hidden' name='CostItems[${i}].Title' value='${t||''}'/><input type='hidden' name='CostItems[${i}].Type' value='${tp||'Inclusion'}'/><input type='hidden' name='CostItems[${i}].SortOrder' value='${s||0}'/><input type='hidden' name='CostItems[${i}].IsActive' value='true'/>`);});
    const d=(document.getElementById('departureText')?.value||'').split('\n').filter(Boolean);
    d.forEach((line,i)=>{const [st,en,fd,ss,gs]=line.split('|');form.insertAdjacentHTML('beforeend',`<input type='hidden' name='FixedDepartures[${i}].StartDate' value='${st}'/><input type='hidden' name='FixedDepartures[${i}].EndDate' value='${en}'/><input type='hidden' name='FixedDepartures[${i}].ForDays' value='${fd||0}'/><input type='hidden' name='FixedDepartures[${i}].Status' value='${ss||'BookingOpen'}'/><input type='hidden' name='FixedDepartures[${i}].GroupSize' value='${gs||''}'/>`);});
    const h=(document.getElementById('highlightText')?.value||'').split('\n').filter(Boolean);
    h.forEach((line,i)=>{const [t,s]=line.split('|');form.insertAdjacentHTML('beforeend',`<input type='hidden' name='Highlights[${i}].Text' value='${t||''}'/><input type='hidden' name='Highlights[${i}].SortOrder' value='${s||0}'/>`);});
    const r=(document.getElementById('reviewText')?.value||'').split('\n').filter(Boolean);
    r.forEach((line,i)=>{const [n,e,ra,st,tx]=line.split('|');form.insertAdjacentHTML('beforeend',`<input type='hidden' name='Reviews[${i}].FullName' value='${n||''}'/><input type='hidden' name='Reviews[${i}].EmailAddress' value='${e||''}'/><input type='hidden' name='Reviews[${i}].Rating' value='${ra||5}'/><input type='hidden' name='Reviews[${i}].ModerationStatus' value='${st||'Pending'}'/><input type='hidden' name='Reviews[${i}].ReviewText' value='${tx||''}'/>`);});
  });
})();
