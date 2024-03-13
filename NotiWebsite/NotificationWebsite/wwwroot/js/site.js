document.getElementById("login-win").style.display = "none";
document.getElementById("sign-up-win").style.display = "none";

function openLogin() {
  const loginBtn = document.getElementById("loginBtn");
  const modal = document.getElementById("login-win");
  loginBtn.disabled = true;
  modal.style.display = "block";
}
function closeLogin() {
  const modal = document.getElementById("login-win");
  const login_username = document.getElementById("login_username");
  const login_password = document.getElementById("login_password");
  const authBtnProp = document.querySelector(".auth-btn");
  modal.classList.add("modal-closing"); // Add the class to trigger the closing animation
  setTimeout(() => {
    modal.style.display = "none";
    modal.classList.remove("modal-closing");
  }, 300); // since anim 0.3s we need 300ms
  login_username.value = "";
  login_password.value = "";
  login_username.placeholder = "Username:";
  login_password.placeholder = "Password";
  authBtnProp.classList.remove("active");
}

function openSignup() {
  const modal = document.getElementById("sign-up-win");
  modal.style.display = "block";
}
function closeSignUp() {
  const modal = document.getElementById("sign-up-win");
  modal.classList.add("modal-closing"); //add closing class

  setTimeout(() => {
    modal.style.display = "none";
    modal.classList.remove("modal-closing");
  }, 300);
}

//view of username placeholder(Login):
login_username.addEventListener("focus", function () {
  login_username.removeAttribute("placeholder");
});
login_username.addEventListener("blur", function () {
  if (!login_username.value) {
    login_username.setAttribute("placeholder", "Username:");
  }
});
//view if password placeholder(Login):
login_password.addEventListener("focus", function () {
  login_password.removeAttribute("placeholder");
});
login_password.addEventListener("blur", function () {
  if (!login_password.value) {
    login_password.setAttribute("placeholder", "Password:");
  }
});
//change auth btn
function changeAuthBtn() {
  const loginUsername = document.getElementById("login_username");
  const loginPassword = document.getElementById("login_password");
  const authBtnProp = document.querySelector(".auth-btn");
  const loginBtn = document.getElementById("loginBtn");
  if (loginUsername.value.trim() !== "" && loginPassword.value.trim() !== "") {
    authBtnProp.classList.remove("disable");
    authBtnProp.classList.add("active");
    loginBtn.disabled = false;
  } else if (authBtnProp.classList.contains("active")) {
    authBtnProp.classList.add("disable");
    authBtnProp.classList.remove("active");
    loginBtn.disabled = true;
  } else {
    loginBtn.disabled = true;
  }
}
