// Modern Portfolio Enhancements
// 1. Intersection Observer for animations
const observerOptions = {
  threshold: 0.1,
  rootMargin: "0px 0px -50px 0px",
};

const observer = new IntersectionObserver((entries) => {
  entries.forEach((entry) => {
    if (entry.isIntersecting) {
      entry.target.classList.add("animate-in");
    }
  });
}, observerOptions);

// 2. Smooth cursor following
class ModernCursor {
  constructor() {
    this.cursor = document.createElement("div");
    this.cursor.className = "modern-cursor";
    document.body.appendChild(this.cursor);

    this.init();
  }

  init() {
    document.addEventListener("mousemove", (e) => {
      this.cursor.style.left = e.clientX + "px";
      this.cursor.style.top = e.clientY + "px";
    });

    // Interactive elements
    document.querySelectorAll("a, button, .interactive").forEach((el) => {
      el.addEventListener("mouseenter", () =>
        this.cursor.classList.add("cursor-hover")
      );
      el.addEventListener("mouseleave", () =>
        this.cursor.classList.remove("cursor-hover")
      );
    });
  }
}

// 3. Magnetic buttons effect
function initMagneticElements() {
  document.querySelectorAll(".magnetic").forEach((element) => {
    element.addEventListener("mousemove", (e) => {
      const rect = element.getBoundingClientRect();
      const x = e.clientX - rect.left - rect.width / 2;
      const y = e.clientY - rect.top - rect.height / 2;

      element.style.transform = `translate(${x * 0.1}px, ${y * 0.1}px)`;
    });

    element.addEventListener("mouseleave", () => {
      element.style.transform = "translate(0px, 0px)";
    });
  });
}

// 4. Typing animation
class TypeWriter {
  constructor(element, texts, speed = 100) {
    this.element = element;
    this.texts = texts;
    this.speed = speed;
    this.textIndex = 0;
    this.charIndex = 0;
    this.isDeleting = false;

    this.type();
  }

  type() {
    const currentText = this.texts[this.textIndex];

    if (this.isDeleting) {
      this.element.textContent = currentText.substring(0, this.charIndex - 1);
      this.charIndex--;
    } else {
      this.element.textContent = currentText.substring(0, this.charIndex + 1);
      this.charIndex++;
    }

    let typeSpeed = this.speed;

    if (this.isDeleting) {
      typeSpeed /= 2;
    }

    if (!this.isDeleting && this.charIndex === currentText.length) {
      typeSpeed = 2000;
      this.isDeleting = true;
    } else if (this.isDeleting && this.charIndex === 0) {
      this.isDeleting = false;
      this.textIndex = (this.textIndex + 1) % this.texts.length;
      typeSpeed = 500;
    }

    setTimeout(() => this.type(), typeSpeed);
  }
}

// 5. Parallax scrolling
function initParallax() {
  window.addEventListener("scroll", () => {
    const scrolled = window.pageYOffset;
    const parallaxElements = document.querySelectorAll(".parallax");

    parallaxElements.forEach((element) => {
      const speed = element.dataset.speed || 0.5;
      const yPos = -(scrolled * speed);
      element.style.transform = `translateY(${yPos}px)`;
    });
  });
}

// 6. Project cards 3D tilt effect
function init3DTilt() {
  document.querySelectorAll(".tilt-card").forEach((card) => {
    card.addEventListener("mousemove", (e) => {
      const rect = card.getBoundingClientRect();
      const x = e.clientX - rect.left;
      const y = e.clientY - rect.top;

      const centerX = rect.width / 2;
      const centerY = rect.height / 2;

      const rotateX = (y - centerY) / 4;
      const rotateY = (centerX - x) / 4;

      card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale3d(1.05, 1.05, 1.05)`;
    });

    card.addEventListener("mouseleave", () => {
      card.style.transform =
        "perspective(1000px) rotateX(0deg) rotateY(0deg) scale3d(1, 1, 1)";
    });
  });
}

// 7. Advanced scroll animations with GSAP
function initGSAPAnimations() {
  if (typeof gsap !== "undefined") {
    // Hero section animations
    gsap
      .timeline()
      .from(".hero-title", {
        duration: 1,
        y: 100,
        opacity: 0,
        ease: "power2.out",
      })
      .from(
        ".hero-subtitle",
        { duration: 1, y: 50, opacity: 0, ease: "power2.out" },
        "-=0.5"
      )
      .from(
        ".hero-buttons",
        { duration: 1, y: 30, opacity: 0, ease: "power2.out" },
        "-=0.5"
      );

    // Skills bar animations
    gsap.registerPlugin(ScrollTrigger);

    gsap.utils.toArray(".skill-bar").forEach((bar) => {
      const percentage = bar.dataset.percentage;
      gsap.to(bar, {
        width: percentage + "%",
        duration: 2,
        ease: "power2.out",
        scrollTrigger: {
          trigger: bar,
          start: "top 80%",
        },
      });
    });

    // Project cards stagger animation
    gsap.to(".project-card", {
      y: 0,
      opacity: 1,
      duration: 0.8,
      stagger: 0.2,
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".projects-grid",
        start: "top 80%",
      },
    });
  }
}

// 8. Modern loading screen
function createLoadingScreen() {
  const loader = document.createElement("div");
  loader.className = "modern-loader";
  loader.innerHTML = `
        <div class="loader-content">
            <div class="loader-logo">
                <svg viewBox="0 0 100 100" width="60" height="60">
                    <circle cx="50" cy="50" r="45" stroke="currentColor" stroke-width="8" fill="none" 
                            stroke-dasharray="283" stroke-dashoffset="283" class="loader-circle"/>
                </svg>
            </div>
            <div class="loader-text">
                <span class="loading-text">Loading Portfolio</span>
                <div class="loading-dots">
                    <span>.</span><span>.</span><span>.</span>
                </div>
            </div>
        </div>
    `;
  document.body.appendChild(loader);

  // Remove loader when page is fully loaded
  window.addEventListener("load", () => {
    setTimeout(() => {
      loader.classList.add("fade-out");
      setTimeout(() => loader.remove(), 500);
    }, 1000);
  });
}

// Initialize all enhancements
document.addEventListener("DOMContentLoaded", () => {
  // Only run on main portfolio pages, not admin
  if (
    !window.location.pathname.includes("/Admin") &&
    !window.location.pathname.includes("/Identity")
  ) {
    createLoadingScreen();
    new ModernCursor();
    initMagneticElements();
    initParallax();
    init3DTilt();
    initGSAPAnimations();

    // Initialize typewriter for hero
    const typeElement = document.querySelector(".typewriter");
    if (typeElement) {
      new TypeWriter(typeElement, [
        "Full-Stack Developer",
        "AI Enthusiast",
        "Problem Solver",
        "Innovation Driver",
      ]);
    }

    // Observe elements for scroll animations
    document.querySelectorAll(".animate-on-scroll").forEach((el) => {
      observer.observe(el);
    });
  }
});
