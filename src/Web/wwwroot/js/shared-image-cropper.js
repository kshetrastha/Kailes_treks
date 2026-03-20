(function () {
    const CROP_CLASS = 'js-crop-image';
    const DEFAULT_ASPECT_RATIO = 478 / 825;
    const MODAL_STYLE_ID = 'sharedImageCropperStyles';
    const state = {
        activeInput: null,
        imageUrl: null,
        cropper: null,
        modal: null,
        image: null
    };

    function ensureStyles() {
        if (document.getElementById(MODAL_STYLE_ID)) return;

        const style = document.createElement('style');
        style.id = MODAL_STYLE_ID;
        style.textContent = `
            #sharedImageCropperModal .modal-content {
                min-height: 100vh;
            }

            #sharedImageCropperModal .modal-body {
                display: flex;
                min-height: 0;
            }

            #sharedImageCropperModal .shared-image-cropper-stage {
                display: flex;
                align-items: center;
                justify-content: center;
                width: 100%;
                min-height: 75vh;
                height: 100%;
                overflow: hidden;
                background: #eef1f7;
            }

            #sharedImageCropperModal .shared-image-cropper-stage img {
                display: block;
                max-width: none;
                max-height: none;
            }

            #sharedImageCropperModal .cropper-container {
                width: 100% !important;
                height: 100% !important;
            }
        `;

        document.head.appendChild(style);
    }

    function ensureModal() {
        if (state.modal) return;

        ensureStyles();

        const modalHtml = `
<div class="modal fade" id="sharedImageCropperModal" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog modal-fullscreen">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Crop image</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="shared-image-cropper-stage rounded">
          <img id="sharedImageCropperPreview" alt="Crop preview" />
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

    function parseIntOrNull(value) {
        if (!value) return null;
        const parsed = parseInt(value, 10);
        return Number.isInteger(parsed) && parsed > 0 ? parsed : null;
    }

    function fillImageViewport(cropper) {
        const containerData = cropper.getContainerData();
        const imageData = cropper.getImageData();
        if (!containerData.width || !containerData.height || !imageData.naturalWidth || !imageData.naturalHeight) {
            return;
        }

        const zoomRatio = Math.max(
            containerData.width / imageData.naturalWidth,
            containerData.height / imageData.naturalHeight
        );

        cropper.zoomTo(zoomRatio);

        const updatedImageData = cropper.getImageData();
        cropper.setCanvasData({
            left: (containerData.width - updatedImageData.width) / 2,
            top: (containerData.height - updatedImageData.height) / 2
        });

        const cropBoxData = cropper.getCropBoxData();
        cropper.setCropBoxData({
            left: (containerData.width - cropBoxData.width) / 2,
            top: (containerData.height - cropBoxData.height) / 2
        });
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
                autoCropArea: 0.8,
                responsive: true,
                background: false,
                dragMode: 'move',
                ready: function () {
                    window.setTimeout(function () {
                        if (state.cropper) {
                            fillImageViewport(state.cropper);
                        }
                    }, 0);
                }
            });
        };

        state.modal.show();
    }

    function applyCrop() {
        if (!state.cropper || !state.activeInput) return;

        const cropWidth = parseIntOrNull(state.activeInput.dataset.cropWidth);
        const cropHeight = parseIntOrNull(state.activeInput.dataset.cropHeight);
        const canvas = state.cropper.getCroppedCanvas({
            width: cropWidth ?? undefined,
            height: cropHeight ?? undefined,
            imageSmoothingEnabled: true,
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
