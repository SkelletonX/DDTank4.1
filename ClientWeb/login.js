const axios = require("axios");
const md5 = require("md5");
const host = "http://s1.plusgames.com.br/pt/ddtank/s01";

function getSession(username, md5Pass)
{
    return axios(`${host}/checkuser.ashx?username=${username}&password=${md5Pass}`,
    {
        "method": "GET"
    })
    .then(resp => {
        const cookies = resp.headers["set-cookie"][1];

        if(!cookies)
            throw "invalid username or password";

        // get ASP login session
        return cookies.substring(0, cookies.indexOf(";"));
    });
}

function getSWF(session)
{
    return axios(`${host}/logingame.aspx`,
    {
        "method": "GET",
        "headers": {
            "Cookie": session
        }
    })
    .then(resp => {
        return resp.data;
    });
}

async function login(username, password)
{
    try {
        const session = await getSession(username, md5(password));
        const swfURL = await getSWF(session);
        
        return swfURL;
    }
    catch(err) {
        return err;
    }
}

module.exports = login;