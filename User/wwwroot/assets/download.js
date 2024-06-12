/*window.openInNewTab = function (url) {
    var win = window.open(url, '_blank');
    win.focus();
};
*/
/*function openInNewTab(url) {
    window.open(url, '_blank');

}*/
// custom.js

window.createBlobUrl = function (base64String, contentType) {
    var byteCharacters = atob(base64String);
    var byteNumbers = new Array(byteCharacters.length);

    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    var byteArray = new Uint8Array(byteNumbers);
    var blob = new Blob([byteArray], { type: contentType });

    return URL.createObjectURL(blob);
};
