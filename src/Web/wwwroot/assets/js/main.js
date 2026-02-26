

// Js fo the search modal

// Modal elements
const openModalBtn = document.getElementById("openModalBtn");
const modalOverlay = document.getElementById("modalOverlay");
const modalBox = document.getElementById("modalBox");
const closeModalIcon = document.getElementById("closeModalIcon");

// Open Modal
openModalBtn.addEventListener("click", () => {
    modalOverlay.classList.remove("opacity-0", "invisible");
    modalBox.classList.remove("opacity-0", "scale-95", "invisible");
    closeModalIcon.classList.remove("opacity-0", "invisible");

    requestAnimationFrame(() => {
        modalOverlay.classList.add("opacity-100");
        modalBox.classList.add("opacity-100", "scale-100");
        closeModalIcon.classList.add("opacity-100");
    });
});

// Close Modal
function closeModal() {
    modalOverlay.classList.remove("opacity-100");
    modalBox.classList.remove("opacity-100", "scale-100");
    closeModalIcon.classList.remove("opacity-100");

    modalOverlay.classList.add("opacity-0");
    modalBox.classList.add("opacity-0", "scale-95");
    closeModalIcon.classList.add("opacity-0");

    setTimeout(() => {
        modalOverlay.classList.add("invisible");
        modalBox.classList.add("invisible");
        closeModalIcon.classList.add("invisible");
    }, 300);
}

modalOverlay.addEventListener("click", closeModal);
closeModalIcon.addEventListener("click", closeModal);


// Header section
const header = document.getElementById("siteHeader");
const logo = document.getElementById("headerLogo");
const modalSearchIcon = document.getElementById("openModalBtn");
const navLinks = document.querySelectorAll(".navLinks");

const defaultLogo = "./assets/img/white.png";
const whiteLogo = "./assets/img/logo.png";
const darkLogo = "./assets/img/dark.svg";

let scrolledState = false;

window.addEventListener("scroll", () => {
    const scrollY = window.scrollY;

    if (scrollY > 20 && !scrolledState) {
        scrolledState = true;
        header.classList.add("scrolled");
        header.classList.remove("bg-transparent");
        header.classList.add("bg-white", "shadow-md");

        // Fix: Loop through each nav link
        navLinks.forEach(link => {
            link.classList.remove("text-neutral-50");
            link.classList.add("text-neutral-800");
        });

        modalSearchIcon.classList.remove("text-neutral-50");
        modalSearchIcon.classList.add("text-neutral-800");
        logo.src = darkLogo;
    }
    else if (scrollY <= 20 && scrolledState) {
        scrolledState = false;
        header.classList.remove("scrolled");
        header.classList.remove("bg-white", "shadow-md");
        header.classList.add("bg-transparent");

        // Fix: Loop again on reset
        navLinks.forEach(link => {
            link.classList.remove("text-neutral-800");
            link.classList.add("text-neutral-50");
        });

        modalSearchIcon.classList.remove("text-neutral-800");
        modalSearchIcon.classList.add("text-neutral-50");
        logo.src = defaultLogo;
    }
});

// Hover logo change only when NOT scrolled
header.addEventListener("mouseenter", () => {
    if (!scrolledState) logo.src = whiteLogo;
});
header.addEventListener("mouseleave", () => {
    if (!scrolledState) logo.src = defaultLogo;
});