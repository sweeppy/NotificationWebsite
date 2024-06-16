const currentDate = document.querySelector(".current-date"),
  daysTag = document.querySelector(".days"),
  prevNextIcon = document.querySelectorAll(".icons span");

const hours = document.getElementById("hours"),
  minutes = document.getElementById("minutes"),
  dayPart = document.getElementById("day-part");

let date = new Date(),
  currYear = date.getFullYear(),
  currMonth = date.getMonth(),
  selectedDays;

let postDayOfTheMonth, postMonth, postYear;

const months = [
  "January",
  "February",
  "March",
  "April",
  "May",
  "June",
  "July",
  "August",
  "September",
  "October",
  "November",
  "December",
];

const renderCalendar = () => {
  let firstDayOfMonth = new Date(currYear, currMonth, 1).getDay(),
    lastDateOfMonth = new Date(currYear, currMonth + 1, 0).getDate(),
    lastDayOfMonth = new Date(currYear, currMonth, lastDateOfMonth).getDay(),
    lastDateOfLastMonth = new Date(currYear, currMonth, 0).getDate();

  currentDate.innerText = `${months[currMonth]} ${currYear}`;
  let liTag = "";

  for (let i = firstDayOfMonth; i > 0; i--) {
    liTag += `<li class="inactive">${lastDateOfLastMonth - i + 1}</li>`;
  }

  for (let i = 1; i <= lastDateOfMonth; i++) {
    liTag += `<li>${i}</li>`;
  }

  for (let i = lastDayOfMonth; i < 6; i++) {
    liTag += `<li class="inactive">${i - lastDayOfMonth + 1}</li>`;
  }
  postMonth = currMonth;
  postYear = currYear;

  daysTag.innerHTML = liTag;
  selectedDays = document.querySelectorAll(".days li");
  PrepareToSelectDay(selectedDays);
};

renderCalendar();

prevNextIcon.forEach((icon) => {
  icon.addEventListener("click", () => {
    currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
    if (currMonth < 0 || currMonth > 11) {
      date = new Date(currYear, currMonth);
      currYear = date.getFullYear();
      currMonth = date.getMonth();
    } else {
      date = new Date();
    }
    renderCalendar();
  });
});

function PrepareToSelectDay(selectedDays) {
  selectedDays.forEach((day) => {
    day.addEventListener("click", () => {
      if (day.classList.contains("inactive")) {
        if (day.textContent < 7) {
          changeMonth(1, day); // Move to the previous month
        } else {
          changeMonth(-1, day); // Move to the next month
        }
      } else {
        selectedDays.forEach((otherDay) => {
          otherDay.classList.remove("active");
        });
        day.classList.add("active");
        postDayOfTheMonth = parseInt(day.textContent);
      }
    });
  });
}

function changeMonth(change, selectedDay) {
  console.log(currMonth, change);
  currMonth += change;
  console.log(currMonth);
  if (currMonth < 0 || currMonth > 11) {
    date = new Date(currYear, currMonth);
    currYear = date.getFullYear();
    currMonth = date.getMonth();
  } else {
    date = new Date();
  }
  renderCalendar();
  selectedDays.forEach((day) => {
    if (
      day.textContent === selectedDay.textContent &&
      !day.classList.contains("inactive")
    ) {
      day.classList.add("active");
      postDayOfTheMonth = parseInt(day.textContent);
      return;
    }
  });
}

const CreateTime = () => {
  let optionHours = "";
  let optionMinutes = `<option value="${0}">00</option>`;

  for (let i = 0; i < 12; i++) {
    optionHours += `<option value="${i}">${i}</option>`;
  }
  for (let i = 15; i < 60; i += 15) {
    optionMinutes += `<option value="${i}">${i}</option>`;
  }
  hours.innerHTML = optionHours;
  minutes.innerHTML = optionMinutes;
};
CreateTime();

async function CreateNewNotification() {
  const postHeader = document.getElementById("title").value,
    postMessage = document.getElementById("message").value,
    postSocial = document.querySelector(".chosen").getAttribute("name");

  let hours24 = 0;
  dayPart.value === "PM"
    ? (hours24 = parseInt(hours.value) + 12)
    : (hours24 = hours.value);

  const postDate = new Date(
    Date.UTC(
      postYear,
      postMonth,
      postDayOfTheMonth,
      hours24,
      parseInt(minutes.value)
    )
  );

  if (postDate > date) {
    const requestData = {
      dateTimeParam: postDate.toISOString(),
      header: postHeader,
      message: postMessage,
      social: postSocial,
    };
    try {
      let response;
      switch (postSocial) {
        case "vk":
          break;
        case "telegram":
          response = await fetch(
            "http://localhost:5019/api/gmail/SendTelegramMessage",
            {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
                Cookie: document.cookie,
              },
              body: JSON.stringify(requestData),
            }
          );
          break;
        case "gmail":
          response = await fetch(
            "http://localhost:5019/api/gmail/gmailSendMessage",
            {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
                Cookie: document.cookie,
              },
              body: JSON.stringify(requestData),
            }
          );
          break;
      }
    } catch (error) {
      console.error("Fetch error:", error);
    }
  }
  document.location.reload();
  console.log("End CreateNewNotification function");
}
