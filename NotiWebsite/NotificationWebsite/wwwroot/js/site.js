document.getElementById("login-win").style.display = "none";
document.getElementById("sign-up-win").style.display = "none";
const login_username = document.getElementById("login_username");
const login_password = document.getElementById("login_password");
const signUp_mail = document.getElementById("signUp_mail");
const signUp_username = document.getElementById("signUp_username");
const signUp_password = document.getElementById("signUp_password");
inputs = [
  login_username,
  login_password,
  signUp_mail,
  signUp_username,
  signUp_password,
];
placeholderListener(inputs);

function openAuthWin(modalWin, acceptBtn) {
  modalWin.style.display = "block";
  acceptBtn.disabled = true;
}
function closeAuthWin(modal, inputsProp, authBtn) {
  modal.classList.add("modal-closing"); // Add the class to trigger the closing animation
  setTimeout(() => {
    modal.style.display = "none";
    modal.classList.remove("modal-closing");

    inputsProp.forEach((element) => {
      element.value = "";
      element.placeholder = `Enter ${element.getAttribute("name")}:`;
    });

    authBtn.classList.remove("active");
  }, 300); // since anim 0.3s we need 300ms
}
function openLogin() {
  const loginBtn = document.getElementById("loginBtn");
  const modal = document.getElementById("login-win");
  openAuthWin(modal, loginBtn);
}
function openSignup() {
  const modal = document.getElementById("sign-up-win");
  const signUpBtn = document.getElementById("signUpBtn");
  openAuthWin(modal, signUpBtn);
}

function closeLogin() {
  const modal = document.getElementById("login-win");
  const login_username = document.getElementById("login_username");
  const login_password = document.getElementById("login_password");
  const authBtn = document.getElementById("loginBtn");
  const loginProp = [login_username, login_password];
  closeAuthWin(modal, loginProp, authBtn);
}
function closeSignUp() {
  const modal = document.getElementById("sign-up-win");
  const mail = document.getElementById("signUp_mail");
  const username = document.getElementById("signUp_username");
  const password = document.getElementById("signUp_password");
  registerProp = [mail, username, password];
  const authBtnProp = document.getElementById("signUpBtn");
  closeAuthWin(modal, registerProp, authBtnProp);
}

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

//check that every input filled
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
  const username = document.getElementById("login_username");
  const password = document.getElementById("login_password");
  inputsProp = [username, password];
  changeAuthBtn(inputsProp, loginBtn);
}
//change the sign up btn state(active/disabled)
function changeStateSignUpBtn() {
  const signUpBtn = document.getElementById("signUpBtn");
  const mail = document.getElementById("signUp_mail");
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
