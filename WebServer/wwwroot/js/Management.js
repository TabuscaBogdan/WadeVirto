function manageAccount() {
    var email = localStorage.getItem("MusicEmail");
    var token = localStorage.getItem("VirtoUserToken");

    if (!email || !token || email=="undefined") {
        window.location.replace("https://localhost:44372/LoginPage.html");
    }

    document.getElementById("AccManagement").innerHTML = email;

}