if ("serviceWorker" in navigator) {
  caches.keys().then(function (names) {
    for (let name of names) caches.delete(name);
  });
  navigator.serviceWorker.getRegistrations().then(function (registrations) {
    for (let registration of registrations) {
      registration.unregister();
    }
  });
}

document.addEventListener("DOMContentLoaded", function () {
  function showErrorNotification(message) {
    const notificationElement = document.getElementById("notification");
    const notificationMessage = document.getElementById("notificationMessage");

    // Nachricht setzen
    notificationMessage.textContent = message;

    // Benachrichtigung anzeigen
    notificationElement.classList.add("show");

    // Automatisches Schließen nach 3 Sekunden
    setTimeout(() => {
      closeNotification();
    }, 10000);
  }

  function closeNotification() {
    const notificationElement = document.getElementById("notification");
    notificationElement.classList.remove("show");
  }

  function validateForm() {
    const name = document.getElementById("name").value.trim();
    const namePattern = /^[a-zA-ZäöüÄÖÜßéàèÉÀÈ]+(?:\s[a-zA-ZäöüÄÖÜßéàèÉÀÈ]+)+$/;
    const email = document.getElementById("email").value;
    const phone = document.getElementById("phone").value;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const phonePattern = /^\d{10,15}$/;

    if (!namePattern.test(name)) {
      showErrorNotification("Bitte geben Sie einen gültigen Namen ein.");
      return false;
    }

    if (!emailPattern.test(email)) {
      showErrorNotification("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
      return false;
    }

    if (!phonePattern.test(phone)) {
      showErrorNotification("Bitte geben Sie eine gültige Telefonnummer ein.");
      return false;
    }

    return true;
  }

  const scrollToTopButton = document.getElementById("scrollToTop");

  // Show or hide the button when scrolling
  window.onscroll = function () {
    if (
      document.body.scrollTop > 200 ||
      document.documentElement.scrollTop > 200
    ) {
      scrollToTopButton.style.display = "flex";
    } else {
      scrollToTopButton.style.display = "none";
    }
  };

  // Scroll to the top when the button is clicked
  scrollToTopButton.addEventListener("click", function () {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  });

  window.openSidebar = function () {
    document.getElementById("mySidebar").style.width = "250px";
  };

  window.closeSidebar = function () {
    document.getElementById("mySidebar").style.width = "0";
  };

  const form = document.getElementById("serviceForm");
  form.onsubmit = function (event) {
    event.preventDefault(); // Verhindert das Standard-Formular-Submit

    if (validateForm()) {
      const formData = new FormData(form);
      const currentDate = new Date().toISOString(); // Aktuelles Datum
      const pickupDate = document.getElementById("pickup-date").value;
      // Zusätzliche Daten in das FormData-Objekt einfügen
      formData.append("create_date", currentDate);
      formData.append("pickup_date", pickupDate);
      // Senden an den Server

      fetch("http://localhost:5000/api/registration", {
        method: "POST",
        body: JSON.stringify({
          ...Object.fromEntries(formData),
          pickup_date: pickupDate,
        }), // Hier wird pickupDate hinzugefügt
        headers: {
          "Content-Type": "application/json",
        },
      })
        .then((response) => response.json())
        .then((data) => {
          // Hier nehmen wir an, dass die Daten im 'data' Objekt zurückgegeben werden
          const name = document.getElementById("name").value;
          const email = document.getElementById("email").value;
          const phone = document.getElementById("phone").value;
          const service = document.getElementById("service").value;
          const priority = document.getElementById("priority").value;
          const pickupDate = document.getElementById("pickup-date").value;
          const price = document.getElementById("preis").value;

          // Redirect to the 'ende.html' page with URL parameters
          window.location.href = `ende.html?name=${encodeURIComponent(
            name
          )}&email=${encodeURIComponent(email)}&phone=${encodeURIComponent(
            phone
          )}&service=${encodeURIComponent(
            service
          )}&priority=${encodeURIComponent(
            priority
          )}&pickup-date=${encodeURIComponent(
            pickupDate
          )}&preis=${encodeURIComponent(price)}`;
        })
        .catch((error) => {
          console.error("Fehler:", error);
          showErrorNotification("Es gab ein Problem mit der Serviceanmeldung.");
        });
    }
  };

  // Logik für das Abholdatum
  const prioritySelect = document.getElementById("priority");

  function updatePickupDate() {
    const priority = prioritySelect.value;
    const currentDate = new Date();
    let additionalDays;

    switch (priority) {
      case "tief":
        additionalDays = 5;
        break;
      case "standard":
        additionalDays = 0;
        break;
      case "express":
        additionalDays = -2;
        break;
      default:
        additionalDays = 0;
    }

    const totalDays = 7 + additionalDays;
    const pickupDate = new Date(currentDate);
    pickupDate.setDate(currentDate.getDate() + totalDays);

    const formattedDate = `${pickupDate
      .getDate()
      .toString()
      .padStart(2, "0")}.${(pickupDate.getMonth() + 1)
      .toString()
      .padStart(2, "0")}.${pickupDate.getFullYear().toString().slice(-2)}`;

    document.getElementById("pickup-date").value = formattedDate;
  }

  prioritySelect.addEventListener("change", updatePickupDate);

  // Initial pickup date calculation
  updatePickupDate();
  prioritySelect.value = "tief"; // Set initial value to "tief"
});

document.addEventListener("DOMContentLoaded", function () {
  const priorityElement = document.getElementById("priority");
  const serviceElement = document.getElementById("service");
  const priceElement = document.getElementById("preis");

  function calculateTotalPrice() {
    const priorityPrice = parseFloat(
      priorityElement.options[priorityElement.selectedIndex].text.split(
        " - CHF "
      )[1]
    );
    const servicePrice = parseFloat(
      serviceElement.options[serviceElement.selectedIndex].text.split(
        " - CHF "
      )[1]
    );
    const totalPrice = priorityPrice + servicePrice;
    priceElement.value = `CHF ${totalPrice.toFixed(2)}`;

    console.log("priorityPrice:", priorityPrice);
    console.log("servicePrice:", servicePrice);
  }

  priorityElement.addEventListener("change", calculateTotalPrice);
  serviceElement.addEventListener("change", calculateTotalPrice);

  // Initial calculation
  calculateTotalPrice();
});

document.addEventListener("DOMContentLoaded", function () {
  const urlParams = new URLSearchParams(window.location.search);
  const price = urlParams.get("preis");
  console.log("Preis aus der URL:", price); // Überprüfen Sie den Preis hier

  // Display the price
  document.getElementById("priceDisplay").innerText = `Total Price: ${price}`;
  if (price) {
    document.getElementById("preis").value = price;
  }
});

document
  .getElementById("downloadInvoice")
  .addEventListener("click", function () {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Read URL parameters
    const urlParams = new URLSearchParams(window.location.search);
    const name = urlParams.get("name");
    const email = urlParams.get("email");
    const phone = urlParams.get("phone");
    const service = urlParams.get("service");
    const priority = urlParams.get("priority");
    const pickupDate = urlParams.get("pickup-date");
    const price = urlParams.get("preis");

    console.log("Daten für PDF:", {
      name,
      email,
      phone,
      service,
      priority,
      pickupDate,
      price,
    });

    // Insert invoice data into the PDF
    doc.text("Rechnung Jetstream-Service", 20, 20);
    doc.text(`Name: ${name}`, 20, 30);
    doc.text(`E-Mail: ${email}`, 20, 40);
    doc.text(`Phone: ${phone}`, 20, 50);
    doc.text(`Service: ${service}`, 20, 60);
    doc.text(`Priority: ${priority}`, 20, 70);
    doc.text(`Pickup Date: ${pickupDate}`, 20, 80);
    doc.text(`Total Price: ${price}`, 20, 90);

    // Download PDF
    doc.save("Rechnung.pdf");
  });
