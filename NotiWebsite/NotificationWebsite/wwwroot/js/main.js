document.addEventListener("DOMContentLoaded", function () {
  var notificationStatusElements = document.querySelectorAll(
    ".notification-status"
  );

  notificationStatusElements.forEach(function (element) {
    var status = element.textContent.toLowerCase();

    switch (status) {
      case "planned":
        element.classList.add("planned");
        break;
      case "sent":
        element.classList.add("sent");
        break;
      case "canceled":
        element.classList.add("canceled");
        break;
    }
  });
});

function showNotificationInfo(status, socialNetwork, message, header, date) {
  var notificationInfo = document.querySelector(".notification-information-bg");
  notificationInfo.innerHTML = `
      <div class="header-notification-information poppins-extrabold-italic">${header}</div>
      <div class="notification-information-SS">
        <div class="notification-information-social poppins-light-italic">Social network: ${socialNetwork}</div>
        <div class="notification-information-date poppins-light-italic">Date: ${date}</div>
      </div>
      <div class="notification-information-message poppins-extralight-italic">Social network: ${message}</div>
      <div class="notification-information-status poppins-extralight-italic">Status: ${status}</div>`;
  notificationInfo.style.display = "flex";
  notificationInfo.classList.add("flex-column");

  var notificationHeader = document.querySelector(
    ".header-notification-information"
  );
  notificationHeader.classList.remove("show-notification-header-animation");
  void notificationHeader.offsetWidth;
  notificationHeader.classList.add("show-notification-header-animation");

  var notificationMessage = document.querySelector(
    ".notification-information-message"
  );
  notificationMessage.classList.remove("show-notification-message-animation");
  void notificationMessage.offsetWidth;
  notificationMessage.classList.add("show-notification-message-animation");

  var notificationSocial = document.querySelector(
    ".notification-information-social"
  );
  notificationSocial.classList.remove("show-notification-social-animation");
  void notificationSocial.offsetWidth;
  notificationSocial.classList.add("show-notification-social-animation");
}

const modalContainer = document.getElementById("modal-container");
const modal = document.getElementById("modal");
const addIcon = document.querySelector(".add-icon");
const closeModal = document.querySelector(".close-modal-add-notification");

addIcon.onclick = function () {
  modalContainer.style.display = "block";
  modal.style.display = "flex";
  document.body.style.overflow = "hidden";
};

closeModal.onclick = function () {
  modal.classList.add("close-modal-win-animation");
  setTimeout(function () {
    modal.style.display = "none";
    modalContainer.style.display = "none";
    modal.classList.remove("close-modal-win-animation");
  }, 400);
};
