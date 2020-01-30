async function RegisterUser(jobject) {
    const response = await fetch('https://localhost:44316/api/Authentication/Registration', {
        method: 'POST',
        body: jobject,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const loginAttempt = await response.json();
    return loginAttempt;
}

async function SendRegistration() {
    var form = document.forms["registerForm"].getElementsByTagName("input");
    var jbody = {}
    var email = form[0].value;

    jbody["Email"] = form[0].value;
    jbody["Password"] = form[1].value;
    jbody["Re-Password"] = form[2].value;

    if (jbody["Password"] === jbody["Re-Password"]) {
        jbody = JSON.stringify(jbody);
        var response = await RegisterUser(jbody);
        if (response["token"] !== "Login failed!") {
            localStorage.setItem("MusicEmail", email);
            localStorage.setItem("VirtoUserToken", response["token"]);
            window.location.replace('https://localhost:44372/MainPage.html');
        } else {
            window.alert("Login Failed!");
        }
    } else {
        window.alert("Passwords do not match!");
    }

    

}