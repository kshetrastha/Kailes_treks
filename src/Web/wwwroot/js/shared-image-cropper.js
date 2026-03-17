(function () {
    const CROP_CLASS = 'js-crop-image';
    const DEFAULT_ASPECT_RATIO = 478 / 825;
    const state = {
        activeInput: null,
        imageUrl: null,
        cropper: null,
        modal: null,
        image: null
    };

    function ensureModal() {
        if (state.modal) return;

        const modalHtml = `
<div class="modal fade" id="sharedImageCropperModal" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog modal-fullscreen">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Crop image</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="bg-light rounded overflow-hidden h-100" style="min-height:75vh;">
          <img id="sharedImageCropperPreview" alt="Crop preview" style="display:block; width:100%; max-width:100%;" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" id="sharedImageCropperApply" class="btn btn-primary">Apply crop</button>
      </div>
    </div>
  </div>
</div>`;

        document.body.insertAdjacentHTML('beforeend', modalHtml);

        const modalElement = document.getElementById('sharedImageCropperModal');
        if (!modalElement || !window.bootstrap) return;

        state.modal = new window.bootstrap.Modal(modalElement);
        state.image = document.getElementById('sharedImageCropperPreview');

        modalElement.addEventListener('hidden.bs.modal', cleanupCropper);
        document.getElementById('sharedImageCropperApply')?.addEventListener('click', applyCrop);
    }

    function cleanupCropper() {
        if (state.cropper) {
            state.cropper.destroy();
            state.cropper = null;
        }

        if (state.imageUrl) {
            URL.revokeObjectURL(state.imageUrl);
            state.imageUrl = null;
        }

        if (state.image) {
            state.image.removeAttribute('src');
        }

        state.activeInput = null;
    }

    function parseFloatOrNull(value) {
        if (!value) return null;
        const parsed = parseFloat(value);
        return Number.isFinite(parsed) ? parsed : null;
    }

    function onFileSelect(event) {
        const input = event.target;
        if (!(input instanceof HTMLInputElement) || !input.files || input.files.length === 0) return;
        if (input.dataset.skipCropOnce === 'true') {
            input.dataset.skipCropOnce = 'false';
            return;
        }

        const file = input.files[0];
        if (!file.type.startsWith('image/')) return;

        ensureModal();
        if (!state.modal || !state.image || typeof window.Cropper === 'undefined') return;

        state.activeInput = input;
        state.imageUrl = URL.createObjectURL(file);
        state.image.src = state.imageUrl;

        const aspectRatio = parseFloatOrNull(input.dataset.aspectRatio) ?? DEFAULT_ASPECT_RATIO;

        state.image.onload = function () {
            if (!state.image) return;
            state.cropper = new window.Cropper(state.image, {
                aspectRatio: aspectRatio,
                viewMode: 1,
                autoCropArea: 1,
                responsive: true,
                background: false
            });
        };

        state.modal.show();
    }

    function applyCrop() {
        if (!state.cropper || !state.activeInput) return;

        const outputWidth = parseInt(state.activeInput.dataset.cropWidth || '0', 10);
        const outputHeight = parseInt(state.activeInput.dataset.cropHeight || '0', 10);

        const canvas = state.cropper.getCroppedCanvas({
            width: outputWidth > 0 ? outputWidth : undefined,
            height: outputHeight > 0 ? outputHeight : undefined,
            imageSmoothingQuality: 'high'
        });

        if (!canvas) return;

        const originalName = state.activeInput.files && state.activeInput.files[0]
            ? state.activeInput.files[0].name
            : 'cropped-image.jpg';
        const ext = originalName.includes('.') ? originalName.substring(originalName.lastIndexOf('.')) : '.jpg';

        canvas.toBlob(function (blob) {
            if (!blob || !state.activeInput) return;

            const croppedFile = new File([blob], originalName.replace(/\.[^/.]+$/, '') + '-cropped' + ext, {
                type: blob.type || 'image/jpeg'
            });

            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(croppedFile);
            state.activeInput.files = dataTransfer.files;
            state.activeInput.dataset.skipCropOnce = 'true';
            state.activeInput.dispatchEvent(new Event('change', { bubbles: true }));

            if (state.modal) {
                state.modal.hide();
            }
        }, 'image/jpeg', 0.92);
    }

    document.addEventListener('change', function (event) {
        const target = event.target;
        if (!(target instanceof HTMLInputElement)) return;
        if (!target.classList.contains(CROP_CLASS)) return;
        onFileSelect(event);
    });
})();
