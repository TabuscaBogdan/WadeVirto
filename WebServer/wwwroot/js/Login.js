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
    jbody["Email"] = form[0].value;
    jbody["Password"] = form[1].value;

    jbody = JSON.stringify(jbody);
    var response = await LoginUser(jbody);
    console.log(response);
    
}

