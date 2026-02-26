function initHtmlEditor(selector, invalidEle) {
    if (!window.tinymce || !selector) {
        return;
    }

    tinymce.remove(selector);
    tinymce.init({
        selector: selector,
        invalid_elements: invalidEle,
        extended_valid_elements: 'i[class]',
        valid_children: '+div[i]',
        forced_root_block: false,
        verify_html: false,
        valid_elements: '*[*]',
        height: 300,
        browser_spellcheck: true,
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        setup: function (editor) {
            editor.on('focus', function () {
                $(document).trigger('focusin');
            });
            editor.on('change', function () {
                editor.save();
            });
        }
    });
}

function initDescriptionEditors(invalidEle) {
    if (!window.tinymce) {
        return;
    }

    const textareas = document.querySelectorAll('textarea');

    textareas.forEach(function (textarea, index) {
        const idText = (textarea.id || '').toLowerCase();
        const nameText = (textarea.name || '').toLowerCase();
        const isDescriptionField = idText.indexOf('description') !== -1 || nameText.indexOf('description') !== -1;

        if (!isDescriptionField || textarea.dataset.htmlEditorInitialized === 'true') {
            return;
        }

        if (!textarea.id) {
            textarea.id = 'htmlEditorDescription' + index + '_' + Date.now();
        }

        initHtmlEditor('#' + textarea.id, invalidEle || '');
        textarea.dataset.htmlEditorInitialized = 'true';
    });
}

document.addEventListener('DOMContentLoaded', function () {
    if (!window.tinymce) {
        return;
    }

    initDescriptionEditors('');

    document.addEventListener('submit', function () {
        tinymce.triggerSave();
    }, true);

    const observer = new MutationObserver(function () {
        initDescriptionEditors('');
    });

    observer.observe(document.body, { childList: true, subtree: true });
});
