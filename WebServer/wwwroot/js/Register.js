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
    jbody["Email"] = form[0].value;
    jbody["Password"] = form[1].value;
    jbody["Re-Password"] = form[2].value;

    if (jbody["Password"] === jbody["Re-Password"]) {
        jbody = JSON.stringify(jbody);
        var response = await RegisterUser(jbody);
        console.log(response);
    } else {
        window.alert("Passwords do not match!");
    }

    

}