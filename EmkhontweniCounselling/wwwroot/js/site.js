// ========================================
// Smooth Scroll for Internal Links
// ========================================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    });
});

// ========================================
// Floating Circles / Hero Animation
// ========================================
const circles = document.querySelectorAll('.healing-circle');

circles.forEach((circle, index) => {
    let direction = index % 2 === 0 ? 1 : -1;
    let position = 0;

    function float() {
        position += direction * 0.2;
        if (position > 15 || position < -15) direction *= -1;
        circle.style.transform = `translateY(${position}px)`;
        requestAnimationFrame(float);
    }

    float();
});

// ========================================
// Fade-in on Scroll for Sections
// ========================================
const faders = document.querySelectorAll('section');

const appearOptions = {
    threshold: 0.2,
    rootMargin: "0px 0px -50px 0px"
};

const appearOnScroll = new IntersectionObserver((entries, observer) => {
    entries.forEach(entry => {
        if (!entry.isIntersecting) return;
        entry.target.classList.add('fade-in');
        observer.unobserve(entry.target);
    });
}, appearOptions);

faders.forEach(fader => {
    fader.classList.add('opacity-0'); // start invisible
    appearOnScroll.observe(fader);
});

// ========================================
// Button Hover Shadow Animation
// ========================================
const royalButtons = document.querySelectorAll('.royal-btn');

royalButtons.forEach(btn => {
    btn.addEventListener('mouseenter', () => {
        btn.style.transform = "translateY(-2px)";
        btn.style.boxShadow = "0 12px 25px rgba(0,0,0,0.15)";
    });

    btn.addEventListener('mouseleave', () => {
        btn.style.transform = "translateY(0)";
        btn.style.boxShadow = "0 6px 15px rgba(0,0,0,0.08)";
    });
});

// ========================================
// Optional: Service Card Hover Animations
// ========================================
const serviceCards = document.querySelectorAll('.service-card');

serviceCards.forEach(card => {
    card.addEventListener('mouseenter', () => {
        card.style.transform = "translateY(-5px)";
        card.style.boxShadow = "0 12px 25px rgba(0,0,0,0.1)";
    });
    card.addEventListener('mouseleave', () => {
        card.style.transform = "translateY(0)";
        card.style.boxShadow = "0 8px 20px rgba(0,0,0,0.05)";
    });
});
