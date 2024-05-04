const currentDate = document.querySelector('.current-date'),
daysTag = document.querySelector('.days');

let date = new Date(),
currYear = date.getFullYear(),
currMonth = date.getMonth();


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
    let lastDateOfMonth = new Date(currYear, currMonth + 1, 0).getDate();
    currentDate.innerText = `${months[currMonth]} ${currYear}`;
    let liTag = ''
    for (let i = 1; i < lastDateOfMonth; i++) {
        liTag += `<li>${i}</li>`
    }

    daysTag.innerHTML = liTag;
}

renderCalendar();