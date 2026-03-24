$(function () {
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
        if ($('#' + MODAL_STYLE_ID).length) return;

        const style = `
            <style id="${MODAL_STYLE_ID}">
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

                //#sharedImageCropperModal .shared-image-cropper-stage img {
                //    display: block;
                //    max-width: none;
                //    max-height: none;
                //}            
            </style>
        `;

        $('head').append(style);
    }

    function ensureModal() {
        if (state.modal) return;

  

        const modalHtml = `
<div class="modal fade" id="sharedImageCropperModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog">
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

        $('body').append(modalHtml);
        ensureStyles();
        const modalElement = document.getElementById('sharedImageCropperModal');
        if (!modalElement || !window.bootstrap) return;

        state.modal = new bootstrap.Modal(modalElement);
        state.image = document.getElementById('sharedImageCropperPreview');

        $('#sharedImageCropperModal').on('hidden.bs.modal', function () {
            cleanupCropper();
        });

        $('#sharedImageCropperApply').on('click', function () {
            applyCrop();
        });
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
            $(state.image).removeAttr('src');
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

    function getFittedCropBox(containerData, aspectRatio) {
        if (!containerData.width || !containerData.height) return null;

        let width = containerData.width;
        let height = width / aspectRatio;

        if (height > containerData.height) {
            height = containerData.height;
            width = height * aspectRatio;
        }

        return {
            width: width,
            height: height,
            left: (containerData.width - width) / 2,
            top: (containerData.height - height) / 2
        };
    }

    function fillImageViewport(cropper, aspectRatio) {
        const containerData = cropper.getContainerData();
        const imageData = cropper.getImageData();

        if (!containerData.width || !containerData.height || !imageData.naturalWidth || !imageData.naturalHeight) {
            return;
        }

        const cropBox = getFittedCropBox(containerData, aspectRatio);
        if (!cropBox) return;

        cropper.setCropBoxData(cropBox);

        const zoomRatio = Math.max(
            cropBox.width / imageData.naturalWidth,
            cropBox.height / imageData.naturalHeight
        );

        cropper.zoomTo(zoomRatio);

        const updatedImageData = cropper.getImageData();
        cropper.setCanvasData({
            left: cropBox.left + ((cropBox.width - updatedImageData.width) / 2),
            top: cropBox.top + ((cropBox.height - updatedImageData.height) / 2)
        });

        cropper.setCropBoxData(cropBox);
    }

    function onFileSelect(input) {
        if (!input || !input.files || input.files.length === 0) return;

        if ($(input).data('skipCropOnce') === true || $(input).attr('data-skip-crop-once') === 'true') {
            $(input).data('skipCropOnce', false);
            $(input).attr('data-skip-crop-once', 'false');
            return;
        }

        const file = input.files[0];
        if (!file.type.startsWith('image/')) return;

        ensureModal();
        if (!state.modal || !state.image || typeof window.Cropper === 'undefined') return;

        state.activeInput = input;
        state.imageUrl = URL.createObjectURL(file);
        $(state.image).attr('src', state.imageUrl);

        const aspectRatio = parseFloatOrNull($(input).data('aspect-ratio')) ?? DEFAULT_ASPECT_RATIO;

        state.image.onload = function () {
            if (!state.image) return;

            state.cropper = new Cropper(state.image, {
                aspectRatio: aspectRatio,
                viewMode: 2,
                autoCropArea: 1,
                responsive: true,
                background: false,
                dragMode: 'move',
                ready: function () {
                    setTimeout(function () {
                        if (state.cropper) {
                            fillImageViewport(state.cropper, aspectRatio);
                        }
                    }, 0);
                }
            });
        };

        state.modal.show();
    }

    function applyCrop() {
        if (!state.cropper || !state.activeInput) return;

        const $input = $(state.activeInput);
        const cropWidth = parseIntOrNull($input.data('crop-width'));
        const cropHeight = parseIntOrNull($input.data('crop-height'));

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

        const ext = originalName.includes('.')
            ? originalName.substring(originalName.lastIndexOf('.'))
            : '.jpg';

        canvas.toBlob(function (blob) {
            if (!blob || !state.activeInput) return;

            const croppedFile = new File(
                [blob],
                originalName.replace(/\.[^/.]+$/, '') + '-cropped' + ext,
                { type: blob.type || 'image/jpeg' }
            );

            const dataTransfer = new DataTransfer();
            dataTransfer.items.add(croppedFile);
            state.activeInput.files = dataTransfer.files;

            $(state.activeInput).data('skipCropOnce', true);
            $(state.activeInput).attr('data-skip-crop-once', 'true');
            $(state.activeInput).trigger('change');

            if (state.modal) {
                state.modal.hide();
            }
        }, 'image/jpeg', 0.92);
    }

    $(document).on('change', '.' + CROP_CLASS, function () {
        onFileSelect(this);
    });
});