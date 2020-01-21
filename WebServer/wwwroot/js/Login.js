async function LoginUser(jobject) {
    const response = await fetch('https://localhost:44316/api/Authentication/Login', {
        method: 'POST',
        body: jobject,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const loginAttempt = await response.json();
    return loginAttempt;
}

async function SendLogin() {
    var form = document.forms["loginForm"].getElementsByTagName("input");
    var jbody = {}
    var email = form[0].value;
    jbody["Email"] = email;
    jbody["Password"] = form[1].value;

    jbody = JSON.stringify(jbody);
    var response = await LoginUser(jbody);

    if (response["token"] !== "Login failed!") {
        localStorage.setItem("MusicEmail", email);
        localStorage.setItem("VirtoUserToken", response["token"]);
        window.location.replace('https://localhost:44372/MainPage.html');
    } else {
        window.alert("Login Failed!");
    }
    console.log(response);
    
}

