
// tabs for expeditions
document.addEventListener("DOMContentLoaded", () => {
    const buttons = document.querySelectorAll(".tab-btn");
    const contents = document.querySelectorAll(".tab-content");

    function showTab(index) {
        contents.forEach(c => c.classList.add("hidden"));
        const active = document.querySelector(`.tab-content[data-content="${index}"]`);
        if (active) active.classList.remove("hidden");
    }

    buttons.forEach(btn => {
        btn.addEventListener("click", () => {
            const index = btn.getAttribute("data-tab");

            // remove highlight from all buttons
            buttons.forEach(b => b.classList.remove("text-amber-500"));

            // highlight active button
            btn.classList.add("text-amber-500");

            showTab(index);
        });
    });

    // default selected tab
    showTab(0);
    if (buttons[0]) buttons[0].classList.add("text-amber-500");
});


// tabs for Treeking
document.addEventListener("DOMContentLoaded", () => {
    const buttonsTrekking = document.querySelectorAll(".tab-btn-trekking");
    const contentsTrekking = document.querySelectorAll(".tab-content-trekking");

    function showTab(index) {
        contentsTrekking.forEach(c => c.classList.add("hidden"));
        const active = document.querySelector(`.tab-content-trekking[data-content="${index}"]`);
        if (active) active.classList.remove("hidden");
    }

    buttonsTrekking.forEach(btn => {
        btn.addEventListener("click", () => {
            const index = btn.getAttribute("data-tab");

            // remove highlight from all buttonsTrekking
            buttonsTrekking.forEach(b => b.classList.remove("text-amber-500"));

            // highlight active button
            btn.classList.add("text-amber-500");

            showTab(index);
        });
    });

    // default selected tab
    showTab(0);
    if (buttonsTrekking[0]) buttonsTrekking[0].classList.add("text-amber-500");
});
