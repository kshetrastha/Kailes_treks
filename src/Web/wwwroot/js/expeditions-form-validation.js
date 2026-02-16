(() => {
  'use strict';

  const forms = document.querySelectorAll('.needs-validation');
  forms.forEach((form) => {
    form.addEventListener('submit', (event) => {
      if (form.checkValidity()) {
        return;
      }

      event.preventDefault();
      event.stopPropagation();
      form.classList.add('was-validated');

      const firstInvalidField = form.querySelector(':invalid');
      if (firstInvalidField instanceof HTMLElement) {
        const tabPane = firstInvalidField.closest('.tab-pane');
        if (tabPane?.id) {
          const tabTrigger = document.querySelector(`[data-bs-toggle="tab"][href="#${tabPane.id}"]`);
          if (tabTrigger) {
            bootstrap.Tab.getOrCreateInstance(tabTrigger).show();
          }
        }

        firstInvalidField.focus();
      }
    }, false);
  });
})();
