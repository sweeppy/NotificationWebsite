const login_Email = document.getElementById("login_Email");
const login_password = document.getElementById("login_password");
const signUp_mail = document.getElementById("signUp_mail");
const signUp_username = document.getElementById("signUp_username");
const signUp_password = document.getElementById("signUp_password");
inputs = [
  login_Email,
  login_password,
  signUp_mail,
  signUp_username,
  signUp_password,
];
placeholderListener(inputs);

//live change placeholders
function placeholderListener(fields) {
  fields.forEach((element) => {
    element.addEventListener("focus", function () {
      element.removeAttribute("placeholder");
    });
    element.addEventListener("blur", function () {
      if (!element.value) {
        element.setAttribute(
          "placeholder",
          `Enter ${element.getAttribute("name")}:`
        );
      }
    });
  });
}

//check that all fields are filled
function checkFill(inputsProp) {
  for (let element of inputsProp) {
    if (element.value.trim() === "") {
      return false;
    }
  }
  return true;
}
//change the login btn state(active/disabled)
function changeStateLoginBtn() {
  const loginBtn = document.getElementById("loginBtn");
  const username = document.getElementById("login_Email");
  const password = document.getElementById("login_password");
  inputsProp = [username, password];
  changeAuthBtn(inputsProp, loginBtn);
}
//change the sign up btn state(active/disabled)
function changeStateSignUpBtn() {
  const signUpBtn = document.getElementById("signUpBtn");
  const mail = document.getElementById("signUp_Email");
  const username = document.getElementById("signUp_username");
  const password = document.getElementById("signUp_password");
  inputsProp = [mail, username, password];
  changeAuthBtn(inputsProp, signUpBtn);
}

//change auth btn
function changeAuthBtn(inputsProp, btn) {
  if (checkFill(inputsProp)) {
    btn.classList.remove("disable");
    btn.classList.add("active");
    btn.disabled = false;
  } else if (btn.classList.contains("active")) {
    btn.classList.add("disable");
    btn.classList.remove("active");
    btn.disabled = true;
  } else {
    btn.disabled = true;
  }
}
