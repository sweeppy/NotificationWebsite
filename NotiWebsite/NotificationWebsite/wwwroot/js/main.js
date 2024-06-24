const modalContainer = document.getElementById("modal-container");
const modalNotification = document.getElementById("modal");
const modalTelegram = document.getElementById("telegramModal");
const addIcon = document.querySelector(".add-icon");
const closeCreateNotifcationSpan = document.querySelector(
  ".close-modal-add-notification"
);
const closeTelegramInstructions = document.querySelector(
  ".close-telegram-window"
);
const chosenIcon_telegram = document.getElementById("chosen-telegram");
const chosenIcon_gmail = document.getElementById("chosen-gmail");
const gmailItem = document.getElementById("gmail-item");
const telegramItem = document.getElementById("telegram-item");

const menuButton = document.getElementById("menuBtn");

const dropdownContent = document.getElementById("dropdown-content");
const applyAction = document.getElementById("applyAction");
const cancelAction = document.getElementById("cancelAction");

let notifcationAction = "";

menuButton.addEventListener("click", function () {
  var dropdownContent = document.getElementById("dropdown-content");
  if (dropdownContent.style.display === "block") {
    dropdownContent.style.display = "none";
  } else {
    dropdownContent.style.display = "block";
    dropdownContent.style.top = this.offsetTop + this.offsetHeight + "px";
    dropdownContent.style.left = this.offsetLeft + "px";
  }
});

function SelectNotifications(action) {
  notifcationAction = action;
  document.querySelectorAll(".selection-checkbox").forEach(function (checkBox) {
    checkBox.style.display = "block";
  });
  dropdownContent.style.display = "none";
  applyAction.style.display = "block";
  cancelAction.style.display = "block";
}

cancelAction.addEventListener("click", function () {
  document.querySelectorAll(".selection-checkbox").forEach(function (checkBox) {
    checkBox.style.display = "none";
    checkBox.classList.remove("checked");

    applyAction.style.display = "none";
    cancelAction.style.display = "none";
  });
  dropdownContent.style.display = "none";
});

applyAction.addEventListener("click", function () {
  console.log(notifcationAction);
  switch (notifcationAction) {
    case "deleteNotificationAction":
      console.log("delete");
      break;

    case "cancelSendingAction":
      console.log("cancel");
      break;
  }
});

document.querySelectorAll(".selection-checkbox").forEach(function (checkBox) {
  checkBox.addEventListener("click", function () {
    this.classList.toggle("checked");
  });
});

const searchInput = document.querySelector(".search-input");
searchInput.addEventListener("input", function () {
  const searchText = searchInput.value.toLowerCase();
  SearchNotification(searchText);
});

function SearchNotification(searchText) {
  const notificationBlocks = document.querySelectorAll(".notification-block");
  notificationBlocks.forEach(function (notification) {
    const header = notification
      .querySelector(".notification-header")
      .textContent.toLowerCase();
    const message = notification
      .querySelector(".notification-body")
      .textContent.toLowerCase();
    if (header.includes(searchText) || message.includes(searchText)) {
      notification.style.display = "flex";
    } else {
      notification.style.display = "none";
    }
  });
}

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
      <div class="notification-information-message poppins-extralight-italic">${message}</div>
      <div class="notification-information-status poppins-extralight-italic">Status: ${status}</div>`;
  notificationInfo.style.display = "flex";
  notificationInfo.classList.add("flex-column");

  const notificationHeader = document.querySelector(
    ".header-notification-information"
  );
  notificationHeader.classList.remove("show-notification-header-animation");
  void notificationHeader.offsetWidth;
  notificationHeader.classList.add("show-notification-header-animation");

  const notificationMessage = document.querySelector(
    ".notification-information-message"
  );
  notificationMessage.classList.remove("show-notification-message-animation");
  void notificationMessage.offsetWidth;
  notificationMessage.classList.add("show-notification-message-animation");

  const notificationSocial = document.querySelector(
    ".notification-information-social"
  );
  notificationSocial.classList.remove("show-notification-social-animation");
  void notificationSocial.offsetWidth;
  notificationSocial.classList.add("show-notification-social-animation");
}

addIcon.onclick = function () {
  modalContainer.style.display = "block";
  modalNotification.style.display = "flex";
};

closeCreateNotifcationSpan.onclick = function () {
  modalNotification.classList.add("close-modal-win-animation");
  setTimeout(function () {
    modalNotification.style.display = "none";
    modalContainer.style.display = "none";
    modalNotification.classList.remove("close-modal-win-animation");
  }, 400);
};

closeTelegramInstructions.onclick = function () {
  modalTelegram.style.display = "none";
  modalTelegram.classList.remove("close-modal-win-animation");
};

function Chosen(iconId, itemId) {
  chosenIcon_gmail.style.visibility = "hidden";
  chosenIcon_telegram.style.visibility = "hidden";
  chosenIcon = document.getElementById(iconId);
  chosenIcon.style.visibility = "visible";

  telegramItem.classList.remove("chosen");
  gmailItem.classList.remove("chosen");
  chosenItem = document.getElementById(itemId);
  chosenItem.classList.add("chosen");
}
