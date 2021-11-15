const {app, ipcMain, BrowserWindow} = require("electron");
const path = require("path");
const login = require("./login");
const plugin = path.join(__dirname, "flash", "pepflashplayer.dll");

// server
const express = require('express');
const server = express();
const port = 9284;
server.use(express.static(path.join(__dirname, 'public')));

//  xhr login integration
ipcMain.handle("login", async (event, {user, pass}) => {
    return await login(user, pass);
});

// load flash player
app.commandLine.appendSwitch('ppapi-flash-path', plugin);
app.commandLine.appendSwitch('ppapi-flash-version', '29.0.0.171');

// create window
function createWindow () {
    const mainWindow = new BrowserWindow({
        width: 1000,
        height: 659,
        webPreferences: {
            plugins: true,
            nodeIntegration: true
        }
    });

    server.listen(port, () => {
        mainWindow.loadURL(`http://localhost:${port}`);
    });
}


app.whenReady().then(() => {
    createWindow()
    app.on("activate", function () {
        if (BrowserWindow.getAllWindows().length === 0) createWindow();
    });
});

app.on("window-all-closed", function () {
    if (process.platform !== "darwin") app.quit()
});