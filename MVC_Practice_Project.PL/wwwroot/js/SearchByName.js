//document.addEventListener("DOMContentLoaded", () => {
//    const loginFormContainer = document.getElementById("signin-form-container");
//    const signupFormContainer = document.getElementById("signup-form-container");
//    const forgotPasswordFormContainer = document.getElementById(
//        "forgot-password-form-container"
//    );

//    const signupLinkLogin = document.getElementById("signup-link-login");
//    const loginLinkSignup = document.getElementById("login-link-signup");
//    const forgotPasswordLink = document.getElementById("forgot-password-link");
//    const backToLoginLinkForgot = document.getElementById(
//        "back-to-login-link-forgot"
//    );

//    const showForm = (formToShow) => {
//        loginFormContainer.classList.add("hidden");
//        signupFormContainer.classList.add("hidden");
//        forgotPasswordFormContainer.classList.add("hidden");
//        formToShow.classList.remove("hidden");
//    };

//    for (const elem of [
//        signupLinkLogin,
//        loginLinkSignup,
//        forgotPasswordLink,
//        backToLoginLinkForgot
//    ]) {
//        elem.addEventListener(
//            "click",
//            (e) => {
//                e.preventDefault();
//                const container = document.getElementById(elem.dataset.container);
//                showForm(container);
//            },
//            false
//        );
//    }
//    showForm(loginFormContainer);
//});
let timer;

function SearchByName(e) {
    clearTimeout(timer);
    var SearchInput = e.target.value;
    var Route = e.target.getAttribute("route");
    
    timer = setTimeout(() => {
        const url = `https://localhost:44302/${Route}?SearchInput=${SearchInput}`;
        fetch(url)
            .then(r => r.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, "text/html");
                const tablePart = doc.getElementById("TableContainer").innerHTML;
                document.getElementById("TableContainer").innerHTML = tablePart;
            });
    }, 1000);
}

document.getElementById("SearchInput").addEventListener("keyup", SearchByName);