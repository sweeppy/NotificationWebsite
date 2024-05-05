const currentDate = document.querySelector('.current-date'),
daysTag = document.querySelector('.days'),
prevNextIcon = document.querySelectorAll('.icons span');

let date = new Date(),
currYear = date.getFullYear(),
currMonth = date.getMonth(),

selectedDays;


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
    "December"
  ];

const renderCalendar = () => {
    let firstDayOfMonth = new Date(currYear, currMonth, 1).getDay(),
    lastDateOfMonth = new Date(currYear, currMonth + 1, 0).getDate(),
    lastDayOfMonth = new Date(currYear, currMonth, lastDateOfMonth).getDay(),
    lastDateOfLastMonth = new Date(currYear, currMonth, 0).getDate();


    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    let liTag = ''

    for (let i = firstDayOfMonth; i > 0; i--) {
        liTag += `<li class="inactive">${lastDateOfLastMonth - i + 1}</li>`
    }

    for (let i = 1; i <= lastDateOfMonth; i++) {
        liTag += `<li>${i}</li>`
    }

    for (let i = lastDayOfMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayOfMonth + 1}</li>`
    }

    daysTag.innerHTML = liTag;
    selectedDays = document.querySelectorAll('.days li');
    PrepareToSelectDay(selectedDays);
}

renderCalendar();

prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => {
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
        if (currMonth < 0 || currMonth > 11) {
                date = new Date(currYear, currMonth);
                currYear = date.getFullYear();
                currMonth = date.getMonth();
            }
            else {
                date = new Date();
            }
        renderCalendar();
    });
});


function PrepareToSelectDay(selectedDays) {
    selectedDays.forEach(day => {
      day.addEventListener("click", () => {
        if (day.classList.contains("inactive")) {
          if (day.textContent < 7) {
            changeMonth(1, day); // Move to the previous month
          } else {
            changeMonth(-1, day); // Move to the next month
          }
        } else {
          selectedDays.forEach(otherDay => {
            otherDay.classList.remove("active");
          });
          day.classList.add("active");
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
    }
    else {
        date = new Date();
    }
    renderCalendar();
    selectedDays.forEach(day => {
        if (day.textContent === selectedDay.textContent && !day.classList.contains('inactive')) {
            day.classList.add("active");
            return;
        }
    });
    
}


