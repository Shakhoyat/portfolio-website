// Micro-interactions and Advanced Features
document.addEventListener("DOMContentLoaded", () => {
  // Add particle system to hero section
  function createParticleSystem() {
    const hero = document.querySelector(".hero-enhanced");
    if (!hero) return;

    for (let i = 0; i < 50; i++) {
      const particle = document.createElement("div");
      particle.className =
        "absolute w-1 h-1 bg-white rounded-full opacity-30 animate-pulse";
      particle.style.left = Math.random() * 100 + "%";
      particle.style.top = Math.random() * 100 + "%";
      particle.style.animationDelay = Math.random() * 3 + "s";
      particle.style.animationDuration = 2 + Math.random() * 3 + "s";
      hero.appendChild(particle);
    }
  }

  // Add hover effects to skill cards
  function initSkillCardEffects() {
    document.querySelectorAll(".glass-card").forEach((card) => {
      card.addEventListener("mouseenter", () => {
        card.style.transform = "translateY(-5px) scale(1.02)";
        card.style.boxShadow = "0 20px 40px rgba(0, 0, 0, 0.1)";
      });

      card.addEventListener("mouseleave", () => {
        card.style.transform = "translateY(0) scale(1)";
        card.style.boxShadow = "";
      });
    });
  }

  // Add typing sound effect (optional)
  function addTypingSound() {
    const typewriterElement = document.querySelector(".typewriter");
    if (typewriterElement) {
      const audio = new Audio(
        "data:audio/wav;base64,UklGRnoGAABXQVZFZm10IBAAAAABAAEAQB8AAEAfAAABAAgAZGF0YQoGAACBhYqFbF1fdJivrJBhNjVgodDbq2EcBj+a2/LDciUFLIHO8tiJNwgZaLvt559NEAxQp+PwtmMcBjiR1/LMeSwFJHfH8N2QQAoUXrTp66hVFApGn+DyvmsbBSuNVfsXBlO6VpBNqrOsVaZYvnxcVaGHXHlNf0qGn1aGOl9ZrnMSH2lXrNlgJWGDxH5xQFlggGNNRG0XJVNr2tmnMSQGJIHE7NVlKQUli8f8jXZBBRJYucH5zWIsECKFyPUuglkDQ3NTorZYrVUKSU3qQENZGSqVRMM/M3lfsWqeWJGOwWoWE2lcqnOYGGdfrmOqcXYaBEAZ/xtTCSkI"
      );
      typewriterElement.addEventListener("input", () => {
        audio.currentTime = 0;
        audio.play().catch(() => {}); // Ignore autoplay restrictions
      });
    }
  }

  // Add scroll reveal animations
  function initScrollReveal() {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add("animate-in");

            // Animate skill bars when they come into view
            if (entry.target.classList.contains("skill-bar")) {
              const percentage = entry.target.dataset.percentage;
              setTimeout(() => {
                entry.target.style.width = percentage + "%";
              }, 200);
            }
          }
        });
      },
      {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px",
      }
    );

    document
      .querySelectorAll(".animate-on-scroll, .skill-bar")
      .forEach((el) => {
        observer.observe(el);
      });
  }

  // Add smooth page transitions
  function initPageTransitions() {
    // Add loading state for navigation
    document.querySelectorAll('a[href^="/"]').forEach((link) => {
      link.addEventListener("click", (e) => {
        if (link.target !== "_blank") {
          document.body.style.opacity = "0.8";
          document.body.style.transform = "scale(0.98)";
        }
      });
    });
  }

  // Add dynamic theme switching with animation
  function enhanceThemeSwitching() {
    const themeToggle = document.getElementById("darkModeToggle");
    if (themeToggle) {
      themeToggle.addEventListener("click", () => {
        document.documentElement.style.transition = "all 0.3s ease";
        setTimeout(() => {
          document.documentElement.style.transition = "";
        }, 300);
      });
    }
  }

  // Add project card animations
  function animateProjectCards() {
    const cards = document.querySelectorAll(
      ".project-card-enhanced, .project-card"
    );
    cards.forEach((card, index) => {
      card.style.opacity = "0";
      card.style.transform = "translateY(50px)";

      setTimeout(() => {
        card.style.transition = "all 0.6s ease";
        card.style.opacity = "1";
        card.style.transform = "translateY(0)";
      }, 100 * index);
    });
  }

  // Add contact form enhancements (if exists)
  function enhanceContactForm() {
    const forms = document.querySelectorAll("form");
    forms.forEach((form) => {
      const inputs = form.querySelectorAll("input, textarea");
      inputs.forEach((input) => {
        input.addEventListener("focus", () => {
          input.parentElement.classList.add("focused");
        });

        input.addEventListener("blur", () => {
          if (!input.value) {
            input.parentElement.classList.remove("focused");
          }
        });
      });
    });
  }

  // Add Easter egg - Konami code
  function addEasterEgg() {
    const konamiCode = [38, 38, 40, 40, 37, 39, 37, 39, 66, 65];
    let konamiIndex = 0;

    document.addEventListener("keydown", (e) => {
      if (e.keyCode === konamiCode[konamiIndex]) {
        konamiIndex++;
        if (konamiIndex === konamiCode.length) {
          // Activate party mode
          document.body.style.animation = "rainbow 2s infinite";
          setTimeout(() => {
            document.body.style.animation = "";
            alert("🎉 You found the secret! Welcome to party mode! 🎉");
          }, 2000);
          konamiIndex = 0;
        }
      } else {
        konamiIndex = 0;
      }
    });
  }

  // Initialize all enhancements
  createParticleSystem();
  initSkillCardEffects();
  addTypingSound();
  initScrollReveal();
  initPageTransitions();
  enhanceThemeSwitching();
  enhanceContactForm();
  addEasterEgg();

  // Animate project cards after a short delay
  setTimeout(animateProjectCards, 500);
});

// Add rainbow animation for Easter egg
const style = document.createElement("style");
style.textContent = `
    @keyframes rainbow {
        0% { filter: hue-rotate(0deg); }
        100% { filter: hue-rotate(360deg); }
    }
`;
document.head.appendChild(style);
