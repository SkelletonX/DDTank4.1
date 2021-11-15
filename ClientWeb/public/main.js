const { ipcRenderer } = require("electron");
const $ = require("jquery");

$("#login").on("submit", e => {
    e.preventDefault();

    const target = $(e.target);
    
    // values
    const user = target.find("input[name=user]").val();
    const pass = target.find("input[name=pass]").val();

    ipcRenderer.invoke("login", {
        user: user,
        pass: pass
    }).then((swfURL) => {
        const key = swfURL.split("key=")[1];

        $("body").html(genSWF(user, key));
    });
});

function genSWF(user, key)
{
    return `<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" class="" id="7road-ddt-game"
        codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"
        name="Main" id="Main">
        <param name="allowScriptAccess" value="always" />
        <param name="movie" value="/Loading.swf?user=${user}&key=${key}&config=http://s1.plusgames.com.br/br/config.xml" />
        <param name="quality" value="low" />
        <param name="menu" value="false">
        <param name="bgcolor" value="#000000" />
        <param name="FlashVars" value="editby=" />
        <param name="allowScriptAccess" value="always" />
        <embed flashvars="editby=" src="/Loading.swf?user=${user}&key=${key}&config=http://s1.plusgames.com.br/br/config.xml"
        width="${window.innerWidth}" height="${window.innerHeight}" align="middle" quality="low" name="Main" allowscriptaccess="always"
        type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
    </object>`;
}